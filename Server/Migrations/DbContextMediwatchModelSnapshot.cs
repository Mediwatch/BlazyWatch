﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server;

namespace Mediwatch.Server.Migrations
{
    [DbContext(typeof(DbContextMediwatch))]
    partial class DbContextMediwatchModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7");

            modelBuilder.Entity("BlazingArticle.Model.BlazingArticleModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreviewImageURL")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreviewParagraph")
                        .HasColumnType("TEXT");

                    b.Property<string>("PreviewTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("_tags")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("BlazingArticles");
                });

            modelBuilder.Entity("Mediwatch.Shared.Models.applicant_session", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("confirmed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("TEXT");

                    b.Property<float>("idFormation")
                        .HasColumnType("REAL");

                    b.Property<Guid>("idUser")
                        .HasColumnType("TEXT");

                    b.Property<bool>("payed")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("Applicant_session");
                });

            modelBuilder.Entity("Mediwatch.Shared.Models.compagny", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("compagnyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("countryCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Compagny");
                });

            modelBuilder.Entity("Mediwatch.Shared.Models.formation", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Contact")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Former")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Target")
                        .HasColumnType("TEXT");

                    b.Property<int?>("compagnyid")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("compagnyid");

                    b.ToTable("Formation");
                });

            modelBuilder.Entity("Mediwatch.Shared.Models.orderInfo", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("billingAdress")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("currency")
                        .HasColumnType("TEXT");

                    b.Property<string>("formationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("invoiceId")
                        .HasColumnType("TEXT");

                    b.Property<float>("price")
                        .HasColumnType("REAL");

                    b.Property<string>("userId")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("OrderInfo");
                });

            modelBuilder.Entity("Mediwatch.Shared.Models.tag", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("description")
                        .HasColumnType("TEXT");

                    b.Property<string>("tag_name")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Mediwatch.Shared.Models.formation", b =>
                {
                    b.HasOne("Mediwatch.Shared.Models.compagny", null)
                        .WithMany("compagnyFormation")
                        .HasForeignKey("compagnyid");
                });
#pragma warning restore 612, 618
        }
    }
}
