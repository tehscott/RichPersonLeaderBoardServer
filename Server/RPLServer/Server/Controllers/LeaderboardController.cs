using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using System.Web.Mvc;

namespace Server.Controllers
{
    public class LeaderboardController : Controller
    {
        public List<Person> Persons {
            get
            {
                return new List<Person>
                {
                    new Person {Name = "theTrueMikeBrown", Wealth = 100.00M},
                    new Person {Name = "tehScott", Wealth = 50.00M},
                    new Person {Name = "mattiam", Wealth = 0.00M}
                };
            }
        }

        public ActionResult Index()
        {
            return Json(Persons, JsonRequestBehavior.AllowGet);
        }
    }
}