using System;
using HP.ScalableTest.Framework.Manifest;
using System.ComponentModel;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    [ObjectFactory(ExecutionMode.Scheduled)]
    internal class ScheduleBasedEngine : EngineBase
    {
        private readonly OfficeWorkerDetail _workerDetail = null;
        private readonly DateTime _baseTime = default(DateTime);
        private readonly EngineFlowControlMonitor _flowControlMonitor = new EngineFlowControlMonitor();
        VirtualResourceInstanceStatusLogger _virtualResourceStatusLogger;

        public ScheduleBasedEngine(OfficeWorkerDetail worker)
            : base(worker)
        {
            _workerDetail = worker;
            _baseTime = DateTime.Now;
            TraceFactory.Logger.Debug("Creating...");
        }

        public override void Run()
        {
            throw new NotImplementedException("We no longer implement a plain ScheduleBasedEngine");
        }

        /// <summary>
        /// Runs the Schedule of activities.
        /// </summary>
        public void Run(VirtualResourceInstanceStatusLogger logger)
        {
            var schedule = LegacySerializer.DeserializeXml<ExecutionSchedule>(_workerDetail.ExecutionSchedule);
            _virtualResourceStatusLogger = logger;

     
            if (schedule.SegmentCount > 0)
            {
                TraceFactory.Logger.Debug("Will process {0} segments in this scheduled execution.".FormatWith(schedule.SegmentCount));
            }
            else
            {
                TraceFactory.Logger.Debug("There are no scheduled items to execute, aborting...");
                Halt();
                return;
            }

            // Start the flow monitor to collect time on any pause events that will be used to adjust the total run time
            _flowControlMonitor.Start();

            // Set the base time in the scheduled execution object as the base time reference.
            schedule.BaseTime = _baseTime;
            TraceFactory.Logger.Debug("Base Time {0}".FormatWith(_baseTime));

            try
            {
                // Handle any startup delay that may occur because there is a staggered startup.
                var endTime = schedule.CalculateInitialEndTime();

                // Begin executing the schedule according to execution type (Repeat or Duration)
                int count = 0;
                if (schedule.UseDuration)
                {
                    TraceFactory.Logger.Debug("Duration execution starting...");

                    TimeSpan totalDuration = new TimeSpan(0, schedule.Duration, 0);

                    var scheduledExpiration = _baseTime.Add(totalDuration);
                    TraceFactory.Logger.Debug("Schedule Expiration at {0}".FormatWith(scheduledExpiration));

                    if (scheduledExpiration < endTime)
                    {
                        TraceFactory.Logger.Debug("Expiration earlier than initial end time, stopping at {0}".FormatWith(scheduledExpiration));

                        // The schedule expiration will hit before the first end time, so just delay
                        // for the expiration period and then return.  Adjust the expiration to account
                        // for any wall clock time lost from the base.
                        TimeSpan lostTime = DateTime.Now - _baseTime;
                        TimeSpan duration = totalDuration - lostTime;

                        _flowControlMonitor.Pause(() => !ExecutionHalted, duration, TimeSpan.FromSeconds(1));
                        return;
                    }
                    else if (endTime > _baseTime)
                    {
                        // Sleep for any startup time that is there because of a stagger setting
                        TimeSpan duration = endTime - DateTime.Now;

                        TraceFactory.Logger.Debug("Initial sleep until {0}".FormatWith(endTime));
                        _flowControlMonitor.Pause(() => !ExecutionHalted, duration, TimeSpan.FromSeconds(1));
                    }

                    // While the endtime is less than the schedule's expiration, execution the next
                    // schedule.  When endTime exceeds the schedules expiration, then we are done.
                    do
                    {
                        // Calculate a new end time based on the previous value and the current schedule iteration
                        TraceFactory.Logger.Debug("End Time {0}".FormatWith(endTime));
                        var scheduleSegment = schedule.CalculateNextEndTime(endTime, count++);

                        endTime = scheduleSegment.Item1;
                        var state = scheduleSegment.Item2;

                        TraceFactory.Logger.Debug("New End Time {0}, State {1}".FormatWith(endTime, state));

                        if (endTime > scheduledExpiration.Add(_flowControlMonitor.PauseTime))
                        {
                            endTime = scheduledExpiration.Add(_flowControlMonitor.PauseTime);
                            TraceFactory.Logger.Debug("End time later than Schedule Expiration");
                        }

                        RunSchedule(state, endTime);

                        // Include any pause time that has accumulated during the run to ensure the
                        // test runs the for the full duration.

                    } while (endTime < scheduledExpiration.Add(_flowControlMonitor.PauseTime));

                    TraceFactory.Logger.Debug("Duration execution complete");
                }
                else
                {
                    TraceFactory.Logger.Debug("Repeat Count execution starting...");

                    if (endTime > _baseTime)
                    {
                        // Sleep for any startup time that is there because of a stagger setting
                        TimeSpan duration = endTime - DateTime.Now;

                        TraceFactory.Logger.Debug("Initial sleep until {0}".FormatWith(endTime));
                        _flowControlMonitor.Pause(() => !ExecutionHalted, duration, TimeSpan.FromSeconds(1));
                    }

                    // Execute each segment until the total repeat count is met, which is the 
                    // number of segments x repeat count.
                    do
                    {
                        endTime = endTime + _flowControlMonitor.PauseTime;

                        var scheduleSegment = schedule.CalculateNextEndTime(endTime, count++);
                        endTime = scheduleSegment.Item1;
                        var state = scheduleSegment.Item2;
                        TraceFactory.Logger.Debug("State: " + state + " , Endtime: " + endTime);
                        RunSchedule(state, endTime);
                        TraceFactory.Logger.Debug(count + " of " + schedule.TotalIterations);
                    } while (count < schedule.TotalIterations);

                    TraceFactory.Logger.Debug("Repeat Count execution complete");
                }
            }
            catch (WorkerHaltedException ex)
            {
                TraceFactory.Logger.Debug(ex.Message);
                return;
            }
        }

        private void RunSchedule(WorkerExecutionState state, DateTime endTime)
        {
            TimeSpan duration = endTime - DateTime.Now;

            if (state == WorkerExecutionState.Idle)
            {
                TraceFactory.Logger.Debug("Idle wait for {0} mins".FormatWith(duration.TotalMinutes));
                TraceFactory.Logger.Debug("Paused for {0} mins".FormatWith(_flowControlMonitor.PauseTime.TotalMinutes));

                _virtualResourceStatusLogger.Update(_virtualResourceStatusLogger.Index + 1, Enum.GetName(typeof(Runtime.RuntimeState), 6), false, "EngineIdle");

                ExecutionServices.DataLogger.AsInternal().Submit(_virtualResourceStatusLogger);
                _flowControlMonitor.Pause(() => !ExecutionHalted, duration, TimeSpan.FromSeconds(1));
            }
            else
            {
                _virtualResourceStatusLogger.Update(_virtualResourceStatusLogger.Index + 1, Enum.GetName(typeof(Runtime.RuntimeState), 6), true, "EngineActive");

                ExecutionServices.DataLogger.AsInternal().Submit(_virtualResourceStatusLogger);

                new DurationBasedEngine(_workerDetail, ActivityQueue, PacingInfo, (int)duration.TotalMinutes).Run();

            }
            
        }
    }
}
