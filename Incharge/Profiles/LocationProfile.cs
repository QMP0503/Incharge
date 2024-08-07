﻿using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class LocationProfile:Profile
    {
        public LocationProfile() 
        {
            CreateMap<Location, LocationVM>().ReverseMap().ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            }); ; 
        }
    }
}
