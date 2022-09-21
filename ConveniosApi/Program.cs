using ConveniosApi.ConveniosMedicosEntities;
using ConveniosApi.Helpers;
using ConveniosApi.Models;
using ConveniosApi.Services;
using ConveniosApi.ServiciosAcromaxEntities;
using DalSoft.Hosting.BackgroundQueue.DependencyInjection;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Serilog;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

//SeriLog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console());

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("key"));
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddDbContext<convenios_medicosContext>();
builder.Services.AddDbContext<servicios_acromaxContext>();
//JWT
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
                var tcs = new TaskCompletionSource<object>();

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    //LifetimeValidator = LifetimeValidator,

                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        // Call this to skip the default logic and avoid using the default response
                        context.HandleResponse();

                        // Write to the response in any way you wish
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("{\"error\":\"No Autorizado\"}");
                    },
                    OnMessageReceived = async context =>
                    {
                        await tcs.Task;
                        var headerToken = context.HttpContext.Request.Headers["Authorization"].ToString();
                        var textToken = headerToken.Split(" ").Last();
                        if (!string.IsNullOrWhiteSpace(textToken))
                        {
                            var protector = new TokenProtector().GetProtector();
                            try
                            {
                                var token = protector.Unprotect(textToken);
                                context.Token = token;
                            }
                            catch
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync("{\"error\":\"Token no válido\"}");
                            }
                        }
                    }
                };

                tcs.TrySetResult(true);
            });
//Add Json encode
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
    options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerSettings.Converters.Add(new DateOnlyNullJsonConverter());
})
.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization();
//Add internacionalization
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("es")
                    };

    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddBackgroundQueue(onException: exception => { });

//Controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API convenios médicos",
        Description = "Api para convenios médicos",
        Contact = new OpenApiContact
        {
            Name = "Daniel Paredes",
            Email = "dparedes@acromax.com.ec",
        },
        License = new OpenApiLicense
        {
            Name = "Todos los derechos reservados Acromax Ecuador.",
            Url = new Uri("https://acromax.com.ec"),
        }
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Utiliza el endpoint /Auth para generar un token de seguridad con tu usuario de Servicios Acromax.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
            }
        });
        var filePath = Path.Combine(System.AppContext.BaseDirectory, "ConveniosApi.xml");
        c.IncludeXmlComments(filePath);
});

builder.Services.AddQuartz(QuartzHelper.Configure);

builder.Services.AddQuartzServer(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();
IWebHostEnvironment env = app.Environment;


app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});


//app.UseDeveloperExceptionPage();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Convenios Medicos ACX");
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(env.ContentRootPath, "downloads")),
    RequestPath = "/downloads"
});

app.UseRouting();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

//Globalization
var supportedCultures = new[]
{
                new CultureInfo("es"),
                new CultureInfo("en"),
            };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
});


app.Run();
