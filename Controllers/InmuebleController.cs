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
public class InmuebleController : ControllerBase
{

private readonly DataContext _context;
private readonly IConfiguration _config;
    public InmuebleController(DataContext contexto, IConfiguration config)
    {
        _context = contexto;
        _config = config;
    }

   

    
    
     [HttpGet()]
     [Authorize]
public IActionResult getInmuebles()
{
     var propietarioEmail = User.Identity.Name;
     var propietario = _context.Propietario.Where(x => x.Email == propietarioEmail).FirstOrDefault();
    var inmuebles = _context.Inmueble.Where(x => x.PropietarioId == propietario.Id).ToList();

    if (inmuebles.Count == 0)
    {
        return NotFound();
    }

    var inmueblesV = new List<InmuebleView>();
    foreach (var inmueble in inmuebles)
    {
        InmuebleView inmuebleV = new InmuebleView(inmueble);
        UsoController usoController = new UsoController(_context, _config);
        TipoController tipoController = new TipoController(_context, _config);
        var uso = _context.Uso.Where(x => x.Id == inmueble.Uso).FirstOrDefault();
        var tipo = _context.Tipo.Where(x => x.Id == inmueble.Tipo).FirstOrDefault();
        inmuebleV.Uso = uso.NombreUso;
        inmuebleV.Tipo = tipo.NombreTipo;
        inmueblesV.Add(inmuebleV);
    }
    
    //crear view y pasarle el inmueble y setearle los datos buscanos las tablas

    Console.WriteLine(inmuebles);
    return Ok(inmueblesV);
}


 [HttpGet("Alquilado/{id}")]
 [Authorize]
public IActionResult getInmueblesAlquilados(int id)
{
    var inmuebles = _context.Inmueble.Where(x => x.PropietarioId == id).ToList();

    if (inmuebles.Count == 0)
    {
        return NotFound();
    }

    var inmueblesV = new List<InmuebleView>();
    foreach (var inmueble in inmuebles)
    {
        InmuebleView inmuebleV = new InmuebleView(inmueble);
        UsoController usoController = new UsoController(_context, _config);
        TipoController tipoController = new TipoController(_context, _config);
        var uso = _context.Uso.Where(x => x.Id == inmueble.Uso).FirstOrDefault();
        var tipo = _context.Tipo.Where(x => x.Id == inmueble.Tipo).FirstOrDefault();
        inmuebleV.Uso = uso.NombreUso;
        inmuebleV.Tipo = tipo.NombreTipo;
        inmueblesV.Add(inmuebleV);
    }
     Console.WriteLine(inmuebles);
    return Ok(inmueblesV);
}

      [HttpGet("fotoInmueble/{nombreImagen}")]
      [Authorize]
        public IActionResult RecuperarImagenInm(string nombreImagen)
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

        [HttpPut("CambiarEstado/{id}")]
      [Authorize]
        public IActionResult CambiarEstado(int id)
        {
            var inmueble = _context.Inmueble.Where(u => u.Id == id).FirstOrDefault();
            inmueble.Estado = !inmueble.Estado;
            _context.SaveChanges();
            return Ok(inmueble);

    if (inmueble == null)
    {
        return NotFound();
    }

    return Ok(inmueble);
        }


        


    

 /*    [HttpPut("{actualizar}")]
     [Authorize]
    public IActionResult ActualizarPerfil(PropietarioView nuevo)
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
      propietario.Contraseña = nuevo.Contraseña;
      
      //guardo cambios
      _context.SaveChanges();
      //recupero nuevo perfil
      var perfilCambiado = _context.Propietario.Where(x => x.Email == User.Identity.Name).Select(x => new PropietarioView(x)).FirstOrDefault();
      //retorno el perfil
      return Ok(propietario);
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
        */
}
