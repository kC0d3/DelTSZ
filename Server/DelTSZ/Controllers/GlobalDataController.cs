using DelTSZ.Models.GlobalData;
using Microsoft.AspNetCore.Mvc;

namespace DelTSZ.Controllers;

[Route("api/global/data")]
[ApiController]
public class GlobalDataController : ControllerBase
{
    [HttpGet("slides")]
    public IActionResult SlidesData()
    {
        try
        {
            return Ok(
                new SlidesData
                {
                    BgImagesAmount = 3,
                    SlidesInterval = 7500,
                    Slides =
                    [
                        new Slide
                        {
                            Img = "/images/backgrounds/bg1.jpg",
                            Bubble = new Dictionary<string, string>
                            {
                                { "h1", "Local Champions" },
                                { "p1", "Fresh, juicy and healthy" },
                                { "p2", "local vegetables every day of the year." },
                                { "button", "Our Products" }
                            }
                        },
                        new Slide
                        {
                            Img = "/images/backgrounds/bg2.jpg",
                            Bubble = new Dictionary<string, string>
                            {
                                { "h1", "Local producers" },
                                { "p1", "Our producers provide reliable" },
                                { "p2", "fresh, juicy and healthy vegetables." },
                                { "button", "Our Products" }
                            }
                        },
                        new Slide
                        {
                            Img = "/images/backgrounds/bg3.jpg",
                            Bubble = new Dictionary<string, string>
                            {
                                { "h1", "DelTSZ" },
                                { "p1", "Our logistic center" },
                                { "p2", "can handle more than 1.000 orders every day." },
                                { "button", "Our Products" }
                            }
                        }
                    ]
                }
            );
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpGet("achievements")]
    public IActionResult AchievementsData()
    {
        try
        {
            return Ok(
                new AchievementsData
                {
                    AchievementDuration = 2000,
                    AchievementStart = 0,
                    AchievementCounters =
                    [
                        new AchievementCounter
                        {
                            Amount = 500,
                            Achievement = "Producer",
                            Description = "We operate together with that much local producer."
                        },
                        new AchievementCounter
                        {
                            Amount = 53000,
                            Achievement = "Tons",
                            Description =
                                "We can provide that much healthy vegetables and support your health every year."
                        },
                        new AchievementCounter
                        {
                            Amount = 150,
                            Achievement = "Hectare greenhouse",
                            Description = "Around that much area available for driven cultivation."
                        },
                        new AchievementCounter
                        {
                            Amount = 96,
                            Achievement = "Biological plant protection",
                            Description = "We say yes to sustainability and environmental consciousness."
                        }
                    ]
                }
            );
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }

    [HttpGet("product-descriptions")]
    public IActionResult ProductDescriptionsData()
    {
        try
        {
            return Ok(
                new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "Paprika400G", new Dictionary<string, string>
                        {
                            { "name", "Paprika 400g" },
                            { "description", "Best for sandwiches, ratatouille and raw consume." }
                        }
                    },
                    {
                        "Tomato200G", new Dictionary<string, string>
                        {
                            { "name", "Tomato 200g" },
                            { "description", "Best for sandwiches, ratatouille and raw consume." }
                        }
                    },
                    {
                        "Tomato500G", new Dictionary<string, string>
                        {
                            { "name", "Tomato 500g" },
                            { "description", "Best for sandwiches, ratatouille and raw consume." }
                        }
                    },
                    {
                        "RatatouilleMix500G", new Dictionary<string, string>
                        {
                            { "name", "Ratatouille mix 500g" },
                            { "description", "Best for ratatouille bases." }
                        }
                    },
                    {
                        "SoupMix750G", new Dictionary<string, string>
                        {
                            { "name", "Soup mix 750g" },
                            { "description", "Best for chicken soup bases." }
                        }
                    },
                    {
                        "Carrot", new Dictionary<string, string>
                        {
                            { "name", "Carrot" },
                            { "description", "Best for Wild sauce and soups." }
                        }
                    },
                    {
                        "Celery", new Dictionary<string, string>
                        {
                            { "name", "Celery" },
                            { "description", "Best for celery bisque and soups." }
                        }
                    },
                    {
                        "Cucumber", new Dictionary<string, string>
                        {
                            { "name", "Cucumber" },
                            { "description", "Best for sandwiches and refreshing drinks." }
                        }
                    },
                    {
                        "Mushroom", new Dictionary<string, string>
                        {
                            { "name", "Mushroom" },
                            { "description", "Best for stew and bisque." }
                        }
                    },
                    {
                        "Onion", new Dictionary<string, string>
                        {
                            { "name", "Onion" },
                            { "description", "Most of the stews bases and extra seasoning." }
                        }
                    },
                    {
                        "ParsleyRoot", new Dictionary<string, string>
                        {
                            { "name", "Parsley root" },
                            { "description", "Best for chicken soups bases." }
                        }
                    },
                    {
                        "Paprika", new Dictionary<string, string>
                        {
                            { "name", "Paprika" },
                            { "description", "Best for sandwiches, ratatouille and raw consume." }
                        }
                    },
                    {
                        "Potato", new Dictionary<string, string>
                        {
                            { "name", "Potato" },
                            { "description", "Best for fried and mashed potato." }
                        }
                    },
                    {
                        "Radish", new Dictionary<string, string>
                        {
                            { "name", "Radish" },
                            { "description", "Best for sandwiches and raw consume." }
                        }
                    },
                    {
                        "Tomato", new Dictionary<string, string>
                        {
                            { "name", "Tomato" },
                            { "description", "Best for sandwiches, ratatouille and raw consume." }
                        }
                    }
                }
            );
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Something went wrong, please try again." });
        }
    }
}