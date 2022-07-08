using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.ExternalLogin;

public class ApplicationDataContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDataContext(DbContextOptions options) : base(options)
    { }
}