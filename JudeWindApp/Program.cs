using JudeWindApp.Attributes;
using JudeWindApp.Services;
using JudeWindApp.Util;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using PdfSharp.Fonts;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
GlobalFontSettings.FontResolver = WindResolverHelper.Instance;

builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.PropertyNamingPolicy = null;
    option.JsonSerializerOptions.IgnoreReadOnlyFields = true;
    option.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    // 充許屬性數量不足傳入
    option.JsonSerializerOptions.AllowTrailingCommas = true;
});
builder.Services.AddDBContext();
builder.Services.AddRepository();
builder.Services.AddIDataService();
builder.Services.AddDataService();
builder.Services.AddScoped(svc => new LogsService());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JudeWind API",
        Description = "",
        Contact = new OpenApiContact { Name = "Readme", Url = new Uri("https://github.com/jaskcdem/JudeWindApp/blob/master/README.md") },
        License = new OpenApiLicense { Name = "License", Url = new Uri("https://github.com/jaskcdem/JudeWindApp/blob/master/LICENSE.txt") },
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, true);
});
//global Filter
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseKestrel(option => option.AddServerHeader = false);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None));
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
