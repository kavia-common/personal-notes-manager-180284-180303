using System.ComponentModel.DataAnnotations;

namespace NotesBackend.Models
{
    /// <summary>
    /// Represents a note with a unique identifier, title, content, and timestamps.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Unique identifier for the note.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the note.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Body content of the note.
        /// </summary>
        [MaxLength(4000)]
        public string? Content { get; set; }

        /// <summary>
        /// Creation timestamp (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp (UTC).
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
