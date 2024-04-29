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
        Dictionary<ProductType, ICollection<(double Amount, ComponentType Component)>>
        ProductMap =
            new()
            {
                {
                    ProductType.Paprika400G,
                    new List<(double, ComponentType)>
                    {
                        (0.4, ComponentType.Paprika)
                    }
                },
                {
                    ProductType.Tomato200G,
                    new List<(double, ComponentType)>
                    {
                        (0.2, ComponentType.Tomato)
                    }
                },
                {
                    ProductType.Tomato500G,
                    new List<(double, ComponentType)>
                    {
                        (0.5, ComponentType.Tomato)
                    }
                },
                {
                    ProductType.RatatouilleMix500G,
                    new List<(double, ComponentType)>
                    {
                        (0.25, ComponentType.Paprika),
                        (0.25, ComponentType.Tomato)
                    }
                },
                {
                    ProductType.SoupMix750G,
                    new List<(double, ComponentType)>
                    {
                        (0.25, ComponentType.Carrot),
                        (0.25, ComponentType.ParsleyRoot),
                        (0.15, ComponentType.Celery),
                        (0.1, ComponentType.Onion)
                    }
                },
            };

    public static IEnumerable<(double, ComponentType)> GetComponentsAmount(this ProductType type)
    {
        return ProductMap[type];
    }
}