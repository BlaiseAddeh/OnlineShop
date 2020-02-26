using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class SpecialTagsController : Controller
    {
		private ApplicationDbContext _db;
		public SpecialTagsController(ApplicationDbContext db)
		{
			_db = db;
		}

        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }

		// Create Get Action Method
		public ActionResult Create()
		{
			return View();
		}

		// Create Post Action Method
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SpecialTags specialTags)
		{
			if (ModelState.IsValid)
			{
				_db.SpecialTags.Add(specialTags);
				await _db.SaveChangesAsync();
				TempData["save"] = "Tag has been saved";
				return RedirectToAction(nameof(Index));
			}
			return View(specialTags);
		}

		// Edit Get Action Method
		public ActionResult Edit(int? id)
		{
			if (id==null)
			{
				return NotFound();
			}
			var specialTag = _db.SpecialTags.Find(id);
			if (specialTag == null)
			{
				return NotFound();
			}
			return View(specialTag);
		}

		// Create Post Action Method
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(SpecialTags specialTags)
		{
			if (ModelState.IsValid)
			{
				_db.Update(specialTags);
				await _db.SaveChangesAsync();
				TempData["edit"] = "Tag has been edited";
				return RedirectToAction(nameof(Index));
			}
			return View(specialTags);
		}

		// Details Get Action Method
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var specialTag = _db.SpecialTags.Find(id);
			if (specialTag == null)
			{
				return NotFound();
			}
			return View(specialTag);
		}

		// Details Post Action Method
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Details(SpecialTags specialTags)
		{
			return RedirectToAction(nameof(Index));
		}


		// Delete Get Action Method
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var specialTag = _db.SpecialTags.Find(id);
			if (specialTag == null)
			{
				return NotFound();
			}
			return View(specialTag);
		}

		// Delete Post Action Method
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int? id, SpecialTags specialTags)
		{
			if (id==null)
			{
				return NotFound();
			}
			if (id != specialTags.Id)
			{
				return NotFound();
			}
			var specialTag = _db.SpecialTags.Find(id);
			if (specialTag == null)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				_db.Remove(specialTag);
				await _db.SaveChangesAsync();
				TempData["delete"] = "Tag has been deleted";
				return RedirectToAction(nameof(Index));
			}
			return View(specialTag);
		}


	}
}


