using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureFunctionAppCallFromWebApp.Controllers
{
    public class HomeController : Controller
    {
        private AppSettings _mySettings { get; set; }

        public HomeController(IOptions<AppSettings> settings)
        {
            _mySettings = settings.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            try
            {
                string azureBaseUrl = _mySettings.AzureFunctionURL + name;

                using (var client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(azureBaseUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            string data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                ViewData["result"] = data;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ViewData["result"] = ex.Message;
            }
            return View();
        }
    }
}
