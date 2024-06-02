using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ApiUsuarios.Models;

namespace ApiUsuarios.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UsuarioContext db)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                AttachUserToContext(context, db, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, UsuarioContext db, string token)
        {
            try
            {
                var userId = Convert.ToInt32(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token)));
               
                var usuario = db.Usuarios.Find(userId);
                if (usuario != null)
                {
                    context.Items["User"] = usuario;
                }
            }
            catch
            {
                
            }
        }
    }
}
