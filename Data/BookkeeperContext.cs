using System;
using Microsoft.EntityFrameworkCore;
using Bookkeeper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bookkeeper.Data
{
    public partial class BookkeeperContext : IdentityDbContext<IdentityUserExtended>
    {
        public BookkeeperContext()
        {
        }

        public BookkeeperContext(DbContextOptions<BookkeeperContext> options)
            : base(options)
        {
        }

        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalTransaction> JournalTransactions { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<JournalEntry>(entity =>
            {
                entity.HasKey(e => e.EntryId)
                    .HasName("PK_JournalEntries_EntryID");

                entity.ToTable("JournalEntries", "Recording");

                entity.Property(e => e.EntryId).HasColumnName("EntryID");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.DebitAmount).HasColumnType("money");
                entity.Property(e => e.CreditAmount).HasColumnType("money");

                entity.Property(e => e.ParentTransactionId).HasColumnName("ParentTransactionID");

                entity.HasOne(d => d.ParentTransaction)
                    .WithMany(p => p.JournalEntries)
                    .HasForeignKey(d => d.ParentTransactionId)
                    .HasConstraintName("FK_JournalEntries_JournalTransactions");
            });

            modelBuilder.Entity<JournalTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK_JournalTransactions_TransactionID");

                entity.ToTable("JournalTransactions", "Recording");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Memo)
                    .IsRequired()
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.RecordedDate).HasColumnType("date");

                entity.Property(e => e.TotalAmount).HasColumnType("money");

                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.JournalTransactions)
                    .HasForeignKey(d => d.UserID)
                    .HasConstraintName("FK_JournalTransactions_UserInfos");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserID)
                    .HasName("PK_UserInfos_UserID");
                    
                entity.Property(e => e.UserID).HasColumnName("UserID");

                entity.Property(e => e.AccountCreation).HasColumnType("date");

                entity.Property(e => e.LastActivity).HasColumnType("smalldatetime");
            });
        }
    }
}
