namespace Crewing
{
    class Rank
    {
        public int Id { get; set; }
        public string? Name { get; set;}
        public Department? Department { get; set; }
        public int Sorting { get; set;}

        public Rank()
        {
            Department = new Department();
            Name = "";
        }
        public static Rank? GetRank(int Rank_Id)
        {
            foreach (Rank rank in Lists.GetLists.Ranks)
                if (rank.Id == Rank_Id)
                    return rank;
            
            return null;
        }
        public static Rank? GetRank(string Rank_Name)
        {
            foreach (Rank rank in Lists.GetLists.Ranks)
                if (rank.Name == Rank_Name)
                    return rank;
            
            return null;
        }
    }
}