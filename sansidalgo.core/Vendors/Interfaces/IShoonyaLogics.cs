using sansidalgo.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sansidalgo.core.Vendors.Interfaces
{
    public interface IShoonyaLogics
    {
        Task<string> PostShoonyaOrder(Order order);
        IEnumerable<WeatherForecast> GetOrders();


    }
}
