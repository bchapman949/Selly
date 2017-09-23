using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Selly.Agent.Windows.Data;

namespace Selly.Agent.Windows.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170701100447_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Selly.Agent.Windows.Data.Config", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Configuration");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Configuration");
                });
        }
    }
}
