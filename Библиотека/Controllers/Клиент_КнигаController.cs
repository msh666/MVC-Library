using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Библиотека;
using PagedList.Mvc;
using PagedList;

namespace Библиотека.Controllers
{
    public class Клиент_КнигаController : Controller
    {
        private БиблиотекаEntities db = new БиблиотекаEntities();

        // GET: Клиент_Книга
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DateStartSortParm = sortOrder == "DateStart" ? "datestart_desc" : "DateStart";
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

            var клиент_Книга = db.Клиент_Книга.Include(к => к.Клиенты).Include(к => к.Книги).OrderBy(x => x.Дата_выдачи);

            if (!String.IsNullOrEmpty(searchString))
            {
                клиент_Книга = клиент_Книга.Where(s => s.Клиенты.Фамилия.Contains(searchString)
                                       || s.Клиенты.Имя.Contains(searchString) || s.Клиенты.Отчество.Contains(searchString)
                                        || s.Книги.Название.Contains(searchString)).OrderBy(s => s.ID_Клиент);
            }

            switch (sortOrder)
            {
                case "Name":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Клиенты.Фамилия);
                    break;
                case "name_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Клиенты.Фамилия);
                    break;
                case "Title":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Книги.Название);
                    break;
                case "title_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Книги.Название);
                    break;
                case "DateStart":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Дата_выдачи);
                    break;
                case "datestart_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Дата_выдачи);
                    break;
                case "DateFinish":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Дата_возврата);
                    break;
                case "datefinish_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Дата_возврата);
                    break;
                default:
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Дата_выдачи);
                    break;
            }

            return View(клиент_Книга.ToPagedList(pageNumber, 10));
        }

        // GET: Клиент_Книга/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Клиент_Книга клиент_Книга = db.Клиент_Книга.Find(id);
            if (клиент_Книга == null)
            {
                return HttpNotFound();
            }
            return View(клиент_Книга);
        }

        // GET: Клиент_Книга/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ID_Клиент = new SelectList(db.Клиенты, "ID_Клиент", "Фамилия");
            ViewBag.ID_Книга = new SelectList(db.Книги, "ID_Книга", "Название");
            return View();
        }

        // POST: Клиент_Книга/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Клиент,ID_Книга,Дата_выдачи,Дата_возврата")] Клиент_Книга клиент_Книга)
        {
            if (ModelState.IsValid)
            {
                db.Клиент_Книга.Add(клиент_Книга);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Клиент = new SelectList(db.Клиенты, "ID_Клиент", "Фамилия", клиент_Книга.ID_Клиент);
            ViewBag.ID_Книга = new SelectList(db.Книги, "ID_Книга", "Название", клиент_Книга.ID_Книга);
            return View(клиент_Книга);
        }

        // GET: Клиент_Книга/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            // }
            Клиент_Книга клиент_Книга = db.Клиент_Книга.Find(id);
            if (клиент_Книга == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Клиент = new SelectList(db.Клиенты, "ID_Клиент", "Фамилия", клиент_Книга.ID_Клиент);
            ViewBag.ID_Книга = new SelectList(db.Книги, "ID_Книга", "Название", клиент_Книга.ID_Книга);
            return View(клиент_Книга);
        }

        // POST: Клиент_Книга/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Выдачи,ID_Клиент,ID_Книга,Дата_выдачи,Дата_возврата")] Клиент_Книга клиент_Книга)
        {
            if (клиент_Книга.Дата_возврата < клиент_Книга.Дата_выдачи)
            {
                ModelState.AddModelError("", "Wrong date");
                return View(клиент_Книга);
            }

            if (ModelState.IsValid)
            {
                db.Entry(клиент_Книга).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Клиент = new SelectList(db.Клиенты, "ID_Клиент", "Фамилия", клиент_Книга.ID_Клиент);
            ViewBag.ID_Книга = new SelectList(db.Книги, "ID_Книга", "Название", клиент_Книга.ID_Книга);
            return View(клиент_Книга);
        }

        // GET: Клиент_Книга/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Клиент_Книга клиент_Книга = db.Клиент_Книга.Find(id);
            if (клиент_Книга == null)
            {
                return HttpNotFound();
            }
            return View(клиент_Книга);
        }

        // POST: Клиент_Книга/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Клиент_Книга клиент_Книга = db.Клиент_Книга.Find(id);
            db.Клиент_Книга.Remove(клиент_Книга);
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

        public ActionResult Debtors(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNumber = (page ?? 1);
            
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DateStartSortParm = sortOrder == "DateStart" ? "datestart_desc" : "DateStart";
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

            var клиент_Книга = db.Клиент_Книга.Include(к => к.Клиенты).Include(к => к.Книги).Where(x => x.Дата_возврата == null).OrderBy(x => x.Дата_выдачи);

            if (!String.IsNullOrEmpty(searchString))
            {
                клиент_Книга = клиент_Книга.Where(s => s.Клиенты.Фамилия.Contains(searchString)
                                       || s.Клиенты.Имя.Contains(searchString) || s.Клиенты.Отчество.Contains(searchString)
                                        || s.Книги.Название.Contains(searchString)).OrderBy(s => s.ID_Клиент);
            }

            switch (sortOrder)
            {
                case "Name":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Клиенты.Фамилия);
                    break;
                case "name_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Клиенты.Фамилия);
                    break;
                case "Title":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Книги.Название);
                    break;
                case "title_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Книги.Название);
                    break;
                case "DateStart":
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Дата_выдачи);
                    break;
                case "datestart_desc":
                    клиент_Книга = клиент_Книга.OrderByDescending(s => s.Дата_выдачи);
                    break;
                default:
                    клиент_Книга = клиент_Книга.OrderBy(s => s.Дата_выдачи);
                    break;
            }

            return View(клиент_Книга.ToPagedList(pageNumber, 10));
        }
    }
}
