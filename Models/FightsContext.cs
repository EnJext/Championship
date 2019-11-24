using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;

namespace WebApplicationFramework.Models
{
    public class FightsContext : DbContext
    {
        public FightsContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FightsContext, WebApplicationFramework.Migrations.Configuration>());
        }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Fighter> Fighters { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fight>().Property(f => f.Time).HasColumnType("date");

            // настройка one-to-many связи для победителя 
            modelBuilder.Entity<Fight>()
                .HasRequired(fight => fight.Winner)
                .WithMany(winner => winner.WinFights)
                .HasForeignKey<int>(fight => fight.Winner_Id)
                .WillCascadeOnDelete(true);

            // настройка one-to-many связи для проигравшего
            modelBuilder.Entity<Fight>()
                .HasRequired(fight => fight.Losser)
                .WithMany(losser => losser.LoseFights)
                .HasForeignKey<int>(fight => fight.Losser_Id)
                .WillCascadeOnDelete(true);
        }

        public void AddFight(Fight fight)
        {

            Fight fightToChange;
            if (fight.FightId == 0)
            {
                var TimeParam = new SqlParameter("@Time", fight.Time);
                var RoundsPram = new SqlParameter("@Rounds", fight.Rounds);
                var WinnerIdParam = new SqlParameter("@WinnerId", fight.Winner.Id);
                var LosserIdParam = new SqlParameter("@LosserId", fight.Losser.Id);
                var Judge1Param = new SqlParameter("@Judge1", fight.Judge1);
                var Judge2Param = new SqlParameter("@Judge2", fight.Judge2);
                var Judge3Param = new SqlParameter("@Judge3", fight.Judge3);

                Database.ExecuteSqlCommand("exec AddFight @Time, @Rounds, @WinnerId, @LosserId," +
                    "@Judge1, @Judge2, @Judge3", TimeParam, RoundsPram, WinnerIdParam, LosserIdParam, Judge1Param,
                    Judge2Param, Judge3Param);
            }
            else if ((fightToChange = Fights.Where(f => f.FightId == fight.FightId).FirstOrDefault())!=null)
            {
                fightToChange.Judge1 = fight.Judge1;
                fightToChange.Judge2 = fight.Judge2;
                fightToChange.Judge3 = fight.Judge3;
                fightToChange.Time = fight.Time;
                fightToChange.Winner = fight.Winner;
                fightToChange.Losser = fight.Losser;
                fightToChange.Rounds = fight.Rounds;
                this.SaveChanges();
            }
            
        }
    }
}