using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie_.Data;
using Zadanie_.Models;

namespace Zadanie_.Controllers.Create
{
    public class AddNewEmpController : Controller
    {

        /*public IActionResult AddNewEmployee2()
        {
            return View();
        }

       
        public IActionResult AddNewEmployee(Employee emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   // if (leadDetails.NextFollowUpDate <= leadDetails.LeadDate)
                    //{
                   //     ViewBag.Message = "Follow up date cannot be less than Lead date.";
                   //     return View("AddLead");
                   // }
                   

                    DatabaseAccess db = new DatabaseAccess();
                    if (db.AddEmployee(emp))
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }


    }
*/


        string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
                "Integrated Security=true;TrustServerCertificate=true;";


        private readonly ILogger<AddNewEmpController> _logger;

        public AddNewEmpController(ILogger<AddNewEmpController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddNewEmployee(Employee2 emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string AddEmp = "INSERT INTO Employee2 (PersonalNumber, FirstName," +
                            "Surname, DateOfBirth, IdentificationNumber) " +
                            "VALUES (@PersonalNumber, @FirstName, @Surname," +
                            "@DateOfBirth, @IdentificationNumber)";

                        using (SqlCommand cmd = new SqlCommand(AddEmp, connection))
                        {
                            cmd.Parameters.AddWithValue("@PersonalNumber", emp.PersonalNumber);
                            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                            cmd.Parameters.AddWithValue("@Surname", emp.Surname);
                            cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
                            cmd.Parameters.AddWithValue("@IdentificationNumber", emp.IdentificationNumber);

                            connection.Open();
                            cmd.ExecuteNonQuery();
                            //connection.Close();
                        }
                    }
                    TempData["SuccessMessage"] = "Employee was added successfully.";
                    return RedirectToAction("Index", "Emp");
                }
                catch (SqlException SQLError)
                {
                    _logger.LogError(SQLError, "An SQL error occured while adding new employee.");
                    ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                }

                catch (Exception error)
                {
                    _logger.LogError(error, "An error occured while adding new employee.");
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
            return View("AddNewEmployee2", emp);
        }


        public IActionResult AddNewEmployee2()
        {
            return View();
        }

    }
}
