using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Zadanie_.Models
{
    public class CP2
    {
        [Key]
        [DisplayName("CP Identification")]
        public int CP_Identificator { get; set; }
        [DisplayName("CP Date")]
        public DateTime CP_Date { get; set; }
        [DisplayName("Employee ID")]
        public string EmployeePersonalNumber { get; set; }

        [DisplayName("Start Location")]
        public int StartCityID { get; set; }
        [DisplayName("Final Location")]
        public int EndCityID { get; set; }
        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; }
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }
        //public TransportType _TransportType { get; set; }
        /* public Status _Status { get; set; }
         [DisplayName("Transportation Mode")]
         public List<SelectListItem> TransportationMode { get; set; }
         public List<Status> Status { get; set; }
        */
    }

    public class TransportationMode
    {
        public string CompanyCar { get; set; }
        public string Bus { get; set; }
        public string PublicTransport { get; set; }
        public string Walking { get; set; }
        public string Train { get; set; }
        public string Taxi { get; set; }
        public string Flight { get; set; }
    }


    public class Status
    {
        public string Created { get; set; }
        public string Approved { get; set; }
        public string Invoiced { get; set; }
        public string Cancelled { get; set; }
    }
}
