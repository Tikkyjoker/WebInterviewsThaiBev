using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebInterviewsThaiBev.Models;
using WebInterviewsThaiBev.Models.Schema;

namespace WebInterviewsThaiBev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SubmitForm([FromForm] FormDataModel model)
        {
            if (ModelState.IsValid && model.Profile.ContentType.StartsWith("image"))
            {
                var row = new Contact()
                {
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Birthday = model.BirthDay,
                    Occupation = model.Occupation,
                    Sex = model.Sex,
                    ProfileType = model.Profile?.ContentType,
                    ProfileContent = model.Profile?.Length > 0 ? ConvertToBase64(model.Profile) : null
                };
                _context.Contacts.Add(row);
                _context.SaveChanges();
                return Json(new { success = true, id = row.Id.ToString() });
            }

            return BadRequest(new { success = false, message = "Invalid data!" });
        }
        private string ConvertToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
        public IActionResult GetProfile(Guid id)
        {
            var contact = _context.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null || contact.ProfileContent == null || contact.ProfileType == null)
                return NotFound();
            var fileBytes = Convert.FromBase64String(contact.ProfileContent);
            return File(fileBytes, contact.ProfileType);
        }
    }
}
