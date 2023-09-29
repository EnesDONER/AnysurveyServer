using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace WebAPI.Configurations
{
    public class CustomUserIdColumn: ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // HttpContextAccessor örneği üzerinden JWT'yi al
            var httpContextAccessor = new HttpContextAccessor();
            var userid = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userid))
            {
                var getValue = propertyFactory.CreateProperty("UserId", userid);
                logEvent.AddPropertyIfAbsent(getValue);
            }
        }
    }
}
