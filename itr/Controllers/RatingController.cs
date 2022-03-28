using itr.Infrastructure;
using itr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace itr.Controllers
{
    public class RatingController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly itrContext context;

        public RatingController(UserManager<AppUser> userManager,
                                    itrContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SetRating(int Id, double rating)
        {
            ArticleRating articleRating = new ArticleRating();
            var user = userManager.GetUserAsync(User);
            var rates = context.ArticleRatings.Where(u => u.UserName == user.Result.UserName && u.articleId == Id).ToList();
            if(rates.Count == 0) {
                articleRating.UserName = user.Result.UserName;
                articleRating.Rating = rating;
                articleRating.articleId = Id;

                context.ArticleRatings.Add(articleRating);
                context.SaveChanges();
            }

            return await Task.Run<ActionResult>(() =>
            {
                if (true)
                {
                    return RedirectToAction("Details", "Articles", new { id = Id });
                }
                else
                {
                    return View("Index");
                }
            });
            
        }
    }
}
