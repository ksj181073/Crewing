#nullable disable

using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Salaros.Configuration;

namespace Crewing
{
    public static class Statics
    {
        static public string conf_file = @"crewapp.conf";
        public static string GetConfigValue(string section, string key)
        {
            ConfigParser appConfig;

            if (File.Exists(Statics.conf_file))
                appConfig = new ConfigParser(Statics.conf_file);
            else
                throw new FileNotFoundException($"The file configuration '{Statics.conf_file}' cannot be found", Statics.conf_file);

            return appConfig.GetValue(section, key);
        }
        public static dynamic GetVesselInfo()
        {
            string json = "";
            string vesselFile = "";

            vesselFile = GetConfigValue("FILES", "Vessel");

            if (File.Exists(vesselFile))
            {
                using (StreamReader reader = new StreamReader(vesselFile))
                {
                    json = reader.ReadToEnd();
                }
            }
            else
                throw new FileNotFoundException($"The file configuration '{vesselFile}' cannot be found", vesselFile);

            return JObject.Parse(json);
        }
        public static List<Deck> GetVesselAccommodation()
        {
            string vesselFile = GetConfigValue("FILES", "Cabins");

            if (File.Exists(vesselFile))
            {
                using(StreamReader reader = new StreamReader(vesselFile))
                {
                    string json = reader.ReadToEnd();

                    return JsonConvert.DeserializeObject<List<Deck>>(json);
                }
            }
            else
                throw new FileNotFoundException($"The configuration file '{vesselFile}' cannot be found", vesselFile);
        }
        private static List<Department> GetDepartments(SqliteConnection con)
        {
            List<Department> departments = new List<Department>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Departments";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Department dept = new Department();

                        if (!reader.IsDBNull(0))
                            dept.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            dept.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            dept.SortOrder = reader.GetInt32(2);

                        departments.Add(dept);
                    }
                }
            }
            
            return departments;
        }
        private static List<Gender> GetGenders(SqliteConnection con)
        {
            List<Gender> genders = new List<Gender>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Genders";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Gender gender = new Gender();

                        if (!reader.IsDBNull(0))
                            gender.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            gender.Name = reader.GetString(1);

                        genders.Add(gender);
                    }
                }
            }
            
            return genders;
        }
        private static List<IdDocumentType> GetIdDocumentTypes(SqliteConnection con)
        {
            List<IdDocumentType> idDocumentTypes = new List<IdDocumentType>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM IdDocumentTypes";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IdDocumentType idDocumentType = new IdDocumentType();

                        if (!reader.IsDBNull(0))
                            idDocumentType.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            idDocumentType.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            idDocumentType.Code = reader.GetChar(1);

                        idDocumentTypes.Add(idDocumentType);
                    }
                }
            }
            
            return idDocumentTypes;
        }
        private static List<CertificateType> GetCertificateTypes(SqliteConnection con)
        {
            List<CertificateType> certificateTypes = new List<CertificateType>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CertificateTypes";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CertificateType certificateType = new CertificateType();

                        if (!reader.IsDBNull(0))
                            certificateType.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            certificateType.Name = reader.GetString(1);

                        certificateTypes.Add(certificateType);
                    }
                }
            }
            
            return certificateTypes;
        }
        private static List<CrewEventType> GetCrewEventTypes(SqliteConnection con)
        {
            List<CrewEventType> crewEventTypes = new List<CrewEventType>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CrewEventTypes";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CrewEventType crewEventType = new CrewEventType();

                        if (!reader.IsDBNull(0))
                            crewEventType.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            crewEventType.Name = reader.GetString(1);

                        crewEventTypes.Add(crewEventType);
                    }
                }
            }
            
            return crewEventTypes;
        }
        private static List<VesselEventType> GetVesselEventTypes(SqliteConnection con)
        {
            List<VesselEventType> vesselEventTypes = new List<VesselEventType>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VesselEventTypes";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VesselEventType vesselEventType = new VesselEventType();

                        if (!reader.IsDBNull(0))
                            vesselEventType.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            vesselEventType.Name = reader.GetString(1);

                        vesselEventTypes.Add(vesselEventType);
                    }
                }
            }
            
            return vesselEventTypes;
        }
        private static List<Nationality> GetNationalities(SqliteConnection con)
        {
            List<Nationality> nationalities = new List<Nationality>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Nationalities";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Nationality nationality = new Nationality();

                        if (!reader.IsDBNull(0))
                            nationality.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            nationality.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            nationality.ISO2_Code = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                            nationality.ISO3_Code = reader.GetString(3);

                        nationalities.Add(nationality);
                    }
                }
            }
            
            return nationalities;
        }
        private static List<Rank> GetRanks(SqliteConnection con)
        {
            List<Rank> ranks = new List<Rank>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Ranks";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rank rank = new Rank();

                        if (!reader.IsDBNull(0))
                            rank.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            rank.Name = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            rank.Sorting = reader.GetInt32(2);
                        
                        if (!reader.IsDBNull(3))
                        {
                            foreach (Department dept in Lists.GetLists.Departments)
                            {
                                if (dept.Id == reader.GetInt32(3))
                                {
                                    rank.Department = dept;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            rank.Department = new Department();
                        }

                        ranks.Add(rank);
                    }
                }
            }
            
            return ranks;
        }
        private static List<IdDocument> GetIdDocuments(SqliteConnection con)
        {
            List<IdDocument> idDocuments = new List<IdDocument>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM IdDocuments";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IdDocument idDocument = new IdDocument();

                        if (!reader.IsDBNull(0))
                            idDocument.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            idDocument.Number = reader.GetString(1);
                        
                        if (!reader.IsDBNull(2))
                        {
                            foreach (IdDocumentType doctype in Lists.GetLists.IdDocumentTypes)
                            {
                                if (doctype.Id == reader.GetInt32(2))
                                {
                                    idDocument.DocumentType = doctype;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            idDocument.DocumentType = new IdDocumentType();
                        }
                        
                        if (!reader.IsDBNull(3))
                        {
                            foreach (Nationality country in Lists.GetLists.Nationalities)
                            {
                                if (country.Id == reader.GetInt32(3))
                                {
                                    idDocument.IssuingCountry = country;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            idDocument.IssuingCountry = new Nationality();
                        }

                        if (!reader.IsDBNull(4))
                            idDocument.IssueDate = DateOnly.Parse(reader.GetString(4));
                        if (!reader.IsDBNull(5))
                            idDocument.ExpiryDate = DateOnly.Parse(reader.GetString(5));
                        
                        if (!reader.IsDBNull(6))
                        {
                            foreach (Person person in Lists.GetLists.Persons)
                            {
                                if (person.Id == reader.GetInt32(6))
                                {
                                    person.IdDocuments.Add(idDocument);
                                    break;
                                }
                            }
                        }

                        if (!reader.IsDBNull(7))
                            idDocument.InUse = reader.GetBoolean(7);

                        idDocuments.Add(idDocument);
                    }
                }
            }
            
            return idDocuments;
        }
        private static List<Certificate> GetCertificates(SqliteConnection con)
        {
            List<Certificate> certificates = new List<Certificate>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Certificates";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Certificate certificate = new Certificate();

                        if (!reader.IsDBNull(0))
                            certificate.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            certificate.Number = reader.GetString(1);
                        
                        if (!reader.IsDBNull(2))
                        {
                            foreach (CertificateType certificateType in Lists.GetLists.CertificateTypes)
                            {
                                if (certificateType.Id == reader.GetInt32(2))
                                {
                                    certificate.CertificateType = certificateType;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            certificate.CertificateType = new CertificateType();
                        }
                        
                        if (!reader.IsDBNull(3))
                            certificate.Number = reader.GetString(3);
                        if (!reader.IsDBNull(4))
                            certificate.IssueDate = DateOnly.Parse(reader.GetString(4));
                        if (!reader.IsDBNull(5))
                            certificate.ExpiryDate = DateOnly.Parse(reader.GetString(5));

                        if (!reader.IsDBNull(6))
                        {
                            foreach (Person person in Lists.GetLists.Persons)
                            {
                                if (person.Id == reader.GetInt32(6))
                                {
                                    person.Certificates.Add(certificate);
                                    break;
                                }
                            }
                        }

                        certificates.Add(certificate);
                    }
                }
            }
            
            return certificates;
        }
        private static List<Person> GetPersons(SqliteConnection con)
        {
            List<Person> persons = new List<Person>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Persons";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Person person = new Person();

                        if (!reader.IsDBNull(0))
                            person.Id = reader.GetInt32(0);
                        if (!reader.IsDBNull(1))
                            person.FirstName = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            person.LastName = reader.GetString(2);
                        
                        if (!reader.IsDBNull(3))
                        {
                            foreach (Nationality nationality in Lists.GetLists.Nationalities)
                            {
                                if (nationality.Id == reader.GetInt32(3))
                                {
                                    person.Nationality = nationality;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            person.Nationality = new Nationality();
                        }

                        if (!reader.IsDBNull(4))
                        {
                            person.Rank = new Rank();

                            foreach (Rank rank in Lists.GetLists.Ranks)
                            {
                                if (rank.Id == reader.GetInt32(4))
                                {
                                    person.Rank = rank;
                                    break;
                                }
                            }
                        }                          

                        if (!reader.IsDBNull(5))
                            person.DateOfBirth = DateOnly.Parse(reader.GetString(5));
                        if (!reader.IsDBNull(6))
                            person.PlaceOfBirth = reader.GetString(6);
                        
                        if (!reader.IsDBNull(7))
                        {
                            foreach (Gender gender in Lists.GetLists.Genders)
                            {
                                if (gender.Id == reader.GetInt32(7))
                                {
                                    person.Gender = gender;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            person.Gender = new Gender();
                        }
                        
                        // if (!reader.IsDBNull(8))
                        //     person.MedicalIssue = DateOnly.Parse(reader.GetString(8));

                        // if (!reader.IsDBNull(9))
                        //     person.MedicalExpiry = DateOnly.Parse(reader.GetString(9));

                        persons.Add(person);
                    }
                }
            }
            
            return persons;
        }       
        private static List<VesselEvent> GetVesselEvents(SqliteConnection con)
        {
            List<VesselEvent> vesselEvents = new List<VesselEvent>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VesselEvents";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        VesselEvent vesselEvent = new VesselEvent();

                        if (!reader.IsDBNull(0))
                            vesselEvent.Id = reader.GetInt32(0);
                        
                        if (!reader.IsDBNull(1))
                        {
                            foreach (VesselEventType eventType in Lists.GetLists.VesselEventTypes)
                            {
                                if (eventType.Id == reader.GetInt32(1))
                                {
                                    vesselEvent.EventType = eventType;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            vesselEvent.EventType = new VesselEventType();
                        }

                        if (!reader.IsDBNull(2))
                            vesselEvent.Date = DateOnly.Parse(reader.GetString(2));
                        if (!reader.IsDBNull(3))
                            vesselEvent.Time = TimeOnly.Parse(reader.GetString(3));
                        
                        if (!reader.IsDBNull(4))
                            vesselEvent.Place = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                            vesselEvent.Remark = reader.GetString(5);

                        vesselEvents.Add(vesselEvent);
                    }
                }
            }
            
            return vesselEvents;
        }
        private static List<CrewEvent> GetCrewEvents(SqliteConnection con)
        {
            List<CrewEvent> crewEvents = new List<CrewEvent>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM CrewEvents";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CrewEvent crewEvent = new CrewEvent();

                        if (!reader.IsDBNull(0))
                            crewEvent.Id = reader.GetInt32(0);
                        
                        if (!reader.IsDBNull(1))
                        {
                            foreach (CrewEventType eventType in Lists.GetLists.CrewEventTypes)
                            {
                                if (eventType.Id == reader.GetInt32(1))
                                {
                                    crewEvent.EventType = eventType;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            crewEvent.EventType = new CrewEventType();
                        }

                        if (!reader.IsDBNull(2))
                        {
                            foreach (Person person in Lists.GetLists.Persons)
                            {
                                if (person.Id == reader.GetInt32(2))
                                {
                                    // person.Events.Add(crewEvent);
                                    crewEvent.Person = person;
                                    break;
                                }
                            }
                        }

                        if (!reader.IsDBNull(3))
                            crewEvent.Date = DateOnly.Parse(reader.GetString(3));
                        if (!reader.IsDBNull(4))
                            crewEvent.Time = TimeOnly.Parse(reader.GetString(4));
                        
                        if (!reader.IsDBNull(5))
                            crewEvent.Place = reader.GetString(5);
                        if (!reader.IsDBNull(6))
                            crewEvent.Remark = reader.GetString(6);

                        crewEvents.Add(crewEvent);

                    }
                }
            }
            
            return crewEvents;
        }
        private static List<CrewMember> GetVesselCrew(SqliteConnection con)
        {
            List<CrewMember> crewMembers = new List<CrewMember>();
            string db_file = GetConfigValue("FILES", "Db");

            using (SqliteCommand command = con.CreateCommand())
            {
                command.CommandText = "SELECT * FROM VesselCrew";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CrewMember crewMember = new CrewMember();

                        if (!reader.IsDBNull(0))
                            crewMember.Id = reader.GetInt32(0);
                        
                        if (!reader.IsDBNull(1))
                        {
                            foreach (Person person in Lists.GetLists.Persons)
                            {
                                if (person.Id == reader.GetInt32(1))
                                {
                                    crewMember.Person = person;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            throw new MissingNamesException("Signed on crew member not found in person-list");
                        }
                        
                        if (!reader.IsDBNull(2))
                        {
                            foreach (Rank rank in Lists.GetLists.Ranks)
                            {
                                if (rank.Id == reader.GetInt32(2))
                                {
                                    crewMember.SignedOnAs = rank;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            throw new MissingRankException("Signed on crew member' rank not found in rank-list");
                        }

                        if (!reader.IsDBNull(3))
                            crewMember.SignOnDate = DateOnly.Parse(reader.GetString(3));
                        
                        if (!reader.IsDBNull(4))
                        {
                            foreach (Deck deck in Vessel.GetVessel.Decks)
                                foreach (Cabin cabin in deck.Cabins)
                                    if (cabin.Number == reader.GetString(4))
                                    {
                                        crewMember.Cabin = cabin;
                                        cabin.PersonsOccupying++;
                                        
                                        break;
                                    }
                        }
                            // crewMember.Cabin_No = reader.GetString(4);

                        crewMembers.Add(crewMember);
                    }
                }
            }
            
            return crewMembers;
        }
        private static void GetCabinOccupation(SqliteConnection con)
        {
        }
        public static void GenerateInitialLists()
        {
            string db_file = GetConfigValue("FILES", "Db");

            if (File.Exists(db_file))
            {
                try
                {
                    using (SqliteConnection con = new SqliteConnection("data source=" + db_file))
                    {
                        con.Open();

                        Lists.GetLists.Departments = GetDepartments(con);
                        Lists.GetLists.Genders = GetGenders(con);                        
                        Lists.GetLists.Nationalities = GetNationalities(con);
                        Lists.GetLists.Ranks = GetRanks(con);
                        Lists.GetLists.Persons = GetPersons(con);
                        Lists.GetLists.IdDocumentTypes = GetIdDocumentTypes(con);
                        Lists.GetLists.IdDocuments = GetIdDocuments(con);
                        Lists.GetLists.CertificateTypes = GetCertificateTypes(con);
                        Lists.GetLists.Certificates = GetCertificates(con);

                        Lists.GetLists.VesselEventTypes = GetVesselEventTypes(con);
                        Lists.GetLists.CrewEventTypes = GetCrewEventTypes(con);

                        Lists.GetLists.VesselEvents = GetVesselEvents(con);
                        Lists.GetLists.CrewEvents = GetCrewEvents(con);

                        Vessel.GetVessel.Crew = GetVesselCrew(con);

                        GetCabinOccupation(con);
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine("EXCEPTION: " + ex.Message);
                    throw;
                }
            }
            else
                throw new FileNotFoundException($"The configuration file '{db_file}' cannot be found", db_file);
        }
    }
}