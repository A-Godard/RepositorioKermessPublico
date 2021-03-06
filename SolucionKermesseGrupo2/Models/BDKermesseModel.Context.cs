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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BDKermesseEntities : DbContext
    {
        public BDKermesseEntities()
            : base("name=BDKermesseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ArqueoCaja> ArqueoCaja { get; set; }
        public virtual DbSet<ArqueoCajaDet> ArqueoCajaDet { get; set; }
        public virtual DbSet<CategoriaGasto> CategoriaGasto { get; set; }
        public virtual DbSet<CategoriaProducto> CategoriaProducto { get; set; }
        public virtual DbSet<Comunidad> Comunidad { get; set; }
        public virtual DbSet<ControlBono> ControlBono { get; set; }
        public virtual DbSet<Denominacion> Denominacion { get; set; }
        public virtual DbSet<Gasto> Gasto { get; set; }
        public virtual DbSet<IngresoComunidad> IngresoComunidad { get; set; }
        public virtual DbSet<IngresoComunidadDet> IngresoComunidadDet { get; set; }
        public virtual DbSet<Kermesse> Kermesse { get; set; }
        public virtual DbSet<ListaPrecio> ListaPrecio { get; set; }
        public virtual DbSet<ListaPrecioDet> ListaPrecioDet { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Opcion> Opcion { get; set; }
        public virtual DbSet<Parroquia> Parroquia { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<RolOpcion> RolOpcion { get; set; }
        public virtual DbSet<RolUsuario> RolUsuario { get; set; }
        public virtual DbSet<TasaCambio> TasaCambio { get; set; }
        public virtual DbSet<TasaCambioDet> TasaCambioDet { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<VwProducto> VwProducto { get; set; }
        public virtual DbSet<VwIngresoComunidad> VwIngresoComunidad { get; set; }
        public virtual DbSet<vwIngresoComunidadDetalle> vwIngresoComunidadDetalle { get; set; }
        public virtual DbSet<VwGasto> VwGasto { get; set; }
        public virtual DbSet<VwRolOpciones> VwRolOpciones { get; set; }
        public virtual DbSet<VwArqueoCaja> VwArqueoCaja { get; set; }
        public virtual DbSet<VwListaPrecio> VwListaPrecio { get; set; }
    }
}
