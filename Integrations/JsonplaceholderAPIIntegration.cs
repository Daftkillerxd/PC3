using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using pc3.DTO;

namespace pc3.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL="https://jsonplaceholder.typicode.com/todos/";
        private readonly HttpClient httpClient;

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger){
            _logger = logger;
            httpClient = new HttpClient();
        }

        public async Task<List<TodoDTO>> GetAll(){
            string requestUrl = $"{API_URL}";
            List<TodoDTO> listado = new List<TodoDTO>();
            try{
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado =  await response.Content.ReadFromJsonAsync<List<TodoDTO>>() ?? new List<TodoDTO>();
                }
            }catch(Exception ex){
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }
            return listado;
        }

            public async Task<String> PostData(String title, String body, int userId)
        {
            string apiUrl = "https://ejemplo.com/api/endpoint";

                    var requestData = new
                {
                    title = title,
                    body = body,
                    userId = userId 
                };

                string jsonData = JsonSerializer.Serialize(requestData);

            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, new StringContent(jsonData));

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Respuesta de la API: " + responseContent);
                        return responseContent;
                    }
                    else
                    {
                        Console.WriteLine("Error en la solicitud. Código de estado: " + (int)response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja excepciones
                Console.WriteLine("Excepción: " + ex.Message);
            }
                return null;

        }
    }

    
}