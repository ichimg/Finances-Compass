using Hangfire.Dashboard;


namespace DebtsCompass.Application.HangfireAuth
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
