using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Mvc;
using YouCan.Repository;
using YouCan.Repository.Repository;
using YouCan.Service.Service;
using YouCan.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем поддержку конфигурации Steeltoe для работы с переменными окружения
builder.AddCloudFoundryConfiguration();

// Добавление конфигураций из appsettings.json и переменных окружения
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();  // Загружаем переменные окружения

// Чтение переменных окружения через Steeltoe
var dbUser = builder.Configuration["DB_USER"];
var dbPassword = builder.Configuration["DB_PASSWORD"];

// Проверяем, что переменные окружения правильно загружены
Console.WriteLine($"CHECK IN PROGRAM: DB_USER: {dbUser}");
Console.WriteLine($"CHECK IN PROGRAM: DB_PASSWORD: {dbPassword}");

// Установка строки подключения с переменными окружения
string connection = $"Server=db;Port=5432;Database=YouCan;User Id={dbUser};Password={dbPassword};";
builder.Configuration["ConnectionStrings:DefaultConnection"] = connection;

// Проверка строки подключения
Console.WriteLine($"Connection String: {connection}");

// Добавление сервисов
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Настройка DbContext и Identity
builder.Services.AddDbContext<YouCanContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("YouCan.Repository")))
    .AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireDigit = true;
    })
    .AddEntityFrameworkStores<YouCanContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<W3RootFileManager>();
builder.Services.AddScoped<LeagueRepository>();

builder.Services.AddHostedService<TariffCheckService>();

builder.Services.AddTransient<IRepository<User>, UserRepository<User>>();
builder.Services.AddTransient<IUserCRUD, UserCRUD>();

builder.Services.AddTransient<IRepository<Answer>, AnswerRepository>();
builder.Services.AddTransient<ICRUDService<Answer>, CRUDService<Answer>>();

builder.Services.AddTransient<IRepository<Lesson>, LessonRepository>();
builder.Services.AddTransient<ICRUDService<Lesson>, CRUDService<Lesson>>();

builder.Services.AddTransient<IRepository<LessonModule>, LessonModuleRepository>();
builder.Services.AddTransient<ICRUDService<LessonModule>, CRUDService<LessonModule>>();

builder.Services.AddTransient<IRepository<OrtTest>, OrtTestRepository>();
builder.Services.AddTransient<ICRUDService<OrtTest>, CRUDService<OrtTest>>();

builder.Services.AddTransient<IRepository<Question>, QuestionRepository>();
builder.Services.AddTransient<ICRUDService<Question>, CRUDService<Question>>();

builder.Services.AddTransient<IRepository<Statistic>, Repository<Statistic>>();
builder.Services.AddTransient<ICRUDService<Statistic>, CRUDService<Statistic>>();

builder.Services.AddTransient<IRepository<Test>, TestRepository>();
builder.Services.AddTransient<ICRUDService<Test>, CRUDService<Test>>();

builder.Services.AddTransient<IRepository<UserLessons>, UserLessonsRepository>();
builder.Services.AddTransient<ICRUDService<UserLessons>, CRUDService<UserLessons>>();

builder.Services.AddTransient<IRepository<UserLevel>, Repository<UserLevel>>();
builder.Services.AddTransient<ICRUDService<UserLevel>, CRUDService<UserLevel>>();

builder.Services.AddTransient<IRepository<UserOrtTest>, UserOrtTestRepository>();
builder.Services.AddTransient<ICRUDService<UserOrtTest>, CRUDService<UserOrtTest>>();

builder.Services.AddTransient<IRepository<OrtInstruction>, Repository<OrtInstruction>>();
builder.Services.AddTransient<ICRUDService<OrtInstruction>, CRUDService<OrtInstruction>>();

builder.Services.AddTransient<IRepository<PassedQuestion>, PassedQuestionsRepository>();
builder.Services.AddTransient<ICRUDService<PassedQuestion>, CRUDService<PassedQuestion>>();

builder.Services.AddTransient<IRepository<Subject>, SubjectRepository>();
builder.Services.AddTransient<ICRUDService<Subject>, CRUDService<Subject>>();

builder.Services.AddTransient<IRepository<QuestionReport>, QuestionReportRepository>();
builder.Services.AddTransient<ICRUDService<QuestionReport>, CRUDService<QuestionReport>>();

builder.Services.AddTransient<IRepository<Tariff>, TariffRepository>();
builder.Services.AddTransient<ICRUDService<Tariff>, CRUDService<Tariff>>();

builder.Services.AddTransient<IRepository<League>, LeagueRepository>();
builder.Services.AddTransient<ICRUDService<League>, CRUDService<League>>();

builder.Services.AddTransient<IRepository<AdminAction>, AdminActionRepository>();
builder.Services.AddTransient<ICRUDService<AdminAction>, CRUDService<AdminAction>>();

builder.Services.AddTransient<IRepository<LessonTime>, LessonTimeRepository>();
builder.Services.AddTransient<ICRUDService<LessonTime>, CRUDService<LessonTime>>();

builder.Services.AddScoped<TwoFactorService>();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var userManager = services.GetRequiredService<UserManager<User>>();
    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

    await AdminInitializer.SeedAdminUser(rolesManager, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();