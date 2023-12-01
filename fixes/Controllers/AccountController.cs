using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fixes.Data;
using fixes.Models;

namespace fixes.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ApplicationDbContext context) : base(context)
        {
            // Constructor de la clase AccountController
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            // Lógica para la acción Register aquí
            return View(); // O return RedirectToAction() dependiendo de la lógica de tu aplicación
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            try
            {
                if (usuario != null)
                {
                    if (await _context.Usuarios.AnyAsync(u => u.NombreUsario == usuario.NombreUsario))
                    {
                        ModelState.AddModelError(nameof(usuario.NombreUsario), "El nombre de usuario ya está en uso.");
                        return View(usuario);
                    }

                    var clienteRol = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
                    if (clienteRol != null)
                    {
                        usuario.RolId = clienteRol.RolId;
                    }

                    usuario.Direcciones = new List<Direccion>
                    {
                        new Direccion
                        {
                            Address = usuario.Direccion,
                            Ciudad = usuario.Ciudad,
                            Estado = usuario.Estado,
                            CodigoPostal = usuario.CodigoPostal
                        }
                    };

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, usuario.NombreUsario));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Cliente"));

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return RedirectToAction("Index", "Home");
                }

                return View(usuario);
            }
            catch (DbException dbException)
            {
                return HandleDbUpdateError(dbException);
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            // if (User.Identity != null && User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Administrador") || User.IsInRole("Staff"))
            //     {
            //         return RedirectToAction("Index", "Dashboard");
            //     }
            //     else
            //     {
            //         return RedirectToAction("Index", "Home");
            //     }
            // }

            // Agregar un retorno para manejar el caso en que el usuario no esté autenticado
            return View();
            // Puedes redirigir a otra acción en el controlador o devolver una vista según lo que necesites
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsario == username && u.Contrasenia == password);

                if (user != null)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, username));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UsarioId.ToString()));

                    var rol = await _context.Roles.FirstOrDefaultAsync(r => r.RolId == user.RolId);

                    if (rol != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, rol.Nombre));
                    }

                    // Obtener el nombre del rol para verificar si el usuario está en ese rol
                    var roleName = rol?.Nombre;

                    // Aquí realizamos la redirección según el rol antes de firmar en HttpContext
                    if (roleName != null && (roleName == "Administrador" || roleName == "Staff"))
                        return RedirectToAction("Index", "Dashboard");
                    else
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Credenciales inválidas.");
                return View();
            }
            catch (Exception e)
            {
                return HandleError(e);
            }
        }

        private IActionResult HandleError(Exception e)
        {
            // Lógica para manejar errores
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            // Lógica para mostrar acceso denegado
            return View();
        }
    }
}
