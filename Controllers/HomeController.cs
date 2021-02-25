using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext dbContext;

        public HomeController(MyContext context) 
        {
            // allows us to access data throughout our methods!
            dbContext = context; // want the field value I created (dbContext) to be the 'context' the frameworks passes to me
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewBag.AllProducts = dbContext.Products.OrderBy(p => p.ProductId).ToList();
            return View("Products");
        }

        [HttpPost("create-product")]
        public IActionResult CreateProduct(Product newProd)
        {
            if(ModelState.IsValid)
            {
                dbContext.Products.Add(newProd);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            
            return View("Products");
        }

        [HttpGet("products/{productId}")]
        public IActionResult ProductDetail(int productId)
        {
            ViewBag.catInProd = dbContext.Products
                .Include(prod => prod.Categories)
                    .ThenInclude(a => a.Category)
                .FirstOrDefault(prod => prod.ProductId == productId);

            ViewBag.AllCategories = dbContext.Categories
            .OrderBy(p => p.CategoryId)
            // not including cats that have current product's id
            .Where(a => a.Products.All(b => b.ProductId != productId))
            .ToList();
            return View("ProductDetail");
        }
        [HttpPost("add-category")]
        public IActionResult AddCat(Association addedCat)
        {
            dbContext.Associations.Add(addedCat);
            dbContext.SaveChanges();
            return Redirect($"/products/{addedCat.ProductId}");

            // redirectToAction - going to a method
            // redirect - going to the route (url)
        }


        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories = dbContext.Categories.OrderBy(p => p.CategoryId).ToList();
            return View("Categories");
        }

        [HttpPost("create-category")]
        public IActionResult CreateCategory(Category newCat)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(newCat);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            
            return View("Categories");
        }

        [HttpGet("categories/{categoryId}")]
        public IActionResult CategoryDetail(int categoryId)
        {
            ViewBag.prodWithCat = dbContext.Categories
                .Include(prod => prod.Products)
                    .ThenInclude(a => a.Product)
                .FirstOrDefault(prod => prod.CategoryId == categoryId);
            
            ViewBag.AllProducts = dbContext.Products
            .OrderBy(p => p.ProductId)
            // not including products that have current product's id
            .Where(a => a.Categories.All(b => b.CategoryId != categoryId))
            .ToList();

            return View("CategoryDetail");
        }

        [HttpPost("add-product")]
        public IActionResult AddProd(Association addedProd)
        {
            dbContext.Associations.Add(addedProd);
            dbContext.SaveChanges();
            return Redirect($"/categories/{addedProd.CategoryId}");
            // return RedirectToAction("CategoryDetail", addedProd)
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
