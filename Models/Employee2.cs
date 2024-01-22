using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zadanie_.Models
{
    public class Employee2
    {
        [Key]
        [DisplayName("Personal Number")]
        public string PersonalNumber { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        public string Surname { get; set; }
        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Identification Number")]
        public int IdentificationNumber { get; set; }
    }
}
