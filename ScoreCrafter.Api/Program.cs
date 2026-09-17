using ProtoBuf.Grpc.Server;

using ScoreCrafter.Infra.Sqllite;
using ScoreCrafter.Api.gRpc.Imple;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration.GetConnectionString("ScoreCrafter")!);

builder.Services.AddCodeFirstGrpc();

var app = builder.Build();

app.MapGrpcService<PurchaseService>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
