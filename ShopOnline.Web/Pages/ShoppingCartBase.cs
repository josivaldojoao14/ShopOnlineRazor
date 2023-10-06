﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
  public class ShoppingCartBase : ComponentBase
  {
    [Inject]
    public IJSRuntime Js { get; set; }

    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }

    [Inject]
    public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

    public List<CartItemDto> ShoppingCartItems { get; set; }

    public string ErrorMessage { get; set; }

    protected string TotalPrice { get; set; }
    protected int TotalQuantity { get; set; }

    protected override async Task OnInitializedAsync()
    {
      try
      {
        ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
        CartChanged();
      }
      catch (Exception ex)
      {
        ErrorMessage = ex.Message;
      }
    }

    protected async Task DeleteItem_Click(int id)
    {
      await ShoppingCartService.DeleteCartItem(id);
      RemoveCartItem(id);
      CartChanged();

      await MakeUpdateQtyButtonVisible(id, false);
    }

    protected async Task UpdateQtyCartItem_Click(int id, int qty)
    {
      try
      {
        if (qty > 0)
        {
          var updateItemDto = new CartItemQtyUpdateDto
          {
            CartItemId = id,
            Qty = qty
          };

          var returnedUpdateItemDto = await ShoppingCartService.UpdateItemQty(updateItemDto);

          await UpdateItemTotalPrice(returnedUpdateItemDto);

          CartChanged();

          await MakeUpdateQtyButtonVisible(id, false);
        }
        else
        {
          var item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
          if (item is not null)
          {
            item.Qty = 1;
            item.TotalPrice = item.Price;
          }
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    protected async Task MakeUpdateQtyButtonVisible(int id, bool visible)
    {
      await Js.InvokeVoidAsync("MakeUpdateQtyButton", id, visible);
    }

    protected async Task UpdateQty_Input(int id)
    {
      await MakeUpdateQtyButtonVisible(id, true);
    }

    private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
    {
      var item = GetCartItem(cartItemDto.Id);
      if (item is not null)
      {
        item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
      }

      await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
    }

    private void CalculateCartSummaryTotals()
    {
      SetTotalPrice();
      SetTotalQuantity();
    }

    private void SetTotalPrice()
    {
      TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
    }

    private void SetTotalQuantity()
    {
      TotalQuantity = ShoppingCartItems.Sum(q => q.Qty);
    }

    private CartItemDto GetCartItem(int id)
    {
      return ShoppingCartItems.FirstOrDefault(c => c.Id == id);
    }

    private async Task RemoveCartItem(int id)
    {
      var cartItemDto = GetCartItem(id);
      ShoppingCartItems.Remove(cartItemDto);

      await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
    }

    private void CartChanged()
    {
      CalculateCartSummaryTotals();
      ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
    }
  }
}
