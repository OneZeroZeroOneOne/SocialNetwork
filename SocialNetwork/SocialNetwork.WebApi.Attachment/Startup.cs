using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialNetwork.Bll.Abstractions;
using SocialNetwork.Bll.Services;
using SocialNetwork.ConfigSettingBll.Abstractions;
using SocialNetwork.Dal;
using SocialNetwork.Dal.Context;
using SocialNetwork.Security.Extensions;
using SocialNetwork.Security.Options;
using SocialNetwork.Utilities;
using SocialNetwork.Utilities.Controllers;
using SocialNetwork.Utilities.Middlewares;
using System.Collections.Generic;
using System.IO;
using SocialNetwork.ConfigSetting.Bll.Abstractions;
using SocialNetwork.ConfigSetting.Bll.Services;

namespace SocialNetwork.WebApi.Attachment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddControllers();
            services.AddControllers()
                .AddApplicationPart(typeof(ConfigSettingController).Assembly)
                .AddControllersAsServices();

            //services.AddAutoMapper(typeof(Startup));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var configClient = new ConfigService("uFPXCG79WkAW0mKMuJg96Q/hg9Rwi9qLkSxddIpUEIxpA");
            configClient.ForceRefresh();

            services.AddSingleton<IConfigService>(configClient);

            services.AddAuthorization(x => x.ConfigurePolicy());

            services.AddTransient<IAttachmentService, AttachmentService>();

            var pathProvider = new AttachmentPathProvider();
            pathProvider.ConfigurePath();

            services.AddSingleton<IAttachmentPathProvider>(pathProvider);

            services.AddTransient(x =>
            {
                var configService = x.GetService<IConfigService>();
                return new PublicContext(configService.GetSetting("connectionString", ""));
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPreviewGeneratorService, PreviewGeneratorService>();

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IAttachmentPathProvider attachmentPathProvider)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "log.log"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			app.UseStaticFiles();
            app.ConfigureCustomExceptionMiddleware();

            //app.UseHttpsRedirection();

            var basePath = "/api/attachment";

            app.UseCors(x => x.AllowAnyOrigin());
            app.UseCors(x => x.AllowAnyHeader());

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(attachmentPathProvider.GetPath(), "Files")),
                RequestPath = new PathString("/Files")
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
            }
            else
            {
                app.UseSwagger(c => c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                        {new OpenApiServer {Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}"}};
                }));
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "My API V1");
                c.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js");
                c.InjectJavascript("https://unpkg.com/browse/webextension-polyfill@0.6.0/dist/browser-polyfill.min.js", type: "text/html");
                c.InjectJavascript("https://gistcdn.githack.com/Forevka/dede3d7ac835e24518ec38a349140dac/raw/94d095aebdc460d42f90732be5c4ec057ac0a374/customJs.js");
            });
        }
    }
}
