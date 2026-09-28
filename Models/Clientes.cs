using System.ComponentModel.DataAnnotations;

namespace WebEmpresa.Models;

public class Clientes
{
    public int Id_cliente { get; set; }

    [Required(ErrorMessage = "El CUI es obligatorio.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El CUI debe tener exactamente 13 dígitos numéricos.")]
    public string? CUI { get; set; }

    [Required(ErrorMessage = "El NIT es obligatorio.")]
    [RegularExpression(@"^\d{1,9}-?[0-9Kk]$", ErrorMessage = "NIT inválido. Ejemplo: 1234567-8 o 1234567K.")]
    public string? NIT { get; set; }

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 100 caracteres.")]
    public string? Nombres { get; set; }

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres.")]
    public string? Apellidos { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 200 caracteres.")]
    public string? Direccion { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener 8 dígitos.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [FechaNoFutura]
    public DateTime? Fecha_nacimiento { get; set; }

    public Clientes Clonar() => (Clientes)MemberwiseClone();
}

public class FechaNoFuturaAttribute : ValidationAttribute
{
    public FechaNoFuturaAttribute()
        : base("La fecha de nacimiento no es válida (no puede ser futura ni anterior a 1900).") { }

    public override bool IsValid(object? value)
    {
        if (value is null) return true; // [Required] se encarga del vacío
        if (value is not DateTime fecha) return false;
        return fecha.Date <= DateTime.Today && fecha.Year >= 1900;
    }
}