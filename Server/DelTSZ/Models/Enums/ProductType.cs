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
        Dictionary<ProductType, ICollection<(decimal Amount, ComponentType Component)>>
        ProductMap =
            new()
            {
                {
                    ProductType.Paprika400G,
                    new List<(decimal, ComponentType)>
                    {
                        (0.4m, ComponentType.Paprika)
                    }
                },
                {
                    ProductType.Tomato200G,
                    new List<(decimal, ComponentType)>
                    {
                        (0.2m, ComponentType.Tomato)
                    }
                },
                {
                    ProductType.Tomato500G,
                    new List<(decimal, ComponentType)>
                    {
                        (0.5m, ComponentType.Tomato)
                    }
                },
                {
                    ProductType.RatatouilleMix500G,
                    new List<(decimal, ComponentType)>
                    {
                        (0.25m, ComponentType.Paprika),
                        (0.25m, ComponentType.Tomato)
                    }
                },
                {
                    ProductType.SoupMix750G,
                    new List<(decimal, ComponentType)>
                    {
                        (0.25m, ComponentType.Carrot),
                        (0.25m, ComponentType.ParsleyRoot),
                        (0.15m, ComponentType.Celery),
                        (0.1m, ComponentType.Onion)
                    }
                },
            };

    public static IEnumerable<(decimal, ComponentType)> GetProductDetails(this ProductType type)
    {
        return ProductMap[type];
    }
}