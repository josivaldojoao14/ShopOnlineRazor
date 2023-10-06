using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
    {
      try
      {
        var products = await _productRepository.GetProducts();

        if (products is null)
        {
          return NotFound();
        }
        else
        {
          var productDtos = products.ConvertToDto();
          return Ok(productDtos);
        }
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
          "Error retrieving data from the database");
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetItem(int id)
    {
      try
      {
        var product = await _productRepository.GetProduct(id);

        if (product is null)
        {
          return BadRequest();
        }
        else
        {
          var productDto = product.ConvertToDto();
          return Ok(productDto);
        }
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
          "Error retrieving data from the database");
      }
    }

    [HttpGet]
    [Route(nameof(GetProductCategories))]
    public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
    {
      try
      {
        var productCategories = await _productRepository.GetCategories();
        var productCategoryDtos = productCategories.ConvertToDto();

        return Ok(productCategoryDtos);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
          "Error retrieving data from the database");
      }
    }

    [HttpGet]
    [Route("{categoryId}/GetProductsByCategory")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
      try
      {
        var products = await _productRepository.GetProductsByCategory(categoryId);
        var productsDtos = products.ConvertToDto();

        return Ok(productsDtos);
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
          "Error retrieving data from the database");
      }
    }
  }
}
