﻿// <auto-generated />
using CryptocurrenciesInfo.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptocurrenciesInfo.Migrations
{
    [DbContext(typeof(MarketsContext))]
    [Migration("20240314164737_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CryptocurrenciesInfo.Models.DataBase.CoinGeckoMarket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Base")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Target")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Trust")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("Id");

                    b.ToTable("CoinMarket");
                });
#pragma warning restore 612, 618
        }
    }
}
