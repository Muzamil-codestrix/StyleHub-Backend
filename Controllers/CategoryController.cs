using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stylHUB.Data_layer;
using stylHUB.Models;

namespace stylHUB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // ===========================================
        // Database Context
        // ===========================================
        private readonly App_DB_Context _context;

        // ===========================================
        // Constructor
        // Dependency Injection
        // ===========================================
        public CategoryController(App_DB_Context context)
        {
            _context = context;
        }

        // =====================================================
        // POST : Add New Category
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest(new
                    {
                        Message = "Category data is required."
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == category.Name);

                if (existingCategory != null)
                {
                    return BadRequest(new
                    {
                        Message = "Category already exists."
                    });
                }

                await _context.Categories.AddAsync(category);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Category added successfully.",
                    Category = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the category.",
                    Error = ex.Message
                });
            }
        }

        // =====================================================
        // GET : Get All Categories
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                if (categories == null || !categories.Any())
                {
                    return NotFound(new
                    {
                        Message = "No categories found."
                    });
                }

                return Ok(new
                {
                    Message = "Categories retrieved successfully.",
                    TotalCategories = categories.Count,
                    Data = categories
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving categories.",
                    Error = ex.Message
                });
            }
        }

        // =====================================================
        // GET : Get Category By Id
        // =====================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound(new
                    {
                        Message = "Category not found."
                    });
                }

                return Ok(new
                {
                    Message = "Category retrieved successfully.",
                    Data = category
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving the category.",
                    Error = ex.Message
                });
            }
        }

        // =====================================================
        // PUT : Update Category
        // =====================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest(new
                    {
                        Message = "Category data is required."
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCategory = await _context.Categories.FindAsync(id);

                if (existingCategory == null)
                {
                    return NotFound(new
                    {
                        Message = "Category not found."
                    });
                }

                var duplicateCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == category.Name && c.Id != id);

                if (duplicateCategory != null)
                {
                    return BadRequest(new
                    {
                        Message = "Category already exists."
                    });
                }

                existingCategory.Name = category.Name;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Category updated successfully.",
                    Data = existingCategory
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while updating the category.",
                    Error = ex.Message
                });
            }
        }

        // =====================================================
        // DELETE : Delete Category
        // =====================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound(new
                    {
                        Message = "Category not found."
                    });
                }

                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Category deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while deleting the category.",
                    Error = ex.Message
                });
            }
        }
    }
}