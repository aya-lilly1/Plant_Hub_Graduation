using AutoMapper;
using Plant_Hub_Models.Models;
using Plant_Hub_ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant_Hub_Core.Mapper
{
    public class  Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<SignupUser, ApplicationUser>().ReverseMap();
            CreateMap<LoginModelView, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, LoginResponse>().ReverseMap();


        }
    }
}
