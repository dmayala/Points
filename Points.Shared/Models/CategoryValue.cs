namespace Points.Shared.Models
{
    public class CategoryValue
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int CategoryId { get; set; }
        public double Points { get; set; }

        public virtual Card Card { get; set; }
        public virtual Category Category { get; set; }
    }
}