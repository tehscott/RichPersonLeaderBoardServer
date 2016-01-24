using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using System.Web.Mvc;
using Data;

namespace Server.Controllers
{
    public class LeaderboardController : Controller
    {
        public PersonDao Dao { get; set; }

        public LeaderboardController()
        {
            Dao = new PersonDao();
        }

        public ActionResult Index(int offset = 0, int perPage = 100)
        {
            return Json(Dao.GetPersons(offset, perPage), JsonRequestBehavior.AllowGet);
        }
    }
}