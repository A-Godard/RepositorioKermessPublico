//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SolucionKermesseGrupo2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TasaCambio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TasaCambio()
        {
            this.TasaCambioDet = new HashSet<TasaCambioDet>();
        }
    
        public int idTasaCambio { get; set; }
        public int monedaO { get; set; }
        public int monedaC { get; set; }
        public string mes { get; set; }
        public int anio { get; set; }
        public int estado { get; set; }
    
        public virtual Moneda Moneda { get; set; }
        public virtual Moneda Moneda1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TasaCambioDet> TasaCambioDet { get; set; }
    }
}
