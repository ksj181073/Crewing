namespace Crewing
{
    class Nationality
    {
        public int Id { get; set; }
        public string? Name { get; set;}
        public string? ISO2_Code { get; set;}
        public string? ISO3_Code { get; set;}
        public Nationality() { }
        public Nationality(string name, string iso2, string iso3)
        {
            Id = 0;
            Name = name;
            ISO2_Code = iso2;
            ISO3_Code = iso3;
        }
        public static Nationality? GetNationality(int Nationality_Id)
        {
            foreach (Nationality nationality in Lists.GetLists.Nationalities)
                if (nationality.Id == Nationality_Id)
                    return nationality;
            
            return null;
        }
        public static Nationality? GetNationality(string Nationality_Name)
        {
            foreach (Nationality nationality in Lists.GetLists.Nationalities)
                if (nationality.Name == Nationality_Name)
                    return nationality;
            
            return null;
        }
    }
}