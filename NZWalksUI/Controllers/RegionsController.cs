using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models.DTO;
using System.Threading.Tasks;

namespace NZWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<RegionsDto> response = new List<RegionsDto>();
            try
            {
                //Get All regions from WebAPI
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7005/API/regions");
                httpResponseMessage.EnsureSuccessStatusCode();

                //var stringResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                //ViewBag.Response = stringResponseBody;

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionsDto>>());


            }
            catch (Exception ex)
            {
                //log the exception
            }

            return View(response);
        }
    }
}
