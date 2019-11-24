using System.Linq;
using System.Web.Mvc;
using WebApplicationFramework.Models;
using System.Data.SqlClient;

namespace WebApplicationFramework.Controllers
{
    public class FightController : Controller
    {
        private FightsContext fightsContext;
        public FightController()
        {
            fightsContext = new FightsContext();
        }

        [HttpPost]
        public ActionResult AddFight(Fight fight, int FighterId)
        {
            fight.Losser = fightsContext.Fighters.FirstOrDefault(f => f.Id == (int)TempData["LosserId"]);
            fight.Winner = fightsContext.Fighters.FirstOrDefault(f => f.Id == (int)TempData["WinnerId"]);
            fight.FightId = 0;
            fightsContext.AddFight(fight);
            return RedirectToAction("UserPage", "User", new { UserId = FighterId });
        }

        private void AddFightByFightPageModel(FightPageViewModel fight)
        {
            Fighter Winner = fightsContext.Fighters.Where(p => p.Name == fight.Winner).FirstOrDefault();
            Fighter Losser = fightsContext.Fighters.Where(p => p.Name == fight.Losser).FirstOrDefault();

            if (Winner != null && Losser != null)
            {
                fightsContext.AddFight(new Fight
                {
                    FightId = fight.FightId,
                    Judge1 = fight.Judge1,
                    Judge2 = fight.Judge2,
                    Judge3 = fight.Judge3,
                    Winner = Winner,
                    Losser = Losser,
                    Rounds = fight.Rounds,
                    Time = fight.Time
                });
            }
        }


        public ActionResult FightPage(int? FightId, int? FighterId)  //(Fight)
        {
            if (FightId == null || FighterId == null)
            {
                return Content(" Error: FightId or FighterId key is null");
            }

            Fight fight = fightsContext.Fights.Find(FightId);

            fightsContext.Entry(fight).Reference(s => s.Winner).Load();
            fightsContext.Entry(fight).Reference(s => s.Losser).Load();

            TempData["WinnerId"] = fight.Winner.Id;
            TempData["LosserId"] = fight.Losser.Id;
            TempData["FighterId"] = FighterId.Value;

            return PartialView("FightPage", fight);
        }
    }
}