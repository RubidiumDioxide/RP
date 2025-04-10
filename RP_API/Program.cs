using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using System;
using System.Dynamic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.SpaServices.Extensions;
using RP_API.Structures.Tree;


var app_builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var config_builder = new ConfigurationBuilder();
config_builder.SetBasePath(Directory.GetCurrentDirectory());
config_builder.AddJsonFile("appsettings.json");
var config = config_builder.Build();


app_builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
app_builder.Services.AddEndpointsApiExplorer();
app_builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
app_builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "client"; 
});

var app = app_builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UseDeveloperExceptionPage();


var spaPath = "/app";
app.Map(new PathString(spaPath), client =>
    {
        client.UseSpaStaticFiles();
        client.UseSpa(spa =>
        {
            spa.Options.SourcePath = "client";
            spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ResponseHeaders headers = ctx.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true,
                        MustRevalidate = true
                    };
                }
            };
        });
    });




app.Run();
