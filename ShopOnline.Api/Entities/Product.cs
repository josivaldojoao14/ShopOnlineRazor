﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Api.Entities
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Qty { get; set; }
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public ProductCategory ProductCategory { get; set; }
  }
}