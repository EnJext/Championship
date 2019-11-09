using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplicationFramework.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System;


namespace WebApplicationFramework.Controllers
{
    public class HomeController : Controller
    {
        FightsContext fightsContext;

        public HomeController()
        {
            fightsContext = new FightsContext();
        }

        public ActionResult Index() //home
        {
            IEnumerable<IndexViewModel> Model = from fighter in fightsContext.Fighters
                                                let Sum = fighter.Fights.Count == 0 ? 0:fighter.Fights
                                                .Where(f => f.Winner.Id == fighter.Id)
                                                .Sum(f => f.Judge1 + f.Judge2 + f.Judge3)
                                                orderby Sum descending
                                                select new IndexViewModel
                                                {
                                                    Name = fighter.Name,
                                                    Id = fighter.Id,
                                                    Rank = Sum,
                                                    Rounds = fighter.Fights.Count()
                                                };
            return View(Model);
        }


    }
}

