using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductVM>().ReverseMap().ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            }); 
        }
    }
}
