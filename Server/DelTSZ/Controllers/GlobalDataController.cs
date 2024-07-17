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
}