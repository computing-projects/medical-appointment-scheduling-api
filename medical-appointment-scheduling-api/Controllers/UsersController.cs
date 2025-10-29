using Microsoft.AspNetCore.Mvc;
using medical_appointment_scheduling_api.Models;
using medical_appointment_scheduling_api.Repositories;
using medical_appointment_scheduling_api.Models.DTO;
using medical_appointment_scheduling_api.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace medical_appointment_scheduling_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repo;
        private readonly ILogger<UsersController> _logger;
        private readonly ProfilePhotoService _profilePhotoService;

        public UsersController(IUsersRepository repo, ILogger<UsersController> logger, ProfilePhotoService profilePhotoService)
        {
            _logger = logger;
            _repo = repo;
            _profilePhotoService = profilePhotoService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            var result = await _repo.RegisterAsync(user);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _repo.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] RedefinirSenhaDto obj)
        {
            var result = await _repo.ResetPasswordAsync(obj);
            return Ok(result);
        }

        [HttpPost("UploadProfilePhoto")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile photo)
        {
            try
            {
                // Get current user email from token
                var email = User.FindFirstValue("email") ?? User.Identity?.Name;
                if (string.IsNullOrEmpty(email))
                    return Unauthorized("Invalid token claims.");

                var user = await _repo.GetByEmailAsync(email);
                if (user == null)
                    return NotFound("User not found.");

                // Validate file
                if (photo == null || photo.Length == 0)
                    return BadRequest("No file uploaded.");

                if (photo.Length > 5 * 1024 * 1024) // 5MB limit
                    return BadRequest("File size must be less than 5MB.");

                if (!photo.ContentType.StartsWith("image/"))
                    return BadRequest("File must be an image.");

                // Delete old photo if exists
                if (!string.IsNullOrEmpty(user.ProfilePhotoUrl))
                {
                    await _profilePhotoService.DeleteProfilePhotoAsync(user.ProfilePhotoUrl);
                }

                // Upload new photo
                using var stream = new MemoryStream();
                await photo.CopyToAsync(stream);
                var imageData = stream.ToArray();

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var url = await _profilePhotoService.UploadProfilePhotoAsync(user.Id, imageData, fileName);

                // Update user record
                user.ProfilePhotoUrl = url;
                await _repo.UpdateAsync(user);

                return Ok(new { profilePhotoUrl = url, message = "Profile photo uploaded successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading profile photo");
                return StatusCode(500, new { 
                    error = "Server error", 
                    message = "Failed to upload profile photo"
                });
            }
        }

        [HttpDelete("DeleteProfilePhoto")]
        public async Task<IActionResult> DeleteProfilePhoto()
        {
            try
            {
                // Get current user email from token
                var email = User.FindFirstValue("email") ?? User.Identity?.Name;
                if (string.IsNullOrEmpty(email))
                    return Unauthorized("Invalid token claims.");

                var user = await _repo.GetByEmailAsync(email);
                if (user == null)
                    return NotFound("User not found.");

                if (string.IsNullOrEmpty(user.ProfilePhotoUrl))
                    return BadRequest("No profile photo to delete.");

                // Only delete from storage if it's not a default avatar
                if (!user.ProfilePhotoUrl.Contains("ui-avatars.com"))
                {
                    await _profilePhotoService.DeleteProfilePhotoAsync(user.ProfilePhotoUrl);
                }

                // Reset to default avatar
                user.ProfilePhotoUrl = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(user.Name)}&size=200&background=random&color=fff&bold=true";
                await _repo.UpdateAsync(user);

                return Ok(new { 
                    message = "Profile photo reset to default", 
                    profilePhotoUrl = user.ProfilePhotoUrl 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profile photo");
                return StatusCode(500, new { error = "Server error", message = "Failed to delete profile photo" });
            }
        }
    }
}
