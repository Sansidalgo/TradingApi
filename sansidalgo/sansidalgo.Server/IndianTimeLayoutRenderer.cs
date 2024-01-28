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
            var indianTime = TimeZoneInfo.ConvertTimeFromUtc(logEvent.TimeStamp.ToUniversalTime(), indianTimeZone);
            builder.Append(indianTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
        }
    }
}
