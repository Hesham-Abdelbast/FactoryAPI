using Application;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddBusinessServices();
builder.Services.AddPersistenceServices(builder.Configuration, builder.Environment);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Configure file size 
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // 50 MB
});

// ✅ Configure CORS to allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()    // ✅ Allow all domains
                .AllowAnyHeader()
                .AllowAnyMethod();   // ✅ Allow GET, POST, PUT, DELETE, etc.
        });
});
var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var uploadFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName,
    builder.Configuration["UploadSettings:UploadFolder"]!);

var RequestPath = builder.Configuration["UploadSettings:RequestPath"]!;
Console.WriteLine("Upload Folder: " + uploadFolder);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadFolder),
    RequestPath = RequestPath,
});


app.MapControllers();

app.Run();
