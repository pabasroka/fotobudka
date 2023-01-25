using backend.Services;

const string origins = "origins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: origins,
        policy  =>
        {
            policy.AllowAnyOrigin();
        });
});

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IPdfBuilderService, PdfBuilderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(origins);

app.UseAuthorization();

app.MapControllers();

app.Run();