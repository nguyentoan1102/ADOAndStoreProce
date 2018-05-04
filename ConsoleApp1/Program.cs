using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Data.OracleClient;
using System.Data;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["cnStr"];
#pragma warning disable CS0618 // Type or member is obsolete
            using (OracleConnection objConn = new OracleConnection(connectionString))
            {
                OracleCommand objCmd = new OracleCommand
                {
#pragma warning restore CS0618 // Type or member is obsolete
                    Connection = objConn,
                    CommandText = "human_resources.get_employee",
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                objCmd.Parameters.Add("cur_employees", OracleType.Cursor).Direction = ParameterDirection.Output;
                try
                {
                    objConn.Open();
                    OracleDataReader objReader = objCmd.ExecuteReader();
                    PrvPrintReader(objReader);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                }
                objConn.Close();
            }

            Console.ReadLine();
        }

        public static void PrvPrintReader(OracleDataReader objReader)
        {
            for (int i = 0; i < objReader.FieldCount; i++)
            {
                System.Console.Write("{0}\t", objReader.GetName(i));
            }
            System.Console.Write("\n");

            while (objReader.Read())
            {
                for (int i = 0; i < objReader.FieldCount; i++)
                {
                    System.Console.Write("{0}\t", objReader[i].ToString());
                }
                System.Console.Write("\n");
            }
        }

        public static void CountSalaryOfEmployees(string connectionString)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            using (OracleConnection objConn = new OracleConnection(connectionString))
#pragma warning restore CS0618 // Type or member is obsolete
            {
#pragma warning disable CS0618 // Type or member is obsolete
                OracleCommand objCmd = new OracleCommand
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    Connection = objConn,
                    CommandText = "count_empl_salary",
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                objCmd.Parameters.Add("pin_deptno", OracleType.Number).Value = 1700000;
                objCmd.Parameters.Add("pout_count", OracleType.Number).Direction = System.Data.ParameterDirection.Output;
                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();

                    Console.WriteLine("Number of employees have salary = 17000  is " + objCmd.Parameters["pout_count"].Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}