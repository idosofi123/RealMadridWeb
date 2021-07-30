using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
//using System.Text.Json.Serialization;


namespace RealMadridWebApp.Models { 

    public enum UserType
    {
        Client,
        Manager,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [CustomBirthDateValidation(ErrorMessage = "Birth Date must be less than Today's Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserType Type { get; set; } = UserType.Client;

        public List<Match> Matches { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                User u = (User)obj;
                return (FirstName == u.FirstName) && (LastName == u.LastName) &&
                       (BirthDate == u.BirthDate) && (PhoneNumber == u.PhoneNumber) &&
                       (EmailAddress == u.EmailAddress) && (Password == u.Password) &&
                       (Type == u.Type);
            }
        }
    }
}
