using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiTpLab.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiTpLab.Controllers;

[ApiController]
[Route("[controller]")]
public class ContratoController : ControllerBase
{

private readonly DataContext _context;
private readonly IConfiguration _config;
    public ContratoController(DataContext contexto, IConfiguration config)
    {
        _context = contexto;
        _config = config;
    }

   

    
    
     [HttpGet("{id}")]
     [Authorize]
public IActionResult getContrato(int id)
{
  
    var contrato = _context.Contrato.FirstOrDefault(x => x.Id == id);

    if (contrato == null)
    {
        return NotFound();
    }

   

        return Ok(contrato);
}

/*

   [HttpGet("vigentes/{id}")]
     [Authorize]
public IActionResult getVigentes(int id)
{
    Console.WriteLine("hola");
   var inmuebles = _context.Inmueble.Where(x => x.PropietarioId == id).ToList();
  
   Console.WriteLine(inmuebles);
   var ContratosViews = new List<ContratoView>();
   Inquilino? inquilino = null;
   foreach (var inmueble in inmuebles)
   {
       var contrato = _context.Contrato.Where(x => x.FechaFinal >= DateTime.Today && x.InmuebleId == inmueble.Id).FirstOrDefault();
       
       if(contrato != null){
        if(contrato.InquilinoId != null){
        inquilino = _context.Inquilino.Where(x => x.Id == contrato.InquilinoId).FirstOrDefault();
       }
       var contView = new ContratoView(contrato, inmueble, inquilino);
       ContratosViews.Add(contView);
       }
   }
    

    if (ContratosViews.Count == 0)
    {
        return NotFound();
    }

       
    //crear view y pasarle el inmueble y setearle los datos buscanos las tablas

    return Ok(ContratosViews);
}*/

[HttpGet("vigentes")]
     [Authorize]
public IActionResult getVigentes()
{
 var propietarioEmail = User.Identity.Name;
   
   
var today = DateTime.Today;

var contratos = _context.Contrato
    .Join(_context.Inmueble,
        contrato => contrato.InmuebleId,
        inmueble => inmueble.Id,
        (contrato, inmueble) => new { Contrato = contrato, Inmueble = inmueble })
    .Join(_context.Inquilino,
        joinResult => joinResult.Contrato.InquilinoId,
        inquilino => inquilino.Id,
        (joinResult, inquilino) => new { Contrato = joinResult.Contrato, Inmueble = joinResult.Inmueble, Inquilino = inquilino })
    .Join(_context.Propietario,
        joinResult => joinResult.Inmueble.PropietarioId,
        propietario => propietario.Id,
        (joinResult, propietario) =>
         new { Contrato = joinResult.Contrato, Inmueble = joinResult.Inmueble, Inquilino = joinResult.Inquilino, Propietario = propietario })
    .Where(x => x.Propietario.Email == propietarioEmail
        && x.Contrato.FechaFinal >= today)
    .Select(x => new ContratoView(x.Contrato, x.Inmueble, x.Inquilino))
    .ToList();

   
   

   

       
    //crear view y pasarle el inmueble y setearle los datos buscanos las tablas

    return Ok(contratos);
}
 


        


    

 
}
