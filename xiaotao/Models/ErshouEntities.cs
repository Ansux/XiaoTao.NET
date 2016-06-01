using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace xiaotao.Models
{
   public partial class es_category
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [StringLength(20)]
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

      public virtual ICollection<es_goods> es_goods { get; set; }
   }

   public partial class es_consult
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [ForeignKey("es_goods")]
      [Display(Name = "商品")]
      public int goods { get; set; }

      [Display(Name = "作者")]
      [ForeignKey("xt_student1")]
      public int writer { get; set; }

      [StringLength(400)]
      [Display(Name = "内容")]
      public string content { get; set; }

      /// <summary>
      /// 用于将咨询条目进行树状归类
      /// </summary>
      [Display(Name = "顶级咨询")]
      public Nullable<int> ori_id { get; set; }

      /// <summary>
      /// 实现的效果(**回复***)
      /// </summary>
      [ForeignKey("xt_student2")]
      [Display(Name = "回复原作者")]
      public Nullable<int> ori_writer { get; set; }

      [Display(Name = "是否显示")]
      public bool is_show { get; set; }

      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      public Nullable<DateTime> update_at { get; set; }

      public virtual xt_student xt_student1 { get; set; }
      public virtual xt_student xt_student2 { get; set; }
      public virtual es_goods es_goods { get; set; }
   }

   public partial class es_goods
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [StringLength(120)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [Display(Name = "单价")]
      public decimal price { get; set; }

      [StringLength(300)]
      [Display(Name = "原始图")]
      public string ori_img { get; set; }

      [StringLength(300)]
      [Display(Name = "缩略图")]
      public string thumb_img { get; set; }

      [Display(Name = "是否全新")]
      public bool is_new { get; set; }

      [Display(Name = "是否上架")]
      public bool is_onsale { get; set; }

      [Display(Name = "是否删除")]
      public bool is_delete { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "更新时间")]
      public Nullable<DateTime> update_at { get; set; }

      [ForeignKey("es_category")]
      [Display(Name = "分类")]
      public Nullable<int> category { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "销售者")]
      public int seller { get; set; }

      [DataType(DataType.Text)]
      [Display(Name = "详情")]
      public string detail { get; set; }

      public virtual es_category es_category { get; set; }
      public virtual xt_student xt_student { get; set; }


      public virtual ICollection<es_consult> es_consult { get; set; }
      public virtual ICollection<es_order> es_order { get; set; }
   }

   public partial class es_order
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Display(Name = "总额")]
      public decimal amount { get; set; }

      [Display(Name = "是否支付")]
      public bool is_pay { get; set; }

      /// <summary>
      /// 0.取消订单，1.创建订单，2.订单支付，3.送货中，4.订单完成，5.订单评论
      /// </summary>
      [Display(Name = "状态")]
      public byte states { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "收货人")]
      public string receiver { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "地址")]
      public string addr { get; set; }

      [Required]
      [StringLength(13)]
      [DataType(DataType.PhoneNumber)]
      [Display(Name = "手机")]
      public string phone { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [ForeignKey("es_goods")]
      [Display(Name = "商品")]
      public int goods { get; set; }

      [Display(Name = "购买者")]
      public int buyer { get; set; }

      public virtual es_goods es_goods { get; set; }
      public virtual xt_student xt_student { get; set; }
   }
   /* end setting */

}
