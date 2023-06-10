using System.ComponentModel.DataAnnotations;

namespace ApiTpLab.Models;

public class Pago
{
    public int Id { get; set; }
    public int ContratoId { get; set; }
    public int? Numero { get; set; }
   
    public DateTime? FechaPago { get; set; }
    public Decimal Monto { get; set; }
    
    
    //imagenes agregar




  
    

   
}
