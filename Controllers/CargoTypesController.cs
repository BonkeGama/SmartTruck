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
    public class CargoTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize(Roles = "Clerk")]
        // GET: CargoTypes
        public ActionResult Index()
        {
            return View(db.cargoTypes.ToList());
        }
        [Authorize(Roles = "Clerk")]
        // GET: CargoTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CargoType cargoType = db.cargoTypes.Find(id);
            if (cargoType == null)
            {
                return HttpNotFound();
            }
            return View(cargoType);
        }
        [Authorize(Roles = "Clerk")]

        // GET: CargoTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CargoTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clerk")]
        public ActionResult Create([Bind(Include = "CargoTypeKey,CargoName,CargoValue")] CargoType cargoType)
        {
            if (ModelState.IsValid)
            {
                db.cargoTypes.Add(cargoType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cargoType);
        }

        // GET: CargoTypes/Edit/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CargoType cargoType = db.cargoTypes.Find(id);
            if (cargoType == null)
            {
                return HttpNotFound();
            }
            return View(cargoType);
        }

        // POST: CargoTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit([Bind(Include = "CargoTypeKey,CargoName,CargoValue")] CargoType cargoType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargoType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargoType);
        }

        // GET: CargoTypes/Delete/5 
        [Authorize(Roles ="Clerk")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CargoType cargoType = db.cargoTypes.Find(id);
            if (cargoType == null)
            {
                return HttpNotFound();
            }
            return View(cargoType);
        }

        // POST: CargoTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clerk")]
        public ActionResult DeleteConfirmed(int id)
        {
            CargoType cargoType = db.cargoTypes.Find(id);
            db.cargoTypes.Remove(cargoType);
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
