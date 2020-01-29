using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetwork.Dal.Context;
using SocialNetwork.Dal.Models;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

            var context = new PublicContext();

            User u = new User();
            u.Name = "vovka";
            u.UserPost = new List<UserPost>()
            {
                new UserPost()
                {
                    Post = new Post()
                    {
                        Date = DateTime.UtcNow,
                        ImgUrl = "qwe",
                        Text = "test text",
                        PostComment = new List<PostComment>()
                        {
                            new PostComment()
                            {
                                Comment = new Comment()
                                {
                                    Date = DateTime.UtcNow,
                                    ImgUrl = "test comment",
                                    Text = "test text comment"
                                }
                            },
                            new PostComment()
                            {
                                Comment = new Comment()
                                {
                                    Date = DateTime.UtcNow,
                                    ImgUrl = "test commen2t",
                                    Text = "test text commen2t"
                                }
                            }
                        }
                    }
                },
                new UserPost()
                {
                    Post = new Post()
                    {
                        Date = DateTime.UtcNow,
                        ImgUrl = "qwe2",
                        Text = "test text2"
                    }
                },
            };

            context.User.Add(u);
            context.SaveChanges();

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
