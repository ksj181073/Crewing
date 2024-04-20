namespace Crewing
{
    class VesselEvent
    {
        public int? Id { get; set; }
        public VesselEventType EventType { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Place { get; set; }
        public string? Remark { get; set; }
    }
}