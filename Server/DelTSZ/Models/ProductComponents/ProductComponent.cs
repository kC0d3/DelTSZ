using DelTSZ.Models.Enums;
using DelTSZ.Models.Products;

namespace DelTSZ.Models.ProductComponents;

public class ProductComponent
{
    //Properties
    
    public int Id { get; init; }
    public ComponentType Type { get; set; }
    public DateTime Received { get; init; }
    public decimal Amount { get; set; }
    
    //Navigation properties
    
    public int? ProductId { get; init; }
    public Product? Product { get; init; }
}