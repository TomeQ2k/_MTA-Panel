using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MTA.API.AppConfigs
{
    public static class SwaggerAppConfig
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
            => services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo {Title = "MTA.API", Version = "v1"});

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlDocumentationFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlDocumentationPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile);

                swagger.IncludeXmlComments(xmlDocumentationPath, true);
            });
    }
}