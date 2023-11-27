using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sansidalgo.core.Helpers
{
    public class OtpEntity
    {
        public int RemaingTime { get; set; }
        public string? OTP { get; set; }
        public OtpEntity(int _remaingTime,string otp) {
            RemaingTime = _remaingTime;
            OTP = otp;
        }
    }
}
