using AutoMapper;
using System.Runtime.CompilerServices;
using Incharge.Models;
using Incharge.DTO;

namespace Incharge.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile() 
        { 
            CreateMap<Employee, EmployeeDTO>().ReverseMap(); //.ReverseMap() is availabe if two-way mapping is needed>
        }
    }
}
