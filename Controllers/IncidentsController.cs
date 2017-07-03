using Demo.Service.Models;
using Demo.Service.Providers;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;

namespace Demo.Service.Controllers {
    public class IncidentsController : ODataController {
        DbServiceContext db = new DbServiceContext();
        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
        private bool isExists(Guid key) {
            return db.Incidents.Any(p => p.Oid == key);
        }

        [EnableQuery]
        public IQueryable<Incident> Get() {
            return db.Incidents
                .Include(i => i.Contact)
                .Include(i => i.Messages);
        }

        [EnableQuery]
        public SingleResult<Incident> Get([FromODataUri] Guid key) {
            IQueryable<Incident> result = db.Incidents
                .Include(i => i.Contact)
                .Where(p => p.Oid == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Incident model) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            model.Oid = Guid.NewGuid();
            model.Id = db.NextIncidentId();
            if(string.IsNullOrEmpty(model.Status))
                model.Status = "NewOrChanged";
            model.CreatedOn = DateTime.UtcNow;
            db.Incidents.Add(model);
            if(model.ContactOid == null || model.ContactOid == Guid.Empty) {
                model.ContactOid = DemoDataSources.MyContactOid;
            }
            await db.SaveChangesAsync();
            return Created(model);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Incident> update) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var entity = await db.Incidents.FindAsync(key);
            if(entity == null) {
                return NotFound();
            }
            update.TrySetPropertyValue("ContactOid", entity.ContactOid);        // Don't allow to change ContactOid field
            update.TrySetPropertyValue("CreatedOn", entity.CreatedOn);        // Don't allow to change CreatedOn field
            update.Patch(entity);
            try {
                await db.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!isExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(entity);
        }
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Incident update) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(key != update.Oid) {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try {
                await db.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!isExists(key)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Updated(update);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key) {
            var entity = await db.Incidents.FindAsync(key);
            if(entity == null) {
                return NotFound();
            }
            db.Incidents.Remove(entity);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public IHttpActionResult GetMessages(Guid key) {
            var incident = db.Incidents.Find(key);
            if(incident != null) {
                return Ok(incident.Messages.AsQueryable());
            } else {
                return NotFound();
            }
        }
    }
}
