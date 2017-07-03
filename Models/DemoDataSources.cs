using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Demo.Service.Models {
    public class DemoDataSources {
        public static readonly Guid MyContactOid = Guid.Parse("cf2c0e95-fcda-49b7-994d-6d65fa3659eb");

        public List<Incident> Incidents { get; set; }
        public SortedList<Guid, Person> Persons { get; set; }

        public DemoDataSources() {
            this.Reset();
            this.Initialize();
        }

        public void Reset() {
            this.Incidents = new List<Incident>();
            this.Persons = new SortedList<Guid, Person>();
        }

        public void Initialize() {
            var dataPath = Path.Combine(System.Environment.CurrentDirectory, "App_Data/");
            if(!System.IO.Directory.Exists(dataPath)) {
                dataPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(this.GetType().Assembly.CodeBase)).Replace("file:\\", "") + "\\App_Data\\";
            }
            var personsJson = JArray.Parse(File.ReadAllText(dataPath + "persons.json"));
            foreach(var personJson in personsJson) {
                var person = new Person();
                person.Oid = Guid.Parse(personJson["Oid"].Value<string>());
                person.Email = personJson["Email"].Value<string>();
                person.FullName = personJson["FullName"].Value<string>();
                this.Persons.Add(person.Oid, person);
            }

            var incidentsJson = JArray.Parse(File.ReadAllText(dataPath + "incidents.json"));
            foreach(var incidentJson in incidentsJson) {
                var incident = new Incident();
                incident.ContactOid = Guid.Parse(incidentJson["ContactOid"].Value<string>());
                incident.CreatedOn = incidentJson["CreatedOn"].Value<DateTime>();
                incident.Id = incidentJson["Id"].Value<int>();
                incident.Oid = Guid.Parse(incidentJson["Oid"].Value<string>());
                incident.Status = incidentJson["Status"].Value<string>();
                incident.Subject = incidentJson["Subject"].Value<string>();

                LoadMessages((JArray)incidentJson["Messages"], incident.Messages);

                this.Incidents.Add(incident);
            }            
            Incidents.Sort((x,y) => -Comparer<DateTime>.Default.Compare(x.CreatedOn, y.CreatedOn));
            DateTime minDate = Incidents[0].CreatedOn;
            TimeSpan delta = DateTime.Now - minDate;
            delta -= TimeSpan.FromDays(1);
            foreach(var incident in Incidents) {
                incident.CreatedOn = incident.CreatedOn.Add(delta);
            }
        }

        public int NextIncidentId() {
            int maxId = 0;
            Incidents.ForEach(i => maxId = Math.Max(maxId, i.Id));
            return maxId + 1;
        }

        private void LoadMessages(JArray messagesJson, ICollection<Message> messages) {
            foreach(var messageJson in messagesJson) {
                var message = new Message();
                message.CreatedBy = messageJson["CreatedBy"].Value<string>();
                message.CreatedOn = messageJson["CreatedOn"].Value<DateTime>();
                message.DescriptionText = messageJson["DescriptionText"].Value<string>();
                message.Oid = Guid.Parse(messageJson["Oid"].Value<string>());
                message.StatusText = messageJson["StatusText"].Value<string>();
                messages.Add(message);
            }
        }
    }
}