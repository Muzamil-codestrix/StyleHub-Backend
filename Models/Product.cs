using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stylHUB.Models
{
    public class Product
    {
        // ===========================================
        // Primary Key
        // ===========================================
        [Key]
        public int Id { get; set; }

        // ===========================================
        // Product Name
        // ===========================================
        [Required]
        public string Title { get; set; } = string.Empty;

        // ===========================================
        // Product Description
        // ===========================================
        [Required]
        public string Description { get; set; } = string.Empty;

        // ===========================================
        // Product Brand
        // ===========================================
        [Required]
        public string Brand { get; set; } = string.Empty;

        // ===========================================
        // Foreign Key
        // Links Product to Category
        // ===========================================
        [Required]
        public int CategoryId { get; set; }

        // ===========================================
        // Navigation Property
        // One Product belongs to One Category
        // ===========================================
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // ===========================================
        // Product Price
        // ===========================================
        [Required]
        public decimal Price { get; set; }

        // ===========================================
        // Available Quantity
        // ===========================================
        [Required]
        public int Stock { get; set; }

        // ===========================================
        // Product Image
        // ===========================================
        public string ImageUrl { get; set; } = string.Empty;
    }
}