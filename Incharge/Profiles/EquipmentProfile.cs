using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class EquipmentProfile:Profile
    {
        public EquipmentProfile() 
        {
            CreateMap<Equipment, EquipmentVM>().ReverseMap(); 
        }
    }
}
