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
            //if (context.EmployeeTypes.Any() || context.Producttypes.Any() || context.Discounts.Any())
            //{
            //    return;   // DB has been seeded
            //}
            //context.EmployeeTypes.AddRange(
            //    new EmployeeType
            //    {
            //        Type = "Trainer",
            //        Salary = 40000
            //    },
            //    new EmployeeType
            //    {
            //       Type = "Manager",
            //       Salary = 50000
            //    },
            //    new EmployeeType
            //    {
            //        Type = "Sales",
            //        Salary = 30000
            //    }
            //);

            //context.Producttypes.AddRange(
            //    new Producttype
            //    {
            //        Id = 1,
            //        Name = "Membership",
            //        Price = 40
            //    },
            //    new Producttype
            //    {
            //        Id = 2,
            //        Name = "Private Training",
            //        Price = 20 //perhour
            //    }
            //);

            //context.Discounts.AddRange(
            //    new Discount
            //    {
            //        Id = 1,
            //        Name = "Birthday",
            //        DiscountValue = 0.1
            //    },
            //    new Discount
            //    {
            //        Id = 2,
            //        Name = "Christmas",
            //        DiscountValue = 0.2
            //    }
            //);
            //if (context.Clients.Any())
            //{
            //    return;
            //}

            //context.Clients.AddRange(
            //    new Client { FirstName = "Emily", LastName = "Carter", Phone = 5551234567, Email = "emily.carter@example.com", Address = "123 Maple St, Springfield, IL 62701", Status = "Signed In" },
            //    new Client { FirstName = "James", LastName = "Robinson", Phone = 5552345678, Email = "james.robinson@example.com", Address = "456 Oak Ave, Madison, WI 53703", Status = "Signed In" },
            //    new Client { FirstName = "Olivia", LastName = "Smith", Phone = 5553456789, Email = "olivia.smith@example.com", Address = "789 Pine Dr, Austin, TX 73301", Status = "Signed In" },
            //    new Client { FirstName = "Michael", LastName = "Johnson", Phone = 5554567890, Email = "michael.johnson@example.com", Address = "321 Birch Rd, Seattle, WA 98101", Status = "Signed In" },
            //    new Client { FirstName = "Ava", LastName = "Brown", Phone = 5555678901, Email = "ava.brown@example.com", Address = "654 Cedar Ln, Portland, OR 97201", Status = "Signed In" },
            //    new Client { FirstName = "Daniel", LastName = "Wilson", Phone = 5556789012, Email = "daniel.wilson@example.com", Address = "987 Elm St, Denver, CO 80201", Status = "Signed In" },
            //    new Client { FirstName = "Sophia", LastName = "Martinez", Phone = 5557890123, Email = "sophia.martinez@example.com", Address = "135 Walnut Blvd, Miami, FL 33101", Status = "Signed In" },
            //    new Client { FirstName = "Matthew", LastName = "Anderson", Phone = 5558901234, Email = "matthew.anderson@example.com", Address = "246 Spruce St, Atlanta, GA 30301", Status = "Signed In" },
            //    new Client { FirstName = "Isabella", LastName = "Thomas", Phone = 5559012345, Email = "isabella.thomas@example.com", Address = "369 Fir Ave, Boston, MA 02101", Status = "Signed In" },
            //    new Client { FirstName = "Ethan", LastName = "Harris", Phone = 5550123456, Email = "ethan.harris@example.com", Address = "147 Willow Ct, Chicago, IL 60601", Status = "Signed In" }
            //);

            //if (context.Employees.Any())
            //{
            //    return;
            //}

            //context.Employees.AddRange(
            //    new Employee { FirstName = "Liam", LastName = "Walker", Phone = 5552345678, Email = "liam.walker@example.com", Address = "124 Oak St, Springfield, IL 62701", RoleId = 1 },
            //    new Employee { FirstName = "Sophia", LastName = "Adams", Phone = 5553456789, Email = "sophia.adams@example.com", Address = "567 Maple Ave, Madison, WI 53703", RoleId = 2 },
            //    new Employee { FirstName = "Mason", LastName = "Brooks", Phone = 5554567890, Email = "mason.brooks@example.com", Address = "890 Pine Dr, Austin, TX 73301", RoleId = 3 },
            //    new Employee { FirstName = "Isabella", LastName = "Morris", Phone = 5555678901, Email = "isabella.morris@example.com", Address = "321 Cedar Rd, Seattle, WA 98101", RoleId = 1 },
            //    new Employee { FirstName = "Ethan", LastName = "Turner", Phone = 5556789012, Email = "ethan.turner@example.com", Address = "654 Birch Ln, Portland, OR 97201", RoleId = 2 },
            //    new Employee { FirstName = "Ava", LastName = "Roberts", Phone = 5557890123, Email = "ava.roberts@example.com", Address = "987 Elm Ct, Denver, CO 80201", RoleId = 3 },
            //    new Employee { FirstName = "Noah", LastName = "Parker", Phone = 5558901234, Email = "noah.parker@example.com", Address = "135 Walnut Blvd, Miami, FL 33101", RoleId = 1 },
            //    new Employee { FirstName = "Olivia", LastName = "Bennett", Phone = 5559012345, Email = "olivia.bennett@example.com", Address = "246 Spruce St, Atlanta, GA 30301", RoleId = 2 },
            //    new Employee { FirstName = "Lucas", LastName = "Foster", Phone = 5550123456, Email = "lucas.foster@example.com", Address = "369 Fir Ave, Boston, MA 02101", RoleId = 3 },
            //    new Employee { FirstName = "Mia", LastName = "Gonzalez", Phone = 5551234567, Email = "mia.gonzalez@example.com", Address = "147 Willow Ct, Chicago, IL 60601", RoleId = 1 }
            //);

            //context.SaveChanges();
            

            //return;


        }
    }
}