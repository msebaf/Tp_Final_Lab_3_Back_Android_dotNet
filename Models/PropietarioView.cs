namespace ApiTpLab.Models;

public class PropietarioView

{
    public PropietarioView(){

    }

    public PropietarioView(Propietario propietario)
    {
        this.Id = propietario.Id;
        this.Email = propietario.Email;
        this.Nombre = propietario.Nombre;
        this.Apellido = propietario.Apellido;
        this.Avatar = propietario.Avatar;
        
        this.Dni = propietario.Dni;
        this.Telefono = propietario.Telefono;
        this.Direccion = propietario.Direccion;
        this.Nacimiento = propietario.Nacimiento;
        
     
    }
    public int Id { get; set; } 
     public string? Nombre { get; set; }="";
    public string? Apellido { get; set; }="";
    public string? Avatar { get; set; }="";
    public string? Email { get; set; }="";
  
    public string Direccion { get; set; }="";
    public DateTime? Nacimiento { get; set; }
    public String? Dni { get; set; }
    public String? Telefono { get; set; }
    
 
    

   
}
