using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class InventoryDAL
    {
        // This member will be used by all methods.
        private SqlConnection sqlCn = null;

        public void OpenConnection(string connectionString)
        {
            sqlCn = new SqlConnection();
            sqlCn.ConnectionString = connectionString;
            sqlCn.Open();
        }

        public void InsertAuto(NewCar newCar)
        {
            // Format and execute SQL statement.
            string sql = string.Format("Insert Into Inventory" +
            "( Make, Color, PetName) Values" +
            "('{0}', '{1}', '{2}')", newCar.Make, newCar.Color, newCar.PetName);
            // Execute using our connection.
            using (SqlCommand cmd = new SqlCommand(sql, sqlCn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public string LookUpPetName(int carID)
        {
            string carPetName = string.Empty;
            //establish name of stored proc.
            using (SqlCommand cmd = new SqlCommand("GetPetName", sqlCn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // Input param.
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@carID";
                param.SqlDbType = SqlDbType.Int;
                param.Value = carID;
                // The default direction is in fact Input, but to be clear:
                param.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(param);
                // Output param.
                param = new SqlParameter();
                param.ParameterName = "@petName";
                param.SqlDbType = SqlDbType.Char;
                param.Size = 10;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                param = new SqlParameter();
                param.ParameterName = "@color";
                param.SqlDbType = SqlDbType.VarChar;
                param.Size = 10;
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);

                // Execute the stored proc.
                cmd.ExecuteNonQuery();
                // Return output param.
                carPetName = (string)cmd.Parameters["@petName"].Value + "\t Car's Color: " + (string)cmd.Parameters["@color"].Value;
            }
            return carPetName;
        }

        public void CloseConnection()
        {
            sqlCn.Close();
        }
    }
}