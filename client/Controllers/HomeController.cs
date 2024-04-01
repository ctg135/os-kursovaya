using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using client.Models;

namespace client.Controllers;

public class HomeController : Controller
{
    
    public record FormServer2(string address);
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Server1(FormServer1Display display){
        return View(display);
    }
    [HttpPost]
    public IActionResult Server1(FormServer1 form){
        var display = new FormServer1Display() 
        {
            address = form.address,
            xpos = form.xpos,
            ypos = form.ypos,
            color = "(150, 19, 248)",
            dimension = "1600x900"
        };
        return View(display);
    }
    [HttpGet]
    public IActionResult Server2(){
        return View();
    }

    [HttpPost]
    public IActionResult Server2(FormServer1 form){
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
