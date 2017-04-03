using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MultipleAuthentication.DatabaseContext;
using MultipleAuthentication.Extensions;
using System.Web.Security;
using System.Security.Claims;

namespace MultipleAuthentication.Controllers
{
    public class SectionsController : Controller
    {
        private LoggingEntities1 db = new LoggingEntities1();

        // GET: Sections
        public ActionResult Index()
        {
            var allClaims = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims;
            Models.Tenant tenantModel = allClaims.GetTenantDetails();
            var sections = db.Sections.Include(s => s.Tenant);
            return View(sections.Where(section=>section.TenantID==tenantModel.TenantID).ToList());
        }

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            var roles = new AuthorizationStoreRoleProvider().GetAllRoles();
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
            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName");
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionName,SectionDescription,Created,Modified,CreatedBy,ModifiedBy,TenantID,Priority,SectionID")] Section section)
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

            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName", section.TenantID);
            return View(section);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            var allRoles = User.GetRoles();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName", section.TenantID);
            return View(section);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionName,SectionDescription,Created,Modified,CreatedBy,ModifiedBy,TenantID,Priority,SectionID")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                section.Modified = DateTime.Now;
                section.ModifiedBy = User.Identity.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TenantID = new SelectList(db.Tenants, "TenantID", "TenantName", section.TenantID);
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
