﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Taka.App.Deliverer.Infra.Data.Context;


namespace Taka.App.Rentals.Infra.Data.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {           
        public AppDbContextFactory()
        {     
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseNpgsql("Host=localhost;Port=5432;Database=Rentaldb;Username=admin;Password=admin123;");
            return new AppDbContext(builder.Options);
        }
    }

}
