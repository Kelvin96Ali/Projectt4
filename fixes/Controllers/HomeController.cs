using Microsoft.AspNetCore.Mvc;
using fixes.Models;
using fixes.Services;

using fixes.Controllers;
using fixes.Data;
using Ecommerce.Services;

namespace fixes.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger; // Declaración única del logger
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            IProductoService productoService,
            ICategoriaService categoriaService
        )
        : base(context)
        {
            _logger = logger;
            _productoService = productoService;
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categorias = await _categoriaService.GetCategorias();
            try
            {

                List<Producto> productosDestacados = await _productoService.GetProductosDestacados();
                return View(productosDestacados);
            }
            catch (Exception e)
            {
                return HandleError(e);

            }
        }

        private IActionResult HandleError(Exception e)
        {
            throw new NotImplementedException();
        }

        public IActionResult DetalleProducto(int id)
        {
            var producto = _productoService.GetProducto(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }




        public async Task<IActionResult> Productos(
        int? categoriaId,
        string? busqueda,
        int pagina = 1)
        {
            try
            {
                int productosPorPagina = 9;

                // Obtiene los productos paginados según los parámetros proporcionados
                var model = await _productoService.GetProductosPaginados(categoriaId, busqueda, pagina, productosPorPagina);

                // Obtiene las categorías para mostrar en la vista
                ViewBag.Categorias = await _categoriaService.GetCategorias();

                // Verifica si la solicitud es una petición AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // Si es una solicitud AJAX, devuelve una vista parcial con los productos
                    return PartialView("_ProductosPartial", model);
                }

                // Si no es una solicitud AJAX, devuelve la vista completa con los productos
                return View(model);
            }
            catch (Exception e)
            {
                // En caso de error al obtener los productos, maneja la excepción
                return HandleDbUpdateError(e);
            }
        }

        private IActionResult HandleDbUpdateError(Exception e)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> AgregarProducto(int id, int cantidad, int? categoriaId, string? busqueda, int pagina = 1)
        {
            var carritoViewModel = await AgregarProductoAlCarrito(id, cantidad);

            if (carritoViewModel != null)
            {
                return RedirectToAction("Productos", new { id, categoriaId, busqueda, pagina });
            }
            else
                return NotFound();


        }

        public async Task<IActionResult> AgregarProductoIndex(int id, int cantidad)
        {
            var carritoViewModel = await AgregarProductoAlCarrito(id, cantidad);

            if (carritoViewModel != null)
            {
                return RedirectToAction("Index");
            }
            else
                return NotFound();


        }

        public async Task<IActionResult> AgregarProductoDetalle(int id, int cantidad)
        {
            var carritoViewModel = await AgregarProductoAlCarrito(id, cantidad);

            if (carritoViewModel != null)
            {
                return RedirectToAction("DetalleProducto", new {id});
            }
            else
                return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }






    }

}








