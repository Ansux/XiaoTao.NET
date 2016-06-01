using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xiaotao.Models
{
   public partial class sp_category
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [StringLength(200)]
      [Display(Name = "简介")]
      public string intro { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [Display(Name = "父级")]
      public Nullable<int> pid { get; set; }

      [Display(Name = "是否核验")]
      public bool verify { get; set; }

      public virtual ICollection<sp_product> sp_product { get; set; }
   }

   public partial class sp_brand
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [Display(Name = "是否核验")]
      public bool verify { get; set; }

      public virtual ICollection<sp_product> sp_product { get; set; }
   }

   public partial class sp_product
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "名称")]
      public string name { get; set; }

      [Required]
      [Display(Name = "单价")]
      public decimal price { get; set; }

      [Required]
      [Display(Name = "库存")]
      public int stock { get; set; }

      [Required]
      [StringLength(200)]
      [Display(Name = "原始图")]
      public string ori_img { get; set; }

      [StringLength(200)]
      [ScaffoldColumn(false)]
      [Display(Name = "缩略图")]
      public string thumb_img { get; set; }

      [Display(Name = "销量")]
      [ScaffoldColumn(false)]
      public int sales { get; set; }

      [Display(Name = "是否上架")]
      public bool is_onsale { get; set; }

      [Display(Name = "是否删除")]
      [ScaffoldColumn(false)]
      public bool is_delete { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      [ScaffoldColumn(false)]
      public Nullable<DateTime> update_at { get; set; }

      [Required]
      [ScaffoldColumn(false)]
      [Display(Name = "店铺")]
      public int store { get; set; }

      [Required]
      [ForeignKey("sp_category")]
      [Display(Name = "分类")]
      public Nullable<int> category { get; set; }

      [Required]
      [ForeignKey("sp_brand")]
      [Display(Name = "品牌")]
      public Nullable<int> brand { get; set; }

      [Display(Name = "详情")]
      public string detail { get; set; }

      public virtual xt_store xt_store { get; set; }
      public virtual sp_brand sp_brand { get; set; }
      public virtual sp_category sp_category { get; set; }

      public virtual ICollection<sp_cart_item> sp_cart_item { get; set; }
      public virtual ICollection<sp_order_item> sp_order_item { get; set; }
   }

   public partial class sp_cart_item
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [ForeignKey("sp_product")]
      [Display(Name = "商品")]
      public int product { get; set; }

      [Display(Name = "数量")]
      public int number { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "学生")]
      public int student { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      public virtual sp_product sp_product { get; set; }
      public virtual xt_student xt_student { get; set; }
   }

   public partial class sp_order
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Display(Name = "总额")]
      public decimal amount { get; set; }

      [Display(Name = "是否支付")]
      public bool is_pay { get; set; }

      /// <summary>
      /// -1.订单删除、0.订单取消、1.等待支付、2.支付成功，等待发货、3.发货成功，等待收货、4.确认收货，订单完成、5.用户评价、6.商家回复
      /// </summary>
      [Display(Name = "状态")]
      public int states { get; set; }

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
      [Display(Name = "手机")]
      public string phone { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "创建时间")]
      public DateTime create_at { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "支付时间")]
      public Nullable<DateTime> pay_time { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "发货时间")]
      public Nullable<DateTime> deliver_time { get; set; }

      [DataType(DataType.DateTime)]
      [ScaffoldColumn(false)]
      [Display(Name = "完成时间")]
      public Nullable<DateTime> finish_time { get; set; }

      [ForeignKey("xt_store")]
      [Display(Name = "店铺")]
      public int store { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "购买者")]
      public int buyer { get; set; }

      public virtual xt_student xt_student { get; set; }
      public virtual xt_store xt_store { get; set; }

      public virtual ICollection<sp_order_item> sp_order_item { get; set; }
   }

   public partial class sp_order_item
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [ForeignKey("sp_order")]
      [Display(Name = "订单")]
      public int oid { get; set; }

      [ForeignKey("sp_product")]
      [Display(Name = "商品")]
      public int product { get; set; }

      [Display(Name = "单价")]
      public decimal price { get; set; }

      [Display(Name = "数量")]
      public int number { get; set; }

      [Display(Name = "是否评论")]
      public bool is_comment { get; set; }

      /// <summary>
      /// 1.好评、2.中评、3.差评
      /// </summary>
      [Display(Name = "评论等级")]
      public Nullable<int> rank { get; set; }

      [StringLength(400)]
      [Display(Name = "评论内容")]
      public string comment { get; set; }

      [Display(Name = "是否回复")]
      public bool is_reply { get; set; }

      [StringLength(400)]
      [Display(Name = "回复内容")]
      public string reply { get; set; }

      public virtual sp_order sp_order { get; set; }
      public virtual sp_product sp_product { get; set; }
   }

   /* end shopping */

}
