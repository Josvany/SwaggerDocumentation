namespace Library.API.Models
{
    /// <summary>
    /// author
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Id del author
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// apellido
        /// </summary>
        public string? LastName { get; set; }
    }
}