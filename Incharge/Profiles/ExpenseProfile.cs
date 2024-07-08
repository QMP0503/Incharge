
using AutoMapper;
using Incharge.Models;
using Incharge.ViewModels;

namespace Incharge.Profiles
{
    public class ExpenseProfile:Profile
    {
        public ExpenseProfile() 
        {
            CreateMap<Expense, ExpenseVM>()
               .ReverseMap()
               .ForAllMembers(opts =>
               {
                   opts.AllowNull();
                   opts.Condition((src, dest, srcMember) => srcMember != null);
               }); //.ReverseMap() is availabe if two-way mapping is needed.
        }
    }
}
