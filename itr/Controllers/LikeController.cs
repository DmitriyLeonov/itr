using itr.Infrastructure;
using itr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace itr.Controllers
{
    public class LikeController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly itrContext context;

        public LikeController(UserManager<AppUser> userManager,
                                    itrContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SetLike(int Id)
        {
            Like like= new Like();
            var user = userManager.GetUserAsync(User);
            var likes = context.Likes.Where(u => u.UserName == user.Result.UserName && u.articleId == Id).ToList();
            if (likes.Count == 0)
            {
                like.UserName = user.Result.UserName;
                like.articleId = Id;
                context.Likes.Add(like);
                context.SaveChanges();
            }
            else
            {
                like.UserName = user.Result.UserName;
                like.articleId = Id;
                context.Likes.Remove(likes[0]);
                await context.SaveChangesAsync();
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
