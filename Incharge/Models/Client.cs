
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Incharge.Models;

public partial class Client
{
    public int Id { get; set; }   //set as primary key
    public string Uuid { get; set; } //show to the public through mapping
    public string FirstName { get; set; }  
    public string LastName { get; set; }  
    public long Phone { get; set; }
    public string? ProfilePicture { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [AllowedValues(typeof(string), new string[] { "Signed In", "Signed Out"  })]
    public string? Status { get; set; }

    //MEMBERSHIP MANAGEMENT DATA
    [AllowedValues(typeof(string), new string[] { "Active", "Inactive", "Suspended", "Overdue" })]
    public string? MembershipStatus { get; set; }
    public string? MembershipName { get; set; }
    public int MembershipProductId { get; set; } = 0; //for searching purposes
    public DateTime? MembershipExpiryDate { get; set; } //set in sales service
    public DateTime? MembershipStartDate { get; set; }


    //FOR PRIVATE TRAINING SESSIONS
    public int TotalTrainingSessions { get; set; }

    public string? Note { get; set; } //for adding additional information about the client

    //membership date - will reset when payment record is recieved each month
    //public DateTime? StartDate { get; set; } // = will be set to the date the client bought their membership
    //public DateTime? EndDate { get; set; }

    [ForeignKey("PaymentRecordId")]
    public int? PaymentRecordId { get; set; }

    public string Address { get; set; } //address of client
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    //will act as record to track membership payments.
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public virtual ICollection<Gymclass> Gymclasses { get; set; } = new List<Gymclass>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    //look up ways to add profile picture. Store on database and how to retrieved
}
