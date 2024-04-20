namespace Crewing
{
    sealed class Vessel
    {
        private static Vessel? vessel = null;
        public List<CrewMember> Crew = new List<CrewMember>();
        public List<Deck> Decks { get; set; }
        
        private Vessel()
        {
            Decks = Statics.GetVesselAccommodation();
            Crew = new List<CrewMember>();
        }
        public static Vessel GetVessel {
            get {
                if (vessel == null)
                    vessel = new Vessel();
                
                return vessel;
            }
        }
        public List<Person> CrewToPersons()
        {
            List<Person> persons = new List<Person>();

            foreach (CrewMember crewMember in Crew)
                persons.Add(crewMember.Person);
            
            return persons;
        }
        public string GenerateCrewList()
        {
            List<Person> persons = this.CrewToPersons();

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
        public int NumberOfBunks()
        {
            int bunks = 0;

            foreach (Deck deck in Decks)
                foreach (Cabin cabin in deck.Cabins)
                    bunks += cabin.Persons;
            
            return bunks;
        }
        public int NumberOfCabins()
        {
            int numOfCabins = 0;

            foreach (Deck deck in Decks)
                foreach (Cabin cabin in deck.Cabins)
                    if (cabin.Persons > 0)
                        numOfCabins++;
            
            return numOfCabins;
        }
        public int NumberOfCabins(int persons)
        {
            int numOfCabins = 0;

            foreach (Deck deck in Decks)
                foreach (Cabin cabin in deck.Cabins)
                    if (cabin.Persons == persons)
                        numOfCabins++;
            
            return numOfCabins;
        }
    }
}