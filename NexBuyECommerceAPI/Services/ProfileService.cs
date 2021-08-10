using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationContext _dbContext;

        public ProfileService()
        {

        }

        public Task<ApplicationUser> ChangeUserRole(MockViewModel updateUserRoleViewModel)
        {
            throw new NotImplementedException();
        }

        public void EditProfile(ApplicationUser user, Cashier Cashier = null, StoreManager storeManager = null)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task RemoveUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> ValidateUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
