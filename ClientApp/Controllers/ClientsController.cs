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
    public class ClientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clients
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            //var clients = db.Clients.Include(c => c.ClientContacts);
            
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



            var clients = from s in db.Clients
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    clients = clients.OrderByDescending(s => s.Name);
                    break;
                case "client_name":
                    clients = clients.OrderBy(s => s.Name);
                    break;

                default:  // Name ascending 
                    clients = clients.OrderBy(s => s.Name);
                    break;
            }
            ViewBag.Count = clients.Count();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(clients.ToPagedList(pageNumber, pageSize));
            //return View(clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            ViewBag.Count = db.ClientContacts.Where(s => s.ClientCode == id).Count();
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {

            var clients = from s in db.Clients
                       where (s.Name.Contains(prefix) || s.Description.Contains(prefix)) 
                       select new { label = s.Name, val = s.Code };


            return Json(clients);

        }
       
        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.Code = new SelectList(db.Clients, "Code", "Name");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Website,Fax,Address,Description,DateCreated")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.Code = CheckifExist(client.Name);
                client.DateCreated = DateTime.Now;
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Code = new SelectList(db.Clients, "Code", "Name", client.Code);
            return View(client);
        }
        public string CheckifExist(string name)
        {
            
            string newcode=GetNextNumber(name);
            var code = db.Clients.Where(x=>x.Code==newcode).Count();
            while (code > 0)
            {
                newcode = GetNextNumber(name);
                code = db.Clients.Where(x => x.Code == newcode).Count();
            }
            return newcode;
        }
        public static String RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static String RandomNum(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static String StringifyNum(int num)
        {
            string number = String.Format("{0:000}",num);
            return number;
        }
        public string GetNextNumber(string name)
        {
            string returnString = "";
            string returnNum = "";
            if (name.Length >= 3)
            {
                if(name.Contains(" "))
                {
                    string rename = "";
                    string[] newname = name.Split(' ');
                    foreach(var n in newname)
                    {
                        rename = rename + n.Substring(0, 1);
                    }
                    if (rename.Length < 3)
                    {
                        rename = rename + RandomString(3 - rename.Length);
                        returnString = rename;
                    }else if (rename.Length == 3)
                    {
                        returnString = rename;
                    }else if (rename.Length > 3)
                    {
                        returnString = rename.Substring(0, 3);
                    }
                }
                else
                {
                    returnString = name.Substring(0, 3);
                }

            }
            else
            {
                returnString = name + RandomString(3 - name.Length);
            }
            returnNum = RandomNum(3);
            string returncode = returnString.ToUpper() + returnNum;
            

            return returncode;
        }
        // GET: Clients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.Code = new SelectList(db.Clients, "Code", "Name", client.Code);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Website,Fax,Address,Description,DateCreated")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Code = new SelectList(db.Clients, "Code", "Name", client.Code);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
