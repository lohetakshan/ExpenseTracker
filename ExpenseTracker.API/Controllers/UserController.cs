using AutoMapper;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Services;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JWTTokenService _JWTTokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, IMapper mapper, JWTTokenService jWTTokenService, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _JWTTokenService = jWTTokenService;
            _logger = logger;
        }

        //Controller Actions integrated with AutoMapper
        [Authorize]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null) return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [Authorize]
        [HttpGet("{UserId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid UserId)
        {
            var user = await _userRepository.GetUserByIdAsync(UserId);
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-username/{UserName}")]
        public async Task<ActionResult<UserDTO>> GetUserByUserName(string UserName)
        {
            var user = await _userRepository.GetUserByUsernameAsync(UserName);
            if (user == null)
                return NotFound();
            return Ok(_mapper.Map<UserDTO>(user));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-email/{Email}")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string Email)
        {
            var user = await _userRepository.GetUserByEmailAsync(Email);
            if (user == null)
                return NotFound();
            return Ok(_mapper.Map<UserDTO>(user));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserRequest request)
        {
            var UserNameExist = await _userRepository.GetUserByUsernameAsync(request.UserName);
            var UserEmailExist = await _userRepository.GetUserByEmailAsync(request.Email);

            if (UserNameExist != null)
                return Conflict("Username already exists.");
            if (UserEmailExist != null)
                return Conflict("Email already exists.");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = request.UserName,
                Name = request.Name,
                Email = request.Email,
                Passwordhash = request.PasswordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = "User"
            };

            await _userRepository.AddUserAsync(user);
            var userDTO = _mapper.Map<UserDTO>(user);
            return CreatedAtAction(nameof(GetUserById), new { UserId = user.UserId }, userDTO);
        }

        [Authorize]
        [HttpPut("{UserId}")]
        public async Task<IActionResult> UpdateUser(Guid UserId, [FromBody] CreateUserRequest request)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();


            var user = await _userRepository.GetUserByIdAsync(UserId);
            if (user == null)
                return NotFound();

            user.UserName = request.UserName;
            user.Name = request.Name;
            user.Email = request.Email;
            user.Passwordhash = request.PasswordHash;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUser(Guid UserId)
        {

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (UserId.ToString() != loggedInUserId && !isAdmin)
                return Forbid();

            var userExists = await _userRepository.UserExistsAsync(UserId);
            if (!userExists)
                return NotFound();

            await _userRepository.DeleteUserAsync(UserId);
            return NoContent();
        }

        //Admin-only action to check if a user exists
        //Task<bool> UserExistsAsync(Guid UserId)

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Login request failed model validation for user: {request.UserName}");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Login attempt for user: {UserName}", request.UserName);

            var user = _userRepository.ValidateUser(request.UserName, request.Password);
            if (user == null)
            {
                //return NotFound(); or
                _logger.LogWarning("Invalid login credentials for user: {UserName}", request.UserName);
                return Unauthorized("Login failed");
            }

            var token = _JWTTokenService.GenerateToken(user);

            _logger.LogInformation("JWT token issued for user: {UserId}-{UserName} with role: {Role}", user.UserId, user.UserName, user.Role);

            return Ok(new JWTResponse
            {
                Token = token,
                UserId = user.UserId.ToString(),
                Role = user.Role,
                UserName = user.UserName
            });
        }
    }
}