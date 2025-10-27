using NotesBackend.Models;
using System.Collections.Concurrent;

namespace NotesBackend.Services
{
    /// <summary>
    /// In-memory implementation of INoteService using a thread-safe concurrent dictionary.
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly ConcurrentDictionary<Guid, Note> _notes = new();

        public NoteService()
        {
            // Seed data
            var now = DateTime.UtcNow;
            var samples = new[]
            {
                new Note
                {
                    Id = Guid.NewGuid(),
                    Title = "Welcome to Notes",
                    Content = "This is a sample note. Feel free to edit or delete it.",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Note
                {
                    Id = Guid.NewGuid(),
                    Title = "Second Note",
                    Content = "You can create, read, update, and delete notes via the API.",
                    CreatedAt = now,
                    UpdatedAt = now
                }
            };

            foreach (var n in samples)
            {
                _notes.TryAdd(n.Id, n);
            }
        }

        public IEnumerable<Note> GetAll() => _notes.Values.OrderByDescending(n => n.UpdatedAt);

        public Note? GetById(Guid id)
        {
            _notes.TryGetValue(id, out var note);
            return note;
        }

        public Note Create(Note note)
        {
            var now = DateTime.UtcNow;

            // Ensure server controls ID and timestamps
            note.Id = note.Id != Guid.Empty ? note.Id : Guid.NewGuid();
            note.CreatedAt = now;
            note.UpdatedAt = now;

            _notes[note.Id] = note;
            return note;
        }

        public Note? Update(Guid id, Note updated)
        {
            if (!_notes.TryGetValue(id, out var existing))
            {
                return null;
            }

            existing.Title = updated.Title;
            existing.Content = updated.Content;
            existing.UpdatedAt = DateTime.UtcNow;

            _notes[id] = existing;
            return existing;
        }

        public bool Delete(Guid id) => _notes.TryRemove(id, out _);
    }
}
