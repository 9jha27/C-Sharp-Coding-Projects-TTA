using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = CalculateQuote(insuree);
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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

        public decimal CalculateQuote(Insuree insuree)
        {
            decimal monthlyBase = 50;
            decimal agePrice;
            decimal yearPrice;
            decimal carPrice;
            decimal ticketPrice;
            decimal duiPrice;
            decimal coveragePrice;
            
            

            //figure out the insuree's age
            var today = DateTime.Today;
            var ageInDays = today.Subtract(insuree.DateOfBirth).TotalDays;
            var insureeAge = (ageInDays / 365); 

            
            //if 18 and under
            if (insureeAge <= 18) 
                {
                    agePrice = 100;
                }

                else if (insureeAge > 18 && insureeAge < 26)
                {
                    agePrice = 50;
                }

                else
                {
                    agePrice = 25;
                }

           if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
            {
                yearPrice = 25;
            }
               else
                {
                    yearPrice = 0;
                }

           if (insuree.CarMake == "Porsche")
            {
                carPrice = 25;
                if (insuree.CarModel == "911 Carrera")
                {
                    carPrice = 50;
                }
            }
                else
                {
                    carPrice = 0;
                }


            //speeding ticket
            if (insuree.SpeedingTickets >= 1)
            {
                ticketPrice = insuree.SpeedingTickets * 10;
                
            }
            else
            {
                ticketPrice = insuree.SpeedingTickets;
            }

            decimal total = monthlyBase + agePrice + yearPrice + carPrice + ticketPrice;

            ////DUI
            if (insuree.DUI == true)
            {
                duiPrice = total * 1.25M; 
            }
            else
            {
                duiPrice = total;
            }

            //full coverage
            if (insuree.CoverageType == true)
            {
                coveragePrice  = duiPrice * 1.5M;
            }
            else
            {
                coveragePrice = duiPrice;
            }

            return coveragePrice;
        }

        public ActionResult Admin()
        {
            return View("Admin");
        }
    }
}
