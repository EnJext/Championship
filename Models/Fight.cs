using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationFramework.Models
{
    public class Fight
    {
        public int FightId { get; set; }
        public DateTime Time { get; set; }
        public int Rounds { get; set; }

        [Column("Winner_Id")]
        public int Winner_Id { get; set; }
        public Fighter Winner { get; set; }

        [Column("Losser_Id")]
        public int Losser_Id { get; set; }
        public Fighter Losser { get; set; }
        public int Judge1 { get; set; }
        public int Judge2 { get; set; }
        public int Judge3 { get; set; }
    }
}