﻿namespace ProductCatalog.API.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Category Category { get; set; } = new();
    public int SellerId { get; set; }

}