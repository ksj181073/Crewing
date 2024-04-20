namespace Crewing
{
    class CrewEventType
    {
        public int? Id { get; set; }
        public string? Name { get; set;}
        public static CrewEventType GetCrewEventType(int CrewEventType_Id)
        {
            foreach (CrewEventType crewEventType in Lists.GetLists.CrewEventTypes)
                if (crewEventType.Id == CrewEventType_Id)
                    return crewEventType;
            
            return null;
        }
        public static CrewEventType GetCrewEventType(string CrewEventType_Name)
        {
            foreach (CrewEventType crewEventType in Lists.GetLists.CrewEventTypes)
                if (crewEventType.Name == CrewEventType_Name)
                    return crewEventType;
            
            return null;
        }
    }
}