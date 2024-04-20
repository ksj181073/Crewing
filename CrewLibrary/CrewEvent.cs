namespace Crewing
{
    class CrewEvent
    {
        public int? Id { get; set; }
        public CrewEventType? EventType { get; set; }
        public Person? Person { get; set;}
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Place { get; set; }
        public string? Remark { get; set; }
        public CrewEvent() { }
        public CrewEvent(CrewEventType eventType, Person person, DateTime dateTime, string place, string remark) : this(eventType, person, dateTime)
        {
            Place = place;
            Remark = remark;
        }
        public CrewEvent(CrewEventType eventType, Person person, DateTime dateTime)
        {
            EventType = eventType;
            Person = person;
            Date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            Time = new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}