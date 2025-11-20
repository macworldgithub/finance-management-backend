using MongoDB.Driver;
using finance_management_backend.Settings;
using finance_management_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Add controllers (for API endpoints)
builder.Services.AddControllers();

// 2) OpenAPI / Swagger (from your template)
// builder.Services.AddOpenApi();


// 🔹 Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3) Bind MongoDbSettings from appsettings.json
var mongoSettings = builder.Configuration
    .GetSection("MongoDbSettings")
    .Get<MongoDbSettings>() ?? new MongoDbSettings();

// 4) Register MongoClient (one per application)
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    return new MongoClient(mongoSettings.ConnectionString);
});

// 5) Register IMongoDatabase (one DB, many collections)
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    Console.WriteLine("✅ Connected to MongoDB at: " + mongoSettings.ConnectionString);
    return client.GetDatabase(mongoSettings.DatabaseName);
});

// 6) Register your own services (we'll create TransactionService next)
// builder.Services.AddSingleton<TransactionService>();
builder.Services.AddSingleton<ProcessService>();
builder.Services.AddSingleton<OwnershipService>(); 
builder.Services.AddSingleton<CosoControlEnvironmentService>();
builder.Services.AddSingleton<IntosaiIfacControlEnvironmentService>();
builder.Services.AddSingleton<OtherControlEnvironmentService>();
builder.Services.AddSingleton<RiskAssessmentInherentRiskService>();
builder.Services.AddSingleton<RiskResponseService>();
builder.Services.AddSingleton<ControlActivityService>();
builder.Services.AddSingleton<ControlAssessmentService>();
builder.Services.AddSingleton<RiskAssessmentResidualRiskService>();
builder.Services.AddSingleton<SoxService>();
builder.Services.AddSingleton<FinancialStatementAssertionService>();
builder.Services.AddSingleton<InternalAuditTestService>();
builder.Services.AddSingleton<GrcExceptionLogService>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    // You could even do a ping here if you want
}

// Log DB name so you can see it
Console.WriteLine("MongoDB Database Name: " + mongoSettings.DatabaseName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controller routes like /api/transactions
app.MapControllers();

app.Run();
