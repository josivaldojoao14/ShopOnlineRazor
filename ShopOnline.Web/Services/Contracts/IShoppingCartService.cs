using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts
{
  public interface IShoppingCartService
  {
    Task<List<CartItemDto>> GetCartItems(int userId);
    Task<CartItemDto> AddCartItem(CartItemToAddDto cartItemToAddDto);
    Task<CartItemDto> DeleteCartItem(int id);
    Task<CartItemDto> UpdateItemQty(CartItemQtyUpdateDto cartItemQtyUpdateDto);

    event Action<int> OnShoppingCartChanged;
    void RaiseEventOnShoppingCartChanged(int totalQty);
  }
}
