using Library.API.Contexts;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.Reflection;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(configure =>
    {
        configure.ReturnHttpNotAcceptable = true;
        // si hay confirado un filtro los ApiConventionMethod no se visualizan
        //configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
        //configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
        //configure.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
    }
    ).AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }).AddXmlDataContractSerializerFormatters();

// configure the NewtonsoftJsonOutputFormatter
builder.Services.Configure<MvcOptions>(configureOptions =>
{
    var jsonOutputFormatter = configureOptions.OutputFormatters
        .OfType<NewtonsoftJsonOutputFormatter>().FirstOrDefault();

    if (jsonOutputFormatter != null)
    {
        // remove text/json as it isn't the approved media type
        // for working with JSON at API level
        if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
        {
            jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
        }
    }
});

builder.Services.AddDbContext<LibraryContext>(
    dbContextOptions => dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:LibraryDBConnectionString"]));

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    //opt.ApiVersionReader = new HeaderApiVersionReader("api-version"); // custom
    //opt.ApiVersionReader = new MediaTypeApiVersionReader(); // default
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("LibraryOpenApiSpecificication", new()
    {
        Title = "Library Api",
        Version = "v1",
        Description = "Descripcion del api",
        Contact = new()
        {
            Email = "jgarcia@gmail",
            Name = "Josvany"
        },
        License = new()
        {
            Name = "License",
        }
    });

    //multiple especificaciones
    //opt.SwaggerDoc("LibraryOpenApiSpecificicationAuthors", new()
    //{
    //    Title = "Library Api (Authors)",
    //    Version = "v1",
    //    Description = "Descripcion del api",
    //    Contact = new()
    //    {
    //        Email = "jgarcia@gmail",
    //        Name = "Josvany"
    //    },
    //    License = new()
    //    {
    //        Name = "License",
    //    }
    //});

    //opt.SwaggerDoc("LibraryOpenApiSpecificicationBooks", new()
    //{
    //    Title = "Library Api Books",
    //    Version = "v1",
    //    Description = "Descripcion del api",
    //    Contact = new()
    //    {
    //        Email = "jgarcia@gmail",
    //        Name = "Josvany"
    //    },
    //    License = new()
    //    {
    //        Name = "License",
    //    }
    //});

    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlCommentFile);

    opt.IncludeXmlComments(xmlCommentFilePath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(conf =>
{
    conf.SwaggerEndpoint("/swagger/LibraryOpenApiSpecificication/swagger.json", "Library Api");
    //conf.SwaggerEndpoint("/swagger/LibraryOpenApiSpecificicationAuthors/swagger.json", "Library Api (Authors)");
    //conf.SwaggerEndpoint("/swagger/LibraryOpenApiSpecificicationBooks/swagger.json", "Library Api (Books)");

    conf.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();