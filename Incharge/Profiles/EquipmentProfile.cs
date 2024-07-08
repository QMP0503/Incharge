using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class EquipmentProfile:Profile
    {
        public EquipmentProfile() 
        {
            CreateMap<Equipment, EquipmentVM>().ReverseMap()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                }); ; 
        }
    }
}
