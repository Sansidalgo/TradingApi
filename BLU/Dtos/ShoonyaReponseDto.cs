using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class ShoonyaReponseDto
    {
        public BaseResponseHandler BaseResponseHandler { get; set; }
        public NorenRestApi NorenRestApi { get; set; }
    }
}
