using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using System.Web.Mvc;
using ClientApp.Models;

namespace ClientApp.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contacts
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //var contacts =db.Contacts.ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "client_name" ? "client_desc" : "client_name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;



            var contacts = from b in db.Contacts
                       select b;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                var con = from x in db.Contacts
                          select new { contac = x, Name = x.Surname + " " + x.FirstName + " " + x.OtherNames, RevName = x.FirstName + " " + x.OtherNames + " " + x.Surname};
                contacts = con.Where(s=>s.Name.Contains(searchString) || s.RevName.Contains(searchString)).Select(s=>s.contac);
                            
                
            }
            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(g=>g.Surname);
                    break;
                case "client_name":
                    contacts = contacts.OrderBy(s => s.OtherNames);
                    break;

                default:  // Name ascending 
                    contacts = contacts.OrderBy(g => g.Surname);
                    break;
            }
            ViewBag.Count = contacts.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));
            
            //return View(contacts);
        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {

            var contacts = from s in db.Contacts
                          where (s.FirstName.Contains(prefix) || s.Surname.Contains(prefix) || s.OtherNames.Contains(prefix))
                          select new { label = s.Surname + " " + s.FirstName+ " "+ s.OtherNames, val = s.Code };


            return Json(contacts);

        }
        // GET: Contacts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Surname,FirstName,OtherNames,EmailAddress,PhoneNumber")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Code = Guid.NewGuid().ToString().Substring(0,16);
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Surname,FirstName,OtherNames,EmailAddress,PhoneNumber")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
