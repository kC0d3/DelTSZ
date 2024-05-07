using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.ProductComponents;
using DelTSZ.Models.Products;
using DelTSZ.Repositories.ComponentRepository;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.ProductRepository;

public class ProductRepository(DataContext dataContext, IComponentRepository componentRepository) : IProductRepository
{
}