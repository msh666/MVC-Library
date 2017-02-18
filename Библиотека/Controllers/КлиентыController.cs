using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Библиотека;
using PagedList.Mvc;
using PagedList;

namespace Библиотека.Controllers
{
    public class КлиентыController : Controller
    {
        private БиблиотекаEntities db = new БиблиотекаEntities();

        // GET: Клиенты
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;

            ViewBag.SurnameSortParm = sortOrder == "Surname" ? "surname_desc" : "Surname";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.PatrSortParm = sortOrder == "Patr" ? "patr_desc" : "Patr";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var clients = db.Клиенты.OrderBy(x => x.ID_Клиент);

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.Фамилия.Contains(searchString)
                                       || s.Имя.Contains(searchString) || s.Отчество.Contains(searchString)).OrderBy(s => s.ID_Клиент);
            }

            switch (sortOrder)
            {
                case "Surname":
                    clients = clients.OrderBy(s => s.Фамилия);
                    break;             
                case "surname_desc":
                    clients = clients.OrderByDescending(s => s.Фамилия);
                    break;
                case "Name":
                    clients = clients.OrderBy(s => s.Имя);
                    break;
                case "name_desc":
                    clients = clients.OrderByDescending(s => s.Имя);
                    break;
                case "Patr":
                    clients = clients.OrderBy(s => s.Отчество);
                    break;
                case "patr_desc":
                    clients = clients.OrderByDescending(s => s.Отчество);
                    break;
                case "Date":
                    clients = clients.OrderBy(s => s.Дата_рождения);
                    break;
                case "date_desc":
                    clients = clients.OrderByDescending(s => s.Дата_рождения);
                    break;
                default:
                    clients = clients.OrderBy(s => s.ID_Клиент);
                    break;

            }

            return View(clients.ToPagedList(pageNumber, 10));
        }

        // GET: Клиенты/Details/5
        [Authorize]
        public ActionResult Details(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DateFinishSortParm = sortOrder == "DateFinish" ? "datefinish_desc" : "DateFinish";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var client = db.Клиент_Книга.Include(к => к.Клиенты).Include(к => к.Книги).Where(x => x.ID_Клиент == id).OrderBy(x => x.Дата_выдачи);

            if (!String.IsNullOrEmpty(searchString))
            {
                client = client.Where(s => s.Книги.Название.Contains(searchString)).OrderBy(s => s.Дата_выдачи);
            }

            switch (sortOrder)
            {               
                case "Title":
                    client = client.OrderBy(s => s.Книги.Название);
                    break;
                case "title_desc":
                    client = client.OrderByDescending(s => s.Книги.Название);
                    break;
                case "DateFinish":
                    client = client.OrderBy(s => s.Дата_выдачи);
                    break;
                case "datefinish_desc":
                    client = client.OrderByDescending(s => s.Дата_выдачи);
                    break;
                default:
                    client = client.OrderBy(s => s.Дата_выдачи);
                    break;
            }

            return View(client.ToPagedList(pageNumber, 10));
        }

        [Authorize]
        public ActionResult Debtors(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DateFinishSortParm = sortOrder == "DateFinish" ? "datefinish_desc" : "DateFinish";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var client = db.Клиент_Книга.Include(к => к.Клиенты).Include(к => к.Книги).Where(x => x.Дата_возврата == null && x.ID_Клиент == id).OrderBy(x => x.Дата_выдачи);

            if (!String.IsNullOrEmpty(searchString))
            {
                client = client.Where(s => s.Книги.Название.Contains(searchString)).OrderBy(s => s.Дата_выдачи);
            }

            switch (sortOrder)
            {
                case "Title":
                    client = client.OrderBy(s => s.Книги.Название);
                    break;
                case "title_desc":
                    client = client.OrderByDescending(s => s.Книги.Название);
                    break;
                case "DateFinish":
                    client = client.OrderBy(s => s.Дата_выдачи);
                    break;
                case "datefinish_desc":
                    client = client.OrderByDescending(s => s.Дата_выдачи);
                    break;
                default:
                    client = client.OrderBy(s => s.Дата_выдачи);
                    break;
            }

            return View(client.ToPagedList(pageNumber, 10));
        }

        // GET: Клиенты/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Клиенты/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Клиент,Фамилия,Имя,Отчество,Дата_рождения")] Клиенты клиенты)
        {
            if (ModelState.IsValid)
            {
                db.Клиенты.Add(клиенты);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(клиенты);
        }

        // GET: Клиенты/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Клиенты клиенты = db.Клиенты.Find(id);
            if (клиенты == null)
            {
                return HttpNotFound();
            }
            return View(клиенты);
        }

        // POST: Клиенты/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Клиент,Фамилия,Имя,Отчество,Дата_рождения")] Клиенты клиенты)
        {
            if (ModelState.IsValid)
            {
                db.Entry(клиенты).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(клиенты);
        }

        // GET: Клиенты/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Клиенты клиенты = db.Клиенты.Find(id);
            if (клиенты == null)
            {
                return HttpNotFound();
            }
            return View(клиенты);
        }

        // POST: Клиенты/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Клиенты клиенты = db.Клиенты.Find(id);
            db.Клиенты.Remove(клиенты);
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
