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
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace SmartTruckerApp5.Controllers
{
    public class paymentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        // GET: payments
        public ActionResult Index()
        {
            //var paymentobj = db.Paymentobj.Include(p => p.cargodetails);

            var userIDLogg = User.Identity.GetUserId();
            List<paymentViewModel> plist = new List<paymentViewModel>();
            var TheGetter = (from gh in db.Paymentobj
                             join hj in db.Users
                             on gh.usersID equals hj.Id
                             where gh.usersID==userIDLogg
                             select new { gh, hj.UserName }).ToList();


            foreach(var item in TheGetter)
            {
                paymentViewModel pvm = new paymentViewModel();
                pvm.paymentKey = item.gh.paymentKey;
                pvm.cargoID = item.gh.cargoID;
                pvm.amountpayed = item.gh.amountpayed;
                pvm.DocumentDate = item.gh.DocumentDate;
                pvm.File = item.gh.File;
                pvm.approval = item.gh.approval;
                pvm.usersID = item.UserName;
                plist.Add(pvm);
            }

            return View(plist);
        }
        [Authorize]
        // GET: payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment payment = db.Paymentobj.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }
        [Authorize]
        // GET: payments/Create
        public ActionResult Create()
        {
            ViewBag.cargoID = new SelectList(db.cargoDetails, "CargoDetailsKey", "CargoDetailsKey");
            return View();
        }
        public byte[] ConvertToBytes(HttpPostedFileBase files)
        {

            BinaryReader reader = new BinaryReader(files.InputStream);
            return reader.ReadBytes(files.ContentLength);
        }
        // POST: payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "paymentKey,cargoID,PaymentType,file,amountpayed,DocumentDate,usersID")] payment Payment, HttpPostedFileBase files)
        {

            if (files != null && files.ContentLength > 0)
            {
                Payment.File = ConvertToBytes(files);
            }
            if (ModelState.IsValid)
            {
                Payment.approval = "Transactions Not Approved";
                Payment.usersID = User.Identity.GetUserId();
                Payment.DocumentDate = DateTime.Now.Date;
                db.Paymentobj.Add(Payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            ViewBag.cargoID = new SelectList(db.cargoDetails, "CargoDetailsKey", "CargoDetailsKey");
            return View(Payment);
        }
        [Authorize]
        // GET: payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment payment = db.Paymentobj.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.cargoID = new SelectList(db.cargoDetails, "CargoDetailsKey", "CargoStatus", payment.cargoID);
            return View(payment);
        }

        // POST: payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(payment Payment )
        {
            if (ModelState.IsValid)
            {
                var updateApproval = db.Paymentobj.Find(Payment.paymentKey);
                updateApproval.approval = Payment.approval;
                db.Entry(updateApproval).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cargoID = new SelectList(db.cargoDetails, "CargoDetailsKey", "CargoStatus", Payment.cargoID);
            return View(Payment);
        }
        [Authorize(Roles = "Administrator")]
        // GET: payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            payment payment = db.Paymentobj.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }
        [Authorize(Roles = "Administrator")]
        // POST: payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            payment payment = db.Paymentobj.Find(id);
            db.Paymentobj.Remove(payment);
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
