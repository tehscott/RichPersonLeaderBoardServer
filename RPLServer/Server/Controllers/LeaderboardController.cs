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
    //[RequireHttps]
    public class LeaderboardController : Controller
    {
        public static BusinessLogic Business { get; }

        static LeaderboardController()
        {
            Business = new BusinessLogic();
        }

        [HttpGet]
        public ActionResult Persons(RankType rankType = RankType.AllTime, int offset = 0, int perPage = 100)
        {
            return Json(Business.GetPersons(rankType, offset: offset, perPage: perPage), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void ResetWealth(RankType rankType = RankType.Day)
        {
            Business.ResetWealth(rankType);
        }

        [HttpGet]
        public ActionResult Person(string id)
        {
            return Json(new List<Person> { Business.GetPerson(id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PersonByName(string name)
        {
            return Json(new List<Person> { Business.GetPerson(name) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Person(CreatePersonRequest request)
        {
            return Json(Business.CreatePerson(request.Name, request.GoogleId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Payments(string id)
        {
            return Json(Business.GetPayments(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// TODO: this method should probably be AXED, however it is useful for debugging.
        /// </summary>
        /// <param name="request"></param>
        [HttpPost]
        public void Payments(CreatePaymentRequest request)
        {
            Business.CreatePayment(request.GoogleId, request.Amount);
        }

        [HttpGet]
        public ActionResult Achievements(string id)
        {
            return Json(Business.GetAchievements(id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Achievements(CreateAchievementRequest request)
        {
            Business.CreateAchievement(request.GoogleId, request.AchievementType);
        }

        [HttpGet]
        public ActionResult GetPersonAndSurroundingPeople(string id, RankType rankType = RankType.Day, int range = 5)
        {
            return Json(Business.GetPersonAndSurroundingPeople(id, range, rankType), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RecordPurchase(PurchaseRecord record)
        {
            var purchaseData = Business.VerifyPurchase(record);

            if (purchaseData != null)
            {
                Business.CreatePayment(purchaseData);
            }

            return Json(new List<object> {
                new { success = purchaseData != null }
            });
        }
    }
}