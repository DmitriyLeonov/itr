using itr.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace itr.Controllers
{
    public class HomeController : Controller
    {
        private readonly itrContext context;
        public HomeController(itrContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var articles = context.Articles.OrderByDescending(x => x.Id)
                                            .Include(x => x.Category)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Articles.Count() / pageSize);
            foreach (var article in articles)
            {
                var ratings = context.ArticleRatings.Where(x => x.articleId == article.Id).Select(r => r.Rating).ToList();
                if (ratings.Count > 0)
                    article.UsersRating = Math.Round(ratings.Average(), 2);
                else
                    article.UsersRating = 0;
            }
            return View(await articles.ToListAsync());
        }
    }
}
