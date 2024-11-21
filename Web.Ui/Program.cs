using SqlSugar;
using Web.Managers;
using Web.Presenters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<SnowflakeIdGenerator>();

var conn = $"Data Source= {Path.Combine(AppContext.BaseDirectory, builder.Configuration.GetConnectionString("FileName") ?? string.Empty)}";
Console.WriteLine(conn);
builder.Services.AddTransient(op => new ConnectionConfig
{
    ConnectionString = conn,
    DbType = DbType.Sqlite,
    IsAutoCloseConnection = true,
    InitKeyType = InitKeyType.Attribute
});

builder.Services.AddTransient<SqliteManager>();

builder.Services.AddScoped<BlogModel>();

builder.Services.AddScoped<BlogPresenter>();
builder.Services.AddScoped<BlogInfoPresenter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
