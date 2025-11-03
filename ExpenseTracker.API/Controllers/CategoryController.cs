using AutoMapper;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [Authorize(Roles ="User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if (categories == null)
                return NotFound();

            var categoryDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(categoryDTO);
        }

        [Authorize]
        [HttpGet("user/{UserId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesByUserId(Guid UserId)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var categories = await _categoryRepository.GetCategoriesByUserIdAsync(UserId);
            if (categories == null)
                return NotFound();

            var categoryDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return Ok(categoryDTO);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{CategoryId}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(Guid CategoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(CategoryId);
            if (category == null)
                return NotFound();
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDTO);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var exists = await _categoryRepository.CategoryNameExistsAsync(request.Name, request.CreatedByUserId);
            if (exists)
                return Conflict("Category name already exists for this user.");

            var category = _mapper.Map<Category>(request);
            category.CategoryId = Guid.NewGuid(); // Automapping in Create

            await _categoryRepository.AddCategoryAsync(category);
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return CreatedAtAction(nameof(GetCategoryById), new { CategoryId = category.CategoryId }, categoryDTO);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("{CategoryId}")]
        public async Task<IActionResult> UpdateCategory(Guid CategoryId, [FromBody] UpdateCategoryRequest request)
        {
            if (CategoryId != request.CategoryId)
            return BadRequest("Category ID mismatch.");

            var category = await _categoryRepository.GetCategoryByIdAsync(CategoryId);
            if (category == null)
                return NotFound();

            //Entity-based check
            //if (category.CreatedByUserId != request.UpdatedByUserId)
            //return Forbid("You can only update your own categories.");

            //Repository-based check
            var isOwner = await _categoryRepository.CategoryBelongsToUserAsync(CategoryId, request.CreatedByUserId);
            if (!isOwner)
                return Forbid("You do not own this category.");

            var nameExists = await _categoryRepository.CategoryNameExistsAsync(request.Name, request.UpdatedByUserId);
            if (nameExists && !string.Equals(category.Name, request.Name, StringComparison.OrdinalIgnoreCase))
                return Conflict("Category name already exists for this user.");

            _mapper.Map(request, category); // Map updated fields to existing entity
            await _categoryRepository.UpdateCategoryAsync(category);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{CategoryId}")]
        public async Task<IActionResult> DeleteCategory(Guid CategoryId, [FromBody] DeleteCategoryRequest request)
        {

            var isAdmin = User.IsInRole("Admin");
            var exists = await _categoryRepository.CategoryExistsAsync(CategoryId);
            if (!exists)
                return NotFound();

            //Entity-based check
            //if (category.CreatedByUserId != request.DeletedByUserId)
            //return Forbid("You can only delete your own categories.");
            
            //Repository-based check
            var isOwner = await _categoryRepository.CategoryBelongsToUserAsync(CategoryId, request.DeletedByUserId);
            if (!isOwner && !isAdmin)
                return Forbid("You do not own this category.");
            
            await _categoryRepository.DeleteCategoryAsync(CategoryId);
            return NoContent();
        }
    }
}