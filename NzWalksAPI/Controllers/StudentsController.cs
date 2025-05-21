using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NzWalksAPI.Controllers
{
    //https://localhost:port/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: https://localhost:port/api/students
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studnetnames = new string[] { "SHubham", "Nikhil", "Abhay", "Ajay", "Atul", "Amol" };

            return Ok(studnetnames);
        }
    }
}
