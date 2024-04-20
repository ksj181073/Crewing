namespace Crewing
{
    public class CrewMember
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public Rank SignedOnAs { get; set; }
        public DateOnly SignOnDate { get; set; }
        public Cabin Cabin { get; set; }
        public CrewMember() { }
        public CrewMember(Person person, Rank rank, DateOnly date)
        {
            Person = person;
            SignedOnAs = rank;
            SignOnDate = date;
        }
    }
}