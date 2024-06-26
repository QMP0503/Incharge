using AutoMapper;
using Incharge.Models;
using Incharge.DTO;

namespace Incharge.Profiles
{
    public class ClientProfile:Profile //Automapper.Profile
    {
        public ClientProfile() 
        {
            CreateMap<Client, ClientDTO>(); //.ReverseMap() is availabe if two-way mapping is needed.
        }
    }
}
