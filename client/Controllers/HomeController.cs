using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using client.Models;
using srv1.ServerData;
using srv2.ServerData;

namespace client.Controllers;

public class HomeController : Controller
{
    public record MyColor(int r, int g, int b);
    private readonly ILogger<HomeController> _logger;
    private static readonly HttpClient client = new HttpClient();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Server1(FormServer1Display display)
    {
        return View(display);
    }
    [HttpPost]
    public async Task<IActionResult> Server1(FormServer1 form)
    {
        string screenX = "";
        string screenY = "";
        string color = "";

        if (!string.IsNullOrWhiteSpace(form.address))
        {
            try
            {
                var response = await client.GetAsync($"http://{form.address}/display");
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Error? error = await response.Content.ReadFromJsonAsync<Error>();
                    // Console.WriteLine(response.StatusCode);
                    // Console.WriteLine(error?.Message);
                }
                else 
                {
                    DisplayInfo? displayInfo = await response.Content.ReadFromJsonAsync<DisplayInfo>();
                    screenX = displayInfo.Width.ToString();
                    screenY = displayInfo.Height.ToString();
                }

                if (form.xpos > 0 && form.ypos > 0)
                {
                    Point point = new Point(){ X = form.xpos, Y = form.ypos };
                    response = await client.PostAsJsonAsync($"http://{form.address}/pixel", point);
                    if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Error? error = await response.Content.ReadFromJsonAsync<Error>();
                        // Console.WriteLine(response.StatusCode);
                        // Console.WriteLine(error?.Message);
                    }
                    else 
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var pixelColor = JsonConvert.DeserializeObject<MyColor>(json);
                        color = $"({pixelColor.r}, {pixelColor.g}, {pixelColor.b})";
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Server1", "Home", new FormServer1Display() {
                     address = form.address,
                     isError = true
                      });
            }
        }

        var display = new FormServer1Display() 
        {
            address = form.address,
            xpos = form.xpos,
            ypos = form.ypos,
            color = color,
            screenX = screenX,
            screenY = screenY
        };

        return View(display);
    }
    [HttpGet]
    public IActionResult Server2(FormServer2Display display){
        return View(display);
    }

    [HttpPost]
    public async Task<IActionResult> Server2(FormServer2 form){

        string pid = "";
        string did = "";

        try 
        {
            var response = await client.GetAsync($"http://{form.address}/handle");
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
            {
                // Error? error = await response.Content.ReadFromJsonAsync<Error>();
                // Console.WriteLine(response.StatusCode);
                // Console.WriteLine(error?.Message);
            }
            else 
            {
                Handles? handles = await response.Content.ReadFromJsonAsync<Handles>();
                pid = handles.ProcessHandle.ToString();
                did = handles.MainHandle.ToString();
            }
        }
        catch (Exception)
        {
                return RedirectToAction("Server2", "Home", new FormServer2Display() {
                     address = form.address,
                     isError = true
                      });
        }
        
        var display = new FormServer2Display()
        {
            address = form.address,
            pid = pid,
            did = did
        };

        return View(display);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
