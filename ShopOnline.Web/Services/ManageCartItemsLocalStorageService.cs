using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services
{
  public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
  {
    private readonly ILocalStorageService _localStorageService;
    private readonly ShoppingCartService _shoppingCartService;

    private const string key = "CartItemCollection";

    public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService,
                                              ShoppingCartService shoppingCartService)
    {
      _localStorageService = localStorageService;
      _shoppingCartService = shoppingCartService;
    }

    public async Task<List<CartItemDto>> GetCollection()
    {
      return await _localStorageService.GetItemAsync<List<CartItemDto>>(key)
          ?? await AddCollection();
    }

    public async Task RemoveCollection()
    {
      await _localStorageService.RemoveItemAsync(key);
    }

    public async Task SaveCollection(List<CartItemDto> cartItemDtos)
    {
      await _localStorageService.SetItemAsync(key, cartItemDtos);
    }

    private async Task<List<CartItemDto>> AddCollection()
    {
      var shoppingCartCollection = await _shoppingCartService.GetCartItems(HardCoded.UserId);

      if (shoppingCartCollection is not null)
      {
        await _localStorageService.SetItemAsync(key, shoppingCartCollection);
      }

      return shoppingCartCollection;
    }
  }
}
