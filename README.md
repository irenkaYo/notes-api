NotesApi

A backend for a notes application on ASP.NET Core. The project was written to explore JWT authentication and refresh tokens.

Stack
.NET 9, ASP.NET Core Web API
Entity Framework Core + PostgreSQL
JWT (access token + refresh token)
BCrypt for password hashing
Docker, Docker Compose
Project Structure
NotesApi/
├── Domain/ # models (User, Note, RefreshToken)
├── Application/ # interfaces, services, DTOs
├── Infrastructure/ # EF Core, repositories, JWT, hasher
└── API/ # controllers, Program.cs, Dockerfile

The project is divided into layers based on the Clean Architecture principle: each layer depends only on the layers below it.

Running via Docker
Create a .env file in the NotesApi/ folder (next to compose.yaml):
env
POSTGRES_PASSWORD=your_password
ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=notes_api;Username=postgres;Password=your_password
JwtOptions__SecretKey=your_very_long_secret_key_at_least_32_characters
Run:
bash
docker compose up --build

After running:

The API is available at http://localhost:8081
Administrator (database UI) is available at http://localhost:8080

Local Run

Requires: .NET 9 SDK and PostgreSQL.

Fill out appsettings.Development.json:

json
{
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Port=5432;Database=notes_api;Username=postgres;Password=your_password"
},
"JwtOptions": {
"SecretKey": "your_very_long_secret_key_at_least_32_characters",
"Issuer": "issuer",
"Audience": "audience",
"ExpiresMinutes": "15"
}
}

Then:

bash
cd NotesApi/API
dotnet run
API
Authorization
Method Path Description
POST /user/auth/register Registration
POST /user/auth/login Login, returns access and refresh token
POST /user/auth/refresh Token Refresh
GET /user/auth/me Current user information (requires token)

Registration:

json
{
"username": "irena",
"password": "12345"
}

Login returns:

json
{
"accessToken": "...",
"refreshToken": "..."
}

Token Refresh:

json
{
"accessToken": "...",
"refreshToken": "..."
}
Notes

All endpoints require the Authorization: Bearer <accessToken> header.

Method Path Description
GET /notes/all All user notes
GET /notes/{id} Note by ID
POST /notes/create Create a note
PUT /notes/update/{id} Update a note
DELETE /notes/delete/{id} Delete a note

Create a note:

json
{
"title": "My note",
"text": "Note text"
}
How authentication works

Upon login, the server issues two tokens: a short-lived access token (JWT) and a long-lived refresh token. The access token is used for requests to protected endpoints. When it expires, the client sends both tokens to /user/auth/refresh and receives a new pair. The old refresh token is then removed from the database.

API Documentation (Swagger)

In Development mode, it is available at http://localhost:<port>/swagger.
