using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;
using System.Runtime.CompilerServices;

namespace Incharge.Profiles
{
    public class SaleProfile:Profile
    {
        public SaleProfile()
        {
            CreateMap<Sale, SaleVM>()
               .ReverseMap()
               .ForAllMembers(opts =>
               {
                   opts.AllowNull();
                   opts.Condition((src, dest, srcMember) => srcMember != null);
               });
        }
    }
}
