using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace xiaotao.Models
{
   public partial class WebContext : DbContext
   {
      public WebContext()
          : base("name=WebModel")
      {
      }

      public virtual DbSet<dn_category> dn_category { get; set; }
      public virtual DbSet<dn_claim> dn_claim { get; set; }
      public virtual DbSet<dn_consult> dn_consult { get; set; }
      public virtual DbSet<dn_goods> dn_goods { get; set; }
      public virtual DbSet<es_category> es_category { get; set; }
      public virtual DbSet<es_consult> es_consult { get; set; }
      public virtual DbSet<es_goods> es_goods { get; set; }
      public virtual DbSet<es_order> es_order { get; set; }
      public virtual DbSet<sp_brand> sp_brand { get; set; }
      public virtual DbSet<sp_category> sp_category { get; set; }
      public virtual DbSet<sp_order> sp_order { get; set; }
      public virtual DbSet<sp_product> sp_product { get; set; }
      public virtual DbSet<xt_admin> xt_admin { get; set; }
      public virtual DbSet<xt_area> xt_area { get; set; }
      public virtual DbSet<xt_role> xt_role { get; set; }
      public virtual DbSet<xt_store_log> xt_store_log { get; set; }
      public virtual DbSet<xt_student> xt_student { get; set; }
      public virtual DbSet<xt_student_address> xt_student_address { get; set; }
      public virtual DbSet<xt_student_log> xt_student_log { get; set; }
      public virtual DbSet<xt_stu_certification> xt_stu_certification { get; set; }
      public virtual DbSet<xt_store> xt_store { get; set; }
      public virtual DbSet<xt_stuinfo_db> xt_stuinfo_db { get; set; }
      public virtual DbSet<sp_order_item> sp_order_item { get; set; }
      public virtual DbSet<sp_cart_item> sp_cart_item { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         modelBuilder.Entity<xt_student_address>().HasRequired(u => u.xt_student)
             .WithMany(d => d.xt_student_address)
             .HasForeignKey(c => c.student)
             .WillCascadeOnDelete(false);
         /* ershou */

         modelBuilder.Entity<sp_product>().HasRequired(u => u.xt_store)
             .WithMany(d => d.sp_product)
             .HasForeignKey(c => c.store)
             .WillCascadeOnDelete(false);
         /* shopping */

         modelBuilder.Entity<dn_consult>().HasRequired(u => u.xt_student1)
             .WithMany(d => d.dn_consult1)
             .HasForeignKey(c => c.writer)
             .WillCascadeOnDelete(false);
         modelBuilder.Entity<dn_claim>().HasRequired(u => u.xt_student)
           .WithMany(d => d.dn_claim)
           .HasForeignKey(c => c.student)
           .WillCascadeOnDelete(false);
         /* donate */

         modelBuilder.Entity<es_consult>().HasRequired(u => u.xt_student1)
             .WithMany(d => d.es_consult1)
             .HasForeignKey(c => c.writer)
             .WillCascadeOnDelete(false);
         modelBuilder.Entity<es_order>().HasRequired(u => u.xt_student)
             .WithMany(d => d.es_order)
             .HasForeignKey(c => c.buyer)
             .WillCascadeOnDelete(false);
         /* ershou */

      }
   }
}