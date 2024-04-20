namespace Crewing
{
    public sealed class Deck
    {
        public string? Name { get; set; }
        public int X_max { get; set; }
        public int Y_max { get; set; }
        public List<Cabin>? Cabins { get; set; }
    }
}