using NorenRestApiWrapper;
using sansidalgo.core.Helpers.Interfaces;

namespace sansidalgo.core.helpers
{
    public class BaseResponseHandler: IBaseResponseHandler
    {
        public AutoResetEvent ResponseEvent = new AutoResetEvent(false);

        public NorenResponseMsg? baseResponse;

        public void OnResponse(NorenResponseMsg Response, bool ok)
        {
            baseResponse = Response;

            ResponseEvent.Set();
        }
    }
}
