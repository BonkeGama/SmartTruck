using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SmartTruckerApp5.Models;

namespace SmartTruckerApp5.Controllers
{
    public class TrucksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trucks
        public ActionResult Index()
        {
            return View(db.trucks.ToList());
        }

        // GET: Trucks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trucks trucks = db.trucks.Find(id);
            if (trucks == null)
            {
                return HttpNotFound();
            }
            return View(trucks);
        }

        // GET: Trucks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trucks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrucksKey,RegistrationNo,TruckStatus,TruckPrice")] Trucks trucks)
        {
            if (ModelState.IsValid)
            {
                if (trucks.TruckRegChecker() == true)
                {
                    ViewBag.message = "The Truck has already Exist!!!";
                    return View(trucks);
                }
                else
                {
                    db.trucks.Add(trucks);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(trucks);
        }

        // GET: Trucks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trucks trucks = db.trucks.Find(id);
            if (trucks == null)
            {
                return HttpNotFound();
            }
            return View(trucks);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrucksKey,RegistrationNo,TruckStatus,TruckPrice")] Trucks trucks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trucks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trucks);
        }

        // GET: Trucks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trucks trucks = db.trucks.Find(id);
            if (trucks == null)
            {
                return HttpNotFound();
            }
            return View(trucks);
        }

        // POST: Trucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trucks trucks = db.trucks.Find(id);
            db.trucks.Remove(trucks);
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
