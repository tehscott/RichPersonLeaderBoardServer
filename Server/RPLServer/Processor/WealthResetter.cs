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
            var date = _dao.GetLastResetDate();
            if (date.Date < DateTime.Today)
            {
                if (date.Year != DateTime.Today.Year)
                {
                    _dao.ResetWealth(RankType.Year);
                }
                else if (date.Month != DateTime.Today.Month)
                {
                    _dao.ResetWealth(RankType.Month);
                }
                
                if (date.DayOfWeek > DateTime.Today.DayOfWeek)
                {
                    _dao.ResetWealth(RankType.Week);
                }

                if (date.Year == DateTime.Today.Year &&
                    date.Month == DateTime.Today.Month &&
                    date.DayOfWeek <= DateTime.Today.DayOfWeek)
                {
                    _dao.ResetWealth();
                }
            }
        }
    }
}
