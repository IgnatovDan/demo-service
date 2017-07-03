using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Demo.Service.Models {
    public class DbServiceContext : DbContext {
        public DbServiceContext() : base("name=DefaultConnection") {
        }
        public System.Data.Entity.DbSet<Incident> Incidents { get; set; }
        public System.Data.Entity.DbSet<Person> Persons { get; set; }
        public System.Data.Entity.DbSet<Message> Messages { get; set; }

        public int NextIncidentId() {
            int maxId = Incidents.Max(p => p.Id);
            return maxId + 1;
        }

    }
}