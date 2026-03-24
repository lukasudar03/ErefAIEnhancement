using ErefAIEnhancement.Data;
using ErefAIEnhancement.Models;
using ErefAIEnhancement.Repositories.Implementations;
using ErefAIEnhancement.Repositories.Interfaces;
using ErefAIEnhancement.Services.Implementations;
using ErefAIEnhancement.Services.Interfaces;
using FluentValidation;
using ErefAIEnhancement.Validators;
using ErefAIEnhancement.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using ErefAIEnhancement.Validators.SubjectValidators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateRoleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateStudentDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSubjectDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateSubjectDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISubjectService, SubjectService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var key = jwtSection["Key"]!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    const string schemeId = "Bearer";

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ErefAIEnhancement API",
        Version = "v1"
    });

    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Unesi JWT token u formatu: Bearer {token}"
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(schemeId, doc),
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseGlobalExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();