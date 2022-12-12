using System.ComponentModel.DataAnnotations;

namespace WeOwn.Modul.Models
{
    public class MovieShow
    {
        [Key]
        public string show_id { get; set; }
        public string? type { get; set; }
        public string? title { get; set; }
        public string? director { get; set; }
        public string? cast { get; set; }
        public string? country { get; set; }
        public DateTime? date_added { get; set; }
        public int? release_year { get; set; }
        public string? rating { get; set; }
        public string? duration { get; set; }
        public string? listed_in { get; set; }
        public string? description { get; set; }


    }
}
