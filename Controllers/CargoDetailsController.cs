using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartTruckerApp5.Models;
using Microsoft.AspNet.Identity;

namespace SmartTruckerApp5.Controllers
{
    public class CargoDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        // GET: CargoDetails
        public ActionResult Index()
        {
            var cargoDetails = db.cargoDetails.Include(c => c.CrgType).Include(c => c.transaction);

            var custId = User.Identity.GetUserId();

            if (User.IsInRole("Customer"))
            {

                return View(cargoDetails.Where(b => b.CustomerID == custId).ToList());
            }
            else
            {

                return View(cargoDetails.ToList());
            }

           

        }

        // GET: CargoDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CargoDetails cargoDetails = db.cargoDetails.Find(id);
            if (cargoDetails == null)
            {
                return HttpNotFound();
            }
            return View(cargoDetails);
        }
        [Authorize]
        // GET: CargoDetails/Create
        public ActionResult Create()
        {
            ViewBag.cargNo = new SelectList(db.cargoTypes, "CargoTypeKey", "CargoName");

            var Customer = (from gh in db.userRoles
                          join jk in db.Users
                          on gh.userKey equals jk.Id
                          where gh.roleKey == "Customer"
                          select new { jk.Id, jk.UserName }).ToList();


            ViewBag.CustomerID = new SelectList(Customer, "Id","UserName");
            
            ViewBag.Destination = new SelectList(db.locations, "LocationKey", "LocationName");
            ViewBag.PickPoint = new SelectList(db.locations, "LocationKey", "LocationName");
            ViewBag.TransactionId = new SelectList(db.transactions, "TransactionsKey", "TransactionsKey");
            return View();
        }

        // POST: CargoDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "CargoDetailsKey,CargoStatus,EstimatedTravellig,CargoWeight,Destination,PickPoint,CargoPayment,CustomerID,TransactionId,Loc_Code,cargNo")] CargoDetails cargoDetails)
        {
            if (ModelState.IsValid)
            {
                cargoDetails.CargoStatus = "Not Yet Collected";
                cargoDetails.CargoPayment = cargoDetails.calccargo();
                if(User.IsInRole("Customer"))
                {
                    cargoDetails.CustomerID = User.Identity.GetUserName();
                }

                db.cargoDetails.Add(cargoDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cargNo = new SelectList(db.cargoTypes, "CargoTypeKey", "CargoName", cargoDetails.cargNo);
            var Customer = (from gh in db.userRoles
                            join jk in db.Users
                            on gh.userKey equals jk.Id
                            where gh.roleKey == "Customer"
                            select new { jk.Id, jk.UserName }).ToList();


            ViewBag.CustomerID = new SelectList(Customer, "Id", "UserName");
            ViewBag.Loc_Code = new SelectList(db.locations, "LocationKey", "LocationName");
            ViewBag.TransactionId = new SelectList(db.transactions, "TransactionsKey", "TransactionsKey", cargoDetails.TransactionId);
            return View(cargoDetails);
        }

        [Authorize]
        // GET: CargoDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CargoDetails cargoDetails = db.cargoDetails.Find(id);
            if (cargoDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.cargNo = new SelectList(db.cargoTypes, "CargoTypeKey", "CargoName", cargoDetails.cargNo);
            var Customer = (from gh in db.userRoles
                            join jk in db.Users
                            on gh.userKey equals jk.Id
                            where gh.roleKey == "Customer"
                            select new { jk.Id, jk.UserName }).ToList();


            ViewBag.CustomerID = new SelectList(Customer, "Id", "UserName");
            ViewBag.Loc_Code = new SelectList(db.locations, "LocationKey", "LocationName");
            ViewBag.TransactionId = new SelectList(db.transactions, "TransactionsKey", "TransactionsKey", cargoDetails.TransactionId);
            return View(cargoDetails);
        }

        // POST: CargoDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CargoDetailsKey,CargoStatus,EstimatedTravellig,CargoWeight,Destination,PickPoint,CargoPayment,CustomerID,TransactionId,Loc_Code,cargNo")] CargoDetails cargoDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargoDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cargNo = new SelectList(db.cargoTypes, "CargoTypeKey", "CargoName", cargoDetails.cargNo);
            var Customer = (from gh in db.userRoles
                            join jk in db.Users
                            on gh.userKey equals jk.Id
                            where gh.roleKey == "Customer"
                            select new { jk.Id, jk.UserName }).ToList();


            ViewBag.CustomerID = new SelectList(Customer, "Id", "UserName");
            ViewBag.Loc_Code = new SelectList(db.locations, "LocationKey", "LocationName");
            ViewBag.TransactionId = new SelectList(db.transactions, "TransactionsKey", "TransactionsKey", cargoDetails.TransactionId);
            return View(cargoDetails);
        }
        [Authorize]
        // GET: CargoDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CargoDetails cargoDetails = db.cargoDetails.Find(id);
            if (cargoDetails == null)
            {
                return HttpNotFound();
            }
            return View(cargoDetails);
        }
        [Authorize]
        // POST: CargoDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CargoDetails cargoDetails = db.cargoDetails.Find(id);
            db.cargoDetails.Remove(cargoDetails);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
