using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ShopOnline.Web.Services
{
  public class ShoppingCartService : IShoppingCartService
  {
    private readonly HttpClient _httpClient;
    public event Action<int> OnShoppingCartChanged;

    public ShoppingCartService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<CartItemDto> AddCartItem(CartItemToAddDto cartItemToAddDto)
    {
      try
      {
        var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
        if (response.IsSuccessStatusCode)
        {
          if (response.StatusCode is HttpStatusCode.NoContent)
          {
            return default(CartItemDto);
          }

          return await response.Content.ReadFromJsonAsync<CartItemDto>();
        }
        else
        {
          var message = await response.Content.ReadAsStringAsync();
          throw new Exception($"Http status code: {response.StatusCode} - Message: {message}");
        }
      }
      catch (Exception)
      {

        throw;
      }
    }

    public async Task<CartItemDto> DeleteCartItem(int id)
    {
      try
      {
        var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");
        if (response.IsSuccessStatusCode)
        {
          return await response.Content.ReadFromJsonAsync<CartItemDto>();
        }

        return default(CartItemDto);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<List<CartItemDto>> GetCartItems(int userId)
    {
      var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
      if (response.IsSuccessStatusCode)
      {
        if (response.StatusCode is HttpStatusCode.NoContent)
        {
          return Enumerable.Empty<CartItemDto>().ToList();
        }

        return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
      }
      else
      {
        var message = await response.Content.ReadAsStringAsync();
        throw new Exception($"Http status code: {response.StatusCode} - Message: {message}");
      }
    }

    public void RaiseEventOnShoppingCartChanged(int totalQty)
    {
      if (OnShoppingCartChanged is not null)
      {
        OnShoppingCartChanged.Invoke(totalQty);
      }
    }

    public async Task<CartItemDto> UpdateItemQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
      try
      {
        var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
        var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json-patch+json");

        var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

        if (response.IsSuccessStatusCode)
        {
          return await response.Content.ReadFromJsonAsync<CartItemDto>();
        }
        return null;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
