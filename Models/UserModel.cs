using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpProject.Models
{
  public class User
  {
    [Key]
    public int UserId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }
    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "Please select date of wedding.")]
    [DateInPast]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public string? Gender { get; set; }
    public string? RelationshipStatus { get; set; }
    public string ProfilePhoto { get; set; } = "~/assets/images/user/blank-profile-picture.jpg";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Photo> Photos { get; set; } = new();
    public int? LocationId { get; set; }
    public Location? Location { get; set; }
  }


  public class UniqueEmailAttribute : ValidationAttribute
  {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value == null)
      {
        return new ValidationResult("Email is required!");
      }
      MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
      if (_context.Users.Any(e => e.Email == value.ToString()))
      {
        return new ValidationResult("Email must be unique!");
      }
      else
      {
        return ValidationResult.Success;
      }
    }
  }

  public class DateInPastAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      DateTime date = Convert.ToDateTime(value);
      if (date > DateTime.Now)
      {
        return new ValidationResult("Date must be in the past.");
      }
      else
      {
        return ValidationResult.Success;
      }
    }
  }
}
