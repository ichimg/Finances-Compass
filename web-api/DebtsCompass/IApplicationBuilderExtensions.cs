using DebtsCompass.Domain.Interfaces;
using Hangfire;

namespace DebtsCompass
{
    public static class HangfireWorker
    {
        public static void StartRecurringJobs(this IApplicationBuilder app)
        {
            RecurringJob.AddOrUpdate<ICurrencyRatesJob>("GetLatestCurrencyRates", job => job.GetLatestCurrencyRates(),
                     "0 21 * * *");
        }
    }
}
