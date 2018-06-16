namespace Zek.Shared.Tasks.Cron
{
    public static class CronExpressions
    {
        public const string EveryMinute = "* * * * *";
        public const string Every5Minutes = "*/5 * * * *";
        public const string At18EveryDay = "0 18 * * *";
        public const string At12EveryDay = "0 12 * * *";
        public const string At12EveryMonday = "0 12 * * 1";
        public const string EveryHour = "0 * * * *";
    }
}