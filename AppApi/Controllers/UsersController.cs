using System.Collections.Generic;
using System.Threading.Tasks;
using AppApi.Data;
using AppApi.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IFriendsRepository _repo;
    private readonly IMapper _mapper;
    public UsersController(IFriendsRepository repo, IMapper mapper)
    {
      _mapper = mapper;
      _repo = repo;
    }
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
      var users = await _repo.GetUsers();
    
      var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);


      return Ok(usersToReturn );
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _repo.GetUser(id);

      var userToReturn = _mapper.Map<UserForDetailedDto>(user);

      return Ok(userToReturn);
    }
  }
}