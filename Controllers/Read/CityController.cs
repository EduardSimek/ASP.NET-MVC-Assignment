using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie_.Data;
using Zadanie_.Models;

namespace Zadanie_.Controllers.Read
{
    public class CityController : Controller
    {
        string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
                "Integrated Security=true;TrustServerCertificate=true;";

        private readonly ILogger<CityController> _logger;

        public CityController(ILogger<CityController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<City2> cities = new List<City2>();
            DatabaseAccess databaseAccess = new DatabaseAccess();

            cities = databaseAccess.GetAllCities();
            return View(cities);
        }

        #region City operations
        public IActionResult EditCity(int CityIdentificator)
        {
            City2 city = new City2();
            DatabaseAccess db = new DatabaseAccess();

            city = db.GetCityByPN(CityIdentificator);
            return View(city);
        }

        public IActionResult EditCityDetails(int CityIdentificator, City2 city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DatabaseAccess db = new DatabaseAccess();
                    if (db._EditCityDetails(CityIdentificator, city))
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }


            catch (SqlException SQLError)
            {
                _logger.LogError(SQLError, "An SQL error occured while editing City.");
                ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public IActionResult DeleteCity(int CityIdentificator)
        {
            City2 city = new City2();
            DatabaseAccess db = new DatabaseAccess();

            city = db.GetCityByPN(CityIdentificator);
            return View(city);
        }

        public IActionResult DeleteCityDetails(int CityIdentificator, City2 city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DatabaseAccess db = new DatabaseAccess();
                    if (db._DeleteCityDetails(CityIdentificator, city))
                    {
                        TempData["SuccessMessage"] = "City was deleted successfully.";
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ViewBag.Message = "Error occurred while deleting the city.";
                        return View();
                    }
                }


                else
                {
                    return View();
                }
            }

            catch (SqlException SQLError)
            {
                _logger.LogError(SQLError, "An SQL error occured while deleting city.");
                ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"An error occurred: {ex.Message}";
                return View();
            }

        }

        public IActionResult SearchCity(string searchString)
        {
            List<City2> cities = new List<City2>();
            DatabaseAccess databaseAccess = new DatabaseAccess();
            cities = databaseAccess.SearchCity(searchString);
            return View("Index", cities);
        }

        public IActionResult Index2City(string sortingOrder)
        {


            ViewBag.CurrentSortOrder = sortingOrder;

            ViewBag.City_ID = sortingOrder == "City_ASC" ? "City_DESC" : "City_ASC";
            ViewBag.StartingCitySortParm = sortingOrder == "CityName_ASC" ? "CityName_DESC" : "CityName_ASC";
            ViewBag.CountrySortParm = sortingOrder == "Country_ASC" ? "Country_DESC" : "Country_ASC";

            string SQLQuery = "SELECT * FROM City2";

            switch (sortingOrder)
            {
                case "City_ASC":
                    SQLQuery += " ORDER BY CityIdentificator ASC";
                    break;
                case "City_DESC":
                    SQLQuery += " ORDER BY CityIdentificator DESC";
                    break;


                case "CityName_ASC":
                    SQLQuery += " ORDER BY CityName ASC";
                    break;
                case "CityName_DESC":
                    SQLQuery += " ORDER BY CityName DESC";
                    break;
                case "Country_ASC":
                    SQLQuery += " ORDER BY Country ASC";
                    break;
                case "Country_DESC":
                    SQLQuery += " ORDER BY Country DESC";
                    break;
            }

            List<City2> _city = new List<City2>();  
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLQuery, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var city = new City2
                        {
                            CityIdentificator = int.TryParse(reader["CityIdentificator"].ToString(), out int cityIdentificator) ? cityIdentificator : 0,
                            CityName = reader["CityName"].ToString(),
                            Country = reader["Country"].ToString(),
                            _Coordinates = new Coordinates
                            {
                                Longitude = float.TryParse(reader["Longitude"].ToString(), out float longitude) ? longitude : 0.0f,
                                Latitude = float.TryParse(reader["Latitude"].ToString(), out float latitude) ? latitude : 0.0f
                            }

                        };
                        _city.Add(city);  
                    }
                }
            }
            return View("Index", _city);
        }

        #endregion
    }
}
