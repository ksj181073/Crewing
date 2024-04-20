namespace Crewing
{
    public sealed class Cabin
    {
        private int _personsOccupying;
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? PhoneNo { get; set; }
        public int Persons { get; set; }
        public bool Crew { get; set; }
        public bool Passenger { get; set; }
        public int X_start { get; set; }
        public int Y_start { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PersonsOccupying  { 
            get {
                return _personsOccupying;
            } 
            set {
                if (value > Persons)
                    _personsOccupying = Persons;
                else
                    _personsOccupying = value;
            } 
        }
        public bool Vacancy()
        {
            if (PersonsOccupying < Persons)
                return true;
            else
                return false;
        }
        public static Cabin? GetCabin(string Cabin_No)
        {
            foreach (Deck deck in Vessel.GetVessel.Decks)
                if (deck.Cabins != null)
                    foreach (Cabin cabin in deck.Cabins)
                        if (cabin.Number == Cabin_No)
                            return cabin;
            
            return null;
        }
    }
}