using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
using Data;
using Processor;

namespace ManualProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var logic = new BusinessLogic();
            TimeBasedAchievementAwarder awarder = new TimeBasedAchievementAwarder(logic);
            awarder.AwardTimeBasedAchievements();

            WealthResetter resetter = new WealthResetter(logic);
            resetter.ResetWealth();
        }
    }
}
