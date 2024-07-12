using Incharge.Data;
using Microsoft.EntityFrameworkCore;
using Incharge.Models;


namespace MyMovies.Models;

public static class SeedData
{

    public static void Initialize(IServiceProvider serviceProvider)
    {
        /*
        * Seed for :
        * - EMPLOYEE TYPE
        * - PRODUCT TYPE
        * - DISCOUNT
        */

        using (var context = new InchargeContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<InchargeContext>>()))
        {
            if (context.EmployeeTypes.Any() || context.Producttypes.Any() || context.Discounts.Any())
            {
                return;   // DB has been seeded
            }
            context.EmployeeTypes.AddRange(
                new EmployeeType
                {
                    Type = "Trainer",
                    Salary = 40000
                },
                new EmployeeType
                {
                   Type = "Manager",
                   Salary = 50000
                },
                new EmployeeType
                {
                    Type = "Sales",
                    Salary = 30000
                }
            );

            context.Producttypes.AddRange(
                new Producttype
                {
                    Id = 1,
                    Name = "Membership",
                    Price = 40
                },
                new Producttype
                {
                    Id = 2,
                    Name = "Private Training",
                    Price = 20 //perhour
                }
            );

            context.Discounts.AddRange(
                new Discount
                {
                    Id = 1,
                    Name = "Birthday",
                    DiscountValue = 0.1
                },
                new Discount
                {
                    Id = 2,
                    Name = "Christmas",
                    DiscountValue = 0.2
                }
            );
            
            context.SaveChanges();
            
            return;


        }
    }
}