using RestSharp;
using SimpleInjector;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    
builder.Services.AddControllers();

var container = new Container();
container.Options.ResolveUnregisteredConcreteTypes = true;// when we dont regester classes and use them in constrctor simple injector automatically registers them for us  
container.Options.EnableAutoVerification = false;
container.Options.SuppressLifestyleMismatchVerification = true;

builder.Services.AddSimpleInjector(container, options =>
{
    options.AddAspNetCore()
           .AddControllerActivation();
});

container.Register<IIntegrationService, IntegrationService>(Lifestyle.Scoped);
container.RegisterSingleton<RestClient>(() => new RestClient());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
