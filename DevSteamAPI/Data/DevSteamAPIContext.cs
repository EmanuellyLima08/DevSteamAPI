﻿using DevSteamAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSteamAPI.Data
{
    public class DevSteamAPIContext : IdentityDbContext
    {
        //Método Construtor
        public DevSteamAPIContext(DbContextOptions<DevSteamAPIContext> options) : base(options)
        {
        }

        //Definir as tabelas do banco de dados
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        //Sobrescever o método OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Jogo>().ToTable("Jogos");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
        }
        public DbSet<DevSteamAPI.Models.Carrinho> Carrinho { get; set; } = default!;
        public DbSet<DevSteamAPI.Models.ItemCarrinho> ItemCarrinho { get; set; } = default!;
    }
}
