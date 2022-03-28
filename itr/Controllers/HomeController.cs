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

            return View(await articles.ToListAsync());
        }
    }
}
