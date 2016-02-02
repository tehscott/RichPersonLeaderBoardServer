using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain;

namespace Processor
{
    public class WealthResetter
    {
        private IDao _dao;

        public WealthResetter(IDao dao)
        {
            _dao = dao;
        }

        public void ResetWealth()
        {
            var date = _dao.GetLastResetDate().ToLocalTime();
            var today = DateTime.Today;
            if (date.Date < today)
            {
                if (date.Year != today.Year)
                {
                    _dao.ResetWealth(RankType.Year);
                }
                else if (date.Month != today.Month)
                {
                    _dao.ResetWealth(RankType.Month);
                }
                
                if (date.DayOfWeek > today.DayOfWeek)
                {
                    _dao.ResetWealth(RankType.Week);
                }

                if (date.Year == today.Year &&
                    date.Month == today.Month &&
                    date.DayOfWeek <= today.DayOfWeek)
                {
                    _dao.ResetWealth();
                }
            }
        }
    }
}
