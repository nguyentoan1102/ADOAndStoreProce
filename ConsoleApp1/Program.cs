using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string dp = ConfigurationManager.AppSettings["provider"];
            string connectionString = ConfigurationManager.AppSettings["cnStr"];
            //Get the factory provider.
            DbProviderFactory df = DbProviderFactories.GetFactory(dp);
            //Now get the connection object.
            // Read data from database with CommandType.Text
            //ReadDataWithDataReader(dp, connectionString, df);
            //Get pet name with storeproceduce
            InventoryDAL invenCar = new InventoryDAL();
            invenCar.OpenConnection(connectionString);
            Console.WriteLine("Car's PetName is:" + invenCar.LookUpPetName(5));
            invenCar.CloseConnection();
            Console.ReadLine();
        }

        private static void ReadDataWithSqlCommand()
        {
        }

        private static void ReadDataWithDataReader(string dataProvider, string connectionString, DbProviderFactory df)
        {
            using (DbConnection cn = df.CreateConnection())
            {
                Console.WriteLine("Your connection object is a: {0}", cn.GetType().Name);
                cn.ConnectionString = connectionString;
                cn.Open();
                // Make command object.
                DbCommand cmd = df.CreateCommand();
                Console.WriteLine("Your command object is a : {0}", cmd.GetType().Name);
                cmd.Connection = cn;
                cmd.CommandText = "Select * From Inventory";

                // Print out data with data reader.
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    Console.WriteLine("Your data reader object is a: {0}", dr.GetType().Name);
                    Console.WriteLine("\n***** Current Inventory *****");
                    while (dr.Read())
                        Console.WriteLine("-> Car #{0} is a {1}.",
                        dr["CarID"], dr["Make"].ToString());
                }
            }
        }
    }
}