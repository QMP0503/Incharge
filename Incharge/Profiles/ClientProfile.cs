using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class ClientProfile:Profile //Automapper.Profile
    {
        public ClientProfile() 
        {
            CreateMap<Client, ClientVM>()
                .ReverseMap()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                }); ; //.ReverseMap() is availabe if two-way mapping is needed.
        }
    }
}
