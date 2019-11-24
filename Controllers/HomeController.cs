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
                                                let Sum = fighter.WinFights.Count() ==0? 0 :
                                                fighter.WinFights.Sum(f => f.Judge1 + f.Judge2 + f.Judge3)
                                                orderby Sum descending
                                                select new IndexViewModel
                                                {
                                                    Name = fighter.Name,
                                                    Id = fighter.Id,
                                                    Rank = Sum,
                                                    Rounds = fighter.WinFights.Count() + fighter.LoseFights.Count()
                                                };
            return View(Model);
        }


    }
}

