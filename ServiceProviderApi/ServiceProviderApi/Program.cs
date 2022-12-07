using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceProviderApi.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<UserDbContext>(opt => opt.UseInMemoryDatabase("DBUser"));
builder.Services.AddDbContext<UserDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("serviceDb")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();
// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//find all users
app.MapGet("/Users",  async  (UserDbContext db) =>
{
    return await db.Users.ToListAsync();
});
//find all service providers
app.MapGet("/ServiceProviders", async (UserDbContext db) =>
{
    return await db.ServiceProviders.ToListAsync();
});
app.MapGet("/ServiceProviders/Electrician/{lat}/{lon}",  (UserDbContext db, double lat, double lon) =>
{
    GoogleLocation gl = new GoogleLocation();
    gl.Lat = lat;
    gl.Long = lon;
    List<ServiceProvider> list= db.ServiceProviders.
    ToList();
    for(int i = 0; i < list.Count(); i++) 
    {
        if(list.ElementAt(i).DistanceFrom(gl)>10)
            list.RemoveAt(i);
    }
    return list;

});
app.MapGet("/ServiceProviders/Technician/{lat}/{lon}",  (UserDbContext db, double lat, double lon) =>
{
    GoogleLocation gl = new GoogleLocation();
    gl.Lat = lat;
    gl.Long = lon;
    List<ServiceProvider> list = db.ServiceProviders.
    ToList();
    for (int i = 0; i < list.Count(); i++)
    {
        if (list.ElementAt(i).DistanceFrom(gl) > 10)
            list.RemoveAt(i);
    }
    return list;
});
app.MapGet("/ServiceProviders/Plumber/{lat}/{lon}",  (UserDbContext db,double lat, double lon) =>
{
    GoogleLocation gl = new GoogleLocation();
    gl.Lat = lat;
    gl.Long = lon;
    List<ServiceProvider> list = db.ServiceProviders.
    ToList();
    for (int i = 0; i < list.Count(); i++)
    {
        if (list.ElementAt(i).DistanceFrom(gl) > 10)
            list.RemoveAt(i);
    }
    return list;
});
app.MapGet("/ServiceProviders/DeliveryMan/{lat}/{lon}", async (UserDbContext db, double lat, double lon) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.DeliveryMan).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Driver/{lat}/{lon}", async (UserDbContext db, double lat, double lon) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Driver).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Carpenter/{lat}/{lon}", async (UserDbContext db, double lat, double lon) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Carpenter).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Mechanic/{lat}/{lon}", async (UserDbContext db, double lat, double lon) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Mechanic).
    ToListAsync();
});
app.MapGet("/Users/{email}", async (UserDbContext db, string email) =>
{
    return db.Users
    .Where(u => u.Email == email).First();
});

app.MapGet("/ServiceProviders/{id}", async (UserDbContext db, int id) =>
{
    return await db.ServiceProviders.FindAsync(id) is ServiceProvider serviceprovider ? Results.Ok(serviceprovider) : Results.NotFound();
});
app.MapPost("/Add_User", async (UserDbContext db, User user) =>
{
    var userExists = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
    if(userExists!=null)
        return Results.Ok("Already Exists");
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Ok(user);
});
app.MapDelete("/Users/{id}", async (UserDbContext db, int id) =>
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/Add_ServiceProvider", async (UserDbContext db, ServiceProvider serviceprovider) =>
{
    var userExists = await db.ServiceProviders.FirstOrDefaultAsync(u => u.Email == serviceprovider.Email);
    if (userExists != null)
        await db.ServiceProviders.AddAsync(serviceprovider);
    await db.AddAsync(serviceprovider);
    await db.SaveChangesAsync();

    return Results.Ok(serviceprovider);
});
app.MapDelete("/ServiceProvider/{id}", async (UserDbContext db, int id) =>
{
    if (await db.ServiceProviders.FindAsync(id) is ServiceProvider serviceprovider)
    {
        db.ServiceProviders.Remove(serviceprovider);
        await db.SaveChangesAsync();
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});


app.MapPost("/Add_ServiceProvider/{sid}/{uid}", async (UserDbContext db, int sid,int uid) =>
{
    UserRequestsServiceR us = new UserRequestsServiceR();
    us.UserID = uid;
    us.ServiceProviderID = sid;
   db.UserRequestsServices.Add(us);
    db.SaveChanges();
    return Results.Ok("ADDED");
});
app.MapPost("/login-user",
[AllowAnonymous] (LoginUser user,UserDbContext db) =>
{
    var users =db.Users.ToList();
    bool match = false;
    int id=0;
    foreach (var temp in users)
    {
        if(temp.Email==user.UserName && temp.Password == user.Password)
        {
            match = true;
            id = temp.UserID;
            break;
        }

    }
    if (match)
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(305),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        LoginRes log = new LoginRes()
        {
            Token = stringToken,
            Id = id
         };
        return Results.Ok(stringToken);
    }
    return Results.Unauthorized();
});
app.MapPost("/login-provider",
[AllowAnonymous] (LoginUser user, UserDbContext db) =>
{
    var users = db.ServiceProviders.ToList();
    bool match = false;
    foreach (var temp in users)
    {
        if (temp.Email == user.UserName && temp.Password == user.Password)
        {
            match = true;
            break;
        }

    }
    if (match)
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
    }
    return Results.Unauthorized();
});




app.MapGet("/myself",
[AllowAnonymous]( HttpContext context) =>
{

    return Results.Ok (context.User.ToString());
});

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
//auth
app.UseAuthentication();
app.UseAuthorization();
app.Run();
public class LoginRes
{
    public string Token { get; set; }
    public int Id { get; set; }
}
public class LoginUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

//Context Class
public class UserDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserDbContext(DbContextOptions<UserDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public DbSet<User> Users { get; set; }
    public DbSet<ServiceProvider> ServiceProviders { get; set; }
    public DbSet<UserRequestsServiceR> UserRequestsServices { get; set; }
}
//Models
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Key]
    public int UserID { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

}

public class UserRequestsServiceR
{
    [Key]
    public Guid ServiceId { get; set; } = Guid.NewGuid();
    public int UserID { get; set; }

    public int ServiceProviderID { get; set; }

}

public class ServiceProvider
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Key]
    public int ServiceProviderID { get; set; }

    public string Email { get; set; }

    public string NID { get; set; }

    public string Address { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    public double Lon { get; set; }

    public double Lat { get; set; }
    public TypeofEmployees ServiceType { get; set; }
    public double DistanceFrom(GoogleLocation temp)
    {
        double lon1 = toRadians(this.Lon);
        double lon2 = toRadians(temp.Long);
        double lat1 = toRadians(this.Lat);
        double lat2 = toRadians(temp.Lat);
        double dlon = lon2 - lon1;
        double dlat = lat2 - lat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dlon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));

        // Radius of earth in
        // kilometers. Use 3956
        // for miles
        double r = 6371;

        // calculate the result
        return (c * r);

    }
    private double toRadians(
       double angleIn10thofaDegree)
    {
        // Angle in 10th
        // of a degree
        return (angleIn10thofaDegree *
                       Math.PI) / 180;
    }


}
public enum TypeofEmployees
{
    Electrician =200,
    Technician =200,
    Plumber =200,
    DeliveryMan =100,
    Driver = 1000,
    Carpenter = 200,
    Mechanic = 200,
}
public class GoogleLocation
{
    public double Long { get; set; }
    public double Lat { get; set; }
}
