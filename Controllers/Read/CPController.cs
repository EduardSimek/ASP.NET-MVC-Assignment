using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Zadanie_.Data;
using Zadanie_.Models;

namespace Zadanie_.Controllers.Read
{
    public class CPController : Controller
    {
        string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
                "Integrated Security=true;TrustServerCertificate=true;";

        private readonly ILogger<CPController> _logger;

        public CPController(ILogger<CPController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            List<CP2> _cp = new List<CP2>();
            DatabaseAccess databaseAccess = new DatabaseAccess();

            _cp = databaseAccess.GetAllCPs();
            return View(_cp);
        }

        #region CP operations
        public IActionResult EditCP(int CP_Identificator)
        {
            CP2 cp = new CP2();
            DatabaseAccess db = new DatabaseAccess();

            cp = db.GetCPByPN(CP_Identificator);
            return View(cp);
        }

        public IActionResult EditCPDetails(int CP_Identificator, CP2 cp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DatabaseAccess db = new DatabaseAccess();
                    if (db.EditCPDetails(CP_Identificator, cp))
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

                _logger.LogError(SQLError, "An SQL error occured while editing CP.");
                ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                return View();
            }


            catch (Exception ex)
            {
                return View();
            }

        }

        public IActionResult DeleteCP(int CP_Identificator)
        {
            CP2 cp = new CP2();
            DatabaseAccess db = new DatabaseAccess();

            cp = db.GetCPByPN(CP_Identificator);
            return View(cp);
        }

        public IActionResult DeleteCPDetails(int CP_Identificator, CP2 cp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DatabaseAccess db = new DatabaseAccess();
                    if (db.DeleteCPDetails(CP_Identificator, cp))
                    {
                        TempData["SuccessMessage"] = "CP was deleted successfully.";
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ViewBag.Message = "Error occurred while deleting CP.";
                        // Specify a view to return, such as returning to a confirmation page or an error page
                        //return View("Error", cp);
                        return View();
                    }
                }
                else
                {
                    //return View("Error", cp);
                    return View();
                }
            }

            catch (SqlException SQLError)
            {
                //ViewBag.Message = $"A database error occurred: {SQLError.Message}";
                //return View("Error");
                //return View();

                _logger.LogError(SQLError, "An SQL error occured while deleting CP.");
                ViewData["ErrorMessage"] = "A database error occured. Please, try again.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"An error occurred: {ex.Message}";
                //return View("Error");
                return View();
            }

        }

        public IActionResult SearchCP(string searchString)
        {
            List<CP2> cp = new List<CP2>();
            DatabaseAccess databaseAccess = new DatabaseAccess();
            cp = databaseAccess.SearchCP(searchString);
            return View("Index", cp);
        }

        public IActionResult Index2CP(string sortingOrder)
        {
 

            ViewBag.CurrentSortOrder = sortingOrder;

            ViewBag.CP_ID = sortingOrder == "CP_ASC" ? "CP_DESC" : "CP_ASC";
            ViewBag.CP_Employee_ID = sortingOrder == "EmployeeID_ASC" ? "EmployeeID_DESC" : "EmployeeID_ASC";

            ViewBag.CP_StartCityID = sortingOrder == "StartCityID_ASC" ? "StartCityID_DESC" : "StartCityID_ASC";
            ViewBag.CP_EndCityID = sortingOrder == "EndCityID_ASC" ? "EndCityID_DESC" : "EndCityID_ASC";

            ViewBag.CP_StartTime = sortingOrder == "StartTime_ASC" ? "StartTime_DESC" : "StartTime_ASC";
            ViewBag.CP_EndTime = sortingOrder == "EndTime_ASC" ? "EndTime_DESC" : "EndTime_ASC";


            string SQLQuery = "SELECT * FROM CP2";

            switch (sortingOrder)
            {
                case "CP_ASC":
                    SQLQuery += " ORDER BY CP_Identificator ASC";
                    break;
                case "CP_DESC":
                    SQLQuery += " ORDER BY CP_Identificator DESC";
                    break;


                case "StartCityID_ASC":
                    SQLQuery += " ORDER BY StartCityID ASC";
                    break;
                case "StartCityID_DESC":
                    SQLQuery += " ORDER BY StartCityID DESC";
                    break;


                case "EndCityID_ASC":
                    SQLQuery += " ORDER BY EndCityID ASC";
                    break;
                case "EndCityID_DESC":
                    SQLQuery += " ORDER BY EndCityID DESC";
                    break;


                case "StartTime_ASC":
                    SQLQuery += " ORDER BY StartTime ASC";
                    break;
                case "StartTime_DESC":
                    SQLQuery += " ORDER BY StartTime DESC";
                    break;


                case "EndTime_ASC":
                    SQLQuery += " ORDER BY EndTime ASC";
                    break;
                case "EndTime_DESC":
                    SQLQuery += " ORDER BY EndTime DESC";
                    break;


            }

            List<CP2> _cp = new List<CP2>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLQuery, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cp = new CP2
                        {
                            CP_Identificator = Convert.ToInt32(reader["CP_Identificator"]),
                            CP_Date = Convert.ToDateTime(reader["CP_Date"]),

                            EmployeePersonalNumber = reader["EmployeePersonalNumber"].ToString(),

                            StartCityID = Convert.ToInt32(reader["StartCityID"]),
                            EndCityID = Convert.ToInt32(reader["EndCityID"]),


                            StartTime = Convert.ToDateTime(reader["StartTime"]),
                            EndTime = Convert.ToDateTime(reader["EndTime"]),

                        };
                        _cp.Add(cp);  //cp
                    }
                }
            }
            return View("Index", _cp);
        }
      

        #endregion
    }
}
