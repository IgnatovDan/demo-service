namespace Demo.Service.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Demo.Service.Models.DbServiceContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }
        public void DebugSeed(Demo.Service.Models.DbServiceContext context) {
            this.Seed(context);
        }
        protected override void Seed(Demo.Service.Models.DbServiceContext context) {
            var ds = new Models.DemoDataSources();
            context.Persons.AddOrUpdate(x => x.Oid, ds.Persons.Values.ToArray());
            context.SaveChanges();
            foreach(var i in ds.Incidents.ToArray()) {
                try {
                    if(i.ContactOid != null) {
                        i.Contact = context.Persons.FirstOrDefault(x => x.Oid == i.ContactOid);
                    }
                    context.Incidents.AddOrUpdate(x => x.Oid, i);
                    context.Messages.AddOrUpdate(x => x.Oid, i.Messages.ToArray());
                    context.SaveChanges();
                } catch(Exception) {
                    if(System.Diagnostics.Debugger.IsAttached == false) {
                        System.Diagnostics.Debugger.Launch();
                    }
                    System.Diagnostics.Debug.WriteLine("Error insert: " + String.Join(", ", (new string[] { i.Oid.ToString(), i.ContactOid.ToString() })), "Inserted");
                }
            }
        }
    }
}
