﻿using Microsoft.EntityFrameworkCore;
using WebLab.Entities;

namespace WebLabRest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
    }
}