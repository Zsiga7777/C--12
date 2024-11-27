var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<ICodeWriter, CodeGenerator>();
//builder.Services.AddScoped<ICodeWriter, CodeGenerator>();
builder.Services.AddTransient<ICodeWriter, CodeGenerator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
	x.UseInlineDefinitionsForEnums();
	x.SwaggerDoc("v1", new() { Title = $"DI sample app", Version = "v1", Description = "Endpoints for DI sample app" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/getCodeFromSingleton", (ICodeWriter singletonCodeGeneratorService) => singletonCodeGeneratorService.GetCode());

app.MapGet("/getCodeFromScoped", (ICodeWriter scopedCodeGeneratorService1, ICodeWriter scopedCodeGeneratorService2) =>
{
	var code1 = scopedCodeGeneratorService1.GetCode();
	var code2 = scopedCodeGeneratorService2.GetCode();

	return $"{code1}\n{code2}";
});

app.MapGet("/getCodeFromTransient", (ICodeWriter transientCodeGeneratorService1, ICodeWriter transientCodeGeneratorService2) =>
{
	var code1 = transientCodeGeneratorService1.GetCode();
	var code2 = transientCodeGeneratorService2.GetCode();

	return $"{code1}\n{code2}";
});

app.Run();
