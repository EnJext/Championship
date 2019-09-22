using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplicationFramework.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data.Entity;

namespace WebApplicationFramework.Controllers
{
    public class HomeController : Controller
    {
        FightsContext fightsContext;

        public HomeController()
        {
            fightsContext = new FightsContext();
        }

        public ActionResult Index()
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

        public ActionResult UserPage(int? UserId)
        {
            Fighter fighter = fightsContext.Fighters.Find(UserId);

            if (fighter == null)
            {
                return HttpNotFound();
            }

            return View(CreateUserPageModelByFighter(fighter));
        }
        public ActionResult FightPage(int? FightId, int? FighterId)
        {
            if (FightId == null|| FighterId==null)
            {
                return Content(" Error: FightId or FighterId key is null");
            }

            Fight fight = fightsContext.Fights.Find(FightId);

            fightsContext.Entry(fight).Reference(s => s.Winner).Load();
            fightsContext.Entry(fight).Reference(s => s.Losser).Load();

            FightPageViewModel Model = new FightPageViewModel
            {
                FighterId = FighterId.Value,
                FightId = fight.FightId,
                Losser = fight.Winner.Name,
                Winner = fight.Losser.Name,
                Judge1 = fight.Judge1,
                Judge2 = fight.Judge2,
                Judge3 = fight.Judge3,
                Rounds = fight.Rounds,
                Time = fight.Time
            };

            return PartialView(Model);
        }


        [HttpPost]
        public ActionResult AddFight (FightPageViewModel fight)
        {
            AddFightByFightPageModel(fight);
            return View("UserPage", CreateUserPageModelByFighter(fightsContext.Fighters.Find(fight.FighterId)));
        }

        public ActionResult Championship()
        {
            return View();
        }

        public string GetFightsDataToJqGrid()
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
                       button = "<button onclick='javascript:Deletefight(" + fight.FightId+")' style='width:100%;' > Видалити </button>"
                   };
        }


         
        [HttpPost]
        public string GetSearchFightsData(DateTime? BeginDate, DateTime? EndDate, string Name, string Result)
        {
            IEnumerable<FightModelForChampPage> Fights = GetFightsDataForChampPage();
            if(BeginDate != null && EndDate != null)
            {
                Fights = Fights
               .Where(f => DateTime.Compare(f.Time, BeginDate.Value) > 0 && DateTime.Compare(f.Time, EndDate.Value) < 0);
            }
            if(!String.IsNullOrEmpty(Name))
            {
                Fights = Fights.Where(f => f.Winner == Name || f.Losser == Name);
            }
            if(Result != null&&!String.IsNullOrEmpty(Name))
            {
                switch(Result)
                {
                    case "Пeремога":Fights = Fights.Where(f => f.Winner == Name);break;
                    case "Поразка":Fights = Fights.Where(f => f.Losser == Name);break;
                    default:break;
                }
            }
          
            return JsonConvert.SerializeObject(Fights.ToList());

        }

        private UserPageViewModel CreateUserPageModelByFighter(Fighter fighter)
        {
            UserPageViewModel fighterModel = new UserPageViewModel()
            {
                Id = fighter.Id,
                Name = fighter.Name,
                Age = fighter.Age,
                Weight = fighter.Weight
            };

            fighterModel.Rating = fightsContext.Fights.Where(f => f.Winner.Id == fighter.Id).Sum(p => p.Judge1 + p.Judge2 + p.Judge3);

            fighterModel.fights = from fight in fightsContext.Fights.Where(f => f.Winner.Id == fighter.Id || f.Losser.Id == fighter.Id)
                           let result = fight.Winner.Id == fighter.Id
                           join enemy in fightsContext.Fighters on (result ? fight.Losser.Id : fight.Winner.Id) equals enemy.Id
                           select new FightUserPageViewModel
                           { EnemyName = enemy.Name, Id = fight.FightId, Result = result, Rounds = fight.Rounds, Time = fight.Time };

            return fighterModel;
        }

        private bool AddFightByFightPageModel(FightPageViewModel fight)
        {
            Fighter Winner = fightsContext.Fighters.Where(p => p.Name == fight.Winner).FirstOrDefault();
            Fighter Losser = fightsContext.Fighters.Where(p => p.Name == fight.Losser).FirstOrDefault();

            if (Winner != null && Losser != null)
            {
                var TimeParam = new SqlParameter("@Time", fight.Time);
                var RoundsPram = new SqlParameter("@Rounds", fight.Rounds);
                var WinnerIdParam = new SqlParameter("@WinnerId", Winner.Id);
                var LosserIdParam = new SqlParameter("@LosserId", Losser.Id);
                var Judge1Param = new SqlParameter("@Judge1", fight.Judge1);
                var Judge2Param = new SqlParameter("@Judge2", fight.Judge2);
                var Judge3Param = new SqlParameter("@Judge3", fight.Judge3);

                fightsContext.Database.ExecuteSqlCommand("exec AddFight @Time, @Rounds, @WinnerId, @LosserId," +
                    "@Judge1, @Judge2, @Judge3", TimeParam, RoundsPram, WinnerIdParam, LosserIdParam, Judge1Param,
                    Judge2Param, Judge3Param);

                return true;
            }
            return false;
        }

        public ActionResult AutocompleteSearch(string term)
        {
            var models = fightsContext.Fighters.Where(a => a.Name.Contains(term))
                         .Select(a => new { value = a.Name })
                         .Distinct();

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFight(int? Id)
        {
            fightsContext.Fights.Remove(fightsContext.Fights.Where(f => f.FightId == Id).FirstOrDefault());
            fightsContext.SaveChanges();
            return Json(GetFightsDataForChampPage().ToList());
        }


    }
}

