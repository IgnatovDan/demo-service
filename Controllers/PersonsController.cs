using Demo.Service.Models;
using Demo.Service.Providers;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData;

namespace Demo.Service.Controllers {
    [EnableQuery]
    public class PersonsController : ODataController {
        private DbServiceContext db = new DbServiceContext();
        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
        public IHttpActionResult GetPersons() {
            return Ok(db.Persons.AsQueryable());
        }
        public IHttpActionResult GetPerson([FromODataUri] Guid key) {
            Person person = db.Persons.FirstOrDefault(p => p.Oid == key);
            return person == null ? (IHttpActionResult)NotFound() : Ok(person);
        }
    }
}
