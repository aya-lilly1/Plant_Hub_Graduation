using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plant_Hub_Core.Mapper;
using Plant_Hub_Core.Managers.Account;
using Plant_Hub_Models.Models;
using Plant_Hub_Core.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Plant_Hub_Core.Managers.Categories;
using Plant_Hub_Core.Managers.Plants;
using Microsoft.Extensions.Configuration;
using Plant_Hub_ModelView;
using Plant_Hub_Core.Managers.Posts;
using GoogleTranslateFreeApi;
using Plant_Hub_Core.Managers.Users;
using Plant_Hub;
using Plant_Hub.ModelRepo;
using Plant_Hub.ModelServices;

var builder = WebApplication.CreateBuilder(args);


var mapperConfiguration = new MapperConfiguration(a => {
    a.AddProfile(new Mapping());
});
var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<Plant_Hub_dbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddSingleton<TranslationService>(); 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});



builder.Services.AddControllers();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddDefaultTokenProviders()
             .AddEntityFrameworkStores<Plant_Hub_dbContext>();
builder.Services.AddOptions();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWTOken"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JWTOken:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JWTOken:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTOken:Key")))
    };
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddScoped<IAccount, Account>();
builder.Services.AddScoped<ICatregory, CategoryRepo>();
builder.Services.AddScoped<IPlant, PlantRepo>();
builder.Services.AddScoped<IPost, PostRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IFileManagement, RepoFile>();
builder.Services.AddScoped<IModelRepos, ModelRepos>();
builder.Services.AddScoped<IPythonScriptExcutor, PythonScriptExcutor> ();
//builder.Services.AddScoped<ConsumePlantModel>();
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
//builder.Services.AddSingleton(serviceProvider =>
//{
//    var mlContext = serviceProvider.GetRequiredService<MLContext>();
//    var env = serviceProvider.GetRequiredService<IHostEnvironment>();
//    var modelPath = Path.Combine(env.ContentRootPath, "ModelPlant", "Newmodel.onnx");

//    var pipeline = mlContext.Transforms.ApplyOnnxModel(
//        outputColumnName: "classLabel", // Updated output column name
//        inputColumnName: nameof(ModelInput.Image),
//        modelFile: modelPath);

//    var model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new[] { new ModelInput() }));

//    // Create the prediction engine here
//    var predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);

//    return predictionEngine;
//});

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // Add console logging
    loggingBuilder.AddDebug();   // Add debug output logging
                                 // You can add other logging providers here if needed
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Plant Hub ", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors("AllowAngularOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.Run();
