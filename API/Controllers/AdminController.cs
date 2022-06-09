using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            this._userManager = userManager;
            this._unitOfWork = unitOfWork;
            this._photoService = photoService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles.");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove previous roles.");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-for-approval")]
        public async Task<ActionResult<IEnumerable<PhotoForApprovalDTO>>> GetPhotosForApproval() // TODO: could benefit from pagination
        {
            var unapprovedPhotos = await _unitOfWork.PhotoRepository.GetUnapprovedPhotos();

            if (unapprovedPhotos == null) return NotFound("Internal error found while getting unapproved photos");

            return Ok(unapprovedPhotos);

        }

        [HttpPost("approve-photo")]
        public async Task<ActionResult> ApprovePhoto([FromQuery] int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);
            
            if (photo == null) return NotFound($"Photo not found");

            if (photo.IsApproved) return Accepted($"Photo is already approved");

            photo.IsApproved = true;
            var user = await _unitOfWork.UserRepository.GetUserByPhotoIdAsync(photoId);

            if (!user.Photos.Any(p => p.IsMain == true)) // if user has no photos set to main...
                photo.IsMain = true;

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to approve photo");
        }

        [HttpPost("reject-photo")]
        public async Task<ActionResult> RejectPhoto([FromQuery] int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null) return NotFound($"Photo not found");

            if (photo.PublicId != null)
            {
                var deletionResult = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (deletionResult.Error != null) return BadRequest(deletionResult.Error.Message);
            }
            _unitOfWork.PhotoRepository.RemovePhoto(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to remove photo");
        }
    }
}
