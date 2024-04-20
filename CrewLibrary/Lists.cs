namespace Crewing
{
    public sealed class Lists
    {
        private static Lists? lists = null;
        private Lists() {
            Persons = new List<Person>();
            IdDocuments = new List<IdDocument>();

            Departments = new List<Department>();
            Genders = new List<Gender>();
            IdDocumentTypes = new List<IdDocumentType>();
            Nationalities = new List<Nationality>();
            Ranks = new List<Rank>();

            Certificates = new List<Certificate>();
            CertificateTypes = new List<CertificateType>();

            CrewEventTypes = new List<CrewEventType>();
            CrewEvents = new List<CrewEvent>();
            VesselEventTypes = new List<VesselEventType>();
            VesselEvents = new List<VesselEvent>();
        }
        public static Lists GetLists {
            get {
                if (lists == null)
                    lists = new Lists();
                
                return lists;
            }
        }
        public List<Person> Persons;
        public List<IdDocument> IdDocuments;
        public List<Department> Departments;
        public List<Gender> Genders;
        public List<IdDocumentType> IdDocumentTypes;
        public List<Nationality> Nationalities;
        public List<Rank> Ranks;
        public List<CertificateType> CertificateTypes;
        public List<Certificate> Certificates;

        public List<CrewEventType> CrewEventTypes;
        public List<CrewEvent> CrewEvents;
        public List<VesselEventType> VesselEventTypes;
        public List<VesselEvent> VesselEvents;
    }
}