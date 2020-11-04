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
    public class AdminController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Admin
        public ActionResult Index()
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insurees = (from c in db.Insurees
                                where c.Id > 0
                                select c).ToList();
                var insureeVms = new List<Insuree>();

                foreach (Insuree insuree in insurees)
                {
                    var insureeVm = new Insuree();
                    insureeVm.Id = insuree.Id;
                    insureeVm.FirstName = insuree.FirstName;
                    insureeVm.LastName = insuree.LastName;
                    insureeVm.EmailAddress = insuree.EmailAddress;
                    insureeVm.Quote = insuree.Quote;
                    insureeVms.Add(insureeVm);
                }
                return View(insureeVms);
            }

        }
    }
}
