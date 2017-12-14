using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using iHentai.Activation;
using iHentai.BackgroundTasks;
using iHentai.Basic.Extensions;

namespace iHentai.Services
{
    internal class BackgroundTaskService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        private static readonly Lazy<IEnumerable<BackgroundTask>> BackgroundTaskInstances =
            new Lazy<IEnumerable<BackgroundTask>>(CreateInstances);

        public static IEnumerable<BackgroundTask> BackgroundTasks => BackgroundTaskInstances.Value;

        public void RegisterBackgroundTasks()
        {
            foreach (var task in BackgroundTasks)
                task.Register();
        }

        public static BackgroundTaskRegistration GetBackgroundTasksRegistration<T>()
            where T : BackgroundTask
        {
            if (BackgroundTaskRegistration.AllTasks.All(t => t.Value.Name != typeof(T).Name))
                return null;

            return (BackgroundTaskRegistration) BackgroundTaskRegistration.AllTasks
                .FirstOrDefault(t => t.Value.Name == typeof(T).Name).Value;
        }

        public void Start(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance?.Task?.Name))?.RunAsync(taskInstance)
                .FireAndForget();
        }

        protected override async Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            Start(args.TaskInstance);

            await Task.CompletedTask;
        }

        private static IEnumerable<BackgroundTask> CreateInstances()
        {
            yield return new NotifyTask();
        }
    }
}