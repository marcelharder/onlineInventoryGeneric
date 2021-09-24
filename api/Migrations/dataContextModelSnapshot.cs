﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.DAL;

namespace api.Migrations
{
    [DbContext(typeof(dataContext))]
    partial class dataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("api.DAL.models.Class_Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<int?>("LocationId1")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId1");

                    b.ToTable("Class_Item");
                });

            modelBuilder.Entity("api.DAL.models.Class_Locations", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Adres")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Contact")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Contact_image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DBBackend")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Fax")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Logo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Naam")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PostalCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RefHospitals")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SMS_mobile_number")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SMS_send_time")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("StandardRef")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Telephone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("mrnSample")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("rp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("triggerOneMonth")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("triggerThreeMonth")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("triggerTwoMonth")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("api.DAL.models.Class_Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Expiry_date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Implant_date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Implant_position")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Location")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Location_code")
                        .HasColumnType("int");

                    b.Property<DateTime>("Manufac_date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Model_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.Property<int>("Procedure_id")
                        .HasColumnType("int");

                    b.Property<string>("Product_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Serial_no")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Size")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<double>("TFD")
                        .HasColumnType("double");

                    b.Property<string>("Type")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Vendor_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("implanted")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("api.DAL.models.Class_ProductType", b =>
                {
                    b.Property<int>("ValveTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Implant_position")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Model_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Vendor_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Vendor_description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("countries")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("uk_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("us_code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("ValveTypeId");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("api.DAL.models.Class_Product_Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("EOA")
                        .HasColumnType("float");

                    b.Property<int?>("PTValveTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.Property<int>("ValveTypeId")
                        .HasColumnType("int");

                    b.HasKey("SizeId");

                    b.HasIndex("PTValveTypeId");

                    b.ToTable("Valve_sizes");
                });

            modelBuilder.Entity("api.DAL.models.Class_Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ArrivalCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("DepTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DepartureCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("api.DAL.models.Class_Vendors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("No")
                        .HasColumnType("int");

                    b.Property<string>("active")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("contact")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("database_no")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("fax")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("reps")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("spare2")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("spare4")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("telephone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("api.DAL.models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("MessageSent")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("RecipientDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<bool>("SenderDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("api.DAL.models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsMain")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PublicId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Url")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("api.DAL.models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("City")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DatabaseRole")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("IBAN")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Interests")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Introduction")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("KnownAs")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LookingFor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Mobile")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Role")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("hospital_id")
                        .HasColumnType("int");

                    b.Property<string>("worked_in")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("api.DAL.models.reorder_policy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("contact")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("mobile")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("rule")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("tel")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ReorderPolicy");
                });

            modelBuilder.Entity("api.DAL.models.rep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("vendor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Reps");
                });

            modelBuilder.Entity("api.DAL.models.Class_Item", b =>
                {
                    b.HasOne("api.DAL.models.Class_Locations", "loc")
                        .WithMany("vendors")
                        .HasForeignKey("LocationId1");
                });

            modelBuilder.Entity("api.DAL.models.Class_Product_Size", b =>
                {
                    b.HasOne("api.DAL.models.Class_ProductType", "PT")
                        .WithMany("Product_size")
                        .HasForeignKey("PTValveTypeId");
                });

            modelBuilder.Entity("api.DAL.models.Class_Transfer", b =>
                {
                    b.HasOne("api.DAL.models.Class_Product", "Product")
                        .WithMany("transfers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("api.DAL.models.Message", b =>
                {
                    b.HasOne("api.DAL.models.User", "Recipient")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("api.DAL.models.User", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("api.DAL.models.Photo", b =>
                {
                    b.HasOne("api.DAL.models.User", "user")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
