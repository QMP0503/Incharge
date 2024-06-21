using Incharge.Models;
using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Incharge.Parameters
{
    public class ClientInfo
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? GymClass { get; set; } //Name of GymClass
        public string? EmployeeName { get; set; } //Find by trainer name if they have one
       
    }
}
