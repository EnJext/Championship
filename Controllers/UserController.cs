
using System.Linq;
using System.Web.Mvc;
using WebApplicationFramework.Models;
using System.Data.SqlClient;

namespace WebApplicationFramework.Controllers
{
    public class UserController : Controller
    {
        FightsContext fightsContext;
        public UserController()
        {
            fightsContext = new FightsContext();
        }

        public ActionResult UserPage(int? UserId) //User
        {
            Fighter fighter = fightsContext.Fighters.Find(UserId);

            if (fighter == null)
            {
                return HttpNotFound();
            }

            return View(CreateUserPageModelByFighter(fighter));
        }

        // place to UserPageModel 
        private UserPageViewModel CreateUserPageModelByFighter(Fighter fighter)
        {
            UserPageViewModel fighterModel = new UserPageViewModel()
            {
                Id = fighter.Id,
                Name = fighter.Name,
                Age = fighter.Age,
                Weight = fighter.Weight
            };

            var WinFights = fightsContext.Fights.Where(f => f.Winner.Id == fighter.Id);
            fighterModel.Rating = WinFights.Count() == 0 ? 0 : WinFights.Sum(p => p.Judge1 + p.Judge2 + p.Judge3);

            fighterModel.fights = from fight in fightsContext.Fights.Where(f => f.Winner.Id == fighter.Id || f.Losser.Id == fighter.Id)
                                  let result = fight.Winner.Id == fighter.Id
                                  join enemy in fightsContext.Fighters on (result ? fight.Losser.Id : fight.Winner.Id) equals enemy.Id
                                  select new FightUserPageViewModel
                                  { EnemyName = enemy.Name, Id = fight.FightId, Result = result, Rounds = fight.Rounds, Time = fight.Time };

            return fighterModel;
        }


        [HttpPost]
        public ActionResult AddFight(FightPageViewModel fight) //User (Fight)
        {
            AddFightByFightPageModel(fight);
            return RedirectToAction(nameof(UserPage), new { UserId = fight.FighterId });
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


        public ActionResult FightPage(int? FightId, int? FighterId) //User (Fight)
        {
            if (FightId == null || FighterId == null)
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


    }
}