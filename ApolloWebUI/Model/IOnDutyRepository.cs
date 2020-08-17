using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public interface IOnDutyRepository
    {
        Task InitAsync();
        Task AddUserAsync(ApplicationUser user);
        Task AddProductAsync(Product product);
        Task<ApplicationUser> RemoveUserAysnc(string id);
        Task<Product> RemoveProductAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<Product>> GetAllProdcutAsync();
        Task<ApplicationUser> FindUserAsync(string id);
        Task<Product> FindProductAsync(string id);
        Task UpdateUserAsync(ApplicationUser user);
        Task UpateProductAsync(Product product);

        Task AddUserProductAsync(UserProduct userProduct);

        Task RemoveUserProductAsync(UserProduct userProduct);
        Task<UserProduct> FindUserProductAsync(string userId);
    }
}
