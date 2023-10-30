using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pc3.DTO;
using pc3.Integrations;

namespace pc3.Controllers.UI
{
    public class TodoController : Controller
    {
        private readonly ILogger<TodoController> _logger;
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public TodoController(ILogger<TodoController> logger,JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _logger = logger;
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> Index()
        {

            List<PostDTO> post =await _jsonplaceholder.GetAllPost();
            List<PostDTO> filtro = post
            .Where(todo => todo.id > 121)
            .OrderBy(todo => todo.title)
            .ToList();

            return View("ListPostView",post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost("/EnviarDatos")]
        public async Task<IActionResult> EnviarDatos(String title, String body, int userId)
        {

            var variable = await _jsonplaceholder.PostData(title,body,userId);
            Console.WriteLine(variable); //CAMBIE ESTO    
            return RedirectToAction("PostData");
        }

        [HttpGet]
        public async Task<IActionResult> PostData()
        {
            return View("Postview");
        }


        public async Task<IActionResult> DeleteData(int postId)
        {
            await _jsonplaceholder.DeleteData(postId);
            return View("Postview");
        }

        public async Task<IActionResult> UpdateData(int userIdRequest,int normalIdRequest,string titleRequest,string bodyRequest){
            await _jsonplaceholder.UpdateData(normalIdRequest,userIdRequest,titleRequest,bodyRequest);
           return RedirectToAction("Index"); 
        }

        [HttpGet]
        public IActionResult UpdateForm(int idPost,int userId)
        {

            Console.WriteLine(idPost + userId + " ");

            ViewBag.userIdTag =userId;
            ViewBag.postIdTag =idPost;
            return View("UpdateView");

        }

        [HttpGet]
        public async Task<IActionResult> VerData(int idPost,int userId){
            var post = await _jsonplaceholder.ViewData(userId,idPost);
            Console.WriteLine(post);
           return View("PostDataView",post); 
        }



    }
}