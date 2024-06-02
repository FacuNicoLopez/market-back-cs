using Microsoft.EntityFrameworkCore;

namespace ApiUsuarios.Models{
    public class UsuarioContext : DbContext {
        public UsuarioContext(DbContextOptions<UsuarioContext> options): base(options){

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Billetera> Billeteras { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
    }
}