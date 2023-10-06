using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
  public class ProductsByCategoryBase : ComponentBase
  {
    [Parameter]
    public int CategoryId { get; set; }

    [Inject]
    public IProductService ProductService { get; set; }

    [Inject]
    public IManageProductsLocalStorageService ManageProductsLocalStorage { get; set; }

    public IEnumerable<ProductDto> Products { get; set; }
    public string CategoryName { get; set; }

    public string ErrorMessage { get; set; }

    protected override async Task OnParametersSetAsync()
    {
      try
      {
        Products = await GetProductCollectionByCategoryId(CategoryId);

        if (Products is not null && Products.Count() > 0)
        {
          var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);
          if (productDto is not null)
          {
            CategoryName = productDto.CategoryName;
          }
        }
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
      }
    }

    private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
    {
      var productCollection = await ManageProductsLocalStorage.GetCollection();

      if (productCollection is not null)
      {
        return productCollection.Where(p => p.CategoryId == categoryId).ToList();
      }
      else
      {
        return await ProductService.GetProductsByCategory(categoryId);
      }
    }
  }
}
