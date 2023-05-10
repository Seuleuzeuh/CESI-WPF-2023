﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CESI_WPF_2023.DAL;

#nullable disable

namespace CESI_WPF_2023.Migrations
{
    [DbContext(typeof(PokedexContext))]
    partial class PokedexContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("CESI_WPF_2023.DAL.Dresseur", b =>
                {
                    b.Property<int>("DresseurId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("DresseurId");

                    b.ToTable("Dresseurs");
                });

            modelBuilder.Entity("CESI_WPF_2023.DAL.PokemonData", b =>
                {
                    b.Property<int>("PokemonDataId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Commentaire")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("DresseurId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("PokemonDataId");

                    b.HasIndex("DresseurId");

                    b.ToTable("PokemonDatas");
                });

            modelBuilder.Entity("CESI_WPF_2023.DAL.PokemonData", b =>
                {
                    b.HasOne("CESI_WPF_2023.DAL.Dresseur", "Dresseur")
                        .WithMany("PokemonDatas")
                        .HasForeignKey("DresseurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dresseur");
                });

            modelBuilder.Entity("CESI_WPF_2023.DAL.Dresseur", b =>
                {
                    b.Navigation("PokemonDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
