using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.Interfaces
{
    public interface IProfileService
    {

        void EditProfile(ApplicationUser user, Cashier Cashier = null, StoreManager storeManager = null);
        List<ApplicationUser> GetAllUsers();

        Task<ApplicationUser> ValidateUser(string userId);

        Task<ApplicationUser> ChangeUserRole(MockViewModel updateUserRoleViewModel);

        Task RemoveUser(string userId);

    }
}
