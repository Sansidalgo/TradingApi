using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Enums
{
    public class DbStatus
    {
        public int Status { get; set; }
        public object? Result {  get; set; }
        public string? Message { get; set; }
        public string GetStatus(Exception ex)
        {

            if (!string.IsNullOrWhiteSpace(ex.InnerException?.Message))
            {
                Message = ex.InnerException.Message;
            }
            else
            {
                Message = ex.Message;
            }
            return Message;
        }

    }
   
}
