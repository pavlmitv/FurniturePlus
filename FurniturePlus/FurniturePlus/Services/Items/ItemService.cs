using FurniturePlus.Controllers;
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

        private IEnumerable<ItemCategoryViewModel> GetItemCategories()
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
    }
}
