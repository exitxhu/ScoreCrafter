using Microsoft.AspNetCore.Server.Kestrel.Core;

using ProtoBuf.Grpc.Reflection;
using ProtoBuf.Grpc.Server;
using ProtoBuf.Meta;


using ScoreCrafter.Api.gRpc.Imple;
using ScoreCrafter.Infra.InMemoryQueue;
using ScoreCrafter.Infra.Sqllite;
using ScoreCrafter.SDK.Model.gRpc.Contractc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplication()
    .AddInmemoryQueueInfrastructure()
    .AddSqlliteInfrastructure(builder.Configuration.GetConnectionString("ScoreCrafter")!);

builder.Services.AddCodeFirstGrpc();
builder.Services.AddCodeFirstGrpcReflection();

var app = builder.Build();


app.MapGrpcService<PurchaseService>();
app.MapGrpcService<UserService>();
app.MapGrpcService<FormulaService>();
app.MapGrpcService<GradeService>();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
    app.MapCodeFirstGrpcReflectionService();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
