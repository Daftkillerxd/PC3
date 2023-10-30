using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using pc3.DTO;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<List<PostDTO>> GetAllPost(){
            string requestUrl = $"{API_URL}";
            List<PostDTO> listado = new List<PostDTO>();
            try{
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado =  await response.Content.ReadFromJsonAsync<List<PostDTO>>() ?? new List<PostDTO>();
                }
            }catch(Exception ex){
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }
            return listado;
        }



        public async Task<string> PostData(string title, string body, int userId)
        {
            string requestUrl = $"{API_URL}";

            var requestData = new{
                    title = title,
                    body = body,
                    userId = userId 
            };

            string jsonData = JsonSerializer.Serialize(requestData);

            try
            {
                using (var httpClient = new HttpClient())
                {

                     var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Realiza la solicitud POST
                    HttpResponseMessage response = await httpClient.PostAsync(requestUrl, content);

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
    

        public async Task<string> DeleteData(int postId)
        {
            string requestUrl = $"https://jsonplaceholder.typicode.com/posts/{postId}";

            using (var httpClient = new HttpClient())
            {
                
                HttpResponseMessage response = await httpClient.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Respuesta de la API: " + responseContent);
                        return responseContent;
                }
                else
                {
                    Console.WriteLine("Error en la solicitud DELETE. Código de estado: " + (int)response.StatusCode);
                    return null;
                }
            }
        }

        public async Task UpdateData(int id,int userId,string title,string body)
        {
            string requestUrl = $"https://jsonplaceholder.typicode.com/posts/{id}";

            var requestData = new
            {
                id = id,
                title = title,
                body = body,
                userId = userId
            };

            // Serializa el objeto en formato JSON
            string jsonData = JsonSerializer.Serialize(requestData);

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Realiza la solicitud PUT
                HttpResponseMessage response = await httpClient.PutAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Solicitud PUT exitosa. Respuesta de la API: " + responseContent);
                }
                else
                {
                    Console.WriteLine("Error en la solicitud PUT. Código de estado: " + (int)response.StatusCode);
                }
            }
        }

        public async Task<PostDTO> ViewData(int userId,int idPost){
            string apiUrl = $"https://jsonplaceholder.typicode.com/posts?userId={userId}&&id={idPost}";

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    List<PostDTO> posts = JsonSerializer.Deserialize<List<PostDTO>>(responseContent);
                    Console.WriteLine(apiUrl);
                    Console.WriteLine(responseContent);
                    return posts[0];
                }
                else
                {
                    throw new Exception("Error en la solicitud GET. Código de estado: " + (int)response.StatusCode);
                }
            }
        }

    
    
    
    
    
    }

    
}