using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Azure.Data.Tables;
using Google.Cloud.Datastore.V1;
using MongoDB.Driver;
using OCDETF.StrikeForce.Business.Library;
using OCDETF.StrikeForce.Web.API.AutoMapper;
using OCDETF.StrikeForce.Web.API.Security;




namespace OCDETF.StrikeForce.Web.API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public AppConfiguration MyAppConfiguration { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //string credential_path = @"C:\i3-Projects\StrikeForce-GC\strikeforce-aa6c2896dc7b.json";
            //System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            MyAppConfiguration = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
            services.AddSingleton(MyAppConfiguration);

            //Enable as required.
            //services.AddApplicationInsightsTelemetry(MyAppConfiguration.InstrumentationKey);

            TableServiceClient AzureTableClient = new TableServiceClient(new Uri(MyAppConfiguration.AzureStorageAccountURL), new TableSharedKeyCredential(MyAppConfiguration.AzureStorageAccountName, MyAppConfiguration.AzureStorageAccountKey));
            services.AddSingleton(AzureTableClient);

            IMongoClient mongoClient = new MongoClient(MyAppConfiguration.MongoDBConnection);
            services.AddSingleton(mongoClient);

            //string credential_path = @"C:\i3-Projects\StrikeForce-master-GC\strikeforce-aa6c2896dc7b.json";
            //System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

            DatastoreDb datastoreDb = DatastoreDb.Create(MyAppConfiguration.GCProjectId);
            services.AddSingleton(datastoreDb);

            //services.AddScoped<IUserService, MongoDBUserService>();
            //services.AddScoped<ILookupService, MongoDBLookupService>();
            //services.AddScoped<IStaffingService, MongoDBStaffingService>();
            //services.AddScoped<IQuarterReportService, MongoDBQuarterReportService>();
            //services.AddScoped<IQuarterlyActivityService, MongoDBQuarterlyActivityService>();
            //services.AddScoped<IRecentDevelopmentService, MongoDBRecentDevelopmentService>();
            //services.AddScoped<IQtrActivityAnalysisService, MongoDBAnalysisService>();

            //services.AddScoped<IUserService, AzTableUserService>();
            //services.AddScoped<ILookupService, AzTableLookupService>();
            //services.AddScoped<IStaffingService, AzTableStaffingService>();
            //services.AddScoped<IQuarterReportService, AzTableQuarterReportService>();
            //services.AddScoped<IQuarterlyActivityService, AzTableQuarterlyActivityService>();
            //services.AddScoped<IRecentDevelopmentService, AzTableRecentDevelopmentService>();
            //services.AddScoped<IQtrActivityAnalysisService, AzTableQtrActivityAnalysisService>();

            services.AddScoped<IUserService, GcpDatastoreUserService>();
            services.AddScoped<ILookupService, GcpDatastoreLookupService>();
            services.AddScoped<IStaffingService, GcpDatastoreStaffingService>();
            services.AddScoped<IQuarterReportService, GcpDatastoreQuarterReportService>();
            services.AddScoped<IQuarterlyActivityService, GcpDatastoreQuarterlyActivityService>();
            services.AddScoped<IRecentDevelopmentService, GcpDatastoreRecentDevService>();
            services.AddScoped<IQtrActivityAnalysisService, GcpDatastoreAnalysisService>();

            services.AddScoped<ILogger, Logger<Startup>>();
            services.AddScoped<IClaimsTransformation, CustomRolesClaimsTransformation>();


            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:5001").AllowAnyMethod().AllowCredentials().AllowAnyHeader();
                                  });
            });

            //Configure Authentication
            if (MyAppConfiguration.AuthenticationMethod == "JWT")
            {
                services.AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MyAppConfiguration.JwtSecret)),
                };
            });
            }
            else if (MyAppConfiguration.AuthenticationMethod == "Microsoft")
            {
                services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            }
            else if (MyAppConfiguration.AuthenticationMethod == "Okta")
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                }).AddCookie()
                .AddOpenIdConnect(
                    options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Authority = "https://i3its.okta.com/oauth2/default";
                        options.RequireHttpsMetadata = true;
                        options.ClientId = "0oa10kdss5k5mLuYr5d7";
                        options.ClientSecret = "QY9efLdaZolAP2HPqBXNzjuJeNYdLyr_dSb1XtPy";
                        options.ResponseType = OpenIdConnectResponseType.Code;
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.SaveTokens = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = "name",
                            RoleClaimType = "groups",
                            ValidateIssuer = true
                        };
                    });
            }

            services.AddAutoMapper(typeof(StrikeForceMapperConfig));


            services.AddControllers()
             .AddJsonOptions(delegate (JsonOptions opt) {
                 opt.JsonSerializerOptions.PropertyNamingPolicy = null;
             });


            //Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Strike Force API",
                    Description = "API to access Strike Force Application Data",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "OCDETF",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });


                // this code is for adding authorization input in swagger ui
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                //      Enter 'Bearer' [space] and then your token in the text input below.
                //      \r\n\r\nExample: 'Bearer 12345abcdef'",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //{
                //    new OpenApiSecurityScheme
                //    {
                //    Reference = new OpenApiReference
                //        {
                //        Type = ReferenceType.SecurityScheme,
                //        Id = "Bearer"
                //        },
                //        Scheme = "oauth2",
                //        Name = "Bearer",
                //        In = ParameterLocation.Header,

                //    },
                //    new List<string>()
                //    }
                //});


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            ////////////////////////////////////////////////////
            //services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(
              "Configuring for {Environment} environment",
              env.EnvironmentName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller}/{action=Index}/{id?}");
            //});

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see h

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
