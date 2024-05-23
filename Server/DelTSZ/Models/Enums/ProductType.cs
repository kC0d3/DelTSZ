namespace DelTSZ.Models.Enums;

public enum ProductType
{
    Paprika400G,
    Tomato200G,
    Tomato500G,
    RatatouilleMix500G,
    SoupMix750G,
}

public static class ProductTypeExtensions
{
    private static readonly
        Dictionary<ProductType, ICollection<(decimal Amount, IngredientType Ingredient)>>
        ProductMap =
            new()
            {
                {
                    ProductType.Paprika400G,
                    new List<(decimal, IngredientType)>
                    {
                        (0.4m, IngredientType.Paprika)
                    }
                },
                {
                    ProductType.Tomato200G,
                    new List<(decimal, IngredientType)>
                    {
                        (0.2m, IngredientType.Tomato)
                    }
                },
                {
                    ProductType.Tomato500G,
                    new List<(decimal, IngredientType)>
                    {
                        (0.5m, IngredientType.Tomato)
                    }
                },
                {
                    ProductType.RatatouilleMix500G,
                    new List<(decimal, IngredientType)>
                    {
                        (0.25m, IngredientType.Paprika),
                        (0.25m, IngredientType.Tomato)
                    }
                },
                {
                    ProductType.SoupMix750G,
                    new List<(decimal, IngredientType)>
                    {
                        (0.25m, IngredientType.Carrot),
                        (0.25m, IngredientType.ParsleyRoot),
                        (0.15m, IngredientType.Celery),
                        (0.1m, IngredientType.Onion)
                    }
                },
            };

    public static IEnumerable<(decimal, IngredientType)> GetProductDetails(this ProductType type)
    {
        return ProductMap[type];
    }

    public static IEnumerable<EnumResponse> GetProductTypes()
    {
        return Enum.GetValues(typeof(ProductType))
            .Cast<ProductType>()
            .Select(p => new EnumResponse
            {
                Index = (int)p,
                Value = p.ToString()
            }).ToList();
    }
}