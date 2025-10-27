using NotesBackend.Models;

namespace NotesBackend.Services
{
    // PUBLIC_INTERFACE
    public interface INoteService
    {
        /// <summary>
        /// Returns all notes.
        /// </summary>
        IEnumerable<Note> GetAll();

        /// <summary>
        /// Returns a note by ID or null if not found.
        /// </summary>
        Note? GetById(Guid id);

        /// <summary>
        /// Creates a new note and returns it.
        /// </summary>
        Note Create(Note note);

        /// <summary>
        /// Updates an existing note and returns it, or null if not found.
        /// </summary>
        Note? Update(Guid id, Note updated);

        /// <summary>
        /// Deletes a note by ID. Returns true if deleted, false if not found.
        /// </summary>
        bool Delete(Guid id);
    }
}
