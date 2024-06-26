using AutoMapper;
using System.Runtime.CompilerServices;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<Employee, EmployeeVM>()
                .ReverseMap()
                .ForAllMembers(opts =>
                {
                    opts.AllowNull();
                    opts.Condition((src, dest, srcMember) => srcMember != null);
                }); //check and test to make sure data is not messy when implemented
        }
    }
}
