using AutoMapper;
using ChallengeBP.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBP.Entities.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MetaViewModel, MetaDetalleViewModel>().ReverseMap(); 
        }
    }
}
