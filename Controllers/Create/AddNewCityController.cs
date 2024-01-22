using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie_.Data;
using Zadanie_.Models;

namespace Zadanie_.Controllers.Create
{
    public class AddNewCityController : Controller
    {
        

        string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
                "Integrated Security=true;TrustServerCertificate=true;";


        private readonly ILogger<AddNewCityController> _logger;

        public AddNewCityController(ILogger<AddNewCityController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddNewCity(City2 city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        string AddCity = "INSERT INTO City2 (CityName," +
                             "Country, Longitude, Latitude) " +
                             "VALUES (@CityName, @Country," +
                             "@Longitude, @Latitude)";

                        using (SqlCommand cmd = new SqlCommand(AddCity, connection))
                        {
                            //cmd.Parameters.AddWithValue("@CityIdentificator", city.CityIdentificator);
                            cmd.Parameters.AddWithValue("@CityName", city.CityName);
                            cmd.Parameters.AddWithValue("@Country", city.Country);
                            cmd.Parameters.AddWithValue("@Longitude", city._Coordinates.Longitude);
                            cmd.Parameters.AddWithValue("@Latitude", city._Coordinates.Latitude);

                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    TempData["SuccessMessage"] = "City was added successfully.";
                    return RedirectToAction("Index", "City");
                }
                catch (SqlException SQLError)
                {
                    _logger.LogError(SQLError, "An SQL error occured while adding new city.");
                    ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                }

                catch (Exception error)
                {
                    _logger.LogError(error, "An error occured while adding new city.");
                    ViewData["ErrorMessage"] = "An error occured while processing your request. Please, try again.";
                }
            }
            else
            {
                foreach (var er in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError($"Validation Error: {er.ErrorMessage}");
                }
            }
            return View("AddNewCity2", city);
        }


        public IActionResult AddNewCity2()
        {
            return View();
        }


    }
}
