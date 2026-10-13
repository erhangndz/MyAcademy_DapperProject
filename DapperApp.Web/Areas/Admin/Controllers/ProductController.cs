using DapperApp.Web.DTOs.Products;
using DapperApp.Web.Services.Categories;
using DapperApp.Web.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(IProductService _productService,
                                   ICategoryService _categoryService) : Controller
    {

        private async Task GetCategoriesAsync()
        {
            var categoryList = await _categoryService.GetAllAsync();

            ViewBag.categories = (from category in categoryList
                                  select new SelectListItem
                                  {
                                      Text = category.Name,
                                      Value = category.CategoryId.ToString()
                                  }).ToList();

        }


        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            await GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            await _productService.CreateAsync(productDto);
            return RedirectToAction("Index");
        }


    }
}
