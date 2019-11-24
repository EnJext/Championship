
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





    }
}