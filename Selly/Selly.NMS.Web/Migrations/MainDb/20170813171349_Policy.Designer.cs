using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Selly.NMS.Web.Models;

namespace Selly.NMS.Web.Migrations.MainDb
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20170813171349_Policy")]
    partial class Policy
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Selly.NMS.Web.Models.Configuration", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("Selly.NMS.Web.Models.Device", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Selly.NMS.Web.Models.PacketDroppedEvent", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DeviceId");

                    b.Property<string>("FilterName");

                    b.Property<string>("LocalAddress");

                    b.Property<int>("LocalPort");

                    b.Property<string>("RemoteAddress");

                    b.Property<int>("RemotePort");

                    b.Property<DateTimeOffset>("Time");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Selly.NMS.Web.Models.Policy", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Policies");
                });

            modelBuilder.Entity("Selly.NMS.Web.Models.PolicyRule", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Action");

                    b.Property<int>("Direction");

                    b.Property<string>("LocalAddress");

                    b.Property<int>("LocalPort");

                    b.Property<string>("Name");

                    b.Property<string>("PolicyId");

                    b.Property<int>("Protocol");

                    b.Property<string>("RemoteAddress");

                    b.Property<int>("RemotePort");

                    b.HasKey("Id");

                    b.HasIndex("PolicyId");

                    b.ToTable("PolicyRules");
                });

            modelBuilder.Entity("Selly.NMS.Web.Models.PacketDroppedEvent", b =>
                {
                    b.HasOne("Selly.NMS.Web.Models.Device", "Device")
                        .WithMany("Events")
                        .HasForeignKey("DeviceId");
                });

            modelBuilder.Entity("Selly.NMS.Web.Models.PolicyRule", b =>
                {
                    b.HasOne("Selly.NMS.Web.Models.Policy", "Policy")
                        .WithMany("Rules")
                        .HasForeignKey("PolicyId");
                });
        }
    }
}
