using DatingApp.api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Data
{
    public class DataContext : DbContext
    {
        // this class represents the session with database
        // it use to save and store that data, 
        // we use this class to use qyuey to via inst framwork.

        public DataContext(DbContextOptions<DataContext> options ) : base (options){}
    
    public DbSet<Value> Values { get; set; }
    public DbSet<User> Users { get; set; }
    }
} 