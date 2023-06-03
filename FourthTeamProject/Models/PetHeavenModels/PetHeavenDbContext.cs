﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FourthTeamProject.PetHeavenModels
{
    public partial class PetHeavenDbContext : DbContext
    {
        public PetHeavenDbContext()
        {
        }

        public PetHeavenDbContext(DbContextOptions<PetHeavenDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<HotelCatagory> HotelCatagory { get; set; }
        public virtual DbSet<HotelImage> HotelImage { get; set; }
        public virtual DbSet<HotelOrder> HotelOrder { get; set; }
        public virtual DbSet<HotelOrderDetail> HotelOrderDetail { get; set; }
        public virtual DbSet<HotelService> HotelService { get; set; }
        public virtual DbSet<HotelToService> HotelToService { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberDiscount> MemberDiscount { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Pet> Pet { get; set; }
        public virtual DbSet<PetType> PetType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCatagory> ProductCatagory { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductOrder> ProductOrder { get; set; }
        public virtual DbSet<ProductOrderDetail> ProductOrderDetail { get; set; }
        public virtual DbSet<ProductRating> ProductRating { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<Salon> Salon { get; set; }
        public virtual DbSet<SalonCatagory> SalonCatagory { get; set; }
        public virtual DbSet<SalonOrder> SalonOrder { get; set; }
        public virtual DbSet<SalonOrderDetail> SalonOrderDetail { get; set; }
        public virtual DbSet<SalonSolution> SalonSolution { get; set; }
        public virtual DbSet<SalonSolutionSalon> SalonSolutionSalon { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<WorkShift> WorkShift { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discount>(entity =>
            {
                entity.Property(e => e.DiscountId)
                    .ValueGeneratedNever()
                    .HasColumnName("DiscountID");

                entity.Property(e => e.DiscountName).IsRequired();

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EmployeeAddress).IsRequired();

                entity.Property(e => e.EmployeeEmail).IsRequired();

                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeName).IsRequired();

                entity.Property(e => e.EmployeePassword).IsRequired();

                entity.Property(e => e.EmployeeRole).IsRequired();

                entity.HasOne(d => d.WorkShiftNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.WorkShift)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__WorkS__37FA4C37");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.HotelCatagoryId).HasColumnName("HotelCatagoryID");

                entity.Property(e => e.HotelName).IsRequired();

                entity.HasOne(d => d.HotelCatagory)
                    .WithMany(p => p.Hotel)
                    .HasForeignKey(d => d.HotelCatagoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hotel_HotelCatogory");
            });

            modelBuilder.Entity<HotelCatagory>(entity =>
            {
                entity.Property(e => e.HotelCatagoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelCatagoryID");

                entity.Property(e => e.HotelCatagoryName).IsRequired();
            });

            modelBuilder.Entity<HotelImage>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.HotelImageId).HasColumnName("HotelImageID");

                entity.Property(e => e.HotelImagePath).IsRequired();

                entity.HasOne(d => d.Hotel)
                    .WithMany()
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelImage_Hotel");
            });

            modelBuilder.Entity<HotelOrder>(entity =>
            {
                entity.HasKey(e => e.HoyelOrderId)
                    .HasName("PK_HotelOrder");

                entity.Property(e => e.HoyelOrderId).HasColumnName("HoyelOrderID");

                entity.Property(e => e.HotelOrderDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PayId).HasColumnName("PayID");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.HotelOrder)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_HotelOrder_Invoice");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.HotelOrder)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_HotelOrder_Member");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.HotelOrder)
                    .HasForeignKey(d => d.PayId)
                    .HasConstraintName("FK_HotelOrder_Payment");
            });

            modelBuilder.Entity<HotelOrderDetail>(entity =>
            {
                entity.Property(e => e.HotelOrderDetailId).HasColumnName("HotelOrderDetailID");

                entity.Property(e => e.CheckIntime).HasColumnType("datetime");

                entity.Property(e => e.CheckOutTime).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.HotelOrderId).HasColumnName("HotelOrderID");

                entity.HasOne(d => d.HotelOrder)
                    .WithMany(p => p.HotelOrderDetail)
                    .HasForeignKey(d => d.HotelOrderId)
                    .HasConstraintName("FK_HotelOrderDetail_HotelOrder");
            });

            modelBuilder.Entity<HotelService>(entity =>
            {
                entity.Property(e => e.HotelServiceId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelServiceID");
            });

            modelBuilder.Entity<HotelToService>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.HotelServiceId).HasColumnName("HotelServiceID");

                entity.HasOne(d => d.Hotel)
                    .WithMany()
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelToService_Hotel");

                entity.HasOne(d => d.HotelService)
                    .WithMany()
                    .HasForeignKey(d => d.HotelServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelToService_HotelService");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MemberBirthday).HasColumnType("date");
            });

            modelBuilder.Entity<MemberDiscount>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Discount)
                    .WithMany()
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_MemberDiscounDiscount");

                entity.HasOne(d => d.Member)
                    .WithMany()
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberDiscounMember");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PayId)
                    .HasName("PK_Payment");

                entity.Property(e => e.PayId).HasColumnName("PayID");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.Property(e => e.PetId).HasColumnName("PetID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PetBirthday).HasColumnType("date");

                entity.Property(e => e.PetTypeId).HasColumnName("PetTypeID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Pet)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_PeMember");

                entity.HasOne(d => d.PetType)
                    .WithMany(p => p.Pet)
                    .HasForeignKey(d => d.PetTypeId)
                    .HasConstraintName("FK_PePetType");
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.Property(e => e.PetTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("PetTypeID");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductCatagoryId).HasColumnName("ProductCatagoryID");

                entity.Property(e => e.ProductContent).IsRequired();

                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.ProductSpecification).IsRequired();

                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");

                entity.HasOne(d => d.ProductCatagory)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductCatagoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProducProductCatagory");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProducProductType1");
            });

            modelBuilder.Entity<ProductCatagory>(entity =>
            {
                entity.Property(e => e.ProductCatagoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductCatagoryID");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.ProductImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductImageID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductImage_Product");
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK_ProductOrder");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderAddress).IsRequired();

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderMemberEmail).IsRequired();

                entity.Property(e => e.OrderMemberName).IsRequired();

                entity.Property(e => e.OrderMemberPhone).IsRequired();

                entity.Property(e => e.PayId).HasColumnName("PayID");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOrder_Invoice");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOrder_Member");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOrder_Payment");
            });

            modelBuilder.Entity<ProductOrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK_ProductOrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductCatagoryName).IsRequired();

                entity.Property(e => e.ProductContent).IsRequired();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).IsRequired();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrderDetail)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductOrderDetail_ProductOrder");
            });

            modelBuilder.Entity<ProductRating>(entity =>
            {
                entity.Property(e => e.ProductRatingId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductRatingID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductRating1).HasColumnName("ProductRating");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductRating)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductRating_Product");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            });

            modelBuilder.Entity<Salon>(entity =>
            {
                entity.Property(e => e.SalonId).HasColumnName("SalonID");

                entity.Property(e => e.SalonCatagoryId).HasColumnName("SalonCatagoryID");

                entity.Property(e => e.SalonName).IsRequired();

                entity.HasOne(d => d.SalonCatagory)
                    .WithMany(p => p.Salon)
                    .HasForeignKey(d => d.SalonCatagoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Salon_SalonCatagory");
            });

            modelBuilder.Entity<SalonCatagory>(entity =>
            {
                entity.Property(e => e.SalonCatagoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("SalonCatagoryID");

                entity.Property(e => e.SalonCatagoryName).IsRequired();
            });

            modelBuilder.Entity<SalonOrder>(entity =>
            {
                entity.Property(e => e.SalonOrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("SalonOrderID");

                entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderMemberEmail).IsRequired();

                entity.Property(e => e.OrderMemberName).IsRequired();

                entity.Property(e => e.OrderMemberPhone).IsRequired();

                entity.Property(e => e.PayId).HasColumnName("PayID");

                entity.Property(e => e.SalonOrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.SalonOrder)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalonOrder_Invoice");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.SalonOrder)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalonOrder_Member");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.SalonOrder)
                    .HasForeignKey(d => d.PayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalonOrder_Payment");
            });

            modelBuilder.Entity<SalonOrderDetail>(entity =>
            {
                entity.Property(e => e.SalonOrderDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("SalonOrderDetailID");

                entity.Property(e => e.Appointment).HasColumnType("datetime");

                entity.Property(e => e.SalonCatagoryName).IsRequired();

                entity.Property(e => e.SalonOrderId).HasColumnName("SalonOrderID");

                entity.Property(e => e.SalonSolutionId).HasColumnName("SalonSolutionID");

                entity.Property(e => e.SalonSolutionName).IsRequired();

                entity.HasOne(d => d.SalonOrder)
                    .WithMany(p => p.SalonOrderDetail)
                    .HasForeignKey(d => d.SalonOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalonOrderDetail_SalonOrder");
            });

            modelBuilder.Entity<SalonSolution>(entity =>
            {
                entity.Property(e => e.SalonSolutionId).HasColumnName("SalonSolutionID");

                entity.Property(e => e.SalonSolutionName).IsRequired();
            });

            modelBuilder.Entity<SalonSolutionSalon>(entity =>
            {
                entity.Property(e => e.SalonSolutionSalonId)
                    .ValueGeneratedNever()
                    .HasColumnName("SalonSolutionSalonID");

                entity.Property(e => e.SalonId).HasColumnName("SalonID");

                entity.Property(e => e.SalonSolutionId).HasColumnName("SalonSolutionID");

                entity.HasOne(d => d.Salon)
                    .WithMany(p => p.SalonSolutionSalon)
                    .HasForeignKey(d => d.SalonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalonSolutionSalon_Salon");

                entity.HasOne(d => d.SalonSolution)
                    .WithMany(p => p.SalonSolutionSalon)
                    .HasForeignKey(d => d.SalonSolutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalonSolutionSalon_SalonSolution");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.Property(e => e.ShoppingCartId)
                    .ValueGeneratedNever()
                    .HasColumnName("ShoppingCartID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<WorkShift>(entity =>
            {
                entity.HasKey(e => e.ShiftId)
                    .HasName("PK__WorkShif__C0A838E1493A269F");

                entity.Property(e => e.ShiftId).HasColumnName("ShiftID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}