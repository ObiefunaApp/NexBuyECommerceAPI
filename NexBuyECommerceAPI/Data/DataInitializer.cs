using Microsoft.AspNetCore.Identity;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Data
{
    public static class DataInitializer
    {


        public static void Seed(UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            SeedAdmin(_userManager, _roleManager);
          //  SeedCategory();
            SeedRoles(_roleManager);
        }

        public static void SeedAdmin(UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {

            if (_userManager.FindByEmailAsync("Admin@admin.com").Result == null)
            {
                var userManager = _userManager;
                var roleManager = _roleManager;
                var user = new IdentityUser()
                {
                    UserName = "Admin@admin.com",
                    Email = "Admin@admin.com",
                    EmailConfirmed = true
                  
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin1234_").Result;

                {
                    if (result.Succeeded)
                    {
                        var admin = new IdentityRole("Admin");
                        IdentityResult roleResult = roleManager.CreateAsync(admin).Result;

                        {
                            if (roleResult.Succeeded)
                            {
                                userManager.AddToRoleAsync(user, "Admin");
                            }
                        }
                    }
                }

            }
        }
        public static void SeedRoles(RoleManager<IdentityRole> _roleManager)
        {
            if (!_roleManager.RoleExistsAsync("Cashier").Result || !_roleManager.RoleExistsAsync("StoreManager").Result)
            {
                var roleManager = _roleManager;

                var cashier = new IdentityRole("Cashier");
                var storeManager = new IdentityRole("StoreManager");

                roleManager.CreateAsync(cashier);
                roleManager.CreateAsync(storeManager);
            }
        }
        //public static void SeedCategory()
        //{
        //    ApplicationContext context = new ApplicationContext();

        //    var ProductCategory = new List<ProductCategory>
        //    {
        //        new ProductCategory() { CategoryName = "Perishable" },
        //        new ProductCategory() { CategoryName = "Kitchen" },
        //        new ProductCategory() { CategoryName = "Furniture" },
        //        new ProductCategory() { CategoryName = "Electronic" },
        //        new ProductCategory() { CategoryName = "Stationary" },
        //        new ProductCategory() { CategoryName = "Others" }
        //    };


        //    context.ProductCategories.AddRange(ProductCategory);
        //    context.SaveChanges();
        //}

    }


}

