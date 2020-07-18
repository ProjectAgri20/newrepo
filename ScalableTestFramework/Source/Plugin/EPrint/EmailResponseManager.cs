using System;
using System.Collections.Generic;
using System.Text;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.EPrint
{
    internal class EmailResponseManager
    {
        IEmailController _emailController = null;

        //Using Arrays here because these collections are essentially static and won't be changing through the life of the plugin instance.
        private static string[] _printedText = new string[2] { "Message printed", "ready for printing" };
        private static string[] _processingText = new string[2] { "Processing your request", "being processed" };
        private static string[] _partialText = new string[1] { "partially ready" }; // Return message when the job is still processing from ePrint 3.0, 3.2 server

        public event EventHandler<StatusChangedEventArgs> EmailResponseReceived;

        public EmailResponseManager(IEmailController controller)
        {
            _emailController = controller;
        }

        public void CheckForEmailResponse()
        {
            try
            {
                StringBuilder logText = new StringBuilder();

                ExecutionServices.SystemTrace.LogDebug("Retrieving Email Responses.");
                // Look through all the received mail to find one with "ePrint" in subject line
                foreach (EmailMessage message in _emailController.RetrieveMessages(EmailFolder.Inbox))
                {
                    if (message.Subject.Contains("ePrint", StringComparison.InvariantCultureIgnoreCase))
                    {
                        logText.Clear();
                        LogHeaders(message, ref logText);

                        if (HasPrinted(message, ref logText))
                        {
                            // The document has been printed.
                            _emailController.Delete(message);
                        }
                        else if (IsProcessing(message, ref logText))
                        {
                            // The document was received
                            _emailController.Delete(message);
                        }
                        else if (IsPartiallyReady(message, ref logText))
                        {
                            // "Partially Ready" is an error condition.
                            _emailController.Delete(message);
                        }
                        else // Unrecognized text in the body of the message. 
                        {
                            logText.Append(Environment.NewLine);
                            logText.AppendLine("Print job failed.");
                            logText.Append(message.Subject).Append("  ");
                            logText.Append(CleanUpHtml(message.Body));
                        }
                    }
                    else // Unrecognized email message.
                    {
                        logText.AppendLine("Non-ePrint related email received: ");
                        logText.Append(message.Subject);
                        logText.Append("BODY: ").Append(CleanUpHtml(message.Body));
                    }

                    logText.AppendLine();
                    OnEmailResponseReceived(logText.ToString());
                }
            }
            catch (Exception ex)
            {
                CleanEmailFolders();
                ExecutionServices.SystemTrace.LogError(ex);
            }
        }

        /// <summary>
        /// Logs header information from the email.
        /// </summary>
        /// <param name="message">The Email message</param>
        /// <param name="logText">The log text</param>
        private void LogHeaders(EmailMessage message, ref StringBuilder logText)
        {
            ExecutionServices.SystemTrace.LogDebug("EMAIL HEADERS");
            foreach (KeyValuePair<string, string> pair in message.Headers)
            {
                logText.Append("Key: ").Append(pair.Key);
                logText.Append("  Value: ").Append(pair.Value);
                ExecutionServices.SystemTrace.LogDebug(logText.ToString());
                logText.Clear();
            }
        }

        /// <summary>
        /// Evaluates the message body to see if the document is currently processing.
        /// </summary>
        /// <param name="message">The Email message to evaluate</param>
        /// <param name="logText">The text to log.</param>
        /// <returns></returns>
        private bool IsProcessing(EmailMessage message, ref StringBuilder logText)
        {
            foreach (string processingText in _processingText)
            {
                if (message.Body.Contains(processingText, StringComparison.InvariantCultureIgnoreCase))
                {
                    // If the server is still processing, the Activity is not done
                    logText.Append(Environment.NewLine);
                    logText.AppendLine("Print job received.");

                    //Log that we received a "processing" reply
                    logText.Append(message.Subject).Append("  ");
                    logText.AppendLine(CleanUpHtml(message.Body));

                    return true;
                }
            }

            // If we get to this point, we did not find any text in the email body indicating that the document is processing.
            return false;
        }

        /// <summary>
        /// Evaluates the message body to see if we received a "partially ready" message.
        /// This condition exists when either the email message or the attachment was processed, but not both.
        /// In this case the ePrint server sends this message back and essentially abandons processing it.
        /// For the purpose of the STF test, this is a failure.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logText"></param>
        /// <returns></returns>
        private bool IsPartiallyReady(EmailMessage message, ref StringBuilder logText)
        {
            foreach (string partialText in _partialText)
            {
                if (message.Body.Contains(partialText, StringComparison.InvariantCultureIgnoreCase))
                {
                    logText.Append(Environment.NewLine);
                    logText.AppendLine("Print job failed");
                    logText.Append(message.Subject).Append("  ");
                    logText.Append(CleanUpHtml(message.Body));

                    return true;
                }
            }

            // No indication of a partially ready document.
            return false;
        }

        /// <summary>
        /// Evaluates the message body to see if the document has been printed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logText"></param>
        /// <returns></returns>
        private bool HasPrinted(EmailMessage message, ref StringBuilder logText)
        {
            foreach (string printedText in _printedText)
            {
                if (message.Body.Contains(printedText, StringComparison.InvariantCultureIgnoreCase))
                {
                    logText.Append(Environment.NewLine);
                    logText.Append(CleanUpHtml(message.Body)).Append(Environment.NewLine);
                    logText.Append(new string('-', 60)).Append(Environment.NewLine);

                    return true;
                }
            }

            // If we get to this point, we did not find any text in the email body indicating that the document was printed.
            return false;
        }

        /// <summary>
        /// Removes some of the markup noise that comes back from the ePrint server.
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        private string CleanUpHtml(string htmlString)
        {
            int start = htmlString.IndexOf("<body>") + 6;
            int end = htmlString.IndexOf("</body>");

            return htmlString.Substring(start, (end - start)).Trim();
        }

        private void CleanEmailFolders()
        {
            _emailController.Clear(EmailFolder.Inbox);
            _emailController.Clear(EmailFolder.SentItems);
        }

        protected void OnEmailResponseReceived(string response)
        {
            if (EmailResponseReceived != null)
            {
                EmailResponseReceived(this, new StatusChangedEventArgs(response));
            }
        }
    }
}
