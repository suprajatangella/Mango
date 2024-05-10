using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _prodService;
        public ProductController(IProductService prodService)
        {
			_prodService=prodService;
		}
        public async Task<IActionResult> ProductIndex()
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
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

	    [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
			ResponseDto? response = await _prodService.CreateProductAsync(productDto);
			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Product Created Successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(productDto);
		}


        public async Task<IActionResult> ProductEdit(int productId)
        {
			ResponseDto? response = await _prodService.GetProductByIdAsync(productId);

			if(Response!=null && response.IsSuccess)
			{
				ProductDto? productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(productDto);
			}
			else
			{
				TempData["error"]=response?.Message;
			}
            return NotFound();
        }

		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto productDto)
		{
			ResponseDto? response = await _prodService.UpdateProductAsync(productDto);

			if (Response != null && response.IsSuccess)
			{
				TempData["success"] = "Product updated successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return NotFound();
		}


		public async Task<IActionResult> ProductDelete(int productId)
        {
			ResponseDto? response = await _prodService.GetProductByIdAsync(productId);

			if (response != null && response.IsSuccess)
			{
				ProductDto product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
				return View(product);
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ProductDelete(ProductDto productDto)
		{
			ResponseDto? response = await _prodService.DeleteProductAsync(productDto.ProductId);

			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Product deleted Successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = response?.Message;
			}


			return View(productDto);
		}
	}
}
