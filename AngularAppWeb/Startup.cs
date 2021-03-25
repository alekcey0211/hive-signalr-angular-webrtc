using AngularAppWeb.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.HttpOverrides;

namespace AngularAppWeb {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();

      // In production, the Angular files will be served from this directory
      //services.AddSpaStaticFiles(configuration => {
      //  configuration.RootPath = "ClientApp/dist/angular-app";
      //});


      services.AddSignalR(conf => {
        conf.StreamBufferCapacity = 100000;
        conf.EnableDetailedErrors = true;
        conf.MaximumReceiveMessageSize = 99999999; // bytes
      });
      //services.AddCors(options => {
      //  options.AddPolicy("_myAllowSpecificOrigins",
      //      builder => {
      //        builder.AllowAnyOrigin()
      //                   .AllowAnyHeader()
      //                   .AllowAnyMethod();
      //      });
      //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        app.UseExceptionHandler("/Error");
        app.UseHttpsRedirection();
      }

      app.UseHttpsRedirection();

      app.UseStaticFiles();

      app.UseRouting();

      app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed((host) => true)
        .WithOrigins(
          new string[] { 
            "http://localhost:4200",
            "localhost",
            "localhost:4200",

            "127.0.0.1:10000",
            "127.0.0.1:10001",
            "127.0.0.1:10002",
            "127.0.0.1:10003",

            "localhost:10000",
            "localhost:10001",
            "localhost:10002",
            "localhost:10003"})
        );

      app.UseForwardedHeaders(new ForwardedHeadersOptions {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseEndpoints(endpoints => {
        //endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chat");
        endpoints.MapHub<WebRtcHub>("/rtc");
      });

      //app.UseSpaStaticFiles();


      //app.UseSpa(spa => {
      //  spa.Options.SourcePath = "ClientApp";

      //  if (env.IsDevelopment()) {
      //    //spa.UseAngularCliServer("start");
      //    spa.UseProxyToSpaDevelopmentServer(baseUri: "http://localhost:4200");
      //  }
      //});
    }
  }
}
