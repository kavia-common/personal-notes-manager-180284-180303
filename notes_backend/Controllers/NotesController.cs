using Microsoft.AspNetCore.Mvc;
using NotesBackend.Models;
using NotesBackend.Services;

namespace NotesBackend.Controllers
{
    [ApiController]
    [Route("notes")]
    [Produces("application/json")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _service;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INoteService service, ILogger<NotesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Returns a list of notes ordered by last updated date (desc).
        /// </summary>
        /// <returns>Array of Note</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Note>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Note>> GetAll()
        {
            var items = _service.GetAll();
            return Ok(items);
        }

        /// <summary>
        /// Returns a single note by its ID.
        /// </summary>
        /// <param name="id">Note GUID</param>
        /// <returns>Note if found</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Note> GetById(Guid id)
        {
            var note = _service.GetById(id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="request">Note payload (title required, content optional)</param>
        [HttpPost]
        [ProducesResponseType(typeof(Note), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Note> Create([FromBody] Note request)
        {
            if (request == null) return BadRequest("Request body required.");

            // Model validation: title is required
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                ModelState.AddModelError(nameof(Note.Title), "Title is required.");
                return ValidationProblem(ModelState);
            }

            // Ignore client-sent timestamps and set server timestamps
            request.CreatedAt = default;
            request.UpdatedAt = default;

            var created = _service.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing note by ID.
        /// </summary>
        /// <param name="id">Note GUID</param>
        /// <param name="request">Updated note data (title required)</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Note> Update(Guid id, [FromBody] Note request)
        {
            if (request == null) return BadRequest("Request body required.");
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                ModelState.AddModelError(nameof(Note.Title), "Title is required.");
                return ValidationProblem(ModelState);
            }

            var updated = _service.Update(id, request);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        /// <summary>
        /// Deletes a note by ID.
        /// </summary>
        /// <param name="id">Note GUID</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            var deleted = _service.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
