﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using alkemyapi.Data;

namespace alkemyapi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221008155032_EditTable")]
    partial class EditTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PeliculaSeriePersonaje", b =>
                {
                    b.Property<int>("PeliculaSeriesId")
                        .HasColumnType("int");

                    b.Property<int>("PersonajesId")
                        .HasColumnType("int");

                    b.HasKey("PeliculaSeriesId", "PersonajesId");

                    b.HasIndex("PersonajesId");

                    b.ToTable("PeliculaSeriePersonaje");
                });

            modelBuilder.Entity("alkemyapi.Models.Calificacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CalificationNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Calificaciones");
                });

            modelBuilder.Entity("alkemyapi.Models.Genero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Imagen")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nombre")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PeliculaSerieId")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("alkemyapi.Models.PeliculaSerie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CalificacionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2(7)");

                    b.Property<int?>("GeneroId")
                        .HasColumnType("int");

                    b.Property<string>("Imagen")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("PersonajeId")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CalificacionId");

                    b.HasIndex("GeneroId");

                    b.ToTable("PeliculaSeries");
                });

            modelBuilder.Entity("alkemyapi.Models.Personaje", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Edad")
                        .HasColumnType("int");

                    b.Property<string>("Historia")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Imagen")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nombre")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("PeliculaSerieId")
                        .HasColumnType("int");

                    b.Property<int?>("Peso")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Personajes");
                });

            modelBuilder.Entity("alkemyapi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PeliculaSeriePersonaje", b =>
                {
                    b.HasOne("alkemyapi.Models.PeliculaSerie", null)
                        .WithMany()
                        .HasForeignKey("PeliculaSeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("alkemyapi.Models.Personaje", null)
                        .WithMany()
                        .HasForeignKey("PersonajesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("alkemyapi.Models.PeliculaSerie", b =>
                {
                    b.HasOne("alkemyapi.Models.Calificacion", "Calificaciones")
                        .WithMany("PeliculaSeries")
                        .HasForeignKey("CalificacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("alkemyapi.Models.Genero", null)
                        .WithMany("PeliculaSeries")
                        .HasForeignKey("GeneroId");

                    b.Navigation("Calificaciones");
                });

            modelBuilder.Entity("alkemyapi.Models.Calificacion", b =>
                {
                    b.Navigation("PeliculaSeries");
                });

            modelBuilder.Entity("alkemyapi.Models.Genero", b =>
                {
                    b.Navigation("PeliculaSeries");
                });
#pragma warning restore 612, 618
        }
    }
}
