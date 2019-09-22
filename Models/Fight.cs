using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFramework.Models
{
    public class Fight
    {
        public int FightId { get; set; }
        public DateTime Time { get; set; }
        public int Rounds { get; set; }
        public int Winner_Id { get; set; }
        public Fighter Winner { get; set; }
        public Fighter Losser { get; set; }
        public int Judge1 { get; set; }
        public int Judge2 { get; set; }
        public int Judge3 { get; set; }
    }
}