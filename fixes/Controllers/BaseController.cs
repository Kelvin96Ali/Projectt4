using System.Data.Common;
using fixes.Data;
using fixes.Models;
using fixes.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace fixes.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context; // Corrected the declaration and access level

        // Constructor to initialize the DbContext
        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override ViewResult View(string? viewName, object? model)
        {
            ViewBag.NumeroProductos = GetCarritoCount();
            return base.View(viewName, model);
        }

        protected int GetCarritoCount()
        {
            int count = 0;

            string? carritoJson = Request.Cookies["carrito"];

            if (!string.IsNullOrEmpty(carritoJson))

            {

                var carrito = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(
                    carritoJson
                );
                if (carrito != null)
                {

                    count = carrito.Count;

                }

            }

            return count;
        }

        public async Task<CarritoViewModel> AgregarProductoAlCarrito(int productoId, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(productoId);

            if (producto != null)
            {
                var carritoViewModel = await GetCarritoViewModelAsync();

                var carritoItem = carritoViewModel.Items.FirstOrDefault(
                    item => item.ProductoId == productoId
                );

                if (carritoItem != null)
                {
                    carritoItem.Cantidad += cantidad; // Corregido el nombre de la propiedad 'cantidad' a 'Cantidad'
                }
                else
                {
                    carritoViewModel.Items.Add(
                        new CarritoItemViewModel
                        {
                            ProductoId = producto.ProductoId,
                            Nombre = producto.Nombre,
                            Precio = producto.Precio,
                            Cantidad = cantidad
                        }
                    );
                }

                carritoViewModel.Total = carritoViewModel.Items.Sum(
                    item => item.Cantidad * item.Precio
                );
                await UpdateCarritoViewModelAsync(carritoViewModel);
                return carritoViewModel;
            }

            return new CarritoViewModel();
        }


        public async Task UpdateCarritoViewModelAsync(CarritoViewModel carritoViewModel)
        {

            var productoIds = carritoViewModel.Items.Select(
                item => new ProductoIdAndCantidad
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad
                }
            )
            .ToList();

            var carritoJson = await Task.Run(() => JsonConvert.SerializeObject(productoIds));
            Response.Cookies.Append(
                "carrito",
                carritoJson,
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) }
            );



        }

        private async Task<CarritoViewModel> GetCarritoViewModelAsync()
        {
            var carritoJson = Request.Cookies["carrito"];

            if (string.IsNullOrEmpty(carritoJson))
            {
                return new CarritoViewModel(); // Retorna un nuevo CarritoViewModel si la cadena de carritoJson está vacía o es nula
            }

            List<ProductoIdAndCantidad>? productoIdsAndCantidades = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(carritoJson);

            var carritoViewModel = new CarritoViewModel();

            if (productoIdsAndCantidades != null)
            {
                foreach (var item in productoIdsAndCantidades)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);

                    if (producto != null)
                    {
                        carritoViewModel.Items.Add(
                            new CarritoItemViewModel
                            {
                                ProductoId = producto.ProductoId,
                                Nombre = producto.Nombre,
                                Precio = producto.Precio,
                                Cantidad = item.Cantidad // Asegúrate de que la propiedad se llame correctamente (por ejemplo, "Cantidad" en lugar de "cantidad")
                            }
                        );
                    }
                }
            }

            carritoViewModel.Total = carritoViewModel.Items.Sum(item => item.Subtotal);

            return carritoViewModel;
        }


        protected IActionResult HandleDbUpdateError(DbException dbUpdateException)
        {
            var ViewModel = new DbErrorViewModel
            {
                ErrorMessage = "Error de base de datos",
                Datails = dbUpdateException.Message // Corregido el error tipográfico de dbUpdateExeception a dbUpdateException
            };
            return View("DbError", ViewModel);
        }




    }
}
