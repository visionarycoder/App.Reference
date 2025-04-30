using Microsoft.AspNetCore.Identity.UI.Services;
using Praetor.Rules;
using Praetor.Rules.Abstractions;
using Praetor.Rules.Sql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEmailSender, AcsEmailSender>();
builder.Services.AddSingleton<IRuleStore>(_ => new SqlRuleStore(builder.Configuration.GetConnectionString("Sql")!))
  .AddMemoryCache()
  .AddSingleton<RulesEngineFactory>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
