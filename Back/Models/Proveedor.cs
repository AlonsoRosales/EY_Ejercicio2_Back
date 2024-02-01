using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProveedor { get; set; }

        [MaxLength]
        public string RazonSocial { get; set; }

        [MaxLength]
        public string NombreComercial { get; set; }

        [StringLength(11)]
        public long IdTributaria { get; set; }

        [StringLength(20)]
        public long Telefono { get; set; }

        [MaxLength(320)]
        public string Correo { get; set; }

        [MaxLength(2048)]
        public string Web { get; set; }

        [MaxLength]
        public string Direccion { get; set; }

        [MaxLength(100)]
        public string Pais { get; set; }

        [Column(TypeName = "decimal(19, 4)")]
        public decimal Facturacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaEdicion { get; set; }
    }
}
