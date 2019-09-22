using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebApplicationFramework.Models
{
    public class FightModelForChampPage
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Losser { get; set; }
        public string button { get; set; }

    }
}