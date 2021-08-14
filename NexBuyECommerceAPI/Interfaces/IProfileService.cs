using Microsoft.AspNetCore.Identity;
using NexBuyECommerceAPI.Entities;
using NexBuyECommerceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IProfileService
    {

        void EditProfile(IdentityUser user, Cashier Cashier = null, StoreManager storeManager = null);
        List<IdentityUser> GetAllUsers();

        Task<IdentityUser> ValidateUser(string userId);

        Task<IdentityUser> ChangeUserRole(MockViewModel updateUserRoleViewModel);

        Task RemoveUser(string userId);

    }
}
