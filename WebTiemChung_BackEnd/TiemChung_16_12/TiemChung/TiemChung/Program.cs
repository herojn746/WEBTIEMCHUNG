using TiemChung;
using TiemChung.Repository;
using TiemChung.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Configuration;
using TiemChung.Model;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddDistributedMemoryCache();
//==================================================

ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<AppSetting>(configuration.GetSection("AppSettings"));
var secretKey = configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opt =>
               {
                   opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       //tự cấp token
                       ValidateIssuer = false,
                       ValidateAudience = false,

                       //ký vào token
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                       ClockSkew = TimeSpan.Zero
                   };
               });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder => builder.WithOrigins("*")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
//==================================================

builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ILoaiTiemChungRepository, LoaiTiemChungRepository>();
builder.Services.AddScoped<IThongTinTiemChungRepository, ThongTinTiemChungRepository>();
builder.Services.AddScoped<IGoiTiemChungRepository, GoiTiemChungRepository>();
builder.Services.AddScoped<IVaccineRepository, VaccineRepository>();
builder.Services.AddScoped<ILoaiVaccineRepository, LoaiVaccineRepository>();
builder.Services.AddScoped<INhaCungCapRepository, NhaCungCapRepository>();
builder.Services.AddScoped<INhapVaccineRepository, NhapVaccineRepository>();
builder.Services.AddScoped<ICTNhapVaccineRepository, CTNhapVaccineRepository>();
builder.Services.AddScoped<ICTVaccineRepository, CTVaccineRepository>();
builder.Services.AddScoped<IXuatVaccineRepository, XuatVaccineRepository>();
builder.Services.AddScoped<ICTXuatVaccineRepository, CTXuatVaccineRepository>();
builder.Services.AddScoped<ICTGoiTiemChungRepository, CTGoiTiemChungRepository>();
builder.Services.AddScoped<IHoGiaDinhRepository, HoGiaDinhRepository>();
//====================================================================================

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowLocalhost3000");
app.MapControllers();

app.Run();
