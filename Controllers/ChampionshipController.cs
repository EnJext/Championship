using System.Linq;
using System.Web.Mvc;
using WebApplicationFramework.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace WebApplicationFramework.Controllers
{
    public class ChampionshipController :Controller
    {
        private FightsContext fightsContext;
        public ChampionshipController()
        {
            fightsContext = new FightsContext();
        }

        public ActionResult Index() => View();

        public string GetFightsDataToJqGrid() // Championship & remake 
        {
            return JsonConvert.SerializeObject(GetFightsDataForChampPage().ToList());
        }

        
        private IEnumerable<FightModelForChampPage> GetFightsDataForChampPage()
        {
            return from fight in fightsContext.Fights
                   join Winner in fightsContext.Fighters on fight.Winner.Id equals Winner.Id
                   join Losser in fightsContext.Fighters on fight.Losser.Id equals Losser.Id
                   select new FightModelForChampPage
                   {
                       Id = fight.FightId,
                       Time = fight.Time,
                       Rounds = fight.Rounds,
                       Winner = Winner.Name,
                       Losser = Losser.Name,
                       button = "<button data-fightId = '" + fight.FightId + "' style='width:100%;' > Видалити </button>"
                   };
        }



        [HttpPost]  //Championship
        public string GetSearchFightsData(DateTime? BeginDate, DateTime? EndDate, string Name, string Result)
        {
            IEnumerable<FightModelForChampPage> Fights = GetFightsDataForChampPage();
            if (BeginDate != null && EndDate != null)
            {
                Fights = Fights
               .Where(f => DateTime.Compare(f.Time, BeginDate.Value) > 0 && DateTime.Compare(f.Time, EndDate.Value) < 0);
            }
            if (!String.IsNullOrEmpty(Name))
            {
                Fights = Fights.Where(f => f.Winner == Name || f.Losser == Name);
            }
            if (Result != null && !String.IsNullOrEmpty(Name))
            {
                switch (Result)
                {
                    case "Пeремога": Fights = Fights.Where(f => f.Winner == Name); break;
                    case "Поразка": Fights = Fights.Where(f => f.Losser == Name); break;
                    default: break;
                }
            }

            return JsonConvert.SerializeObject(Fights.ToList());

        }

        //Championship
        public ActionResult AutocompleteSearch(string term)
        {
            var models = fightsContext.Fighters.Where(a => a.Name.Contains(term))
                         .Select(a => new { value = a.Name })
                         .Distinct();

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        //Championship(User)
        [HttpPost]
        public JsonResult DeleteFight(int? Id)
        {
            fightsContext.Fights.Remove(fightsContext.Fights.Where(f => f.FightId == Id).FirstOrDefault());
            fightsContext.SaveChanges();
            return Json(GetFightsDataForChampPage().ToList());
        }

    }
}