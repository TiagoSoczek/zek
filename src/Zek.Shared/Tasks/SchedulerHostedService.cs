using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zek.Shared.Tasks.Cron;
using Microsoft.Extensions.Logging;

namespace Zek.Shared.Tasks
{
    public class SchedulerHostedService : BaseHostedService
    {
        private readonly ILogger<SchedulerHostedService> logger;
        private readonly List<SchedulerTaskWrapper> scheduledTasks = new List<SchedulerTaskWrapper>();

        public SchedulerHostedService(IEnumerable<IScheduledTask> tasks, ILogger<SchedulerHostedService> logger)
        {
            this.logger = logger;

            InitTasks(tasks);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogTrace("Starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken).ConfigureAwait(false);

                logger.LogTrace("Waiting for next execution");
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken).ConfigureAwait(false);
            }

            logger.LogTrace("Stopping, cancellation token requested");
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var referenceTime = DateTime.UtcNow;

            var tasksThatShouldRun = scheduledTasks.Where(t => t.ShouldRun(referenceTime)).ToList();

            logger.LogTrace("TasksThatShouldRun: {tasksCount}", tasksThatShouldRun.Count);

            foreach (var scheduledTask in tasksThatShouldRun)
            {
                scheduledTask.Increment();

                await Task.Run(
                    async () =>
                    {
                        var task = scheduledTask.Task;

                        try
                        {
                            logger.LogTrace("Executing task {taskDescription}", task.Description);

                            await task.ExecuteAsync(cancellationToken).ConfigureAwait(false);

                            logger.LogTrace("Task executed without errors");
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Fail to execute task");
                        }
                        finally
                        {
                            logger.LogTrace("Task {taskDescription} next run time: {nextRunTime}", task.Description, scheduledTask.NextRunTime);
                        }
                    },
                    cancellationToken).ConfigureAwait(false);
            }
        }

        private class SchedulerTaskWrapper
        {
            public CrontabSchedule Schedule { get; set; }
            public IScheduledTask Task { get; set; }

            public DateTime LastRunTime { get; set; }
            public DateTime NextRunTime { get; set; }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = Schedule.GetNextOccurrence(NextRunTime);
            }

            public bool ShouldRun(DateTime currentTime)
            {
                return NextRunTime < currentTime && LastRunTime != NextRunTime;
            }
        }

        private void InitTasks(IEnumerable<IScheduledTask> tasks)
        {
            var referenceTime = DateTime.UtcNow;

            logger.LogTrace("Scheduling Tasks: {referenceTime}", referenceTime);

            foreach (var task in tasks)
            {
                var schedule = CrontabSchedule.Parse(task.Schedule);

                var schedulerTaskWrapper = new SchedulerTaskWrapper
                {
                    Schedule = schedule,
                    Task = task,
                    NextRunTime = schedule.GetNextOccurrence(referenceTime)
                };

                logger.LogTrace("Task Scheduled: {taskDescription}, Schedule: {schedule}, NextRun: {nextRunTime}",
                    task.Description, schedule.ToString(), schedulerTaskWrapper.NextRunTime);

                scheduledTasks.Add(schedulerTaskWrapper);
            }
        }
    }
}