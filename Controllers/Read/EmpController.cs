using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Zadanie_.Controllers.Create;
using Zadanie_.Data;
using Zadanie_.Models;

namespace Zadanie_.Controllers.Read
{
    public class EmpController : Controller
    {

        private readonly ILogger<EmpController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EmpController(ILogger<EmpController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        //public SqlConnection _connection;  

        //string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
        //        "Integrated Security=true;TrustServerCertificate=true;";

     

        public IActionResult Index()
        {
            List<Employee2> employees = new List<Employee2>();
            DatabaseAccess databaseAccess = new DatabaseAccess();

            employees = databaseAccess.GetAllEmployees();
            return View(employees);
        }

        #region Employee operations
        public IActionResult EditEmployee(string PersonalNumber)
        {
            Employee2 emp = new Employee2();
            DatabaseAccess db = new DatabaseAccess();

            emp = db.GetEmpByPN(PersonalNumber);
            return View(emp);
        }
     
        public IActionResult EditEmployeeDetails(string PersonalNumber, Employee2 emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DatabaseAccess db = new DatabaseAccess();
                    if (db.EditEmpDetails(PersonalNumber, emp))
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }

            catch (SqlException SQLError)
            {
                //ViewBag.Message = $"A database error occurred: {SQLError.Message}";
                //return View("Error");
                //return View();

                _logger.LogError(SQLError, "An SQL error occured while adding new employee.");
                ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public IActionResult DeleteEmployee(string PersonalNumber)
        {
            Employee2 emp = new Employee2();
            DatabaseAccess db = new DatabaseAccess();

            emp = db.GetEmpByPN(PersonalNumber);
            return View(emp);
        }

        public IActionResult DeleteEmployeeDetails(string PersonalNumber, Employee2 emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DatabaseAccess db = new DatabaseAccess();
                    if (db.DeleteEmpDetails(PersonalNumber, emp))
                    {
                        TempData["SuccessMessage"] = "Employee was deleted successfully.";
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ViewBag.Message = "Error occurred while deleting the employee.";
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

                _logger.LogError(SQLError, "An SQL error occured while deleting new CP.");
                ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"An error occurred: {ex.Message}";
                return View();
            }

        }

        public IActionResult SearchEmployee(string searchString)
        {
            List<Employee2> emp = new List<Employee2>();
            DatabaseAccess databaseAccess = new DatabaseAccess();
            emp = databaseAccess.SearchEmployee(searchString);
            return View("Index", emp);
        }
        #endregion

        /*public IActionResult Index(string sortOrder, string sortDirection)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentSortDirection = sortDirection;

            List<Employee> employees = new List<Employee>();
            string SQLQuery = "SELECT * FROM Employee";

            if (!String.IsNullOrEmpty(sortOrder))
            {
                SQLQuery += $" ORDER BY {sortOrder} {sortDirection}";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLQuery, connection);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var emp = new Employee
                        {
                            PersonalNumber = reader["PersonalNumber"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            IdentificationNumber = Convert.ToInt32(reader["IdentificationNumber"]),
                        };
                        employees.Add(emp);
                    }                  
                }
            }
            return View(employees);
        }
        */

        /*  public IActionResult Index2 (string sortingOrder)
        {
            List<Employee> emp = new List<Employee>();
            DatabaseAccess db = new DatabaseAccess();
            emp = db.SortEmp(sortingOrder);
            return View("Index", emp);
        }
      */

        public IActionResult Index2 (string sortingOrder)
        {

            ViewBag.CurrentSortOrder = sortingOrder;

            ViewBag.NameSortParm = sortingOrder == "Name" ? "name_desc" : "Name";
            ViewBag.SurnameSortParm = sortingOrder == "Surname" ? "surname_desc" : "Surname";

            ViewBag.DateOfBirthSortParm = sortingOrder == "DateOfBirth_ASC" ? "DateOfBirth_DESC" : "DateOfBirth_ASC";
            ViewBag.IDNumSortParm = sortingOrder == "IdentificationNumber_ASC" ? "IdentificationNumber_DESC" : "IdentificationNumber_ASC";


            string SQLQuery = "SELECT * FROM Employee2";
            
            switch (sortingOrder)
            {
                case "Name":
                    SQLQuery += " ORDER BY FirstName ASC";
                    break;
                case "name_desc":
                    SQLQuery += " ORDER BY FirstName DESC";
                    break;

                case "Surname":
                    SQLQuery += " ORDER BY Surname ASC";
                    break;
                case "surname_desc":
                    SQLQuery += " ORDER BY Surname DESC";
                    break;

                case "DateOfBirth_ASC":
                    SQLQuery += " ORDER BY DateOfBirth ASC";
                    break;
                case "DateOfBirth_DESC":
                    SQLQuery += " ORDER BY DateOfBirth DESC";
                    break;

                case "IdentificationNumber_ASC":
                    SQLQuery += " ORDER BY IdentificationNumber ASC";
                    break;
                case "IdentificationNumber_DESC":
                    SQLQuery += " ORDER BY IdentificationNumber DESC";
                    break;


            }

            List<Employee2> employees = new List<Employee2>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLQuery, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var emp = new Employee2
                        {
                            PersonalNumber = reader["PersonalNumber"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                            IdentificationNumber = Convert.ToInt32(reader["IdentificationNumber"]),
                        };
                        employees.Add(emp);
                    }
                }
            }
            return View("Index", employees);
        }
       
      
       
        
    }
}
