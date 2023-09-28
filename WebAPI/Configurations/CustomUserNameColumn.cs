using Serilog.Core;
using Serilog.Events;

namespace WebAPI.Configurations
{
    public class CustomUserNameColumn : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // HttpContextAccessor örneği üzerinden JWT'yi al
            var httpContextAccessor = new HttpContextAccessor();
            var username = httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(username))
            {
                var getValue = propertyFactory.CreateProperty("UserName", username);
                logEvent.AddPropertyIfAbsent(getValue);
            }
        }
    }
}
