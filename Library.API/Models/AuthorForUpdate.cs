using System.ComponentModel.DataAnnotations;

namespace Library.API.Models
{
    /// <summary>
    /// actor update
    /// </summary>
    public class AuthorForUpdate
    {
        /// <summary>
        /// Primer Nombre del autor
        /// </summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Apellido del autor
        /// </summary>
        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(100)]
        public string? LastName { get; set; }
    }
}