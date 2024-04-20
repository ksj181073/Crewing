#nullable disable

// dotnet add package Microsoft.Data.Sqlite
using Microsoft.Data.Sqlite;
// dotnet add package System.Data.SQLite
using System.Data.SQLite;

namespace Crewing
{
    class DbInit
    {
        /*
        /   NOT TO BE USED IN PRODUCTION.
        /
        /   Creating the initial dababase file from the exixting Excel solution
        /   via a csv file.
        */

        private static void CreateDB(string db_name)
        {
            using(SQLiteConnection con = new SQLiteConnection("URI=file:" + db_name))
            {
                con.Open();
            }
        }
        private static void CreateTable(string db_name, string table_name, string field_description)
        {
            DbOperations.ExecuteSqlCommand(db_name, "CREATE TABLE IF NOT EXISTS " + table_name + " (" + field_description + ")");
        }
        private static void InsertIntoTable(string db_name, string table_name, string value_description)
        {
            DbOperations.ExecuteSqlCommand(db_name, "INSERT INTO " + table_name + " VALUES " + value_description);
        }
        private static void DeleteRecordFromTable(string db_name, string table_name, string condition)
        {
            DbOperations.ExecuteSqlCommand(db_name, "DELETE FROM " + table_name + " WHERE " + condition);
        }
        public static void FillNationalities()
        {
            using(StreamReader reader = new StreamReader("nationalities.csv"))
            {
                string db_name = "jd-crew.db";

                string value_list = "";

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    value_list += "(\"" + values[0] + "\",'" + values[1] + "','" + values[2] + "'),\n";
                }

                InsertIntoTable(db_name, "Nationalities (Name, ISO2_Code, ISO3_Code)", value_list.Substring(0, value_list.Length - 2));
            }
        }
        public static void TransferMedicalInfo()
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=" + Statics.GetConfigValue("FILES", "Db")))
            {
                con.Open();

                SqliteCommand persCommand = con.CreateCommand();
                SqliteCommand medCommand = con.CreateCommand();

                    persCommand.CommandText = "SELECT * FROM Persons";

                    using (SqliteDataReader reader = persCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(9))
                            {
                                medCommand.CommandText = $"INSERT INTO Certificates (CertificateType_Id, ExpiryDate, Person_Id) VALUES " + 
                                    $"(1, '{reader.GetString(9)}', {reader.GetInt32(0)})";

                                medCommand.ExecuteNonQuery();
                            }
                        }
                    }
            }
        }
        public static void FillPersons()
        {
            using(StreamReader reader = new StreamReader("crewlist.csv"))
            {
                string db_name = "jd-crew.db";

                string value_list = "";

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var values = line.Split(';');

                    value_list += "(\"" + values[3] + "\",\"" + values[2] + "\"";                                                                               //FirstName, LastName
                    value_list += "," + DbOperations.SqlSelectIdFromTable(db_name, "SELECT Id FROM Nationalities WHERE Name=\"" + values[4] + "\"");                             //Nationality_Id
                    value_list += "," + DbOperations.SqlSelectIdFromTable(db_name, "SELECT Id FROM Ranks WHERE Name=\"" + values[5] + "\"");                                     //Rank_Id

                    DateOnly b_date = DateOnly.Parse(values[6]);
                    value_list += ",\"" + String.Format("{0,0:D4}-{1,0:D2}-{2,0:D2}", b_date.Year, b_date.Month, b_date.Day) + "\",\"" + values[7] + "\"";    //DateOfBirth, PlaceOfBirth
                    value_list += "," + DbOperations.SqlSelectIdFromTable(db_name, "SELECT Id FROM Genders WHERE Name=\"" + values[8] + "\"");                                   //Gender_Id
                    
                    try
                    {
                        DateOnly med_exp_date = DateOnly.Parse(values[17]);
                        value_list += ",\"" + String.Format("{0,0:D4}-{1,0:D2}-{2,0:D2}", med_exp_date.Year, med_exp_date.Month, med_exp_date.Day) + "\"";
                    }
                    catch (FormatException fe)
                    {
                        value_list += ",null";
                    }

                    value_list += "),\n";
                }

                InsertIntoTable(db_name, "Persons (FirstName, LastName, Nationality_Id, Rank_Id, DateOfBirth, PlaceOfBirth, Gender_Id, MedicalExpiryDate)", 
                                value_list.Substring(0, value_list.Length - 2));
                
                // Console.WriteLine(value_list.Substring(0, value_list.Length - 2));
            }
        }
        public static void FillIdCards()
        {
            using(StreamReader reader = new StreamReader("crewlist.csv"))
            {
                string db_name = "jd-crew.db";

                string value_list = "";

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    var values = line?.Split(';');
                    
                    string? issue_date = "";
                    string? exp_date = "";
                    DateTime e_date = new DateTime();

                    if(values[13] != "")
                    {
                        try
                        {
                            DateTime i_date = DateTime.Parse(values[14]);
                            issue_date = String.Format("{0,0:D4}-{1,0:D2}-{2,0:D2}", i_date.Year, i_date.Month, i_date.Day);
                        }
                        catch (FormatException)
                        {
                            issue_date = null;
                        }

                        try
                        {
                            e_date = DateTime.Parse(values[15]);
                            exp_date = String.Format("{0,0:D4}-{1,0:D2}-{2,0:D2}", e_date.Year, e_date.Month, e_date.Day);
                        }
                        catch (FormatException)
                        {
                            exp_date = null;
                        }
                        
                        value_list += "(\"" + values[13] + "\"";     //IdDocNumber
                        value_list += "," + DbOperations.SqlSelectIdFromTable(db_name, "SELECT Id FROM IdDocumentTypes WHERE Name=\"" + values[9] + "\"");      //IdDocType_Id
                        value_list += "," + DbOperations.SqlSelectIdFromTable(db_name, "SELECT Id FROM Nationalities WHERE Name=\"" + values[4] + "\"");      //IssuingCountry_Id

                        if (issue_date == null)
                            value_list += ",null";
                        else
                            value_list += ",\"" + issue_date + "\"";

                        if (exp_date == null)
                            value_list += ",null";
                        else
                            value_list += ",\"" + exp_date + "\"";

                        value_list += "," + DbOperations.SqlSelectIdFromTable(db_name, "SELECT Id FROM Persons WHERE LastName=\"" + values[2] + "\" AND FirstName=\"" + values[3] + "\"");      //Person_Id
                        
                        if (DateTime.Compare(e_date, DateTime.Now) <= 0)
                            value_list += "," + false + "),\n";   //InUse
                        else
                            value_list += "," + true + "),\n";   //InUse
                    }
                }

                InsertIntoTable(db_name, "IdDocuments (Number, IdDocumentType_Id, IssuingCountry_Id, IssueDate, ExpiryDate, Person_Id, InUse)", 
                                value_list.Substring(0, value_list.Length - 2));

                // Console.WriteLine(value_list.Substring(0, value_list.Length - 2));
            }
        }
        public static void InitiateDb()
        {
            // string db_name = "jd-crew.db";

            /*CreateDB(db_name);

            CreateTable(db_name, "Departments", "Id INTEGER PRIMARY KEY, Name CHAR, Sorting INT");
            CreateTable(db_name, "Ranks", "Id INTEGER PRIMARY KEY, Name CHAR, Sorting INTEGER, Department_Id INTEGER REFERENCES Departments (Id)");
            CreateTable(db_name, "Nationalities", "Id INTEGER PRIMARY KEY, Name CHAR, ISO2_Code CHAR, ISO3_Code CHAR");
            CreateTable(db_name, "Genders", "Id INTEGER PRIMARY KEY, Name CHAR");

            CreateTable(db_name, "Persons", @"Id INTEGER PRIMARY KEY, 
                                              FirstName CHAR, 
                                              LastName CHAR, 
                                              Nationality_Id INTEGER REFERENCES Nationalities (Id),
                                              Rank_Id INTEGER REFERENCES Ranks (Id),
                                              DateOfBirth INTEGER,
                                              PlaceOfBirth CHAR,
                                              Gender_Id INTEGER REFERENCES Genders (Id)");

            CreateTable(db_name, "IdDocumentTypes", "Id INTEGER PRIMARY KEY, Name CHAR, Code CHAR");
            CreateTable(db_name, "IdDocuments", @"Id INTEGER PRIMARY KEY, 
                                                  Number CHAR, 
                                                  IdDocumentType_Id INTEGER REFERENCES IdDocumentTypes (Id),
                                                  IssuingAuthority CHAR,
                                                  IssueDate INTEGER,
                                                  ExpiryDate INTEGER,
                                                  Person_Id INTEGER REFERENCES Persons (Id),
                                                  InUse BOOLEAN DEFAULT False");

            CreateTable(db_name, "CertificateTypes", "Id INTEGER PRIMARY KEY, Name CHAR");
            CreateTable(db_name, "Certificates", @"Id INTEGER PRIMARY KEY, 
                                                   Number CHAR, 
                                                   CertificateType_Id INTEGER REFERENCES CertificateTypes (Id),
                                                   IssuingAuth CHAR,
                                                   IssueDate INTEGER,
                                                   ExpiryDate INTEGER,
                                                   Person_Id INTEGER REFERENCES Persons (Id)");*/

            /*InsertIntoTable(db_name, "Departments (Id, Name, Sorting)",@"(1, 'Deck', 10),
                                                                         (2, 'Engine', 20),
                                                                         (3, 'Deck Ratings', 30),
                                                                         (4, 'Engine Ratings', 40),
                                                                         (5, 'Catering', 50),
                                                                         (6, 'Diving', 60),
                                                                         (7, 'Survey', 70),
                                                                         (8, 'Project', 80),
                                                                         (9, 'Cadets', 90),
                                                                         (10, 'Others', 1000),
                                                                         (11, 'Passengers', 10000)");*/

            /*InsertIntoTable(db_name, "Ranks (Name, Sorting, Department_Id)", @"('Master', 10, 1),
                                                                              ('Chief Officer', 20, 1),
                                                                              ('First Officer', 30, 1),
                                                                              ('Second Officer', 40, 1),
                                                                              ('Chief Engineer', 10, 2),
                                                                              ('First Engineer', 20, 2),
                                                                              ('Second Engineer', 30, 2),
                                                                              ('Ship Assistant', 10, 3),
                                                                              ('AB', 20, 3),
                                                                              ('OS', 30, 3),
                                                                              ('Motorman', 10, 4),
                                                                              ('Chief Steward', 10, 5),
                                                                              ('Chief Cook', 20, 5),
                                                                              ('Cook', 30, 5),
                                                                              ('Second Cook', 40, 5),
                                                                              ('Steward', 50, 5),
                                                                              ('Stewardess', 60, 5),
                                                                              ('Party Chief', 10, 6),
                                                                              ('Lead Diver', 20, 6),
                                                                              ('Diver', 30, 6),
                                                                              ('Surveyor', 10, 7),
                                                                              ('Project Manager', 10, 8),
                                                                              ('Client Representative', 20, 8),
                                                                              ('Deck Cadet', 10, 9),
                                                                              ('Engine Cadet', 20, 9),
                                                                              ('Dual Cadet', 30, 9),
                                                                              ('Other', 10, 10),
                                                                              ('Passenger', 10, 11)");*/
            
            //InsertIntoTable(db_name, "Genders (Name)", "('Male'), ('Female'), ('Non-binary'), ('Unknown')");

            //InsertIntoTable(db_name, "IdDocumentTypes (Name, Code)", "('Passport', 'P'),('Identity Card', 'I'),('Registration Doc.', 'O'),('Visa', 'O'),(\"Seaman's Book\", 'O'),('Residence Permit','O'),('Muster Book','O'),('Other','O')");

            //InsertIntoTable(db_name, "Persons (FirstName, LastName, Nationality_Id, Rank_Id, DateOfBirth, PlaceOfBirth, Gender_Id)", @"('Kristian Søgaard', 'Jensen', 60, 1, '1973-10-18', 'Køge', 1),
            //                                                                                                                           ('Anders Dam', 'Jensen', 60, 1, '1973-10-18', 'Køge', 1)");

            //DeleteRecordFromTable(db_name, "Persons", "Id BETWEEN 0 AND 500");
        }
    }
}