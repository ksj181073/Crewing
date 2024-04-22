#nullable disable

using Newtonsoft.Json.Linq;

namespace Crewing
{
    static class Program
    {
        static void InitiateData()
        {
            try
            {
                Vessel v = Vessel.GetVessel;
                Statics.GenerateInitialLists();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void Main()
        {
            InitiateData();

            // FullTestOfLogics();
            TestAccommodationInfo();

            // Console.WriteLine("Welcome back again!");

        }
        static void FullTestOfLogics()
        {
            // New Person
            Person newPerson = Person.CreatePerson("Test Testie", "Testessen");
            Person.EditPersonNationality(newPerson, Nationality.GetNationality("URUGUAY"));
            Person.EditPersonRank(newPerson, Rank.GetRank("Chief Officer"));
            Person.EditPersonBirthDate(newPerson, new DateOnly(2000, 12, 24));
            Person.EditPersonBirthPlace(newPerson, "Testyville");
            Person.EditPersonGender(newPerson, Gender.GetGender("Male"));

            // New Id-document
            // Associate with new Person
            IdDocument newDocument1 = IdDocument.CreateIdDocument(newPerson, "ABC12345", IdDocumentType.GetIdDocumentType("Passport"), newPerson.Nationality, 
                new DateOnly(2019, 6, 3), new DateOnly(2029, 6, 3));
            // Set active
            newPerson.ActivateIdDocument(newDocument1);
            // New Id-document
            // Associate with new Person
            IdDocument newDocument2 = IdDocument.CreateIdDocument(newPerson, IdDocumentType.GetIdDocumentType("Registration Doc."));
            IdDocument.EditIdDocumentNumber(newDocument2, "EWD129877");
            IdDocument.EditIssuingCountry(newDocument2, newPerson.Nationality);
            IdDocument.EditIssueDate(newDocument2, new DateOnly(2013, 9, 15));
            IdDocument.EditExpiryDate(newDocument2, new DateOnly(newDocument2.IssueDate.Year + 10, newDocument2.IssueDate.Month, newDocument2.IssueDate.Day));
            // New Id-document
            // Associate with new Person
            IdDocument newDocument3 = IdDocument.CreateIdDocument(newPerson, IdDocumentType.GetIdDocumentType("Passport"));
            // Set no. 2 active
            newPerson.ActivateIdDocument(newDocument2);
            // Set no. 3 active
            try
            {
                newPerson.ActivateIdDocument(newDocument3); // Result in exception, due to no exp date
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Remove no. 3
            IdDocument.RemoveIdDocument(newDocument3);

            // New Certificate
            // Associate with new Person
            Certificate newCertificate1 = Certificate.CreateCertificate(newPerson, CertificateType.GetCertificateType("Galley Hygiene"));
            Certificate.EditCertificateNumber(newCertificate1, "STCW-GH-JAN-298766");
            Certificate.EditIssuingAuthority(newCertificate1, "Maritime Authority");
            Certificate.EditIssueDate(newCertificate1, new DateOnly(2013, 5, 6));
            Certificate.EditExpiryDate(newCertificate1, new DateOnly(2023, 5, 6));

            Console.WriteLine(Cabin.GetCabin("402").Number);

            // Sign on new Person
            newPerson.SignOn(newPerson.Rank, "Køge", Cabin.GetCabin("402"));
            // New Certificate
            // Associate with new Person
            Certificate newCertificate2 = Certificate.CreateCertificate(newPerson, "SDS-3344,2660/90.3", CertificateType.GetCertificateType("Working Environment (Danish §16)"), 
                "New Authority", new DateOnly(1989, 6, 9), new DateOnly());

            // Try to remove new Person... result=exception
            try
            {
                Person.RemovePerson(newPerson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Sign off new Person
            newPerson.SignOff("Aalborg");
            // Remove new Person
            Person.RemovePerson(newPerson);
        }
        static void TestSignOn()
        {
            Lists.GetLists.Persons[45].SignOn(Vessel.GetVessel.Decks[0].Cabins[0]);
        }
        static void TestSignOff()
        {
            Lists.GetLists.Persons[45].SignOff();
        }
        static void TestProgramRun()
        {
            // Console.WriteLine(Vessel.GetVessel.GenerateCrewList());

            // Certificate.CreateCertificate(Vessel.GetVessel.Crew[1].Person, Lists.GetLists.CertificateTypes[3]);
            // Certificate.EditCertificateNumber(Vessel.GetVessel.Crew[1].Person.Certificates[0], "EBB34-78.2311");
            // Certificate.EditCertificateType(Vessel.GetVessel.Crew[1].Person.Certificates[0], 7);
            // Certificate.EditIssuingAuthority(Vessel.GetVessel.Crew[1].Person.Certificates[0], "Søfartsstyrrelsen");
            // Certificate.EditIssueDate(Vessel.GetVessel.Crew[1].Person.Certificates[0], new DateOnly(1980,10,13));
            // Certificate.EditExpiryDate(Vessel.GetVessel.Crew[1].Person.Certificates[0], new DateOnly(2024,4,18));
            // Certificate.RemoveCertificate(Vessel.GetVessel.Crew[1].Person.Certificates[0]);

            // IdDocument.CreateIdDocument(Lists.GetLists.Persons[90], Lists.GetLists.IdDocumentTypes[6]);

            // foreach (IdDocument doc in Vessel.GetVessel.Crew[2].Person.IdDocuments)
                // Console.WriteLine(Vessel.GetVessel.Crew[2].Person.IdDocuments[1].Id);

            // IdDocument.EditIdDocumentNumber(Vessel.GetVessel.Crew[2].Person.IdDocuments[1], "EW-56.778");
            // IdDocument.EditIssuingCountry(Vessel.GetVessel.Crew[2].Person.IdDocuments[1], 60);
            // IdDocument.EditIssueDate(Vessel.GetVessel.Crew[2].Person.IdDocuments[1],new DateOnly(1962,6,3));
            // IdDocument.EditExpiryDate(Lists.GetLists.Persons[90].IdDocuments[2], new DateOnly(2026,8,14));
            // IdDocument.EditIdDocumentType(Vessel.GetVessel.Crew[2].Person.IdDocuments[1], 2);

            // Lists.GetLists.Persons[90].IdDocuments.a
            
            // foreach (IdDocument idd in Lists.GetLists.Persons[90].IdDocuments)
            //     Console.WriteLine(idd.Id + " - " + idd.InUse);

            // foreach (IdDocument idd in Lists.GetLists.Persons[90].IdDocuments)
            //                 Console.WriteLine($"{idd.Id} - {idd.InUse}");

            // IdDocument.RemoveIdDocument(Lists.GetLists.Persons[90].IdDocuments[4]);
            // Lists.GetLists.Persons[90].ActivateIdDocument(Lists.GetLists.Persons[90].IdDocuments[0]);

            // foreach (IdDocument idd in Lists.GetLists.Persons[90].IdDocuments)
            //     Console.WriteLine($"\n\n{idd.Id} - {idd.InUse}");




        }
        static string TestGenerateCrewList(List<Person> persons)
        {
            // List<Person> persons = Lists.GetLists.Persons;

            string crewlist_str = "";

            int max_len_name = "NAME".Length;
            int max_len_rank = "RANK".Length;
            int max_len_nationality = "NATIONALITY".Length;
            int max_len_birthplace = "PLACE OF BIRTH".Length;
            int max_len_idnumber = "DOCUMENT NO.".Length;

            foreach (Person person in persons)
            {
                if (person.Name().Length > max_len_name)
                    max_len_name = person.Name().Length;
                
                if (person.Rank.Name.Length > max_len_rank)
                    max_len_rank = person.Rank.Name.Length;
                
                if (person.Nationality.Name.Length > max_len_nationality)
                    max_len_nationality = person.Nationality.Name.Length;
                
                if (person.PlaceOfBirth.Length > max_len_birthplace)
                    max_len_birthplace = person.PlaceOfBirth.Length;
                
                foreach (IdDocument doc in person.IdDocuments)
                {
                    if (doc.InUse)
                        if (doc.Number.Length > max_len_idnumber)
                            max_len_idnumber = doc.Number.Length;
                }
            }

            int max_len_doctype = "DOCUMENT TYPE".Length;

            foreach (IdDocumentType idt in Lists.GetLists.IdDocumentTypes)
            {
                if (idt.Name.Length > max_len_doctype)
                    max_len_doctype = idt.Name.Length;
            }

            crewlist_str += String.Format("\n{0,-" + max_len_name + "} {1,-" + max_len_rank + "} {2,-" + max_len_nationality + "} {3,14}  {4,-" + 
                    max_len_birthplace + "} {5,-" + max_len_doctype + "} {6,-" + max_len_idnumber + "} {7,10}", 
                    "NAME", "RANK", "NATIONALITY", "DATE OF BIRTH", "PLACE OF BIRTH", "DOCUMENT TYPE", "DOCUMENT NO.", 
                    "EXPIRY DATE");

            foreach (Person person in persons)
            {
                Rank pers_rank = new Rank();
                IdDocument id = new IdDocument();
                
                foreach (IdDocument doc in person.IdDocuments)
                {
                    if (doc.InUse)
                        id = doc;
                        break;
                }

                crewlist_str += String.Format("\n{0,-" + max_len_name + "} {1,-" + max_len_rank + "} {2,-" + max_len_nationality + "} {3,14}  {4,-" + 
                    max_len_birthplace + "} {5,-" + max_len_doctype + "} {6,-" + max_len_idnumber + "} {7,10}", 
                    person.Name(), person.Rank.Name, person.Nationality.Name, person.DateOfBirth, person.PlaceOfBirth, id.DocumentType.Name, id.Number, 
                    (id.ExpiryDate != new DateOnly(1,1,1) ? id.ExpiryDate : ""));
            }

            return crewlist_str;
        }
        static void TestStaticFileInfo()
        {
            Console.WriteLine(Statics.GetConfigValue("FILES", "Icon"));

            dynamic vessel = Statics.GetVesselInfo();
            JArray prenames = vessel.PreviousNames;
            string val = vessel.Dimensions.LOA;

            foreach (string? name in prenames)
                Console.WriteLine(name);
            
            List<Deck> decks = Statics.GetVesselAccommodation();

            foreach (Deck d in decks)
                foreach (Cabin c in d.Cabins)
                    Console.WriteLine(c.Name);
        }
        static void TestAccommodationInfo()
        {
            Vessel vessel = Vessel.GetVessel;
            
            Console.WriteLine();
            vessel.Decks.Reverse();

            foreach (Deck deck in vessel.Decks)
            {
                Console.WriteLine($"{deck.Name.ToUpper()}");

                foreach (Cabin cabin in deck.Cabins)
                    Console.WriteLine($"\t{cabin.Number,-7} - {cabin.Name,-50}{cabin.Persons}");
            }
        }
        static void TestLinq()
        {
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            var numQuery = from num in numbers where (num % 2) == 0 select num;

            foreach (int num in numQuery)
                Console.WriteLine("{0,1} ", num);
        }
        private static void InitiateDatabase()
        {
            //DbInit.FillNationalities();
            //DbInit.InitiateDb();
            //DbInit.FillPersons();
            //DbInit.FillIdCards();

            //CrewMember crew = CreateTestCrewMember();
            //Console.WriteLine("Hello there, " + crew.TitledName() + "!!!\nYou are from " + crew.Nationality.Name + "\nand you are born on " + crew.DateOfBirth.ToString() + " in " + crew.PlaceOfBirth);
        }
    }
}