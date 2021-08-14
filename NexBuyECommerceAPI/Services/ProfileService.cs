using Microsoft.AspNetCore.Identity;
using NexBuyECommerceAPI.DataContext;
using NexBuyECommerceAPI.Entities;
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

        public Task<IdentityUser> ChangeUserRole(MockViewModel updateUserRoleViewModel)
        {
            throw new NotImplementedException();
        }

        public void EditProfile(IdentityUser user, Cashier Cashier = null, StoreManager storeManager = null)
        {
            throw new NotImplementedException();
        }

        public List<IdentityUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task RemoveUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> ValidateUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
