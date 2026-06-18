using ClubDeportivo.TDatos;
using ClubDeportivo.Repositories;
using ClubDeportivo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. CONFIGURACIÓN DE CONEXIÓN
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<TDatosPostgreSQL>(provider =>
    new TDatosPostgreSQL(connectionString));

// ============================================================
// 2. REPOSITORIOS
// ============================================================
builder.Services.AddScoped<IAportes_sociosRepository, Aportes_sociosRepository>();
builder.Services.AddScoped<IDeudas_anterioresRepository, Deudas_anterioresRepository>();
builder.Services.AddScoped<IEgresosRepository, EgresosRepository>();
builder.Services.AddScoped<IOtros_ingresosRepository, Otros_ingresosRepository>();
builder.Services.AddScoped<IPagos_deudaRepository, Pagos_deudaRepository>();
builder.Services.AddScoped<IPeriodosRepository, PeriodosRepository>();
builder.Services.AddScoped<ISociosRepository, SociosRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

// ============================================================
// 3. JWT — DEBE IR ANTES DEL Build() ✅
// ============================================================
var claveSecreta = builder.Configuration["ApiSettings:Secreta"]; // ← mismo key que usas en el repo

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                                       Encoding.ASCII.GetBytes(claveSecreta)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

// ============================================================
// 4. CONTROLLERS + SWAGGER
// ============================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // ✅ Esto permite enviar el token JWT desde Swagger para probar endpoints protegidos
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe tu token JWT aquí"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// ============================================================
// 5. BUILD — todo el registro va ANTES de esta línea
// ============================================================
var app = builder.Build();

// ============================================================
// 6. PIPELINE — middlewares van DESPUÉS del Build()
// ============================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  // ← primero Authentication
app.UseAuthorization();   // ← luego Authorization

app.MapControllers();
app.Run();