// THIS BOILERPLATE FILE IS FOR A PROJECT THAT USES THE ENTITY CORE FRAMEWORK.

/************************ Commands *************************/
// To start a new web app with MVC, create a folder, enter it. then run:
dotnet new MVC
// To restore a folder to make dependencies run:
dotnet restore
// To run a C# app:
dotnet run
// If Microsoft.DotNet.Watcher.Tools is included in your ProjectName.csproj file (will watch for file changes and recompile automatically):
<DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
dotnet watch run
// Set development environment variable for developer exception/error messages in your terminal:
export ASPNETCORE_ENVIRONMENT=Development
// Microsoft SQL Server database provider for Entity Framework Core.
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 2.1.0
// (optional) Install MySQL in your project
dotnet add package MySql.Data -v 8.0.11-*


// When using Entity Framework ORM's Code First Database Creation feature you write your models first, then make migrations, which will create a database schema with the appropriate design.
// First define the migration
dotnet ef migrations add FirstMigration
// Then push the migrations to the database where the actual creation occurs
dotnet ef database update

// After you have created a new mvc app, create a few new directories and files following this scaffolding strategy //

// (root)
// .vscode (Autogenerated. Debugger files)
// > bin (Autogenerated)
// > Controllers (Create this directory)
//   > HomeController.cs
// > Models (Classes and database context files go here)
//   > DbContext.cs
//   > User.cs 
// > obj (Autogenerated)
// > Views (Create this directory)
//   > Home (Must match the controller name from above)
//     > index.cshtml
//   > Shared (This is the last place the compiler will look for views. It is for views that are shared between multiple controllers)
// > wwwroot (autogenerated. This contains your static files (images, css, js).)
//   > css (Create this directory)
//     > style.css
//   > img (Create this directory)
//     > img1.jpg
//   > js (Create this directory)
//     > script.js
// > appsettings.json (This is where you have your DB connection string. It is read by the DbConnection file. It is kept in this file in order to maintain security.)
// > Properties (Create this directory)
//   > MySqlOptions.cs (This contains a model for SQL options. Only create this if you need to access the SQL connection string in your other project in a place other than the appsettings.json file(which connects to the DB))


// END FILE STRUCTURE //

/************************ Configuring your files*************************/
// DONT FORGET TO CHANGE NAMESPACES, DB NAMES, AND PROJECT NAMES!

// YOURNAMESPACE.csproj

<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.All" Version="2.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="2.0.1" />
  </ItemGroup>

  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Tools" Version="2.0.3" />
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />
  </ItemGroup>

</Project>


// END .csproj

// Startup.cs (contains "Using" statements and is where you enable services like session, MVC, DbContext, and other tools.):
// This is configured to connect to a PostgreSQL database.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WeddingContext>(options => options.UseNpgsql(Configuration["DBInfo:ConnectionString"]));
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}


// END Startup.cs

// appsettings.json (Is where you define your DB connection string.. Add this file to .gitignore to prevent it from being pushed to remote repo)
// This file obviously uses a PostgreSQL database. You can change that.

{
  "DBInfo": {
    "Name": "PostGresConnect",
    "ConnectionString": "server=localhost;userId=postgres;password=postgres;port=5432;database=tckr_db;"
  },
  "tools": {
    "Microsoft.EntityFrameworkCore.Tools": "1.0.0-preview2-final"
  },
  "dependencies": {
    "Microsoft.Extensions.Configuration.Json": "1.0.0",
    "Npgsql.EntityFrameworkCore.PostgreSQL": "1.0.0-*",
    "Microsoft.EntityFrameworkCore.Design": {
      "version": "1.0.0-preview2-final",
      "type": "build"
    }
  }
}

// END appsettings.json

// User.cs (User model)

using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public abstract class BaseEntity { }
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // One to Many relationship. One User can have many weddings.
        public List<Atendees> WeddingsAttending { get; set; }
        
        // Create an empty List to avoid null reference errors.
        public User()
        {
            WeddingsAttending = new List<Atendees>();
        }

    }
}

// END User.cs

// Wedding.cs (Example of another class which has a relationship with the User )

using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Weddings
    {
        public int WeddingsId { get; set; }
        public int AdminId { get; set; }
        public string Bride { get; set; }
        public string Groom { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public int NumGuests { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Atendees> Atendees { get; set; }
        public Weddings()
        {
            Atendees = new List<Atendees>();
        }
        
    }
}

// END Wedding.cs

// context.cs (This sits between your project and the database and makes the associations)

using Microsoft.EntityFrameworkCore;
 
namespace WeddingPlanner.Models
{
    public class WeddingContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<Weddings> Weddings { get; set; }
        public DbSet<Atendees> Atendees { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Atendees>().HasKey(c => new { c.UserId, c.WeddingsId});
        }
    }

}

// End context.cs

// HomeController.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
// Grants access to the PasswordHasher function
using Microsoft.AspNetCore.Identity;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{

    public class HomeController : Controller
    {
        private WeddingContext _context;

        public HomeController(WeddingContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // ViewBag.LoginError holds errors when users input incorrect credentials when attempting to login.
            ViewBag.LoginError = "";
            // ViewBag.Register holds errors when there was a DB insertion error upon registration.
            ViewBag.RegisterError = "";
            return View();
        }
    }
}


// End Controller.cs

// web.config Connection string for SQL Server

<configuration>
    <connectionStrings>
    <add name="AjaxPaginationConnectionString"
    connectionString="server =DESKTOP-7H64GG8\SQLEXPRESS; database=ajax_pagination; integrated security=True" providerName="System.Data.SqlClient"/>
    </connectionStrings>
</configuration>

// END web.config