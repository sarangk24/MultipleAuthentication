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
using System.Security.Claims;

namespace MultipleAuthentication.Controllers
{
    public class TenantsController : Controller
    {
        

        private NewsLetterEntities db = new NewsLetterEntities();

        // GET: Tenants
        [Authorize]
        public ActionResult Index()
        {
            Models.Tenant tenantModel = ClaimsIdentityExtensions.GetTenant();
            if (db.Tenants.Where(tenant => tenant.TenantID == tenantModel.TenantID).Count() > 0)
                return this.RedirectToAction("Index","NewsItems");
            else
                return this.RedirectToAction("AdminConsent", "Permissions");

            
                
        }

        // GET: Tenants/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            DatabaseContext.Tenant tenant = new Tenant();
            if (true)
            {
                Models.Tenant tenantModel = ClaimsIdentityExtensions.GetTenant();
                tenant.TenantID = tenantModel.TenantID;
                tenant.TenantName = tenantModel.TenantName;
                tenant.ClientID = Startup.clientId;
                tenant.ClientSecret = new Startup().appKey;
                tenant.URL = tenantModel.TenantDomain;
                tenant.Modified = DateTime.Now.Date;
                tenant.Created = DateTime.Now.Date;
                tenant.CreateBy = User.Identity.Name;
                tenant.ModifiedBy = User.Identity.Name;
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        // POST: Tenants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create()
        //{
        //    DatabaseContext.Tenant tenant=new Tenant();
        //    if (true)
        //    {
        //        Models.Tenant tenantModel = ClaimsIdentityExtensions.GetTenant();
        //        tenant.TenantID = tenantModel.TenantID;
        //        tenant.TenantName = tenantModel.TenantName;
        //        tenant.ClientID = Startup.clientId;
        //        tenant.ClientSecret = new Startup().appKey;
        //        tenant.URL = tenantModel.TenantDomain;
        //        tenant.Modified = DateTime.Now.Date;
        //        tenant.Created = DateTime.Now.Date;
        //        tenant.CreateBy = User.Identity.Name;
        //        tenant.ModifiedBy = User.Identity.Name;
        //        db.Tenants.Add(tenant);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

           
        //}

        // GET: Tenants/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenantID,TenantName,ClientID,ClientSecret,URL,Created,CreateBy,Modified,ModifiedBy")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
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
