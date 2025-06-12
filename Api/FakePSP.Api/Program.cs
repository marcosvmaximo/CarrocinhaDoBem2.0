var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddHttpClient(); // Necessário para enviar o webhook

// Configuração do CORS (Cross-Origin Resource Sharing)
// Permite que a sua API principal chame esta API.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin() // Em produção, seja mais restritivo.
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita o HTTPS Redirection e o CORS
app.UseHttpsRedirection();
app.UseCors(); // MUITO IMPORTANTE: Adicione esta linha

app.UseAuthorization();

app.MapControllers();

app.Run();