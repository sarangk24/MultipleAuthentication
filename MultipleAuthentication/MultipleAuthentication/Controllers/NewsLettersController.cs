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
    public class NewsLettersController : Controller
    {
        private NewsLetterEntities db = new NewsLetterEntities();

        // GET: NewsLetters
        public ActionResult Index()
        {
            var newsLetters = db.NewsLetters.Include(n => n.Tenant);
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
            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName");
            return View();
        }

        // POST: NewsLetters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewsLetterID,Name,Description,Visible,Created,Modified,CreatedBy,ModifiedBy,TenantID")] NewsLetter newsLetter)
        {
            if (ModelState.IsValid)
            {
                db.NewsLetters.Add(newsLetter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName", newsLetter.TenantID);
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
            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName", newsLetter.TenantID);
            return View(newsLetter);
        }

        // POST: NewsLetters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsLetterID,Name,Description,Visible,Created,Modified,CreatedBy,ModifiedBy,TenantID")] NewsLetter newsLetter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsLetter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName", newsLetter.TenantID);
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
