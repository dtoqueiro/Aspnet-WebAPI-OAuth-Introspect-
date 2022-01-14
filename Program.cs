using IdentityModel.AspNetCore.OAuth2Introspection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authentication With Intrpospect

builder.Services.AddAuthentication(OAuth2IntrospectionDefaults.AuthenticationScheme)
    .AddOAuth2Introspection(options =>
    {
        options.IntrospectionEndpoint = "http://localhost:8080/auth/realms/aprime/protocol/openid-connect/token/introspect";

        options.ClientId = "teste";
        options.ClientSecret = "tzMhwuhPbVIlAocHDYwggtZoF3CUHlu7";

        // We are using introspection for JWTs so we need to override the default
        options.SkipTokensWithDots = false;
    });


// Authentication Without Introspect
// -------------------------------------------------------------------------------------
// builder.Services.AddAuthentication(x =>
// {
//     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(x =>
// {
//     x.MetadataAddress = "http://localhost:8080/auth/realms/aprime/.well-known/openid-configuration";
//     x.RequireHttpsMetadata = false; // only for dev
//     x.SaveToken = true;
//     x.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         // NOTE: Usually you don't need to set the issuer since the middleware will extract it 
//         // from the .well-known endpoint provided above. but since I am using the container name in
//         // the above URL which is not what is published issueer by the well-known, I'm setting it here.
//         ValidIssuer = "http://localhost:8080/auth/realms/aprime",

//         ValidAudience = "account",
//         RequireSignedTokens = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ClockSkew = TimeSpan.FromMinutes(1)
//     };
// });
// -------------------------------------------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
