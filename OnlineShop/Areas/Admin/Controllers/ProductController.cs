using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class ProductController : Controller
    {
		private ApplicationDbContext _db;
		private IHostingEnvironment _he;

		public ProductController(ApplicationDbContext db, IHostingEnvironment he)
		{
			_db = db;
			_he = he;
		}
		public IActionResult Index()
        {
            return View(_db.Products.Include(c =>c.ProductTypes).Include(f =>f.SpecialTag).ToList());
        }

		//Post Index action method
		[HttpPost]
		public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
		{
			var products = _db.Products.Include(c =>c.ProductTypes).Include(c=>c.SpecialTag).Where(c=>c.Price >= lowAmount && c.Price <= largeAmount).ToList();
			
			if (lowAmount==null || largeAmount==null)
			{
				products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
			}

			return View(products);
		}

		// Get Create method
		public IActionResult Create()
		{
			ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
			ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");
			return View();
		}

		// Post Create method
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Products products, IFormFile image)
		{
			if (ModelState.IsValid)
			{
				var searchProduct = _db.Products.FirstOrDefault(c => c.Name == products.Name);
				if (searchProduct !=null)
				{
					ViewBag.message = "This product is already exist";
					// Recharger les controls de list
					ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
					ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");
					return View(products);
				}

				if (image!=null)
				{
					var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
					await image.CopyToAsync(new FileStream(name, FileMode.Create));
					products.Image = "images/" + image.FileName;
				}
				if (image == null)
				{
					products.Image = "Images/notfoundimage.png";
				}
				_db.Products.Add(products);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(products);
		}

		// Get Edit Action method
		public ActionResult Edit(int? id)
		{
			ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
			ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");
			if (id==null)
			{
				return NotFound();
			}
			var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);

			if (product==null)
			{
				return NotFound();
			}
			return View(product);
		}

		// Post Edit Action method
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Products products, IFormFile image)
		{
			if (ModelState.IsValid)
			{
				if (image != null)
				{
					var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
					await image.CopyToAsync(new FileStream(name, FileMode.Create));
					products.Image = "images/" + image.FileName;
				}
				if (image == null)
				{
					products.Image = "Images/notfoundimage.png";
				}
				_db.Products.Update(products);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(products);
		}

		// GET Details Action Metod
		public ActionResult Details(int? id)
		{
			if (id==null)
			{
				return NotFound();
			}
			var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}


		// GET Delete Action Metod
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var product = _db.Products.Include(c=>c.ProductTypes)
				.Include(c=>c.SpecialTag).Where(c => c.Id == id).FirstOrDefault();
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		//POST Delete Action Method
		[HttpPost]	
		[ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirm(int? id)
		{
			if (id==null)
			{
				return NotFound();
			}
			var product = _db.Products.FirstOrDefault(c => c.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			_db.Products.Remove(product);
			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

	}
}








