using FurniturePlus.Data;
using FurniturePlus.Data.Models;
using FurniturePlus.Models.Home;
using FurniturePlus.Models.Items;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public ItemSearchModel All([FromQuery] ItemSearchModel query)
        {
            var itemQuery = this.data.Items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CategoryId))
            {
                itemQuery = itemQuery.Where(i => i.CategoryId.ToString() == query.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(query.VendorId))
            {
                itemQuery = itemQuery.Where(i => i.VendorId.ToString() == query.VendorId);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                itemQuery = itemQuery
                    .Where(i =>
                    i.Category.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    i.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    i.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            switch (query.Sorting)
            {
                case ItemSorting.Category:
                    itemQuery = itemQuery.OrderBy(i => i.Category.Name);
                    break;
                case ItemSorting.Name:
                    itemQuery = itemQuery.OrderBy(i => i.Name);
                    break;
                case ItemSorting.PriceAscending:
                    itemQuery = itemQuery.OrderBy(i => i.Price);
                    break;
                case ItemSorting.PriceDescending:
                    itemQuery = itemQuery.OrderByDescending(i => i.Price);
                    break;
            }

            var items = itemQuery
                .Skip((query.CurrentPage - 1) * ItemSearchModel.ItemsPerPage)
                .Take(ItemSearchModel.ItemsPerPage)
                .Select(i => new ItemListingViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    PurchaseCode = i.PurchaseCode,
                    Category = i.Category,
                    ImageUrl = i.ImageUrl,
                    Description = i.Description,
                    Price = i.Price
                })
                .ToList();

            var itemCategories = this.data
                .Items
                .Select(i => i.Category)
                .Distinct()
                .ToList();

            query.Categories = itemCategories;
            query.Items = items;
            query.ItemsCount = itemQuery.Count();

            return query;
        }

        public DetailsViewModel Details(int itemId, string currentUserId, bool isSalesman)
        {
            var isEditable = isSalesman
                && (this.data
                      .Salesmen
                      .FirstOrDefault(s => s.UserId == currentUserId)
                      .VendorId
                      == this.data
                      .Items
                      .FirstOrDefault(i => i.Id == itemId)
                      .VendorId);
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


        public void AddItem(AddItemFormModel item, string currentUserId)
        {
            var itemVendor = this.data
                .Vendors
                .Where(v => v.Id == this.data
                .Salesmen
                .Where(s => s.UserId == currentUserId)
                .FirstOrDefault()
                .VendorId)
                .FirstOrDefault();

            //Purchase Code = first 3 letters of Vendor's name + random 6 digits number;
            var rnd = new Random();
            var purchaseCode = itemVendor.Name.Substring(0, 3).ToUpper() + (rnd.Next(0, 1000000).ToString("D6"));

            var newItem = new Item
            {
                Id = item.Id,
                Name = item.Name,
                PurchaseCode = purchaseCode,
                Category = item.Category,
                CategoryId = item.CategoryId,
                Vendor = itemVendor,
                ImageUrl = item.ImageUrl,
                Description = item.Description,
                Price = item.Price,
            };
            this.data.Items.Add(newItem);
            this.data.SaveChanges();
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

        public IEnumerable<ItemCategoryViewModel> GetItemCategories()
        {
            return data
                .Categories
                .Select(c => new ItemCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        //public bool DoesCategoryExist(int itemId)
        //{
        //    return this.data.Categories.Any(c => c.Id == this.data
        //    .Items
        //    .FirstOrDefault(i => i.Id == itemId)
        //    .CategoryId);
        //}
        public bool DoesCategoryExist(int categoryId)
        {
            return this.data.Categories.Any(c => c.Id == categoryId);
        }

        public Item GetItem(int id)
        {
            return this.data
                .Items.FirstOrDefault(i => i.Id == id);
        }

        public EditItemFormModel EditItem(int itemId)
        {
            var item = this.data
                .Items
                .FirstOrDefault(i => i.Id == itemId);
            return new EditItemFormModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = item.Category,
                    CategoryId = item.CategoryId,
                    Vendor = item.Vendor,
                    ImageUrl = item.ImageUrl,
                    Description = item.Description,
                    Price = item.Price,
                    ItemCategories = this.GetItemCategories()
                };
                
        }

        public void EditItem(EditItemFormModel item)
        {
            var currentItem = this.data
                .Items
                .FirstOrDefault(i => i.Id == item.Id);

            // currentItem.Category = item.Category;
            currentItem.Name = item.Name;
            currentItem.Category = item.Category;
            currentItem.CategoryId = item.CategoryId;
            currentItem.Description = item.Description;
            currentItem.ImageUrl = item.ImageUrl;
            currentItem.Price = item.Price;

            this.data.SaveChanges();
        }
    }
}
