﻿using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
  public class ProductService : IProductService
  {
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<ProductDto> GetProduct(int id)
    {
      try
      {
        var response = await _httpClient.GetAsync($"api/Product/{id}");
        if (response.IsSuccessStatusCode)
        {
          if (response.StatusCode is HttpStatusCode.NoContent)
          {
            return default(ProductDto);
          }

          return await response.Content.ReadFromJsonAsync<ProductDto>();
        }
        else
        {
          var message = await response.Content.ReadAsStringAsync();
          throw new Exception(message);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
    {
      try
      {
        var response = await _httpClient.GetAsync("api/Product/GetProductCategories");
        if (response.IsSuccessStatusCode)
        {
          if (response.StatusCode is HttpStatusCode.NoContent)
          {
            return Enumerable.Empty<ProductCategoryDto>();
          }
          return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();
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

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
      try
      {
        var response = await _httpClient.GetAsync("api/Product");

        if (response.IsSuccessStatusCode)
        {
          if (response.StatusCode is HttpStatusCode.NoContent)
          {
            return Enumerable.Empty<ProductDto>();
          }

          return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
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

    public async Task<IEnumerable<ProductDto>> GetProductsByCategory(int categoryId)
    {
      try
      {
        var response = await _httpClient.GetAsync($"api/Product/{categoryId}/GetProductsByCategory");

        if (response.IsSuccessStatusCode)
        {
          if (response.StatusCode is HttpStatusCode.NoContent)
          {
            return Enumerable.Empty<ProductDto>();
          }

          return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
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
  }
}