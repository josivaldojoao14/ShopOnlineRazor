using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
  public class ProductDetailsBase : ComponentBase
  {
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    [Inject]
    public IManageProductsLocalStorageService ManageProductsLocalStorage { get; set; }

    [Inject]
    public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public ProductDto Product { get; set; }
    public string ErrorMessage { get; set; }
    private List<CartItemDto> ShoppingCartItems { get; set; }

    protected async override Task OnInitializedAsync()
    {
      try
      {
        ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
        Product = await GetProductById(Id);
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
      }
    }

    protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
    {
      try
      {
        var cartItem = await ShoppingCartService.AddCartItem(cartItemToAddDto);

        if (cartItem is not null)
        {
          ShoppingCartItems.Add(cartItem);
          await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        NavigationManager.NavigateTo("/ShoppingCart");
      }
      catch (Exception)
      {
        throw;
      }
    }

    private async Task<ProductDto> GetProductById(int productId)
    {
      var productDtos = await ManageProductsLocalStorage.GetCollection();

      if (productDtos is not null)
      {
        return productDtos.SingleOrDefault(p => p.Id == productId);
      }

      return null;
    }
  }
}
