using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using AppApi.Data;
using AppApi.Dtos;
using AppApi.Helpers;
using AppApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppApi.Controllers
{
  [Authorize]
  [Route("api/users/{userId}/photos")]
  [ApiController]
  public class PhotosController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;
    private readonly IFriendsRepository _repo;
    public PhotosController(IFriendsRepository friendsRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig, IFriendsRepository repo)
    {
      _repo = repo;
      _cloudinaryConfig = cloudinaryConfig;
      _mapper = mapper;

      Account acc = new Account(
          _cloudinaryConfig.Value.CloudName,
          _cloudinaryConfig.Value.ApiKey,
          _cloudinaryConfig.Value.ApiSecret
      );

      _cloudinary = new Cloudinary(acc);
    }

    [HttpGet("{id}", Name = "GetPhoto")]
    public async Task<IActionResult> GetPhoto(int id)
    {
      var photoFromRepo = await _repo.GetPhoto(id);

      var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

      return Ok(photo);
    }

    [HttpPost]
    public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
      {
        return Unauthorized("User not authorized");
      }

      var userFromRepo = await _repo.GetUser(userId);

      var file = photoForCreationDto.File;

      var uploadResults = new ImageUploadResult();

      if (file.Length > 0)
      {
        using (var stream = file.OpenReadStream())
        {
          var uploadParams = new ImageUploadParams()
          {
            File = new FileDescription(file.Name, stream),
            Transformation = new Transformation()
                .Width(500).Height(500).Crop("fill").Gravity("face")
          };

          uploadResults = _cloudinary.Upload(uploadParams);
        }
      }

      photoForCreationDto.Url = uploadResults.Url.ToString();
      photoForCreationDto.PublicId = uploadResults.PublicId;

      var photo = _mapper.Map<Photo>(photoForCreationDto);

      if (!userFromRepo.Photos.Any(u => u.IsMain))
        photo.IsMain = true;

      userFromRepo.Photos.Add(photo);

      if (await _repo.SaveAll())
      {
        var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

        return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
      }

      return BadRequest("Could not add photo");
    }

    [HttpPost("{id}/setMain")]
    public async Task<IActionResult> SetMainPhoto(int userId, int id)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
      {
        return Unauthorized("User not authorized");
      }

      var user = await _repo.GetUser(userId);

      if (!user.Photos.Any(p => p.Id == id))
      {
        return Unauthorized();
      }

      var photoFromRepo = await _repo.GetPhoto(id);

      if (photoFromRepo.IsMain)
        return BadRequest("Photo is already set to main");

      var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
      currentMainPhoto.IsMain = false;

      photoFromRepo.IsMain = true;

      if (await _repo.SaveAll())
        return NoContent();

      return BadRequest("Could not set photo to main");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(int userId, int id)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
      {
        return Unauthorized("User not authorized");
      }

      var user = await _repo.GetUser(userId);

      if (!user.Photos.Any(p => p.Id == id))
      {
        return Unauthorized();
      }

      var photoFromRepo = await _repo.GetPhoto(id);

      if (photoFromRepo.IsMain)
        return BadRequest("Unable to delete main photo");

      if (photoFromRepo.PublicId != null)
      {
        var destroyParams = new DeletionParams(photoFromRepo.PublicId);

        var destroyResult = _cloudinary.Destroy(destroyParams);

        if (destroyResult.Result == "ok")
          _repo.Delete(photoFromRepo);
      }

      if (photoFromRepo.PublicId == null)
      {
        _repo.Delete(photoFromRepo);
      }
      
      if (await _repo.SaveAll())
        return Ok();

      return BadRequest("Unable to delete photo");
    }
  }
}