using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
using Data;
using Domain;

namespace Processor
{
    public class WealthResetter
    {
        private BusinessLogic _logic;

        public WealthResetter(BusinessLogic logic)
        {
            _logic = logic;
        }

        public void ResetWealth()
        {
            var date = _logic.GetLastResetDate().ToLocalTime();
            var today = DateTime.Today;
            if (date.Date < today)
            {
                if (date.Year != today.Year)
                {
                    _logic.ResetWealth(RankType.Year);
                }
                else if (date.Month != today.Month)
                {
                    _logic.ResetWealth(RankType.Month);
                }
                
                if (date.DayOfWeek > today.DayOfWeek)
                {
                    _logic.ResetWealth(RankType.Week);
                }

                if (date.Year == today.Year &&
                    date.Month == today.Month &&
                    date.DayOfWeek <= today.DayOfWeek)
                {
                    _logic.ResetWealth();
                }
            }
        }
    }
}
