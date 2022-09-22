using System.ComponentModel.DataAnnotations;

namespace TestTask.WebApi.Requests;

public class CreateCardRequest
{
    [Required]
    public string Label { get; set; }  = null!;
    
    [Required]
    public IFormFile File { get; set; }
}