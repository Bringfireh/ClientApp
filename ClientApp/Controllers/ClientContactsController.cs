using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClientApp.Models;

namespace ClientApp.Controllers
{
    [Authorize]
    public class ClientContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClientContacts
        public ActionResult Index()
        {
            var clientContacts = db.ClientContacts.Include(c => c.Client).Include(c => c.Contact);
            return View(clientContacts.ToList());
        }

        // GET: ClientContacts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientContact clientContact = db.ClientContacts.Find(id);
            if (clientContact == null)
            {
                return HttpNotFound();
            }
            return View(clientContact);
        }

        // GET: ClientContacts/Create
        public ActionResult Create()
        {
            ViewBag.ClientCode = new SelectList(db.Clients, "Code", "Name");
            ViewBag.ContactCode = new SelectList(db.Contacts, "Code", "Surname");
            return View();
        }

        // POST: ClientContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,ClientCode,ContactCode")] ClientContact clientContact)
        {
            if (ModelState.IsValid)
            {
                db.ClientContacts.Add(clientContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientCode = new SelectList(db.Clients, "Code", "Name", clientContact.ClientCode);
            ViewBag.ContactCode = new SelectList(db.Contacts, "Code", "Surname", clientContact.ContactCode);
            return View(clientContact);
        }

        // GET: ClientContacts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientContact clientContact = db.ClientContacts.Find(id);
            if (clientContact == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientCode = new SelectList(db.Clients, "Code", "Name", clientContact.ClientCode);
            ViewBag.ContactCode = new SelectList(db.Contacts, "Code", "Surname", clientContact.ContactCode);
            return View(clientContact);
        }

        // POST: ClientContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,ClientCode,ContactCode")] ClientContact clientContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientCode = new SelectList(db.Clients, "Code", "Name", clientContact.ClientCode);
            ViewBag.ContactCode = new SelectList(db.Contacts, "Code", "Surname", clientContact.ContactCode);
            return View(clientContact);
        }

        // GET: ClientContacts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClientContact clientContact = db.ClientContacts.Find(id);
            if (clientContact == null)
            {
                return HttpNotFound();
            }
            return View(clientContact);
        }

        // POST: ClientContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ClientContact clientContact = db.ClientContacts.Find(id);
            db.ClientContacts.Remove(clientContact);
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
