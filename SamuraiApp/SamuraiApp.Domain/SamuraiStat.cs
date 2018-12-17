using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
    public class SamuraiStat
    {
        public int SamuraiId { get; private set; }
        public string Name { get; private set; }
        public int NumberOfBattles { get; private set; }
        public string EarliestBattle { get; private set; }
    }
}
