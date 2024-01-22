using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Web.Mvc;
using Zadanie_.Controllers.Create;
using Zadanie_.Models;

namespace Zadanie_.Data
{
    public class DatabaseAccess
    {
        public SqlConnection _connection;

        public DatabaseAccess()
        {
            string connectionString = "server=DESKTOP-SPKDOT3\\SQLEXPRESS;database=DatabaseApp; " +
                "Integrated Security=true;TrustServerCertificate=true;";

            _connection = new SqlConnection(connectionString);
        }




        #region Get All Employees, Cities and CP
        public List<Employee2> GetAllEmployees()
        {
            List<Employee2> employees = new List<Employee2>();

            string sqlQuery = "SELECT * FROM Employee2";

            using (SqlCommand cmd = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        employees.Add(new Employee2
                        {
                            PersonalNumber = dr["PersonalNumber"].ToString(),
                            FirstName = dr["FirstName"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                            IdentificationNumber = Convert.ToInt32(dr["IdentificationNumber"]),
                        });
                    }
                }
            }

            return employees;
        }

        public List<CP2> GetAllCPs()
        {
            List<CP2> cp = new List<CP2>();

            string sqlQuery = "SELECT * FROM CP2";

            using (SqlCommand cmd = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        cp.Add(new CP2
                        {
                            CP_Identificator = Convert.ToInt32(dr["CP_Identificator"]),
                            CP_Date = Convert.ToDateTime(dr["CP_Date"]),

                            EmployeePersonalNumber = dr["EmployeePersonalNumber"].ToString(),
                            StartCityID = Convert.ToInt32(dr["StartCityID"]),

                            EndCityID = Convert.ToInt32(dr["EndCityID"]),
                            StartTime = Convert.ToDateTime(dr["StartTime"]),

                            EndTime = Convert.ToDateTime(dr["EndTime"]),

                        });
                    }
                }
            }

            return cp;
        }

        public List<City2> GetAllCities()
        {
            List<City2> cities = new List<City2>();

            string sqlQuery = "SELECT * FROM City2";

            using (SqlCommand cmd = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        //city.Add(new City
                        /* City city = new City
                         {
                             CityIdentificator = Convert.ToInt32(dr["CityIdentificator"]),
                             StartingCity = dr["StartingCity"].ToString(),
                             EndingCity = dr["EndingCity"].ToString(),
                             Country = dr["Country"].ToString(),
                             _Coordinates = new Coordinates
                             {
                                 Longitude = float.Parse(dr["Longitude"].ToString()),
                                 Latitude = float.Parse(dr["Latitude"].ToString())
                             }


                         };
                        */
                        City2 city = new City2
                        {
                            CityIdentificator = int.TryParse(dr["CityIdentificator"].ToString(), out int cityIdentificator) ? cityIdentificator : 0,
                            CityName = dr["CityName"].ToString(),
                            Country = dr["Country"].ToString(),
                            _Coordinates = new Coordinates
                            {
                                Longitude = float.TryParse(dr["Longitude"].ToString(), out float longitude) ? longitude : 0.0f,
                                Latitude = float.TryParse(dr["Latitude"].ToString(), out float latitude) ? latitude : 0.0f
                            }
                        };
                        cities.Add(city);
                    }
                }
            }

            return cities;
        }
        #endregion


        #region Searching Employees, Cities and CP
        public List<City2> SearchCity(string searchString)
        {
            List<City2> cities = new List<City2>();

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            //string searchSQLCity = "SELECT * FROM City WHERE StartingCity LIKE @searchString";
            string searchSQLCity = "SELECT * FROM City2 " +
                                   "WHERE CityIdentificator LIKE @searchString " +
                                   "OR CityName LIKE @searchString " +
                                   "OR Country LIKE @searchString";

            using (SqlCommand cmd = new SqlCommand(searchSQLCity, _connection))
            {
      
                {
                    cmd.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            City2 city = new City2
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
                            cities.Add(city);
                        }
                    }
                }
            }

            return cities; // Assuming you have a view to display the results
        }

        public List<CP2> SearchCP(string searchString)
        {
            List<CP2> _cp = new List<CP2>();

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            //string searchSQLCity = "SELECT * FROM City WHERE StartingCity LIKE @searchString";
            string searchSQLCP =   "SELECT * FROM CP2 " +
                                   "WHERE CP_Identificator LIKE @searchString " +
                                   "OR EmployeePersonalNumber LIKE @searchString " +
                                   "OR StartCityID LIKE @searchString " +
                                   "OR EndCityID LIKE @searchString " +
                                   "OR StartTime LIKE @searchString " +
                                   "OR EndTime LIKE @searchString";

            using (SqlCommand cmd = new SqlCommand(searchSQLCP, _connection))
            {

                {
                    cmd.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CP2 cp = new CP2
                            {
                                CP_Identificator = int.TryParse(reader["CP_Identificator"].ToString(), out int CP_Identificator) ? CP_Identificator : 0,
                                CP_Date = reader["CP_Date"] != DBNull.Value ? Convert.ToDateTime(reader["CP_Date"]) : default(DateTime),

                                EmployeePersonalNumber = reader["EmployeePersonalNumber"].ToString(),
                                StartCityID = int.TryParse(reader["StartCityID"].ToString(), out int StartCityID) ? StartCityID : 0,

                                EndCityID = int.TryParse(reader["EndCityID"].ToString(), out int EndCityID) ? EndCityID : 0,

                                StartTime = reader["StartTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartTime"]) : default(DateTime),
                                EndTime = reader["EndTime"] != DBNull.Value ? Convert.ToDateTime(reader["EndTime"]) : default(DateTime)
                                /* _Coordinates = new Coordinates
                                 {
                                     Longitude = float.TryParse(reader["Longitude"].ToString(), out float longitude) ? longitude : 0.0f,
                                     Latitude = float.TryParse(reader["Latitude"].ToString(), out float latitude) ? latitude : 0.0f
                                 }
                                */
                            };
                            _cp.Add(cp);
                        }
                    }
                }
            }

            return _cp; // Assuming you have a view to display the results
        }

        /*public List<Employee2> SearchEmployee(string searchString)
        {
            List<Employee2> _emp = new List<Employee2>();

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            //string searchSQLCity = "SELECT * FROM City WHERE StartingCity LIKE @searchString";
            string searchSQLEmployee = "SELECT * FROM Employee2 " +
                                       "WHERE PersonalNumber LIKE @searchString " +
                                       "OR FirstName LIKE @searchString " +
                                       "OR Surname LIKE @searchString " +
                                       "OR DateOfBirth LIKE @searchString " +
                                       "OR IdentificationNumber LIKE @searchString";

            using (SqlCommand cmd = new SqlCommand(searchSQLEmployee, _connection))
            {

                {
                    cmd.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee2 emp = new Employee2
                            {
                                PersonalNumber = reader["PersonalNumber"].ToString(),

                                FirstName = reader["FirstName"].ToString(),
                                Surname = reader["Surname"].ToString(),

                                DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : default(DateTime),
                                IdentificationNumber = int.TryParse(reader["IdentificationNumber"].ToString(), out int IdentificationNumber) ? IdentificationNumber : 0
                                                   
                               
                            };
                            _emp.Add(emp);
                        }
                    }
                }
            }

            return _emp; // Assuming you have a view to display the results
        
        }
        */
        public List<Employee2> SearchEmployee(string searchString)
        {
            List<Employee2> employees = new List<Employee2>();

            string searchSQLEmploye = "SELECT * FROM Employee2 " +
                                       "WHERE PersonalNumber LIKE @searchString " +
                                       "OR FirstName LIKE @searchString " +
                                       "OR Surname LIKE @searchString " +
                                       "OR DateOfBirth LIKE @searchString " +
                                       "OR IdentificationNumber LIKE @searchString";

            using (var cmd = new SqlCommand(searchSQLEmploye, _connection))
            {
                cmd.Parameters.Add(new SqlParameter("@searchString", SqlDbType.VarChar)).Value = $"%{searchString}%";
                _connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var emp = new Employee2 {
                            PersonalNumber = reader["PersonalNumber"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? default(DateTime) : Convert.ToDateTime(reader["DateOfBirth"]),
                            IdentificationNumber = reader.IsDBNull(reader.GetOrdinal("IdentificationNumber")) ? 0 : Convert.ToInt32(reader["IdentificationNumber"])
                        };
                        employees.Add(emp);
                    }
                }
            }
            return employees;
        }

        #endregion 



        #region CRUD for Employees
        public bool AddEmployee(Employee2 emp)
        {
            
            try
            {             
                    string AddEmp = "INSERT INTO Employee2 (PersonalNumber, FirstName," +
                    "Surname, DateOfBirth, IdentificationNumber) " +
                    "VALUES (@PersonalNumber, @FirstName, @Surname," +
                    "@DateOfBirth, @IdentificationNumber)";

                    using (SqlCommand cmd = new SqlCommand(AddEmp, _connection))
                    {
                        cmd.Parameters.AddWithValue("@PersonalNumber", emp.PersonalNumber);
                        cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                        cmd.Parameters.AddWithValue("@Surname", emp.Surname);
                        cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
                        cmd.Parameters.AddWithValue("@IdentificationNumber", emp.IdentificationNumber);

                        _connection.Open();
                        int i = cmd.ExecuteNonQuery();
                        _connection.Close();

                        if (i >= 1) return true;
                        else return false;
                }
                
            }

            catch (SqlException SQLError)
            {
                throw new ApplicationException("A database error occurred while adding a new employee. Please try again later.");
            }
            catch (Exception error)
            {
                throw new ApplicationException("An error occurred while processing your request. Please try again later.");
  
            }
        }

         public Employee2 GetEmpByPN(string PersonalNumber)
         {
             Employee2 emp = new Employee2();

             SqlCommand cmd = new SqlCommand("GetEmpDetailsByPN2", _connection);
             cmd.CommandType = System.Data.CommandType.StoredProcedure;

             SqlParameter parameter;

            //cmd.Parameters.Add(new SqlParameter("@PersonalNumber", PersonalNumber));
            //cmd.Parameters.AddWithValue("@PersonalNumber", PersonalNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PersonalNumber", PersonalNumber);



            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

             DataTable dt = new DataTable();

             dataAdapter.Fill(dt);

             foreach (DataRow dr in dt.Rows)
             {
                 emp = new Employee2
                 {
                     PersonalNumber = dr["PersonalNumber"].ToString(),
                     FirstName = dr["FirstName"].ToString(),
                     Surname = dr["Surname"].ToString(),
                     DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                     IdentificationNumber = Convert.ToInt32(dr["IdentificationNumber"]),
                 };
             }
             return emp;
         }
        

        /*public Employee GetEmpByPN(string PersonalNumber)
        {
            Employee emp = new Employee();

            using (SqlCommand cmd = new SqlCommand("GetEmpDetailsByPN", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PersonalNumber", PersonalNumber));
                //cmd.Parameters.AddWithValue("@PersonalNumber", PersonalNumber ?? (object)DBNull.Value);

            

                DataTable dt = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dt);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    emp = new Employee
                    {
                        PersonalNumber = dr["PersonalNumber"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                        IdentificationNumber = Convert.ToInt32(dr["IdentificationNumber"]),
                    };
                }
            }

            return emp;
        }
        */



        public bool EditEmpDetails(string PersonalNumber, Employee2 emp)
        {
            SqlCommand cmd = new SqlCommand("EditEmp2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PersonalNumber", PersonalNumber);

            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
            cmd.Parameters.AddWithValue("@Surname", emp.Surname);
            cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@IdentificationNumber", emp.IdentificationNumber);

            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1) return true;
            else return false;
        }

        public bool DeleteEmpDetails(string PersonalNumber, Employee2 emp)
        {
            SqlCommand cmd = new SqlCommand("DeleteEmp2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PersonalNumber", PersonalNumber);

            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1) return true;
            else return false;
        }
        #endregion


        #region CRUD for Cities
        public City2 GetCityByPN(int CityIdentificator)
        {
            {
                City2 city = new City2();

                SqlCommand cmd = new SqlCommand("GetCityByCI2", _connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter parameter;

                cmd.Parameters.AddWithValue("@CityIdentificator", CityIdentificator);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                dataAdapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    city = new City2
                    {
                        CityIdentificator = Convert.ToInt32(dr["CityIdentificator"]),
                        CityName = dr["CityName"].ToString(),
                        Country = dr["Country"].ToString(),
                        _Coordinates = new Coordinates
                        {
                            Longitude = float.TryParse(dr["Longitude"].ToString(), out float longitude) ? longitude : 0.0f,
                            Latitude = float.TryParse(dr["Latitude"].ToString(), out float latitude) ? latitude : 0.0f
                        }
                       
                    };
                }
                return city;
            }
        }

        public bool _EditCityDetails(int CityIdentificator, City2 city)
        {
            SqlCommand cmd = new SqlCommand("EditCity2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CityIdentificator", CityIdentificator);

            cmd.Parameters.AddWithValue("@CityName", city.CityName);
            cmd.Parameters.AddWithValue("@Country", city.Country);
            cmd.Parameters.AddWithValue("@Longitude", city._Coordinates.Longitude);
            cmd.Parameters.AddWithValue("@Latitude", city._Coordinates.Latitude);

            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1) return true;
            else return false;
        }

        public bool _DeleteCityDetails(int CityIdentificator, City2 city)
        {
            SqlCommand cmd = new SqlCommand("DeleteCity2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CityIdentificator", CityIdentificator);

            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1) return true;
            else return false;
        }

        #endregion


        #region CRUD for CP

        public CP2 GetCPByPN(int CP_Identificator)
        {
            CP2 cp = new CP2();

            SqlCommand cmd = new SqlCommand("GetCPByPN2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter parameter;

            //cmd.Parameters.Add(new SqlParameter("@PersonalNumber", PersonalNumber));
            //cmd.Parameters.AddWithValue("@PersonalNumber", PersonalNumber ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CP_Identificator", CP_Identificator);



            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            dataAdapter.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                cp = new CP2
                {
                    CP_Identificator = Convert.ToInt32(dr["CP_Identificator"]),
                    CP_Date = Convert.ToDateTime(dr["CP_Date"]),

                    EmployeePersonalNumber = dr["EmployeePersonalNumber"].ToString(),

                    StartCityID = Convert.ToInt32(dr["StartCityID"]),
                    EndCityID = Convert.ToInt32(dr["EndCityID"]),

                    StartTime = Convert.ToDateTime(dr["StartTime"]),
                    EndTime = Convert.ToDateTime(dr["EndTime"]),
                };
            }
            return cp;
        }


        public bool EditCPDetails(int CP_Identificator, CP2 cp)
        {
            SqlCommand cmd = new SqlCommand("EditCP2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CP_Identificator", CP_Identificator);



            cmd.Parameters.AddWithValue("@CP_Date", cp.CP_Date);
            cmd.Parameters.AddWithValue("@EmployeePersonalNumber", cp.EmployeePersonalNumber);

            cmd.Parameters.AddWithValue("@StartCityID", cp.StartCityID);
            cmd.Parameters.AddWithValue("@EndCityID", cp.EndCityID);

            cmd.Parameters.AddWithValue("@StartTime", cp.StartTime);
            cmd.Parameters.AddWithValue("@EndTime", cp.EndTime);


            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1) return true;
            else return false;
        }
       

        public bool DeleteCPDetails(int CP_Identificator, CP2 cp)
        {
            SqlCommand cmd = new SqlCommand("DeleteCP2", _connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CP_Identificator", CP_Identificator);

            _connection.Open();
            int i = cmd.ExecuteNonQuery();
            _connection.Close();

            if (i >= 1) return true;
            else return false;
        }
    }
    #endregion
}
