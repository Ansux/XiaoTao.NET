using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace xiaotao.Models
{
   public partial class dn_category
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [StringLength(10)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [StringLength(300)]
      [Display(Name = "简介")]
      public string intro { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [Display(Name = "父级")]
      public Nullable<int> pid { get; set; }

      [Display(Name = "是否核验")]
      public bool verify { get; set; }

      public virtual ICollection<dn_goods> dn_goods { get; set; }
   }

   public partial class dn_claim
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      /// <summary>
      /// -1.删除记录 0.取消认领 1.认领提交 2.认领完成 3.评价
      /// </summary>
      [Display(Name = "状态")]
      public int states { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [ForeignKey("dn_goods")]
      [Display(Name = "物品")]
      public int goods { get; set; }

      [Display(Name = "学生")]
      public int student { get; set; }

      public virtual dn_goods dn_goods { get; set; }
      public virtual xt_student xt_student { get; set; }
   }

   public partial class dn_consult
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [ForeignKey("dn_goods")]
      [Display(Name = "物品")]
      public int goods { get; set; }

      [ForeignKey("xt_student1")]
      [Display(Name = "咨询者")]
      public int writer { get; set; }

      [StringLength(400)]
      [Display(Name = "咨询内容")]
      public string content { get; set; }

      [ForeignKey("xt_student2")]
      [Display(Name = "回复者")]
      public Nullable<int> answer { get; set; }

      [StringLength(400)]
      [Display(Name = "回复回复")]
      public string reply { get; set; }

      [Display(Name = "是否显示")]
      public bool is_show { get; set; }

      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      public Nullable<DateTime> update_at { get; set; }

      public virtual dn_goods dn_goods { get; set; }

      public virtual xt_student xt_student1 { get; set; }

      public virtual xt_student xt_student2 { get; set; }
   }

   public partial class dn_goods
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [StringLength(100)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [StringLength(200)]
      [DataType(DataType.ImageUrl)]
      [Display(Name = "原始大图")]
      public string ori_img { get; set; }

      [StringLength(200)]
      [DataType(DataType.ImageUrl)]
      [Display(Name = "缩略图")]
      public string thumb_img { get; set; }

      [Display(Name = "是否匿名")]
      public bool is_anonymous { get; set; }

      [Display(Name = "是否上架")]
      public bool is_onsale { get; set; }

      [Display(Name = "是否删除")]
      public bool is_delete { get; set; }

      /// <summary>
      /// 1.到捐赠中心站 2.上门认领
      /// </summary>
      [Display(Name = "认领方式")]
      public int claim_type { get; set; }

      [StringLength(50)]
      [Display(Name = "认领地址")]
      public string claim_addr { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      public Nullable<DateTime> update_at { get; set; }

      [ForeignKey("dn_category")]
      [Display(Name = "分类")]
      public Nullable<int> category { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "捐赠者")]
      public int donor { get; set; }

      [Display(Name = "详情")]
      public string detail { get; set; }

      public virtual dn_category dn_category { get; set; }
      public virtual xt_student xt_student { get; set; }

      public virtual ICollection<dn_claim> dn_claim { get; set; }
      public virtual ICollection<dn_consult> dn_consult { get; set; }
   }
   /* end setting */
}
