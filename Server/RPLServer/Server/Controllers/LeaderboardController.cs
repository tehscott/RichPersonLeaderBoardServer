using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain;
using System.Web.Mvc;
using Business;
using Server.Models;

namespace Server.Controllers
{
    public class LeaderboardController : Controller
    {
        public static BusinessLogic Business { get; }

        static LeaderboardController()
        {
            Business = new BusinessLogic();
        }

        [HttpGet]
        public ActionResult Persons(int offset = 0, int perPage = 100)
        {
            return Json(Business.GetPersons(offset, perPage), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Person(int id)
        {
            return Json(Business.GetPerson(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Person(CreatePersonRequest request)
        {
            return Json(Business.CreatePerson(request.Name), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Payments(int id, int offset = 0, int perPage = int.MaxValue)
        {
            return Json(Business.GetPayments(id, offset, perPage), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Payments(CreatePaymentRequest request)
        {
            Business.CreatePayment(request.PersonId, request.Amount);
        }

        [HttpGet]
        public ActionResult Achievements(int id)
        {
            return Json(Business.GetAchievements(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Achievements(CreateAchievementRequest request)
        {
            Business.CreateAchievement(request.PersonId, request.AchievementType);
        }
    }
}