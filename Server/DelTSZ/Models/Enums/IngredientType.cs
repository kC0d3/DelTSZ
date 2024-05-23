namespace DelTSZ.Models.Enums;

public enum IngredientType
{
    Carrot,
    Celery,
    Cucumber,
    Mushroom,
    Onion,
    ParsleyRoot,
    Paprika,
    Potato,
    Radish,
    Tomato,
}

public static class IngredientTypeExtensions
{
    public static IEnumerable<EnumResponse> GetIngredientTypes()
    {
        return Enum.GetValues(typeof(IngredientType))
            .Cast<IngredientType>()
            .Select(i => new EnumResponse
            {
                Index = (int)i,
                Value = i.ToString()
            }).ToList();
    }
}