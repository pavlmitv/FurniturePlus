﻿using FurniturePlus.Controllers;
using FurniturePlus.Data;
using FurniturePlus.Infrastructure;
using FurniturePlus.Models.Home;
using FurniturePlus.Models.Items;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FurniturePlus.Services.Items
{
    public class ItemService : IItemService
    {
        private readonly FurniturePlusDbContext data;

        public ItemService(FurniturePlusDbContext data)
        {
            this.data = data;
        }

        public DetailsViewModel Details(int itemId, bool isEditable)
        {
            return this.data
                    .Items
                    .Where(i => i.Id == itemId)
                    .Select(i => new DetailsViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        PurchaseCode = i.PurchaseCode,
                        ImageUrl = i.ImageUrl,
                        Description = i.Description,
                        Price = i.Price,
                        IsEditable = isEditable
                    })
                    .FirstOrDefault();
        }

        public List<ItemIndexViewModel> GetAllItemsForHomePage()
        {
           return this.data
                    .Items
                    .Select(i => new ItemIndexViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        PurchaseCode = i.PurchaseCode,
                        ImageUrl = i.ImageUrl,
                        Description = i.Description,
                        Price = i.Price
                    })
                    .ToList();
        }

        public int ItemsCount()
        {
            return this.data.Items.Count(); 
        }
    }
}
