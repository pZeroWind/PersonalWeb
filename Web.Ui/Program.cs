using SqlSugar;
using Web.Managers;
using Web.Presenters;
using Web.Ui.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<SnowflakeIdGenerator>();

// 添加用户Session服务
//services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
});
// 指定Session保存方式:分发内存缓存
builder.Services.AddDistributedMemoryCache();

var conn = $"Data Source= {Path.Combine(AppContext.BaseDirectory, builder.Configuration.GetConnectionString("FileName") ?? string.Empty)}";

builder.Services.AddTransient(op => new ConnectionConfig
{
    ConnectionString = conn,
    DbType = DbType.Sqlite,
    IsAutoCloseConnection = true,
    InitKeyType = InitKeyType.Attribute
});
var aboutVModel = builder.Configuration.GetSection("About").Get<AboutVModel>();
if (aboutVModel != null)
{
    builder.Services.AddSingleton(aboutVModel);
}

builder.Services.AddTransient<SqliteManager>();

builder.Services.AddScoped<BlogModel>();
builder.Services.AddScoped<ProjectModel>();
builder.Services.AddScoped<SkillModel>();
builder.Services.AddScoped<AdminModel>();

builder.Services.AddScoped<BlogPresenter>();
builder.Services.AddScoped<BlogInfoPresenter>();
builder.Services.AddScoped<AdminPresenter>();
builder.Services.AddScoped<ProjectPresenter>();
builder.Services.AddScoped<SkillPresenter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
