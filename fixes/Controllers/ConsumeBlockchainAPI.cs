using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace fixes.Controllers
{
    public class BlockchainController : Controller
    {
        private readonly HttpClient _client;

        public BlockchainController()
        {
            _client = new HttpClient();
            // Configurar aquí cualquier configuración adicional para HttpClient
            // Por ejemplo, configurar encabezados, autenticación, etc.
        }

        public async Task<IActionResult> ConsumeBlockchainAPI()
        {
            try
            {
                // URL de la API de la blockchain que deseas consumir
                string apiUrl = "URL_DE_LA_API_DE_LA_BLOCKCHAIN_AQUI";

                // Realizar una solicitud GET a la API
                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta como una cadena JSON
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Aquí puedes procesar la respuesta JSON según las necesidades de tu aplicación
                    // Por ejemplo, devolver la respuesta a una vista para mostrarla
                    return View("MostrarRespuesta", jsonResponse);
                }
                else
                {
                    ViewBag.ErrorMessage = "Error al llamar a la API: " + response.StatusCode;
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error: " + ex.Message;
                return View("Error");
            }
        }
    }
}
