using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MultipleAuthentication.DatabaseContext;
using MultipleAuthentication.Helper;

namespace MultipleAuthentication.Controllers
{
    public class NewsLettersController : Controller
    {
        private LoggingEntities1 db = new LoggingEntities1();
        private string tenantID = new TenantProvider().GetTenantDetails().TenantID;
        // GET: NewsLetters
        public ActionResult Index()
        {
            var newsLetters = db.NewsLetters.Include(n => n.Section);
          newsLetters = newsLetters.Select(x => x).Where(y => y.Section.TenantID == tenantID);
            return View(newsLetters.ToList());
        }

        // GET: NewsLetters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsLetter newsLetter = db.NewsLetters.Find(id);
            if (newsLetter == null)
            {
                return HttpNotFound();
            }
            return View(newsLetter);
        }

        // GET: NewsLetters/Create
        public ActionResult Create()
        {
            ViewBag.SectionID = new SelectList(db.Sections.Where(items=>items.TenantID==tenantID), "SectionID", "SectionName");
            return View();
        }

        // POST: NewsLetters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HeadLine,NewsDescription,ActualLink,Created,Modified,CreatedBy,ModifiedBy,SectionID,Priority,ID")] NewsLetter newsLetter)
        {
            if (ModelState.IsValid)
            {
                newsLetter.Created = DateTime.Now;
                newsLetter.Modified = DateTime.Now;
                newsLetter.CreatedBy = User.Identity.Name;
                newsLetter.ModifiedBy = User.Identity.Name;
                db.NewsLetters.Add(newsLetter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", newsLetter.SectionID);
            return View(newsLetter);
        }

        // GET: NewsLetters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsLetter newsLetter = db.NewsLetters.Find(id);
            if (newsLetter == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", newsLetter.SectionID);
            return View(newsLetter);
        }

        // POST: NewsLetters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HeadLine,NewsDescription,ActualLink,Created,Modified,CreatedBy,ModifiedBy,SectionID,Priority,ID")] NewsLetter newsLetter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsLetter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", newsLetter.SectionID);
            return View(newsLetter);
        }

        // GET: NewsLetters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsLetter newsLetter = db.NewsLetters.Find(id);
            if (newsLetter == null)
            {
                return HttpNotFound();
            }
            return View(newsLetter);
        }

        // POST: NewsLetters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsLetter newsLetter = db.NewsLetters.Find(id);
            db.NewsLetters.Remove(newsLetter);
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
