using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie_.Data;
using Zadanie_.Models;

namespace Zadanie_.Controllers.Create
{
    public class AddNewCPController : Controller
    {
        string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
                "Integrated Security=true;TrustServerCertificate=true;";


        private readonly ILogger<AddNewCPController> _logger;

        public AddNewCPController(ILogger<AddNewCPController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddNewCP(CP2 cp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        string AddCp = "INSERT INTO CP2 (EmployeePersonalNumber," +
                            "StartCityID, EndCityID, StartTime," +
                            "EndTime) " +

                            "VALUES (@EmployeePersonalNumber, @StartCityID, " +
                            "@EndCityID," +
                            "@StartTime, @EndTime)";

                        using (SqlCommand cmd = new SqlCommand(AddCp, connection))
                        {
                            cmd.Parameters.AddWithValue("@EmployeePersonalNumber", cp.EmployeePersonalNumber);
                            cmd.Parameters.AddWithValue("@StartCityID", cp.StartCityID);
                            cmd.Parameters.AddWithValue("@EndCityID", cp.EndCityID);
                            cmd.Parameters.AddWithValue("@StartTime", cp.StartTime);
                            cmd.Parameters.AddWithValue("@EndTime", cp.EndTime);

                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                    }
                    TempData["SuccessMessage"] = "CP was added successfully.";
                    return RedirectToAction("Index", "CP");
                }
                catch (SqlException SQLError)
                {
                    _logger.LogError(SQLError, "An SQL error occured while adding new CP.");
                    ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                }

                catch (Exception error)
                {
                    _logger.LogError(error, "An error occured while adding new CP.");
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
            return View("AddNewCP2", cp);
        }


        public IActionResult AddNewCP2()
        {
            return View();
        }
     

    }
}
