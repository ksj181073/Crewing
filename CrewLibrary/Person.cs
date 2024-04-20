using Microsoft.Data.Sqlite;

namespace Crewing
{
    class Person
    {
        private string? _firstname;
        private string? _lastname;
        public int? Id { get; set; }
        public string? FirstName { 
            get => _firstname;
            set => _firstname = value?.Trim();
        }
        public string? LastName { 
            get => _lastname;
            set => _lastname = value?.Trim();
        }
        public Nationality Nationality { get; set; }
        public Rank? Rank { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public List<IdDocument> IdDocuments { get; set; }
        public List<Certificate> Certificates { get; set; }
        public List<CrewEvent> Events { get; set; }
        public string Name() { return (FirstName + " " + LastName).Trim(); }
        public Person()
        {
            IdDocuments = new List<IdDocument>();
            Certificates = new List<Certificate>();
            Rank = new Rank();
            Nationality = new Nationality();
            Gender = new Gender();
        }
        
        public override string ToString()
        {
            return $"{this.Id,0:D3}: " + $"{this.Rank.Name} {this.Name()}".Trim() + $", from {this.Nationality.Name} born on {this.DateOfBirth} in {this.PlaceOfBirth}";
        }
        public static Person CreatePerson(string firstName, string lastName)
        {
            if (firstName == "" || lastName == "")
                throw new MissingNamesException("Both first and last name must contain text.");
            
            Person person = new Person();

            person.FirstName = firstName;
            person.LastName = lastName;
            
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            {
                con.Open();

                using (SqliteTransaction crewte_person_transaction = con.BeginTransaction(deferred: true))
                {
                    using (SqliteCommand command = con.CreateCommand())
                    {
                        command.CommandText = $"INSERT INTO Persons (FirstName, LastName) VALUES ('{person.FirstName}', '{person.LastName}')";                
                        command.ExecuteNonQuery();

                        command.CommandText = $"SELECT Id FROM Persons WHERE FirstName='{firstName}' AND LastName='{lastName}' ORDER BY Id DESC LIMIT 1";
                        person.Id = Convert.ToInt32(command.ExecuteScalar());

                        DateTime today = DateTime.Now;

                        command.CommandText = "INSERT INTO CrewEvents (CrewEventType_Id, Person_Id, Date, Time) VALUES (" +
                            $"{CrewEventType.GetCrewEventType("Profile created").Id}, {person.Id}, '{today.Year,0:D4}-{today.Month,0:D2}-{today.Day,0:D2}', '{today.Hour}:{today.Minute}')";
                        command.ExecuteNonQuery();
                    }

                    crewte_person_transaction.Commit();
                }
            }


            // using (SqliteTransaction sign_on_transaction = con.BeginTransaction(deferred: true))
            // {
            //     SqliteCommand sign_on_command = con.CreateCommand();
            //     sign_on_command.CommandText = "INSERT INTO VesselCrew (Person_Id, Rank_Id, SignOnDate) VALUES (" +
            //         $"{this.Id}, {signing_on_as.Id}, '{sign_on_date.Year,0:D4}-{sign_on_date.Month,0:D2}-{sign_on_date.Day,0:D2}')";

            //     sign_on_command.ExecuteNonQuery();

            //     sign_on_command.CommandText = "INSERT INTO CrewEvents (CrewEventType_Id, Person_Id, Date, Time, Place) VALUES (" +
            //         $"{signingOnEventType.Id}, {this.Id}, '{sign_on_date.Year,0:D4}-{sign_on_date.Month,0:D2}-{sign_on_date.Day,0:D2}', '{sign_on_time}', '{place}')";
            //     sign_on_command.ExecuteNonQuery();

            //     sign_on_command.CommandText = $"UPDATE VesselCrew SET Cabin_No='{cabin.Number}' WHERE Person_Id={this.Id}";
            //     sign_on_command.ExecuteNonQuery();

            //     sign_on_transaction.Commit();
            // }



            Lists.GetLists.Persons.Add(person);

            return person;
        }
        public static Person CreatePerson(string firstName, string lastName, Nationality nationality, Rank rank, DateOnly birthDate, string birthPlace, Gender gender)
        {
            Person person = CreatePerson(firstName, lastName);
            EditPersonNationality(person, nationality.Id);
            EditPersonRank(person, rank.Id);
            EditPersonBirthDate(person, birthDate);
            EditPersonBirthPlace(person, birthPlace);
            EditPersonGender(person,gender.Id);

            return person;
        }
        public static void EditPersonFirstName(Person person, string firstName)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET FirstName={firstName} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.FirstName = firstName;
        }
        public static void EditPersonLastName(Person person, string lastName)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET LastName={lastName} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.LastName = lastName;
        }
        public static void EditPersonNationality(Person person, int nationalityId)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET Nationality_Id={nationalityId} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            foreach (Nationality nationality in Lists.GetLists.Nationalities)
                if (nationality.Id == nationalityId)
                    person.Nationality = nationality;
        }
        public static void EditPersonNationality(Person person, Nationality nationality)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET Nationality_Id={nationality.Id} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.Nationality = nationality;
        }
        public static void EditPersonRank(Person person, int rankId)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET Rank_Id={rankId} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            foreach (Rank rank in Lists.GetLists.Ranks)
                if (rank.Id == rankId)
                    person.Rank = rank;
        }
        public static void EditPersonRank(Person person, Rank rank)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET Rank_Id={rank.Id} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.Rank = rank;
        }
        public static void EditPersonBirthDate(Person person, DateOnly birthDate)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET DateOfBirth='{birthDate.Year,0:D4}-{birthDate.Month,0:D2}-{birthDate.Day,0:D2}' WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.DateOfBirth = birthDate;
        }
        public static void EditPersonBirthPlace(Person person, string birthPlace)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET PlaceOfBirth='{birthPlace}' WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.PlaceOfBirth = birthPlace;
        }
        public static void EditPersonGender(Person person, int genderId)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET Gender_Id={genderId} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            foreach (Gender gender in Lists.GetLists.Genders)
                if (gender.Id == genderId)
                    person.Gender = gender;
        }
        public static void EditPersonGender(Person person, Gender gender)
        {
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                command.CommandText = $"UPDATE Persons SET Gender_Id={gender.Id} WHERE Id={person.Id}";                
                command.ExecuteNonQuery();
            }

            person.Gender = gender;
        }
        public static void RemovePerson(Person person)
        {
            foreach (CrewMember crewMember in Vessel.GetVessel.Crew)
            {
                if (crewMember.Person == person)
                    throw new MissingNamesException($"{person.Name()} is signed on as {crewMember.SignedOnAs.Name}." +
                        $"\n{char.ToUpper(person.GetPronouns()[0][0])}{person.GetPronouns()[0].Substring(1)} must be signed off the vessel before removing {person.GetPronouns()[1].ToLower()}");
            }

            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();
                
                command.CommandText = $"SELECT Person_Id FROM VesselCrew WHERE Person_Id={person.Id}";

                if (command.ExecuteReader().HasRows)
                    throw new SignedOnException($"{person.Name()} is signed on in the backend database." +
                        $"\nSuggest shutting down and restarting the application to align the data.");
                
                using (SqliteCommand del_command = con.CreateCommand())
                {
                    del_command.CommandText = $"DELETE FROM Persons WHERE Id={person.Id}";
                    del_command.ExecuteNonQuery();

                    Lists.GetLists.Persons.Remove(person);
                    person = null;
                }
            }
        }
        public string[] GetPronouns()
        {
            string[] pronouns = new string[2];

            switch (this.Gender.Name.ToLower())
            {
                case "male":
                    pronouns[0] = "he";
                    pronouns[1] = "him";
                    break;
                case "female":
                    pronouns[0] = "she";
                    pronouns[1] = "her";
                    break;
                default:
                    pronouns[0] = "they";
                    pronouns[1] = "them";
                    break;
            }

            return pronouns;
        }
        public bool ValidMedical()
        {
            bool valid = false;

            foreach (Certificate certificate in this.Certificates)
            {
                if (certificate.CertificateType.Name == "Medical" && certificate.ExpiryDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    valid = true;
                    break;
                }
            }

            return valid;
        }
        public DateOnly? GetMedicalExpiration()
        {
            DateOnly? exp = null;

            foreach (Certificate certificate in this.Certificates)
            {
                if (certificate.CertificateType.Name == "Medical")
                {
                    exp = certificate.ExpiryDate;
                    break;
                }
            }

            return exp;
        }
        public void SignOn(Cabin cabin)
        {
            SignOn(this.Rank, DateOnly.FromDateTime(DateTime.Now), TimeOnly.FromDateTime(DateTime.Now), "", cabin);
        }
        public void SignOn(Rank signing_on_as, Cabin cabin)
        {
            SignOn(signing_on_as, DateOnly.FromDateTime(DateTime.Now), TimeOnly.FromDateTime(DateTime.Now), "", cabin);
        }
        public void SignOn(Rank signing_on_as, string place, Cabin cabin)
        {
            SignOn(signing_on_as, DateOnly.FromDateTime(DateTime.Now), TimeOnly.FromDateTime(DateTime.Now), place, cabin);
        }
        public void SignOn(Rank signing_on_as, DateOnly sign_on_date, TimeOnly sign_on_time, string place, Cabin cabin)
        {
            CrewEventType signingOnEventType = new CrewEventType();

            if (!this.OnBoard())
            {
                if (cabin.Persons == 0)
                    throw new CabinNotLivableException($"Cabin {cabin.Number} does not allow for people living there.");
                // Check for vacancy in cabin
                if (!cabin.Vacancy())
                    throw new NoVacancyException($"Cabin {cabin.Number} is occupied!\nPlease choose another cabin.");

                // Check through VesselCrew table to see if the person already is signed on in the backend database.
                try
                {
                    using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
                    {
                        con.Open();

                        using (SqliteCommand command = con.CreateCommand())
                        {
                            string sql = $"SELECT * FROM VesselCrew WHERE Person_Id={this.Id}";

                            command.CommandText = sql;

                            using (SqliteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                    throw new AlreadySignedOnException($"{Rank.Name} {Name()} is already signed on as {this.RankOnBoard().Name} in the backend database" +
                                        "\nLocal copy of the data is not in sync with backend database." +
                                        "\nSuggest that you shutdown and restart the application for a fresh reload of the database.");
                                else
                                    {
                                        DateTime latestOnSignDateTime = new DateTime();
                                        DateTime latestOffSignDateTime = new DateTime();

                                        // Get Id of signing on event type
                                        foreach (CrewEventType eventTypeOn in Lists.GetLists.CrewEventTypes)
                                        {
                                            if (eventTypeOn.Name == "Signing on")
                                            {
                                                signingOnEventType = eventTypeOn;

                                                // Get latest Signing On
                                                using (SqliteCommand sign_on_command = con.CreateCommand())
                                                {
                                                    string sign_on_sql = $"SELECT * FROM CrewEvents WHERE Person_Id={this.Id} AND CrewEventType_Id={eventTypeOn.Id} ORDER BY Date, Time DESC LIMIT 1";
                                                    sign_on_command.CommandText = sign_on_sql;

                                                    using (SqliteDataReader sign_on_reader = sign_on_command.ExecuteReader())
                                                    {
                                                        // It is only relevant to look further, looking for off-signings, if previous on-signing exists
                                                        if (sign_on_reader.HasRows)
                                                        {
                                                            DateOnly latestOnSignDate = new DateOnly();
                                                            TimeOnly latestOnSignTime = new TimeOnly();

                                                            while (reader.Read())
                                                            {
                                                                if (!reader.IsDBNull(3))
                                                                    latestOnSignDate = DateOnly.Parse(reader.GetString(3));
                                                                if (!reader.IsDBNull(4))
                                                                    latestOnSignTime = TimeOnly.Parse(reader.GetString(4));
                                                            }

                                                            latestOnSignDateTime = new DateTime(latestOnSignDate.Year, latestOnSignDate.Month, latestOnSignDate.Day, 
                                                                                                latestOnSignTime.Hour, latestOnSignTime.Minute, latestOnSignTime.Second);

                                                            // Get Id of signing on event type
                                                            foreach (CrewEventType eventTypeOff in Lists.GetLists.CrewEventTypes)
                                                            {
                                                                if (eventTypeOff.Name == "Signing off")
                                                                {
                                                                    // Get latest Signing Off
                                                                    using (SqliteCommand sign_off_command = con.CreateCommand())
                                                                    {
                                                                        string sign_off_sql = $"SELECT CrewEventType_Id FROM CrewEvents WHERE Person_Id={this.Id} AND CrewEventType_Id={eventTypeOff.Id} ORDER BY Date, Time DESC LIMIT 1";
                                                                        sign_off_command.CommandText = sign_off_sql;

                                                                        using (SqliteDataReader sign_off_reader = sign_off_command.ExecuteReader())
                                                                        {
                                                                            if (sign_off_reader.HasRows)
                                                                            {
                                                                                DateOnly latestOffSignDate = new DateOnly();
                                                                                TimeOnly latestOffSignTime = new TimeOnly();

                                                                                while (reader.Read())
                                                                                {
                                                                                    if (!reader.IsDBNull(3))
                                                                                        latestOffSignDate = DateOnly.Parse(reader.GetString(3));
                                                                                    if (!reader.IsDBNull(4))
                                                                                        latestOffSignTime = TimeOnly.Parse(reader.GetString(4));
                                                                                }
                                                                                
                                                                                latestOffSignDateTime = new DateTime(latestOffSignDate.Year, latestOffSignDate.Month, latestOffSignDate.Day, 
                                                                                                                     latestOffSignTime.Hour, latestOffSignTime.Minute, latestOffSignTime.Second);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                break;
                                            }
                                        }

                                        if (latestOffSignDateTime >= latestOnSignDateTime)
                                        {
                                            // Go ahead with on-signing
                                            Vessel.GetVessel.Crew.Add(new CrewMember(this, signing_on_as, sign_on_date));
                                            Lists.GetLists.CrewEvents.Add(new CrewEvent(signingOnEventType, this, 
                                                new DateTime(sign_on_date.Year, sign_on_date.Month, sign_on_date.Day, sign_on_time.Hour, sign_on_time.Minute, sign_on_time.Second)));
                                        
                                            // Update backend database via a transaction
                                            using (SqliteTransaction sign_on_transaction = con.BeginTransaction(deferred: true))
                                            {
                                                SqliteCommand sign_on_command = con.CreateCommand();
                                                sign_on_command.CommandText = "INSERT INTO VesselCrew (Person_Id, Rank_Id, SignOnDate) VALUES (" +
                                                    $"{this.Id}, {signing_on_as.Id}, '{sign_on_date.Year,0:D4}-{sign_on_date.Month,0:D2}-{sign_on_date.Day,0:D2}')";

                                                sign_on_command.ExecuteNonQuery();

                                                sign_on_command.CommandText = "INSERT INTO CrewEvents (CrewEventType_Id, Person_Id, Date, Time, Place) VALUES (" +
                                                    $"{signingOnEventType.Id}, {this.Id}, '{sign_on_date.Year,0:D4}-{sign_on_date.Month,0:D2}-{sign_on_date.Day,0:D2}', '{sign_on_time}', '{place}')";
                                                sign_on_command.ExecuteNonQuery();

                                                sign_on_command.CommandText = $"UPDATE VesselCrew SET Cabin_No='{cabin.Number}' WHERE Person_Id={this.Id}";
                                                sign_on_command.ExecuteNonQuery();

                                                sign_on_transaction.Commit();
                                            }

                                            foreach (CrewMember crewMember in Vessel.GetVessel.Crew)
                                                if (crewMember.Person.Id == this.Id)
                                                {
                                                    crewMember.Cabin = cabin;
                                                    cabin.PersonsOccupying++;
                                                }
                                        }
                                        else
                                            throw new CronologyException($"{Rank.Name} {Name()} has a sign-on event later than any sign-off event in the backend database" +
                                                                "\nLocal copy of the data is not in sync with backend database." +
                                                                "\nSuggest that you shutdown and restart the application for a fresh reload of the database.");
                                    }
                            }
                        }
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine("SQLITE_EXCEPTION: " + ex.Message);
                }
            }

            else
                throw new AlreadyOnLocalListException($"{Rank.Name} {Name()} is already signed on as {this.RankOnBoard().Name}");
        }
        public void SignOff()
        {
            SignOff(DateOnly.FromDateTime(DateTime.Now), TimeOnly.FromDateTime(DateTime.Now), "");
        }
        public void SignOff(string place)
        {
            SignOff(DateOnly.FromDateTime(DateTime.Now), TimeOnly.FromDateTime(DateTime.Now), place);
        }
        public void SignOff(DateOnly sign_off_date, TimeOnly sign_off_time, string place)
        {
            CrewMember signOffCrewMember = new CrewMember();
            CrewEventType signingOffEventType = new CrewEventType();
            Cabin cabin = new Cabin();

            // Get Id of signing on event type
            foreach (CrewEventType eventTypeOn in Lists.GetLists.CrewEventTypes)
                if (eventTypeOn.Name == "Signing off")
                {
                    signingOffEventType = eventTypeOn;
                    break;
                }

            foreach (CrewMember crewMember in Vessel.GetVessel.Crew)
                if (crewMember.Person == this)
                {
                    cabin = crewMember.Cabin;
                    signOffCrewMember = crewMember;
                    break;
                }
            
            using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
            // using (SqliteCommand command = con.CreateCommand())
            {
                con.Open();

                using (SqliteTransaction sign_off_transaction = con.BeginTransaction(deferred: true))
                {
                    using (SqliteCommand sign_off_command = con.CreateCommand())
                    {
                        sign_off_command.CommandText = $"DELETE FROM VesselCrew WHERE Person_Id={this.Id}";
                        sign_off_command.ExecuteNonQuery();

                        Vessel.GetVessel.Crew.Remove(signOffCrewMember);

                        sign_off_command.CommandText = "INSERT INTO CrewEvents (CrewEventType_Id, Person_Id, Date, Time, Place) VALUES (" +
                            $"{signingOffEventType.Id}, {this.Id}, '{sign_off_date.Year,0:D4}-{sign_off_date.Month,0:D2}-{sign_off_date.Day,0:D2}', '{sign_off_time}', '{place}')";
                        sign_off_command.ExecuteNonQuery();

                        Lists.GetLists.CrewEvents.Add(new CrewEvent(signingOffEventType, this, 
                            new DateTime(sign_off_date.Year, sign_off_date.Month, sign_off_date.Day, sign_off_time.Hour, sign_off_time.Minute, sign_off_time.Second)));
                                            
                        cabin.PersonsOccupying--;

                        sign_off_transaction.Commit();
                    }
                }
            }
        }
        public bool OnBoard()
        {
            if (this.RankOnBoard() == null)
                return false;
            else
                return true;
        }
        public Rank? RankOnBoard()
        {
            Rank signed_on_rank = new Rank();
            bool signed_on = false;

            foreach (CrewMember crewMember in Vessel.GetVessel.Crew)
                if (crewMember.Person == this)
                {
                    signed_on_rank = crewMember.SignedOnAs;
                    signed_on = true;
                    break;
                }
            
            if (signed_on)
                return signed_on_rank;
            else
                return null;
        }
        public void ActivateIdDocument(IdDocument idDocument)
        {
            if (!idDocument.InUse)
            {
                DateTime today = DateTime.Today;

                // Check to see if the ID-document has expired.
                if (idDocument.ExpiryDate < new DateOnly(today.Year, today.Month, today.Day))
                    throw new IdDocumentExpiredException("Id document has expired and can therefore not be the active document");
                
                using (SqliteConnection con = new SqliteConnection("data source=" + Statics.GetConfigValue("FILES", "Db")))
                {
                    con.Open();

                    using (SqliteTransaction set_active_transaction = con.BeginTransaction(deferred: true))
                    {
                        using (SqliteCommand command = con.CreateCommand())
                        {
                            command.CommandText = $"UPDATE IdDocuments SET InUse=false WHERE Person_Id={this.Id}";
                            command.ExecuteNonQuery();

                            command.CommandText = $"UPDATE IdDocuments SET InUse=true WHERE Person_Id={this.Id} AND Id={idDocument.Id}";
                            command.ExecuteNonQuery();

                            set_active_transaction.Commit();
                        }
                    }

                    foreach (IdDocument id in this.IdDocuments)
                        if (id == idDocument)
                            id.InUse = true;
                        else
                            id.InUse = false;
                }
            }
        }
    }
}