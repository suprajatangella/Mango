using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IProductService _prodService;
		public HomeController(ILogger<HomeController> logger, IProductService prodService)
        {
            _logger = logger;
			_prodService = prodService;
		}

        public async Task<IActionResult> Index()
        {
			List<ProductDto?> products = new List<ProductDto?>();
			ResponseDto? response = await _prodService.GetAllProductsAsync();

			if (response != null && response.IsSuccess)
			{
				products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(products);
		}

        public async Task<IActionResult> Details(int productId)
        {
            ProductDto product = new ProductDto();
            ResponseDto? response = await _prodService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(product);
        }

        [Authorize(Roles = SD.RoleAdmin)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
