using AutoMapper;
using BLU.Dtos;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OptionsSettingRequestDto, TblOptionsSetting>();
            CreateMap<ShoonyaCredentialRequestDto, TblShoonyaCredential>();

            // Add more mappings if needed
        }
    }
}
