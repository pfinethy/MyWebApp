var builder = WebApplication.CreateBuilder(args);

// Enable CORS so React (usually on port 5173) can talk to .NET
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowReactApp");


// -----------------------------
// TASK DATA (from previous assignment)
// -----------------------------
var tasks = new List<TodoItem> {
    new(1, "Learn .NET", true),
    new(2, "Master React", false)
};

// Get tasks
app.MapGet("/api/tasks", () => tasks);

// Add new task
app.MapPost("/api/tasks", (TodoItem newItem) =>
{
    tasks.Add(newItem);
    return Results.Created($"/api/tasks/{newItem.Id}", newItem);
});

var users = new List<User>();
var nextUserId = 1;

// Get all users
app.MapGet("/api/users", () => users);

// Create new user
app.MapPost("/api/users", (CreateUserRequest req) =>
{
    var newUser = new User(
        nextUserId++,
        req.Username,
        req.Email,
        req.PasswordDigest,
        DateTime.UtcNow
    );

    users.Add(newUser);

    return Results.Created($"/api/users/{newUser.Id}", newUser);
});

// Update user
app.MapPut("/api/users/{id}", (int id, UpdateUserRequest req) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user == null)
        return Results.NotFound();

    var updatedUser = user with
    {
        Username = req.Username ?? user.Username,
        Email = req.Email ?? user.Email
    };

    users.Remove(user);
    users.Add(updatedUser);

    return Results.Ok(updatedUser);
});


app.Run();


// -----------------------------
// DATA MODELS
// -----------------------------
record TodoItem(int Id, string Title, bool IsCompleted);

record User(
    int Id,
    string Username,
    string Email,
    string PasswordDigest,
    DateTime CreatedAt
);

record CreateUserRequest(
    string Username,
    string Email,
    string PasswordDigest
);

record UpdateUserRequest(
    string? Username,
    string? Email
);