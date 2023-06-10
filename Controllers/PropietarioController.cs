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
public class PropietarioController : ControllerBase
{

private readonly DataContext _context;
private readonly IConfiguration _config;
    public PropietarioController(DataContext contexto, IConfiguration config)
    {
        _context = contexto;
        _config = config;
    }

   

    [HttpPost("{login}")]
    public IActionResult login(LoginView login)
    {
      var propietario = _context.Propietario.FirstOrDefault(x => x.Email == login.Email);
      if(propietario == null)
      {
        return NotFound();
      }
      
      String hashed  = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: login.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
          if(hashed != propietario.Contraseña)
          {
            return BadRequest("Contraseña incorrecta");
          }
          
          var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(_config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, propietario.Email),
						new Claim("id", propietario.Id.ToString()),
						//new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: _config["TokenAuthentication:Issuer"],
						audience: _config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
          Console.WriteLine("Token: " +token);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));

      
      
    }
    
     [HttpGet()]
     [Authorize]
    public IActionResult MiPerfil()
    {
       var propietario = _context.Propietario.FirstOrDefault(x => x.Email == User.Identity.Name);
  if (propietario == null)
  {
    return NotFound();
  }
  
  PropietarioView propietarioView = new PropietarioView(propietario);
  Console.WriteLine(propietarioView);
  
  return Ok(propietarioView);
    }



     [HttpPut("{actualizar}")]
     [Authorize]
    public IActionResult ActualizarPerfil(Propietario nuevo)
    {
      //recupeeo propietario
      var propietario = _context.Propietario.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
      
      if(propietario == null)
      {
        return NotFound();
      }
      //actualizo datos
     
      propietario.Telefono = nuevo.Telefono;
     propietario.Dni = nuevo.Dni;
     
      propietario.Email = nuevo.Email;
      propietario.Nombre = nuevo.Nombre;
      propietario.Apellido = nuevo.Apellido;
      if(nuevo.Contraseña != "") {
        String hashed  = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: nuevo.Contraseña,
					salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
        propietario.Contraseña = hashed;
      }
     
      //guardo cambios
      _context.SaveChanges();
      //recupero nuevo perfil
      var perfilCambiado = _context.Propietario.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
      PropietarioView propietarioView = new PropietarioView(perfilCambiado);
      //retorno el perfil
      return Ok(propietarioView);
    }


      [HttpGet("fotoPerfil/{nombreImagen}")]
      [Authorize]
        public IActionResult RecuperarImagen(string nombreImagen)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", nombreImagen);

            if (System.IO.File.Exists(imagePath))
            {
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(imageBytes, "image/jpeg");
            }
            else
            {
                return NotFound(); // Si la imagen no existe
            }
        }
}
