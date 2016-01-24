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

        public ActionResult Index()
        {
            return Json(Dao.GetPersons(0, 100), JsonRequestBehavior.AllowGet);
        }
    }
}