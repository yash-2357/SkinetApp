﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data {
  public class StoreContextSeed {
    public static async Task SeedDataAsync(StoreContext context) {
      if (!context.Products.Any()) {
        var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
        if (products == null) return;

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
      }
    }
  }
}
