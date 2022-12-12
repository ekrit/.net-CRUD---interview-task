using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using WeOwn.Data;
using WeOwn.Helper;
using WeOwn.Modul.Models;
using WeOwn.Modul.ViewModels;



namespace WeOwn.Modul.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MovieShowController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public MovieShowController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public List<MovieShow> getAll()
        {
            var movieShow = dbContext.MovieShow.ToList();

            return movieShow;
        }


        // task 1

        // Retrieval of all movies/tv shows together with their properties ordered by the 
        // release year in descending order(a big plus would be to implement pagination as 
        // well).

        [HttpGet("{page}")]
        public ActionResult<List<MovieShow>> getAll_task1(int page)
        {
            var pageResult = 50f; 

            var pageCount = Math.Ceiling(dbContext.MovieShow.Count() / pageResult);

            var movieShow = dbContext.MovieShow.OrderByDescending(x=> x.release_year).Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToList();

            var response = new Pagination
            {
                movieShows = movieShow,
                currentPage = page,
                pages = (int)pageCount
            };
            
            return Ok(response);
        }



        // task 2

        // Retrieval of all movies for a specified director ordered by rating in descending order
        // (a big plus would be to implemented a fallback in case a specified director did not
        // direct any movies where the endpoint would then return tv shows for that director 
        // if any).


        [HttpGet]
        public ActionResult getAll_task2([FromHeader] string DirectorName)
        {
            var temp = dbContext.MovieShow.Where(x => x.director == DirectorName && x.type == "Movie").OrderByDescending(x => x.rating).ToList();

            if(temp.Count == 0)
            {
                temp = dbContext.MovieShow.Where(x => x.director == DirectorName).ToList();
            }

            if(temp.Count == 0)
            {
                return Ok("NO movies or tv shows!");
            }

            return Ok(temp);
        }

        // task 3

        // Adding a new movie/tv show with implemented validation: type, title, director, 
        // release_year, the rating cannot be empty, the rating, if provided, cannot exceed 
        // 10 nor can it be lower than 1, the description, if provided, cannot be longer than 
        // 250 characters (bonus would be to implement this validation on the database level
        // as well).

        [HttpPost]         
        public ActionResult addNew_task3([FromBody] MovieShowVM x)
        {
            int count = dbContext.MovieShow.Count() + 1;

            string new_id = 's' + count.ToString();

            var temp = new MovieShow()
            {
                show_id = new_id,
                type = x.type,
                title = x.title,
                director = x.director,
                cast = x.cast,
                country = x.country,
                date_added = x.date_added,
                release_year = x.release_year,
                rating = x.rating,
                duration = x.duration,
                listed_in = x.listed_in,
                description = x.description
            };

            if (temp.type != "Movie" && temp.type != "TV Show")
                return Ok("Type you entered is not okay!");

            if (temp.rating == "")
                return Ok("Rating cannot be empty!");

            if (temp.description.Length > 250)
                return Ok("Description cannot be longer than 250 characters!");

            // I have not implemented every validation since i was confused with some tasks. 
            // The data i got was in Excel and i had hard time transforming it into sql database
            // The data there was mosthly stored as strings, as you could notice even Primary Key was a string which is unusual

            dbContext.MovieShow.Add(temp);
            dbContext.SaveChanges();

            return Ok(temp);
        }


        // task 4

        // Update and delete functionalities bring a bonus as well. Feel free to define and 
        // implement how they should behave.

        [HttpDelete]
        public ActionResult deleteMovieShow_task4([FromHeader] string id)
        {
            var temp = dbContext.MovieShow.Where(x => x.show_id == id).SingleOrDefault();

            if (temp != null)
            {
                dbContext.MovieShow.Remove(temp);
                dbContext.SaveChanges();

                return Ok(temp);
            }

            return Ok("not done");
        }

        [HttpPut]
        public ActionResult updateMovieShow_task4([FromBody] MovieShowVM x, [FromHeader] string id)
        {
            var temp = dbContext.MovieShow.Where(x => x.show_id == id).SingleOrDefault();

            if(temp != null)
            {
                temp.type = x.type;
                temp.title = x.title;
                temp.director = x.director;
                temp.cast = x.cast;
                temp.country = x.country;
                temp.date_added = x.date_added;
                temp.release_year = x.release_year;
                temp.rating = x.rating;
                temp.duration = x.duration;
                temp.listed_in = x.listed_in;
                temp.description = x.description;

                dbContext.SaveChanges();

                return Ok(temp);
            }    

            return Ok("not done");
        }


    }
}
