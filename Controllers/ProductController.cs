using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using stylHUB.Data_layer;
using stylHUB.Models;

namespace stylHUB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly App_DB_Context _context;

        public ProductController(App_DB_Context context)
        {
            _context = context;
        }

        // =====================================================
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            try
            {
                // ===========================================
                // Check if request body is empty
                // ===========================================
                if (product == null)
                {
                    return BadRequest("Product data is required.");
                }

                // ===========================================
                // Save product to database
                // ===========================================
                await _context.Products.AddAsync(product);

                // ===========================================
                // Commit changes
                // ===========================================
                await _context.SaveChangesAsync();

                // ===========================================
                // Return Success
                // ===========================================
                return Ok(new
                {
                    Message = "Product added successfully.",
                    Product = product
                });
            }
            catch (Exception ex)
            {
                // ===========================================
                // Return Error
                // ===========================================
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the product.",
                    Error = ex.Message
                });
            }

        }
        // =====================================================
        // GET : Get All Products
        // URL:
      
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                // ===========================================
                // Get all products from the database
                // ===========================================
                var products = await _context.Products.ToListAsync();

                // ===========================================
                // Check if no products exist
                // ===========================================
                if (products == null || !products.Any())
                {
                    return NotFound(new
                    {
                        Message = "No products found."
                    });
                }

                // ===========================================
                // Return all products
                // ===========================================
                return Ok(new
                {
                    Message = "Products retrieved successfully.",
                    TotalProducts = products.Count,
                    Data = products
                });
            }
            catch (Exception ex)
            {
                // ===========================================
                // Return server error
                // ===========================================
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving products.",
                    Error = ex.Message
                });
            }
        }
        // =====================================================
        // GET : Get Product By Id
      
        // =====================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                // ===========================================
                // Find product by Id
                // ===========================================
                var product = await _context.Products.FindAsync(id);

                // ===========================================
                // Check if product exists
                // ===========================================
                if (product == null)
                {
                    return NotFound(new
                    {
                        Message = $"Product with Id {id} not found."
                    });
                }

                // ===========================================
                // Return product
                // ===========================================
                return Ok(new
                {
                    Message = "Product retrieved successfully.",
                    Data = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving the product.",
                    Error = ex.Message
                });
            }
        }


        // =====================================================
        // PUT : Update Product
      
        // =====================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            try
            {
                // ===========================================
                // Find existing product
                // ===========================================
                var existingProduct = await _context.Products.FindAsync(id);

                // ===========================================
                // Check if product exists
                // ===========================================
                if (existingProduct == null)
                {
                    return NotFound(new
                    {
                        Message = $"Product with Id {id} not found."
                    });
                }

                // ===========================================
                // Update product properties
                // ===========================================
                existingProduct.Title = product.Title;
                existingProduct.Description = product.Description;
                existingProduct.Brand = product.Brand;
                existingProduct.Category = product.Category;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.ImageUrl = product.ImageUrl;

                // ===========================================
                // Save changes
                // ===========================================
                await _context.SaveChangesAsync();

                // ===========================================
                // Return success
                // ===========================================
                return Ok(new
                {
                    Message = "Product updated successfully.",
                    Data = existingProduct
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while updating the product.",
                    Error = ex.Message
                });
            }
        }
        // =====================================================
        // DELETE : Delete Product
       
        // =====================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // ===========================================
                // Find product
                // ===========================================
                var product = await _context.Products.FindAsync(id);

                // ===========================================
                // Check if product exists
                // ===========================================
                if (product == null)
                {
                    return NotFound(new
                    {
                        Message = $"Product with Id {id} not found."
                    });
                }

                // ===========================================
                // Remove product
                // ===========================================
                _context.Products.Remove(product);

                // ===========================================
                // Save changes
                // ===========================================
                await _context.SaveChangesAsync();

                // ===========================================
                // Return success
                // ===========================================
                return Ok(new
                {
                    Message = "Product deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while deleting the product.",
                    Error = ex.Message
                });
            }
        }
    }
}