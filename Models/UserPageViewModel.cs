using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFramework.Models
{
    public class UserPageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public int Rating { get; set; }
        public IEnumerable<FightUserPageViewModel> fights { get; set; }
    }

    public class FightUserPageViewModel
    {
        public DateTime Time { get; set; }
        public int Rounds { get; set; }
        public string EnemyName { get; set; }
        public bool Result { get; set; }
        public int Id { get; set; }
    }
}