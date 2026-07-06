using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Forward.Mvc.Models;
using UrlShortener.Forward.Mvc.Services;

namespace UrlShortener.Forward.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ForwardApiClient _client;


    public HomeController(ILogger<HomeController> logger,ForwardApiClient client)
    {
        _logger = logger;
        _client = client;

    }


    [HttpGet]
    public IActionResult Index()
    {
        return View(new ForwardViewModel());
    }
    
    
     [HttpPost]
    public async Task<IActionResult> Index(ForwardViewModel model)
    {
        var code =model.ShortUrl.Split('/').Last();

        var destinationUrl =await _client.GetDestinationUrl(code);
    
       if (string.IsNullOrWhiteSpace(destinationUrl))
        {
            ModelState.AddModelError("","Short URL not found.");

            return View(model);
        }
        
        return Redirect(destinationUrl);

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
