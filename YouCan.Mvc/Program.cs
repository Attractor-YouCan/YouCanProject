using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using YouCan.Mvc.Services;
using YouCan.Mvc.Services.Email;
using YouCan.Repository;
using YouCan.Service;
using YouCan.Services;
using YouCan.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<YouCanContext>(options => options.UseNpgsql(connection, x => x.MigrationsAssembly("YouCan.Repository")))
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
builder.Services.AddTransient<IUserCrud, UserCrud>();

builder.Services.AddTransient<IRepository<Answer>, AnswerRepository>();
builder.Services.AddTransient<ICrudService<Answer>, CrudService<Answer>>();

builder.Services.AddTransient<IRepository<Lesson>, LessonRepository>();
builder.Services.AddTransient<ICrudService<Lesson>, CrudService<Lesson>>();

builder.Services.AddTransient<IRepository<LessonModule>, LessonModuleRepository>();
builder.Services.AddTransient<ICrudService<LessonModule>, CrudService<LessonModule>>();

builder.Services.AddTransient<IRepository<OrtTest>, OrtTestRepository>();
builder.Services.AddTransient<ICrudService<OrtTest>, CrudService<OrtTest>>();

builder.Services.AddTransient<IRepository<Question>, QuestionRepository>();
builder.Services.AddTransient<ICrudService<Question>, CrudService<Question>>();

builder.Services.AddTransient<IRepository<Statistic>, Repository<Statistic>>();
builder.Services.AddTransient<ICrudService<Statistic>, CrudService<Statistic>>();

builder.Services.AddTransient<IRepository<Test>, TestRepository>();
builder.Services.AddTransient<ICrudService<Test>, CrudService<Test>>();

builder.Services.AddTransient<IRepository<UserLessons>, UserLessonsRepository>();
builder.Services.AddTransient<ICrudService<UserLessons>, CrudService<UserLessons>>();

builder.Services.AddTransient<IRepository<UserLevel>, Repository<UserLevel>>();
builder.Services.AddTransient<ICrudService<UserLevel>, CrudService<UserLevel>>();

builder.Services.AddTransient<IRepository<UserOrtTest>, UserOrtTestRepository>();
builder.Services.AddTransient<ICrudService<UserOrtTest>, CrudService<UserOrtTest>>();

builder.Services.AddTransient<IRepository<OrtInstruction>, Repository<OrtInstruction>>();
builder.Services.AddTransient<ICrudService<OrtInstruction>, CrudService<OrtInstruction>>();

builder.Services.AddTransient<IRepository<PassedQuestion>, PassedQuestionsRepository>();
builder.Services.AddTransient<ICrudService<PassedQuestion>, CrudService<PassedQuestion>>();

builder.Services.AddTransient<IRepository<Subject>, SubjectRepository>();
builder.Services.AddTransient<ICrudService<Subject>, CrudService<Subject>>();

builder.Services.AddTransient<IRepository<QuestionReport>, QuestionReportRepository>();
builder.Services.AddTransient<ICrudService<QuestionReport>, CrudService<QuestionReport>>();

builder.Services.AddTransient<IRepository<Tariff>, TariffRepository>();
builder.Services.AddTransient<ICrudService<Tariff>, CrudService<Tariff>>();

builder.Services.AddTransient<IRepository<League>, LeagueRepository>();
builder.Services.AddTransient<ICrudService<League>, CrudService<League>>();

builder.Services.AddTransient<IRepository<AdminAction>, AdminActionRepository>();
builder.Services.AddTransient<ICrudService<AdminAction>, CrudService<AdminAction>>();

builder.Services.AddTransient<IRepository<LessonTime>, LessonTimeRepository>();
builder.Services.AddTransient<ICrudService<LessonTime>, CrudService<LessonTime>>();

builder.Services.AddTransient<IRepository<UserExperience>, UserExperienceRepository>();
builder.Services.AddTransient<ICrudService<UserExperience>, CrudService<UserExperience>>();

builder.Services.AddTransient<IRepository<Announcement>, AnnouncementRepository>();
builder.Services.AddTransient<ICRUDService<Announcement>, CRUDService<Announcement>>();

builder.Services.AddScoped<TwoFactorService>();

//After all the dependencies
builder.Services.AddRazorTemplating();

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

var supportedCultures = new[]
{
    new CultureInfo("ru"),
    new CultureInfo("ky")
};

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture("ru"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new UserProfileRequestCultureProvider(builder.Services.BuildServiceProvider().GetService<UserManager<User>>()),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    }
});

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