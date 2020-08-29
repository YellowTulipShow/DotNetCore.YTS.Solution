using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YTS.Shop.Models;

namespace YTS.Shop
{
    public class YTSEntityContext : DbContext
    {
        public YTSEntityContext(DbContextOptions<YTSEntityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().ToTable("Administrator");
            modelBuilder.Entity<ManagerGroup>().ToTable("ManagerGroup");
            modelBuilder.Entity<Managers>().ToTable("Managers");
            modelBuilder.Entity<SystemSetType>().ToTable("SystemSetType");
            modelBuilder.Entity<Menus>().ToTable("Menus");

            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ProductDamagedRecord>().ToTable("ProductDamagedRecord");
            modelBuilder.Entity<ProductEnterWarehouseRecord>().ToTable("ProductEnterWarehouseRecord");
            modelBuilder.Entity<ProductNumberRecord>().ToTable("ProductNumberRecord");
            modelBuilder.Entity<ShopInfo>().ToTable("ShopInfo");
            modelBuilder.Entity<UserExpensesRecord>().ToTable("UserExpensesRecord");
            modelBuilder.Entity<UserGroup>().ToTable("UserGroup");
            modelBuilder.Entity<UserMoneyRecord>().ToTable("UserMoneyRecord");
            modelBuilder.Entity<UserRechargeRecord>().ToTable("UserRechargeRecord");
            modelBuilder.Entity<UserRechargeSet>().ToTable("UserRechargeSet");
            modelBuilder.Entity<UserRefundMoneyRecord>().ToTable("UserRefundMoneyRecord");
            modelBuilder.Entity<UserReturnGoodsRecord>().ToTable("UserReturnGoodsRecord");
            modelBuilder.Entity<Users>().ToTable("Users");
        }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<ManagerGroup> ManagerGroup { get; set; }
        public DbSet<Managers> Managers { get; set; }
        public DbSet<SystemSetType> SystemSetType { get; set; }
        public DbSet<Menus> Menus { get; set; }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductDamagedRecord> ProductDamagedRecord { get; set; }
        public DbSet<ProductEnterWarehouseRecord> ProductEnterWarehouseRecord { get; set; }
        public DbSet<ProductNumberRecord> ProductNumberRecord { get; set; }
        public DbSet<ShopInfo> ShopInfo { get; set; }
        public DbSet<UserExpensesRecord> UserExpensesRecord { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<UserMoneyRecord> UserMoneyRecord { get; set; }
        public DbSet<UserRechargeRecord> UserRechargeRecord { get; set; }
        public DbSet<UserRechargeSet> UserRechargeSet { get; set; }
        public DbSet<UserRefundMoneyRecord> UserRefundMoneyRecord { get; set; }
        public DbSet<UserReturnGoodsRecord> UserReturnGoodsRecord { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
