using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseMetadata
{
    class Query
    {
        static string connectionString = @"Data Source=PAWAN-DESKTOP;Initial Catalog=Test;User ID=sa;Password=test123";

        static SqlConnection dbConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public static IEnumerable<DataRow> eachLine(string queryString)
        {
            using (var connection = dbConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach(var dataRow in dataTable.Rows)
                {
                    yield return (DataRow)dataRow;
                }
            }
        }
    }
}
