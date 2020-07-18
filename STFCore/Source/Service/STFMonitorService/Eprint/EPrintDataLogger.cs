using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.DataLog;

//namespace HP.ScalableTest.Service.Monitor.Eprint
namespace HP.ScalableTest.Service.Monitor.Eprint
{
    /// <summary>
    /// Facilitates logging of EPrint Job data.
    /// </summary>
    internal class EPrintDataLogger
    {
        private const string EndState = "OK";
        
        /// <summary>
        /// Describes the end result of a call to DBDataReader.Read
        /// </summary>
        private enum ReadResult
        {
            /// <summary>
            /// Read method returned 'false'.
            /// </summary>
            EndOfResultSet,
            /// <summary>
            /// Read method threw an error.
            /// </summary>
            Skip,
            /// <summary>
            /// Read method returned 'true'.
            /// </summary>
            Success
        }

        /// <summary>
        /// Logs all ePrint job data using the specified <see cref="DbDataReader" />.
        /// </summary>
        /// <param name="jobsToProcess">The jobs to process.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>
        /// A list of all job Ids processed.
        /// </returns>
        public static List<PendingJob> LogAll(List<PendingJob> jobsToProcess, DbDataReader reader)
        {
            List<PendingJob> processedJobs = new List<PendingJob>();
            ReadResult readResult = ReadResult.Success;
            int pendingJobId = -1;

            while (readResult != ReadResult.EndOfResultSet)
            {
                readResult = ReadData(reader);

                switch (readResult)
                {
                    case ReadResult.Success:
                        pendingJobId = (int)reader["ID"];
                        EPrintServerJobLogger log = ValidateJobData(pendingJobId, reader);
                        PendingJob pendingJob = jobsToProcess.Where(j => j.JobId == pendingJobId).FirstOrDefault();
                        if (log != null)
                        {
                            Submit(log, pendingJob != null ? pendingJob.IsInsert : true);
                            pendingJob.Status = GetPendingJobStatus(log);
                            if (pendingJob.Status == PendingJobStatus.EndstateReached)
                            {
                                // Add to list of processed jobs
                                processedJobs.Add(pendingJob);                                
                            }
                        }
                        else
                        {
                            // Not an STF Job.  Ensure it gets removed from the pending table.
                            pendingJob.Status = PendingJobStatus.NotAnSTFJob;
                            processedJobs.Add(pendingJob);
                        }
                        break;
                    case ReadResult.Skip:
                        continue;
                    default:
                        break;
                }
            }

            return processedJobs;
        }

        /// <summary>
        /// Executes DbDataReader.Read() and evaluates the results.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>
        /// EndOfResultSet - Read method returned 'false'.
        /// Skip - Read method threw an error.
        /// Success - Read method returned 'true'.
        /// </returns>
        private static ReadResult ReadData(DbDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    return ReadResult.Success;
                }

                return ReadResult.EndOfResultSet;
            }
            catch (SqlException sqlEx)
            {
                StringBuilder message = new StringBuilder();
                if (sqlEx.Number == 1205)
                {
                    message.Append("Deadlock encountered.  ");
                }
                else
                {
                    //Unexpected SQL exception.
                    message.Append(sqlEx.ToString());
                    message.Append(Environment.NewLine);
                }
                message.Append("Continue to process.");
                TraceFactory.Logger.Debug(message.ToString());

                return ReadResult.Skip;
            }
        }

        /// <summary>
        /// Parses the STF PrintJobId <see cref="Guid"/> out of the ePrint job file name.
        /// </summary>
        /// <param name="jobFileName"></param>
        /// <returns>STF PrintJobId <see cref="Guid"/></returns>
        private static Guid ParsePrintJobId(string jobFileName)
        {
            try
            {
                return UniqueFile.ExtractId(jobFileName);
            }
            catch (FormatException)
            {
                //TraceFactory.Logger.Error(ex);
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Parses the STF SessionId from the subtitle field of the ePrint job data.
        /// </summary>
        /// <param name="subTitle">The email subject from the ePrint database</param>
        /// <returns>The SessionId</returns>
        private static string ParseSessionId(string subTitle)
        {
            string[] parts = subTitle.Split(':');

            return (parts.Length > 1) ? parts[1] : subTitle;
        }

        /// <summary>
        /// Determines whether or not an ePrint job should be logged to the STF.
        /// </summary>
        /// <param name="reader">The data reader</param>
        /// <returns>An <see cref="EPrintServerJobLogger"/> object if the record should be logged.  Null othewise.</returns>
        private static EPrintServerJobLogger ValidateJobData(int ePrintJobId, DbDataReader reader)
        {
            string fileName = ((string)reader["JobName"]).Trim();
            
            // If we can parse a PrintJobId out of the file name, we're good.
            Guid printJobId = ParsePrintJobId(fileName);
            if (printJobId == Guid.Empty)
            {
                // No print job Id.  Check for email body. 
                if (! IsSTFJob(fileName, reader["Subtitle"] as string))
                {
                    // Unrecognized job.  Log and Abort.
                    TraceFactory.Logger.Warn("Unrecognized STF-generated ePrint job: {0}  {1}".FormatWith(ePrintJobId, fileName));
                    return null;
                }
            }

            // At this point we know it's an STF print job
            return CreateLog(printJobId, reader);
        }

        /// <summary>
        /// Determines whether or not an ePrint job was sent by STF.
        /// </summary>
        /// <param name="fileName">The filename of the ePrint job</param>
        /// <param name="subTitle">The email subject text</param>
        /// <returns></returns>
        private static bool IsSTFJob(string fileName, string subTitle)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(subTitle))
            {
                return false;
            }
            
            return (fileName.Contains("message_body")
                && subTitle.Contains("SessionId"));
        }

        /// <summary>
        /// Creates a new <see cref="EPrintServerJobLogger"/> using the provided <see cref="DbDataReader"/>.
        /// </summary>
        /// <param name="printJobId">The STF Print job Id.</param>
        /// <param name="reader">The data reader</param>
        /// <returns></returns>
        private static EPrintServerJobLogger CreateLog(Guid printJobId, DbDataReader reader)
        {
            string sessionId = ParseSessionId(reader["Subtitle"] as string);
            EPrintServerJobLogger log = new EPrintServerJobLogger((int)reader["ID"], sessionId);

            log.PrintJobClientId = printJobId;
            log.SessionId = ParseSessionId(reader["Subtitle"] as string);
            log.JobName = reader["JobName"] as string;
            log.JobStatus = reader["JobStatus"] as string;
            log.JobStartDateTime = (DateTime)reader["CreatedAt"];
            log.LastStatusDateTime = (DateTime)reader["LastStatusAt"];
            log.EPrintTransactionId = (int)reader["GroupID"];
            log.JobFolderId = reader["JobFolderId"] as string;
            log.EmailAccount = FormatEmail(reader["NTUserAccount"] as string);
            log.EmailReceivedDateTime = (DateTime)reader["userDateTime"];
            log.TransactionStatus = reader["TransactionStatus"] as string;
            log.PrinterName = reader["Printer"] as string;

            return log;
        }

        /// <summary>
        /// Formats the specified NT User name as an email address.
        /// </summary>
        /// <param name="ntUser">The fully qualified NT user account name</param>
        /// <returns>NT User string formatted as an email address</returns>
        private static string FormatEmail(string ntUser)
        {
            string[] parsed = ntUser.Split(new char[] { '\\' });

            if (ntUser == null)
            {
                return string.Empty;
            }

            if (parsed.Count() < 2)
            {
                return ntUser;
            }

            StringBuilder result = new StringBuilder();
            result.Append(parsed[1]);
            result.Append("@");
            result.Append(parsed[0]);

            return result.ToString();
        }

        /// <summary>
        /// Determines whether to Update or Insert the log data.
        /// </summary>
        /// <param name="log">The <see cref="EPrintServerJobLogger"/></param>
        /// <param name="update"></param>
        private static void Submit(EPrintServerJobLogger log, bool insert)
        {
            DataLogger dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);

            if (insert)
            {
                dataLogger.SubmitAsync(log);
                return;
            }

            TraceFactory.Logger.Debug("UPDATE:");
            TraceFactory.Logger.Debug(log.ToString());
            dataLogger.UpdateAsync(log);
        }

        private static PendingJobStatus GetPendingJobStatus(EPrintServerJobLogger log)
        {
            if (log.JobStatus == EndState && log.TransactionStatus == EndState)
            {
                return PendingJobStatus.EndstateReached;
            }

            return PendingJobStatus.ContinueProcessing;
        }
    }
}
