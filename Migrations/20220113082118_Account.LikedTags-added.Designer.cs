﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PicturesAPI.Entities;

namespace PicturesAPI.Migrations
{
    [DbContext(typeof(PictureDbContext))]
    [Migration("20220113082118_Account.LikedTags-added")]
    partial class AccountLikedTagsadded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PicturesAPI.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AccountCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2022, 1, 13, 9, 21, 17, 691, DateTimeKind.Local).AddTicks(6445));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("LikedTags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Dislike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid?>("DisLikedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DisLikerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DisLikedId");

                    b.HasIndex("DisLikerId");

                    b.ToTable("Dislikes");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid?>("LikedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("LikerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LikedId");

                    b.HasIndex("LikerId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("PictureAdded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2022, 1, 13, 9, 21, 17, 685, DateTimeKind.Local).AddTicks(9943));

                    b.Property<string>("Tags")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Picture web URL");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Dislike", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Picture", "DisLiked")
                        .WithMany("Dislikes")
                        .HasForeignKey("DisLikedId");

                    b.HasOne("PicturesAPI.Entities.Account", "DisLiker")
                        .WithMany("Dislikes")
                        .HasForeignKey("DisLikerId");

                    b.Navigation("DisLiked");

                    b.Navigation("DisLiker");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Like", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Picture", "Liked")
                        .WithMany("Likes")
                        .HasForeignKey("LikedId");

                    b.HasOne("PicturesAPI.Entities.Account", "Liker")
                        .WithMany("Likes")
                        .HasForeignKey("LikerId");

                    b.Navigation("Liked");

                    b.Navigation("Liker");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Picture", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Account", "Account")
                        .WithMany("Pictures")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Account", b =>
                {
                    b.Navigation("Dislikes");

                    b.Navigation("Likes");

                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Picture", b =>
                {
                    b.Navigation("Dislikes");

                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}
