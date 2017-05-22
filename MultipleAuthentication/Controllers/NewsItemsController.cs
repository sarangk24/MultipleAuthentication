using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoreLinq;
using MultipleAuthentication.DatabaseContext;
using MultipleAuthentication.Helper;

namespace MultipleAuthentication.Controllers
{
    public class NewsItemsController : Controller
    {
        private NewsLetterEntities db = new NewsLetterEntities();

        // GET: NewsItems
        public ActionResult Index()
        {
            var tenantId = new TenantProvider().GetTenantDetails().TenantID;
            var newsItems = db.NewsItems.Include(n => n.Section).Where(newsItem => newsItem.Section.NewsLetter.TenantID ==tenantId);
               
            return View(newsItems.ToList());
        }

        // GET: NewsItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            return View(newsItem);
        }

        // GET: NewsItems/Create
        public ActionResult Create()
        {
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName");
            return View();
        }

        // POST: NewsItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewsItemName,NewsItemDescription,ActualLink,Visible,Created,Modified,CreatedBy,ModifiedBy,SectionID")] NewsItem newsItem)
        {
            if (ModelState.IsValid)
            {
                newsItem.Created = DateTime.Now;
                newsItem.CreatedBy = User.Identity.Name;
                newsItem.Modified = DateTime.Now;
                newsItem.ModifiedBy = User.Identity.Name;
                db.NewsItems.Add(newsItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", newsItem.SectionID);
            return View(newsItem);
        }

        // GET: NewsItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", newsItem.SectionID);
            return View(newsItem);
        }

        // POST: NewsItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsItemID,NewsItemName,NewsItemDescription,ActualLink,Visible,Created,Modified,CreatedBy,ModifiedBy,SectionID")] NewsItem newsItem)
        {
            if (ModelState.IsValid)
            {
                newsItem.ModifiedBy = User.Identity.Name;
                newsItem.Modified = DateTime.Now;
                db.Entry(newsItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SectionID = new SelectList(db.Sections, "SectionID", "SectionName", newsItem.SectionID);
            return View(newsItem);
        }

        // GET: NewsItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            return View(newsItem);
        }

        // POST: NewsItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsItem newsItem = db.NewsItems.Find(id);
            db.NewsItems.Remove(newsItem);
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
