using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Domain.Models;
public class Product
{
    public int Id { get; set; } // should not be auto incremnted
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; } = default!; // is this a good idea?
}
