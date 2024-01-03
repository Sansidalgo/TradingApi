using NLog.LayoutRenderers;
using NLog;
using System.Text;

namespace sansidalgo.Server
{
    [LayoutRenderer("indianTime")]
    public class IndianTimeLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var indianTime = TimeZoneInfo.ConvertTime(logEvent.TimeStamp, TimeZoneInfo.Utc, indianTimeZone);
            builder.Append(indianTime.ToString("yyyy-MM-dd HH:mm:ss.fff zzz", System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
