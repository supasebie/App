using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AppApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ValuesController : ControllerBase
  {
    private readonly DataContext _context;
    public ValuesController(DataContext context)
    {
      _context = context;
    }
    [HttpGet]
    public IActionResult GetValues()
    {
        var values = _context.Values.ToList();

        return Ok(values);
    }
    [HttpGet("{id}")]
    public IActionResult GetValue(int id)
    {
        var value = _context.Values.FirstOrDefault(x => x.Id == id );

        return Ok(value);
    }
  }
}