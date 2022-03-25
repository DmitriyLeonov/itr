using itr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itr.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly itrContext context;

        public MainMenuViewComponent(itrContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
