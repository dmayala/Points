namespace Points.Shared.Models
{
    public class Valuation
    {
        public int Id { get; set; }
        public double Points { get; set; }
        public string Reason { get; set; }

        public Card Card { get; set; }
        public Category Category { get; set; }
    }
}