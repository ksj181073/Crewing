// dotnet add package Microsoft.Data.Sqlite
using Microsoft.Data.Sqlite;
// dotnet add package System.Data.SQLite
using System.Data.SQLite;

namespace Crewing
{
    static class DbOperations
    {
        // public void SignOn (Rank signing_on_as, DateOnly sign_on_date)

        // public static void DB_SignOn (Rank signing_on_as, DateOnly sign_on_date)
        // {
        //     try
        //     {
        //         using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
        //         {
        //             con.Open();
                    
        //             using (SqliteCommand sql = con.CreateCommand())
        //             {
        //                 sql.CommandText = "SELECT ";
        //                 sql.ExecuteNonQuery();
        //             }
        //         }
        //     }
        //     catch (SqliteException ex)
        //     {
        //         Console.WriteLine("EXCEPTION: " + ex.Message);
        //     }


        //     // Vessel.GetVessel.Crew.Add(new CrewMember(this, signing_on_as, sign_on_date));
        // }

        public static void ExecuteSqlCommand(string db_name, string sql_command)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection("data source=" + db_name))
                {
                    con.Open();
                    
                    using (SqliteCommand sql = con.CreateCommand())
                    {
                        sql.CommandText = sql_command;
                        sql.ExecuteNonQuery();
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("SQLITE_EXCEPTION: " + ex.Message);
            }
        }
        public static string? SqlSelectIdFromTable(string db_name, string sql_command)
        {
            try
            {
                using (SqliteConnection con = new SqliteConnection("data source=" + db_name))
                {
                    con.Open();
                    
                    using (SqliteCommand sql = con.CreateCommand())
                    {
                        sql.CommandText = sql_command;

                        using (SqliteDataReader sql_reader = sql.ExecuteReader())
                        {
                            string Id = "";

                            while (sql_reader.Read())
                                Id = sql_reader.GetString(0);
                            
                            if (Id != "")
                                return Id;
                            else
                                return "null";
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("SQLITE_EXCEPTION: " + ex.Message);

                return null;
            }
        }
    }
}