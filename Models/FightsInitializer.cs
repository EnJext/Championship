using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplicationFramework.Models
{
    public class FightsInitializer : DropCreateDatabaseIfModelChanges<FightsContext>
    {
        protected override void Seed(FightsContext db)
        {
            Fighter Valik = new Fighter { Age = 19, Name = "Новак Валентин", Weight = 79 };
            Fighter Vlad = new Fighter { Age = 15, Name = "Влад Самчук", Weight = 80 };
            db.Fighters.Add(Valik);
            db.Fighters.Add(Vlad);

            db.Fights.Add(new Fight { Judge1 = 1, Judge2 = 1, Judge3 = 1, Winner = Valik, Losser = Vlad, Rounds = 3, Time = DateTime.Now });
            db.Fights.Add(new Fight { Judge1 = 1, Judge2 = 1, Judge3 = 1, Winner = Valik, Losser = Vlad, Rounds = 3, Time = DateTime.Now });
            db.Fights.Add(new Fight { Judge1 = 1, Judge2 = 1, Judge3 = 1, Winner = Valik, Losser = Vlad, Rounds = 3, Time = DateTime.Now });
            db.Fights.Add(new Fight { Judge1 = 1, Judge2 = 1, Judge3 = 1, Winner = Vlad, Losser = Valik, Rounds = 3, Time = DateTime.Now });
            db.Fights.Add(new Fight { Judge1 = 1, Judge2 = 1, Judge3 = 1, Winner = Vlad, Losser = Valik, Rounds = 3, Time = DateTime.Now });

            base.Seed(db);
        }
    }
}