using System.ComponentModel.DataAnnotations;

namespace stylHUB.Models
{
    public class Category
    {
        // ===========================================
        // Primary Key
        // ===========================================
        [Key]
        public int Id { get; set; }

        // ===========================================
        // Category Name
        // ===========================================
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // ===========================================
        // Navigation Property
        // One Category has Many Products
        // ===========================================
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}