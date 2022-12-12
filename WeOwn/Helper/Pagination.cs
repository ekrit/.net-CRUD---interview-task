using WeOwn.Modul.Models;

namespace WeOwn.Helper
{
    public class Pagination
    {
        public List<MovieShow> movieShows { get; set; } = new List<MovieShow>();
        public int pages { get; set; }
        public int currentPage { get; set; }
    }
}
