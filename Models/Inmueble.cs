namespace ApiTpLab.Models;

public class Inmueble
{
    public int Id { get; set; }
    public string? Direccion { get; set; }="";
    public int? Uso { get; set; }
    public int? Tipo { get; set; }
    public int? CantidadDeAmbientes { get; set; }
    public decimal? Precio { get; set; }
    public int? PropietarioId { get; set; }
    public bool Estado { get; set; }
    //imagenes agregar




  
    

   
}
