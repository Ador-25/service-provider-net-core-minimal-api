using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(opt => opt.UseInMemoryDatabase("DBUser"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//find all users
app.MapGet("/Users", async (UserDbContext db) =>
{
    return await db.Users.ToListAsync();
});
//find all service providers
app.MapGet("/ServiceProviders", async (UserDbContext db) =>
{
    return await db.ServiceProviders.ToListAsync();
});
app.MapGet("/ServiceProviders/Electrician", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Electrician).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Technician", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Technician).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Plumber", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Plumber).
    ToListAsync();
});
app.MapGet("/ServiceProviders/DeliveryMan", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.DeliveryMan).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Driver", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Driver).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Carpenter", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Carpenter).
    ToListAsync();
});
app.MapGet("/ServiceProviders/Mechanic", async (UserDbContext db) =>
{
    return await db.ServiceProviders.
    Where(u => u.ServiceType == TypeofEmployees.Mechanic).
    ToListAsync();
});
app.MapGet("/Users/{id}", async (UserDbContext db, int id) =>
{
    return await db.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound();
});

app.MapGet("/ServiceProviders/{id}", async (UserDbContext db, int id) =>
{
    return await db.ServiceProviders.FindAsync(id) is ServiceProvider serviceprovider ? Results.Ok(serviceprovider) : Results.NotFound();
});
app.MapPost("/Add_User", async (UserDbContext db, User user) =>
{
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
    await db.ServiceProviders.AddAsync(serviceprovider);
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
app.Run();






//Context Class
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<ServiceProvider> ServiceProviders { get; set; }
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
    [Key,Column(Order =1)]
    public int UserID { get; set; }
    public User User { get; set; }

    [Key, Column(Order = 2)]
    public int ServiceProviderID { get; set; }
    public ServiceProvider ServiceProvider { get; set; }
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

