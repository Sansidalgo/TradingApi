using NorenRestApiWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sansidalgo.core.Helpers.Interfaces
{
    internal interface IBaseResponseHandler
    {
        public void OnResponse(NorenResponseMsg Response, bool ok);
    }
}
