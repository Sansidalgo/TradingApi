using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class DropDownsResponseDto
    {
        public List<TblBroker> Borkers { get; set; }
        public List<TblShoonyaCredential> Credentials { get; set; }
        public List<TblOptionsSetting> OptionsSettings { get; set; }
    }
}
