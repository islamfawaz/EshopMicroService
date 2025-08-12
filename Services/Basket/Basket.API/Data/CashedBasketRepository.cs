using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CashedBasketRepository(IBasketRepository repository,IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
           
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(cachedBasket))
            {
                var basketFromCache = JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
                if (basketFromCache is not null)
                    return basketFromCache;
            }
               
        
            var basket = await repository.GetBasketAsync(userName, cancellationToken);

             await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);            
          
            return basket;
        }
        


        public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasketAsync(cart, cancellationToken);
            await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }
        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasketAsync(userName, cancellationToken); 
            await cache.RemoveAsync(userName,cancellationToken);
            return true;
        }

       

    }
}
