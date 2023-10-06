using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
  public class ShoppingCartRepository : IShoppingCartRepository
  {
    private readonly ShopOnlineDataDbContext _dbContext;

    public ShoppingCartRepository(ShopOnlineDataDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    private async Task<bool> CartItemExists(int cartId, int productId)
    {
      return await _dbContext.CartItems.AnyAsync(c => c.CartId == cartId
                                                 && c.ProductId == productId);
    }

    public async Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto)
    {
      if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
      {
        var item = await (from product in _dbContext.Products
                          where product.Id == cartItemToAddDto.ProductId
                          select new CartItem
                          {
                            CartId = cartItemToAddDto.CartId,
                            ProductId = product.Id,
                            Qty = cartItemToAddDto.Qty,
                          }).SingleOrDefaultAsync();

        if (item is not null)
        {
          var result = await _dbContext.CartItems.AddAsync(item);
          await _dbContext.SaveChangesAsync();
          return result.Entity;
        }
      }
      return null;
    }

    public async Task<CartItem> DeleteItem(int id)
    {
      var item = await _dbContext.CartItems.FindAsync(id);
      if (item is not null)
      {
        _dbContext.CartItems.Remove(item);
        await _dbContext.SaveChangesAsync();
      }

      return item;
    }

    public async Task<CartItem?> GetItem(int id)
    {
      return await (from cart in _dbContext.Carts
                    join cartItem in _dbContext.CartItems
                    on cart.Id equals cartItem.CartId
                    where cartItem.Id == id
                    select new CartItem
                    {
                      Id = cartItem.Id,
                      ProductId = cartItem.ProductId,
                      Qty = cartItem.Qty,
                      CartId = cartItem.CartId
                    }).SingleOrDefaultAsync();

    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
      return await (from cart in _dbContext.Carts
                    join cartItem in _dbContext.CartItems
                    on cart.Id equals cartItem.CartId
                    where cart.UserId == userId
                    select new CartItem
                    {
                      Id = cartItem.Id,
                      ProductId = cartItem.ProductId,
                      Qty = cartItem.Qty,
                      CartId = cartItem.CartId
                    }).ToListAsync();
    }

    public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
      var item = await _dbContext.CartItems.FindAsync(id);

      if (item is not null)
      {
        item.Qty = cartItemQtyUpdateDto.Qty;
        await _dbContext.SaveChangesAsync();
        return item;
      }

      return null;
    }
  }
}
