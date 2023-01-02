using backend.Services;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
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
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("cors", policyBuilder =>
//     {
//         policyBuilder.AllowAnyMethod()
//             .AllowAnyHeader()
//             .SetIsOriginAllowed(origin => true)
//             .AllowCredentials()
//             .WithOrigins(builder.Configuration["AllowedOrigins"]);
//     });
// });

var app = builder.Build();

// app.UseCors("cors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();