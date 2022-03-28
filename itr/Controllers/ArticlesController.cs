using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using itr.Infrastructure;
using itr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace itr.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly itrContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public ArticlesController(IWebHostEnvironment webhostEnvironment, UserManager<AppUser> userManager,  
                                    itrContext context)
        {
            webHostEnvironment = webhostEnvironment;
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            Article article = await context.Articles.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            var ratings = context.ArticleRatings.Where(x => x.articleId == id).Select(r => r.Rating).ToList();
            var likes = context.Likes.Where(x => x.articleId == id).Select(r => r.UserName).ToList();
            if (ratings.Count > 0)
                article.UsersRating = Math.Round(ratings.Average(), 2);
            else
                article.UsersRating = 0;
            article.Likes = likes.Count();
            return View(article);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }

        // POST articles/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                article.Slug = article.Name.ToLower().Replace(" ", "-");
                var user = await userManager.GetUserAsync(User);
                article.UserName = user.UserName;
                var slug = await context.Articles.FirstOrDefaultAsync(x => x.Slug == article.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The article already exists.");
                    return View(article);
                }

                string imageName = "noimage.png";
                if (article.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/articles");
                    imageName = Guid.NewGuid().ToString() + "_" + article.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await article.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                article.Image = imageName;

                context.Add(article);
                await context.SaveChangesAsync();

                TempData["Success"] = "The article has been added!";

                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET articles/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Article article = await context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", article.CategoryId);

            return View(article);
        }

        // POST articles/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", article.CategoryId);

            if (ModelState.IsValid)
            {
                article.Slug = article.Name.ToLower().Replace(" ", "-");

                var slug = await context.Articles.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == article.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The article already exists.");
                    return View(article);
                }

                if (article.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/articles");

                    if (!string.Equals(article.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, article.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + article.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await article.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    article.Image = imageName;
                }

                context.Update(article);
                await context.SaveChangesAsync();

                TempData["Success"] = "The article has been edited!";

                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET articles/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Article article = await context.Articles.FindAsync(id);

            if (article == null)
            {
                TempData["Error"] = "The article does not exist!";
            }
            else
            {
                if (!string.Equals(article.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/articles");
                    string oldImagePath = Path.Combine(uploadsDir, article.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                context.Articles.Remove(article);
                await context.SaveChangesAsync();

                TempData["Success"] = "The article has been deleted!";
            }

            return RedirectToAction("Index");
        }

        // GET /articles/category
        public async Task<IActionResult> ArticlesByCategory(string categorySlug, int p = 1)
        {
            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");

            int pageSize = 6;
            var articles = context.Articles.OrderByDescending(x => x.Id)
                                            .Where(x => x.CategoryId  == category.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Articles.Where(x => x.CategoryId == category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;

            return View(await articles.ToListAsync());
        }
    }
}