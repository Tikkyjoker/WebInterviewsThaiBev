namespace WebInterviewsThaiBev.Models;

public class FormDataModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public IFormFile Profile { get; set; }
    public DateOnly BirthDay { get; set; }
    public string Occupation { get; set; }
    public string Sex { get; set; }
}