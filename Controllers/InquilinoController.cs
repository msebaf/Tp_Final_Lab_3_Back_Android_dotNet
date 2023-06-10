using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiTpLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiTpLab.Controllers;

[ApiController]
[Route("[controller]")]
public class InquilinoController : ControllerBase
{

private readonly DataContext _context;
private readonly IConfiguration _config;
    public InquilinoController(DataContext contexto, IConfiguration config)
    {
        _context = contexto;
        _config = config;
    }

    [HttpGet("{id}")]
     [Authorize]
public IActionResult getIqnuilino(int id)
{
    
    var inquilino = _context.Inquilino.FirstOrDefault(x => x.Id == id);

    if (inquilino == null)
    {
        return NotFound();
    }

   

        return Ok(inquilino);
}

    
    
   
}
