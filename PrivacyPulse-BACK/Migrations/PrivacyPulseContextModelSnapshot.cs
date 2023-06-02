﻿// <auto-generated />
using System;
using CampingAPI.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace PrivacyPulse_BACK.Migrations
{
    [DbContext(typeof(PrivacyPulseContext))]
    partial class PrivacyPulseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.FriendRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FromUserId")
                        .HasColumnType("int");

                    b.Property<int>("ToUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<int?>("FromUserId")
                        .HasColumnType("int");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("FromUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.MessageContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CipherText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ForUserId")
                        .HasColumnType("int");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ForUserId");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageContents");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EncryptedPrivateKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.UserChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("UserChats");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.UserFriend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FriendUserId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FriendUserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.FriendRequest", b =>
                {
                    b.HasOne("PrivacyPulse_BACK.Entities.User", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrivacyPulse_BACK.Entities.User", "ToUser")
                        .WithMany()
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.Message", b =>
                {
                    b.HasOne("PrivacyPulse_BACK.Entities.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrivacyPulse_BACK.Entities.User", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserId");

                    b.Navigation("Chat");

                    b.Navigation("FromUser");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.MessageContent", b =>
                {
                    b.HasOne("PrivacyPulse_BACK.Entities.User", "ForUser")
                        .WithMany()
                        .HasForeignKey("ForUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrivacyPulse_BACK.Entities.Message", "Message")
                        .WithMany("MessageContents")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ForUser");

                    b.Navigation("Message");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.UserChat", b =>
                {
                    b.HasOne("PrivacyPulse_BACK.Entities.Chat", "Chat")
                        .WithMany("UserChats")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrivacyPulse_BACK.Entities.User", "User")
                        .WithMany("UserChats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.UserFriend", b =>
                {
                    b.HasOne("PrivacyPulse_BACK.Entities.User", "FriendUser")
                        .WithMany()
                        .HasForeignKey("FriendUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PrivacyPulse_BACK.Entities.User", "User")
                        .WithMany("Friends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FriendUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.Chat", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UserChats");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.Message", b =>
                {
                    b.Navigation("MessageContents");
                });

            modelBuilder.Entity("PrivacyPulse_BACK.Entities.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("UserChats");
                });
#pragma warning restore 612, 618
        }
    }
}
