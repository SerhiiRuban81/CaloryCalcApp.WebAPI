using CaloryCalcLibrary;

namespace CaloryCalcApp.WebAPI.Models.DTOs
{
    public class RegisterUserDTO
    {
        //public sealed class RegisterRequest
        //{
        /// <summary>
        /// The user's email address which acts as a user name.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public required string Password { get; init; }

        public double Weight { get; set; } = default!; // Вага

        public int Height { get; set; } = default!; // Зріст

        public Sex Sex { get; set; }

        //public DateOnly DateOfBirth { get; set; } = new DateOnly();
}
    //}
}
