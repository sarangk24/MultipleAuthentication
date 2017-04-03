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
using MultipleAuthentication.Constants;
using MultipleAuthentication.Models;

namespace MultipleAuthentication.Controllers
{
   [CustomAuthorize(Roles = "Company Administrator")]
    public class TenantsController : Controller
    {
       
        private LoggingEntities1 db = new LoggingEntities1();

        // GET: Tenants
        public ActionResult Index()
        {
           
            return View(db.Tenants.ToList());
        }

        // GET: Tenants/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MultipleAuthentication.DatabaseContext.Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenantID,TenantName,ClientID,ClientSecret,URL,Created,Modified,CreatedBy,ModifiedBy")] DatabaseContext.Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                
                var allClaims = ((System.Security.Claims.ClaimsIdentity)User.Identity).Claims;
               Models.Tenant tenantModel= allClaims.GetTenantDetails();
                tenant.ClientID = Startup.clientId;
                tenant.ClientSecret = new Startup().appKey;
                tenant.Created = DateTime.Now;
                tenant.CreatedBy = User.Identity.Name;
                tenant.Modified = DateTime.Now;
                tenant.ModifiedBy= User.Identity.Name;
                tenant.TenantName = tenantModel.TenantName;
                tenant.TenantID = tenantModel.TenantID;
                db.Tenants.Add(tenant);
                    db.SaveChanges();

                
                //After successfully addition of tenant details, we are going to ask admin consent for the same

                return Redirect("https://login.microsoftonline.com/" + tenantModel.TenantID + "/oauth2/authorize?client_id=" + Startup.clientId + "&response_type=code&redirect_uri=" + ActiveDirectoryConstants.GetCurrentApplicationUrl + "&nonce=1234&resource=https://graph.windows.net&prompt=admin_consent");
            }

            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          MultipleAuthentication.DatabaseContext.Tenant tenant = db.Tenants.Find(id);
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
        public ActionResult Edit([Bind(Include = "TenantID,TenantName,ClientID,ClientSecret,URL,Created,Modified,CreatedBy,ModifiedBy")] MultipleAuthentication.DatabaseContext.Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                tenant.Modified = DateTime.Now;
                tenant.ModifiedBy = User.Identity.Name;
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
          MultipleAuthentication.DatabaseContext.Tenant tenant = db.Tenants.Find(id);
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
          MultipleAuthentication.DatabaseContext.Tenant tenant = db.Tenants.Find(id);
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
