using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
        public int UserId{get;set;}
        [Required]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters!")]
        [Display(Name = "First Name:")]
        public string FirstName{get;set;}
        [Required]
        [MinLength(2, ErrorMessage="Last name must be at least 2 characters!")]
        [Display(Name = "Last Name:")]
        public string LastName{get;set;}
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address:")]
        public string Email{get;set;}
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password{get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPassword{get;set;}
        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;
        public List<Guests> Attending{get;set;}
    }
}