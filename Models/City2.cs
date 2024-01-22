using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Zadanie_.Models
{
    public class City2
    {
        [Key]
        [DisplayName("City Identificator")]
        public int CityIdentificator { get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
        public string Country { get; set; }
        public Coordinates _Coordinates { get; set; }
    }

    public class Coordinates
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
    }
}
