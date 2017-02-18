using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Библиотека;
using PagedList.Mvc;
using PagedList;

namespace Библиотека.Controllers
{
    public class КнигиController : Controller
    {
        private БиблиотекаEntities db = new БиблиотекаEntities();

        // GET: Книги
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageNumber = (page ?? 1);

            ViewBag.CurrentSort = sortOrder;

            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
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

            var book = db.Книги.OrderBy(x => x.ID_Книга);

            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(s => s.Название.Contains(searchString)
                                       || s.Автор.Contains(searchString) || s.Год_написания.Contains(searchString)).OrderBy(s => s.ID_Книга);
            }

            switch (sortOrder)
            {
                case "Title":
                    book = book.OrderBy(s => s.Название);
                    break;
                case "title_desc":
                    book = book.OrderByDescending(s => s.Название);
                    break;
                case "Name":
                    book = book.OrderBy(s => s.Автор);
                    break;
                case "name_desc":
                    book = book.OrderByDescending(s => s.Автор);
                    break;
                case "Date":
                    book = book.OrderBy(s => s.Год_написания);
                    break;
                case "date_desc":
                    book = book.OrderByDescending(s => s.Год_написания);
                    break;
                default:
                    book = book.OrderBy(s => s.ID_Книга);
                    break;

            }

            return View(book.ToPagedList(pageNumber,10));
        }

        // GET: Книги/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Книги книги = db.Книги.Find(id);
            if (книги == null)
            {
                return HttpNotFound();
            }
            return View(книги);
        }

        // GET: Книги/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Книги/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Книга,Название,Автор,Год_написания,Описание,Фото")] Книги книги, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage == null)
                {
                    FileStream fs = System.IO.File.OpenRead(@"classic.jpg");
                    byte[] img = new byte[fs.Length];
                    книги.Фото = img;
                }
                else
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    книги.Фото = imageData;
                }
                db.Книги.Add(книги);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(книги);
        }

        // GET: Книги/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Книги книги = db.Книги.Find(id);
            if (книги == null)
            {
                return HttpNotFound();
            }
            return View(книги);
        }

        // POST: Книги/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Книга,Название,Автор,Год_написания,Описание,Фото")] Книги книги, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage == null)
                {
                    FileStream fs = System.IO.File.OpenRead(@"classic.jpg");
                    byte[] img = new byte[fs.Length];
                    книги.Фото = img;
                }
                else
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    книги.Фото = imageData;
                }
                db.Entry(книги).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(книги);
        }

        // GET: Книги/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Книги книги = db.Книги.Find(id);
            if (книги == null)
            {
                return HttpNotFound();
            }
            return View(книги);
        }

        // POST: Книги/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Книги книги = db.Книги.Find(id);
            db.Книги.Remove(книги);
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
