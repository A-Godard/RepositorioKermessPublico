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
    
    public partial class VwGasto
    {
        public int ID { get; set; }
        public string Kermesse { get; set; }
        public string Categoria { get; set; }
        public System.DateTime fechGasto { get; set; }
        public string concepto { get; set; }
        public double monto { get; set; }
        public string Usuario { get; set; }
    }
}
