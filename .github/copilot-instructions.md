# Copilot instructions

This is an ASP.NET Core (.NET 10) Web API for managing users (`UserManagementAPI`).

- Controllers live in `Controllers/`, models in `Models/`, custom middleware in `Middleware/`.
- Keep endpoint responses and status codes consistent with the existing patterns in `UsersController.cs` (e.g. `NotFound(new { error = "..." })`, `BadRequest(new { error = "..." })`).
- Validate incoming `User` data (non-empty `Name`/`Email`, valid email format) before passing it to `IUserRepository`.
- Preserve the middleware pipeline order in `Program.cs`: error handling, then authentication, then logging.
- Prefer minimal, idiomatic ASP.NET Core code over introducing new frameworks or libraries.
