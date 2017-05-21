using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MultipleAuthentication.DatabaseContext;

namespace MultipleAuthentication.Controllers
{
    public class SectionsController : Controller
    {
        private NewsLetterEntities db = new NewsLetterEntities();

        // GET: Sections
        public ActionResult Index()
        {
            var sections = db.Sections.Include(s => s.NewsLetter);
            return View(sections.ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            ViewBag.NewsLetterID = new SelectList(db.NewsLetters, "NewsLetterID", "Name");
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionName,SectionDescription,Visible,Created,Modified,CreatedBy,ModifiedBy,NewsLetterID")] Section section)
        {
            if (ModelState.IsValid)
            {
                section.Created = DateTime.Now;
                section.CreatedBy = User.Identity.Name;
                section.Modified = DateTime.Now;
                section.ModifiedBy = User.Identity.Name;
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NewsLetterID = new SelectList(db.NewsLetters, "NewsLetterID", "Name", section.NewsLetterID);
            return View(section);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.NewsLetterID = new SelectList(db.NewsLetters, "NewsLetterID", "Name", section.NewsLetterID);
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionID,SectionName,SectionDescription,Visible,Created,Modified,CreatedBy,ModifiedBy,NewsLetterID")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                section.ModifiedBy = User.Identity.Name;
                section.Modified = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NewsLetterID = new SelectList(db.NewsLetters, "NewsLetterID", "Name", section.NewsLetterID);
            return View(section);
        }

        // GET: Sections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Section section = db.Sections.Find(id);
            db.Sections.Remove(section);
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
