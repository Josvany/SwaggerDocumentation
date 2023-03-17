using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    /// <summary>
    /// actor update
    /// </summary>
    public class AuthorForUpdate
    {
        /// <summary>
        /// nombre actor
        /// </summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string? FirstName { get; set; }
        /// <summary>
        /// apellido actor
        /// </summary>
        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(100)]
        public string? LastName { get; set; }
    }
}