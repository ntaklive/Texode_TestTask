using System.ComponentModel.DataAnnotations;

namespace TestTask.WebApi.Requests;

public class UpdateCardRequest
{
    [Required]
    public string Label { get; set; }  = null!;
    
    public IFormFile? File { get; set; }
}