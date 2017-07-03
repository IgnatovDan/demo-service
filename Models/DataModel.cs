using Demo.Service.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.OData.Builder;

namespace Demo.Service.Models {
    public class Incident {
        public Incident() {
            this.Messages = new List<Message>();
        }
        [Key]
        public Guid Oid { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Id { get; set; }
        public string StatusString
        {
            get
            {
                switch(Status) {
                    case "Accepted":
                        return "Accepted";
                    case "Closed":
                        return "Closed";
                    case "Executing":
                        return "In Progress";
                    case "NewOrChanged":
                        return "New Or Changed";
                    case "Return":
                        return "Returned For Improvement";
                    case "Wait":
                        return "Waiting For Client";
                    case "WaitToClose":
                        return "Done. Waiting To Be Closed.";
                    default:
                        return "Unknown";
                }
            }
            set
            {
                switch(value) {
                    case "Accepted":
                        Status = "Accepted";
                        break;
                    case "Closed":
                        Status = "Closed";
                        break;
                    case "In Progress":
                        Status = "Executing";
                        break;
                    case "New Or Changed":
                        Status = "NewOrChanged";
                        break;
                    case "Returned For Improvement":
                        Status = "Return";
                        break;
                    case "Waiting For Client":
                        Status = "Wait";
                        break;
                    case "Done. Waiting To Be Closed.":
                        Status = "WaitToClose";
                        break;
                    default:
                        Status = "Unknown";
                        break;
                }
            }
        }

        public Guid ContactOid { get; set; }

        [ForeignKey("ContactOid")]
        public virtual Person Contact { get; set; }
        [Contained]
        public virtual ICollection<Message> Messages { get; set; }
        [NotMapped]
        public string NewMessage { get; set; }

        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string History
        {
            get
            {
                if(Messages == null || Messages.Count == 0)
                    return "";
                var res = new StringBuilder();
                foreach(var message in Messages) {
                    res.Append(message.CreatedOn.ToString("d"))
                        .Append(" by ")
                        .Append(message.CreatedBy)
                        .AppendLine()
                        .Append(message.DescriptionText)
                        .AppendLine()
                        .Append("Status: ")
                        .Append(message.StatusText)
                        .AppendLine()
                        .Append("=======")
                        .AppendLine();
                }
                return res.ToString();
            }
            set { }
        }
        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string StatusColor
        {
            get
            {
                switch(Status) {
                    case "Closed":
                        return "rgba(223, 243, 220, 0.9)";
                    case "WaitToClose":
                        return "rgba(243, 220, 220, 0.9)";
                    default:
                        return "transparent";
                }
            }
            set { }
        }
        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ContactFullName
        {
            get
            {
                if(Contact != null) { return Contact.FullName; }
                return "";
            }
            set { }
        }
    }

    public class Message {
        [Key]
        public Guid Oid { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string DescriptionText { get; set; }

        public string StatusText { get; set; }
    }

    public class Person : IComparable<Person> {
        [Key]
        public Guid Oid { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int CompareTo(Person other) {
            return Comparer<Guid>.Default.Compare(Oid, other.Oid);
        }
    }
}