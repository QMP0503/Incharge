using AutoMapper;
using Incharge.ViewModels;
using Incharge.Models;

namespace Incharge.Profiles
{
    public class DiscountProfile:Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountVM>().ReverseMap()
               .ForAllMembers(opts =>
               {
                   opts.AllowNull();
                   opts.Condition((src, dest, srcMember) => srcMember != null);
               });
        }
    }
}
