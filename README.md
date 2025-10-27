# personal-notes-manager-180284-180303

Notes Backend (ASP.NET Core, .NET 8)
- Port: 3001
- Swagger UI: http://localhost:3001/docs
- OpenAPI JSON: http://localhost:3001/openapi.json

Endpoints
- GET /notes — list all notes
- GET /notes/{id} — get a note by ID
- POST /notes — create a note (JSON: { "title": "required", "content": "optional" })
- PUT /notes/{id} — update an existing note (JSON: { "title": "required", "content": "optional" })
- DELETE /notes/{id} — delete a note

Model
- id: GUID (server generated)
- title: string, required, max 200
- content: string, optional, max 4000
- createdAt: Date (UTC)
- updatedAt: Date (UTC)

Run locally (Development)
- Launch via `dotnet run` from notes_backend directory (port 3001 configured in launchSettings.json)
- CORS is enabled for local dev (AllowAll)
- Sample seed notes are available on startup

Example cURL
- Create:
  curl -s -X POST http://localhost:3001/notes -H "Content-Type: application/json" -d '{"title":"My first","content":"Body"}'
- Get all:
  curl -s http://localhost:3001/notes
- Update:
  curl -s -X PUT http://localhost:3001/notes/{id} -H "Content-Type: application/json" -d '{"title":"Updated","content":"New"}'
- Delete:
  curl -s -X DELETE http://localhost:3001/notes/{id}