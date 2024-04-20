namespace Crewing
{
    public class Gender
    {
        public int Id { get; set; }
        public string? Name { get; set;}

        public static Gender? GetGender(int Gender_Id)
        {
            foreach (Gender gender in Lists.GetLists.Genders)
                if (gender.Id == Gender_Id)
                    return gender;
            
            return null;
        }
        public static Gender? GetGender(string Gender_Name)
        {
            foreach (Gender gender in Lists.GetLists.Genders)
                if (gender.Name == Gender_Name)
                    return gender;
            
            return null;
        }
    }
}