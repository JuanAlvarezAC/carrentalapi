using System.ComponentModel.DataAnnotations;



namespace CarRental.Models

{

    public class Usuario

    {

        public int Id { get; set; }



        [Required]

        [MaxLength(100)]

        public string Username { get; set; } = string.Empty;



        [Required]

        public string PasswordHash { get; set; } = string.Empty;



        public string Role { get; set; } = "User"; // Ejemplos: "Admin", "User" 

        [EmailAddress]

        public required string Email { get; set; }

    }

}