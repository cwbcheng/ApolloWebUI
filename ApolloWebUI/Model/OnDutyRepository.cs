using ApolloWebUI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class OnDutyRepository : IOnDutyRepository, IDisposable
    {
        private AppDbContext _onDutyContent;

        public OnDutyRepository(AppDbContext onDuty)
        {
            _onDutyContent = onDuty;
        }

        public async Task AddProductAsync(Product product)
        {
            _onDutyContent.Products.Add(product);
            await _onDutyContent.SaveChangesAsync();
        }

        public async Task AddUserAsync(ApplicationUser user)
        {
            _onDutyContent.Users.Add(user);
            await _onDutyContent.SaveChangesAsync();
        }

        public async Task AddUserProductAsync(UserProduct userProduct)
        {
            _onDutyContent.Add(userProduct);
            await _onDutyContent.SaveChangesAsync();
        }

        public void Dispose()
        {
            _onDutyContent?.Dispose();
        }

        public Task<Product> FindProductAsync(string id)
        {
            return _onDutyContent.Products.SingleOrDefaultAsync(o => o.Id == id);
        }

        public Task<ApplicationUser> FindUserAsync(string id)
        {
            return _onDutyContent.Users.SingleOrDefaultAsync(o => o.Id == id);
        }

        public Task<UserProduct> FindUserProductAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProdcutAsync()
        {
            return await _onDutyContent.Products.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _onDutyContent.Users.ToListAsync();
        }

        public Task InitAsync()
        {
            return Task.FromResult<object>(null);
        }

        public async Task<Product> RemoveProductAsync(string id)
        {
            var product = await _onDutyContent.Products.SingleOrDefaultAsync(o => o.Id == id);
            if (product == null)
            {
                return null;
            }

            _onDutyContent.Products.Remove(product);
            await _onDutyContent.SaveChangesAsync();
            return product;
        }

        public async Task<ApplicationUser> RemoveUserAysnc(string id)
        {
            var user = await _onDutyContent.Users.SingleOrDefaultAsync(o => o.Id == id);
            if (user == null)
            {
                return null;
            }

            _onDutyContent.Remove(user);
            await _onDutyContent.SaveChangesAsync();
            return user;
        }

        public async Task RemoveUserProductAsync(UserProduct userProduct)
        {
            _onDutyContent.Remove(userProduct);
            await _onDutyContent.SaveChangesAsync();
        }

        public async Task UpateProductAsync(Product product)
        {
            _onDutyContent.Products.Update(product);
            await _onDutyContent.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _onDutyContent.Users.Update(user);
            await _onDutyContent.SaveChangesAsync();
        }
    }
}
