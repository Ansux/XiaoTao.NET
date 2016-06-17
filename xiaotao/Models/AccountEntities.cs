using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace xiaotao.Models
{
   public partial class xt_role
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "名称")]
      public string name { get; set; }

      public virtual ICollection<xt_admin> xt_admin { get; set; }
   }

   public partial class xt_admin
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "登录ID")]
      public string login_id { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "密码")]
      [DataType(DataType.Password)]
      public string login_pwd { get; set; }

      [StringLength(200)]
      [DataType(DataType.ImageUrl)]
      [Display(Name = "头像")]
      public string avatar { get; set; }

      [StringLength(10)]
      [Display(Name = "姓名")]
      public string real_name { get; set; }

      [Display(Name = "性别")]
      public Nullable<byte> sex { get; set; }

      [StringLength(50)]
      [Display(Name = "邮箱")]
      [DataType(DataType.EmailAddress)]
      public string email { get; set; }

      [StringLength(13)]
      [Display(Name = "电话")]
      [DataType(DataType.PhoneNumber)]
      public string phone { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      [ScaffoldColumn(false)]
      public Nullable<DateTime> update_at { get; set; }

      [ForeignKey("xt_role")]
      [Display(Name = "角色")]
      public int role { get; set; }

      public virtual xt_role xt_role { get; set; }
   }

   public partial class xt_student
   {
      [Key]
      public int id { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "邮箱")]
      [System.Web.Mvc.Remote("IsExistEmail", "Account","Mall", ErrorMessage = "此邮箱已被注册！")]
      [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
      public string email { get; set; }

      [Required]
      [StringLength(50)]
      [DataType(DataType.Password)]
      [Display(Name = "密码")]
      public string pwd { get; set; }

      [StringLength(200)]
      [DataType(DataType.Upload)]
      [Display(Name = "头像")]
      public string avatar { get; set; }

      [System.Web.Mvc.Remote("IsExistSno", "Account", "Mall", ErrorMessage = "此学号已被注册！")]
      [Display(Name = "学号")]
      public Nullable<int> sno { get; set; }

      [StringLength(10)]
      [Display(Name = "姓名")]
      public string sname { get; set; }

      [StringLength(200)]
      [DataType(DataType.ImageUrl)]
      [Display(Name = "证件照")]
      public string voucher { get; set; }

      [Display(Name = "性别")]
      public Nullable<byte> sex { get; set; }

      [StringLength(13)]
      [System.Web.Mvc.Remote("IsExistPhone", "Account", "Mall", ErrorMessage = "此手机已被注册！")]
      [Display(Name = "电话")]
      [DataType(DataType.PhoneNumber)]
      public string phone { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      [ScaffoldColumn(false)]
      public Nullable<DateTime> update_at { get; set; }

      [DefaultValue(false)]
      [Display(Name = "是否验证")]
      [ScaffoldColumn(false)]
      public bool verify { get; set; }

      [DefaultValue(true)]
      [Display(Name = "启用状态")]
      [ScaffoldColumn(false)]
      public bool states { get; set; }

      public virtual ICollection<dn_claim> dn_claim { get; set; }
      public virtual ICollection<dn_consult> dn_consult1 { get; set; }
      public virtual ICollection<dn_consult> dn_consult2 { get; set; }
      public virtual ICollection<dn_goods> dn_goods { get; set; }
      public virtual ICollection<es_consult> es_consult1 { get; set; }
      public virtual ICollection<es_consult> es_consult2 { get; set; }
      public virtual ICollection<es_goods> es_goods { get; set; }
      public virtual ICollection<es_order> es_order { get; set; }
      public virtual ICollection<sp_cart_item> sp_cart_item { get; set; }
      public virtual ICollection<sp_order> sp_order { get; set; }
      public virtual ICollection<xt_student_log> xt_student_log { get; set; }
      public virtual ICollection<xt_student_address> xt_student_address { get; set; }
      public virtual ICollection<xt_stu_certification> xt_stu_certification { get; set; }
   }

   public partial class xt_store
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(20)]
      [System.Web.Mvc.Remote("IsExistLoginID", "Store", "Mall", ErrorMessage = "此登录名已被注册！")]
      [Display(Name = "登录ID")]
      public string login_id { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "密码")]
      [DataType(DataType.Password)]
      public string login_pwd { get; set; }

      [StringLength(200)]
      [Display(Name = "头像")]
      public string avatar { get; set; }

      [Required]
      [StringLength(100)]
      [Display(Name = "店名")]
      public string name { get; set; }

      [Required]
      [StringLength(200)]
      [Display(Name = "执照")]
      public string license { get; set; }

      [StringLength(200)]
      [Display(Name = "简介")]
      [DataType(DataType.MultilineText)]
      public string intro { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "店主")]
      public string shopkeeper { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "身份证号")]
      [MinLength(15, ErrorMessage = "格式错误！")]
      public string id_no { get; set; }

      [Required]
      [StringLength(200)]
      [Display(Name = "身份证件")]
      [DataType(DataType.ImageUrl)]
      public string id_cart { get; set; }

      [Display(Name = "性别")]
      public Nullable<byte> sex { get; set; }

      [Required]
      [StringLength(50)]
      [Display(Name = "邮箱")]
      [DataType(DataType.EmailAddress)]
      public string email { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "电话")]
      [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "格式不正确")]
      public string phone { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [Display(Name = "更新时间")]
      [ScaffoldColumn(false)]
      public Nullable<DateTime> update_at { get; set; }

      [Display(Name = "是否核验")]
      [ScaffoldColumn(false)]
      public bool verify { get; set; }

      [Display(Name = "启用状态")]
      [ScaffoldColumn(false)]
      public bool states { get; set; }

      public virtual ICollection<sp_order> sp_order { get; set; }
      public virtual ICollection<sp_product> sp_product { get; set; }
      public virtual ICollection<xt_store_log> xt_store_log { get; set; }
   }

   public partial class xt_student_log
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      /// <summary>
      /// 1.注册 2.登录 3.修改资料
      /// </summary>
      [Display(Name = "类型")]
      public byte kind { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "学生")]
      public int student { get; set; }

      [StringLength(200)]
      [Display(Name = "备注")]
      public string remark { get; set; }

      public virtual xt_student xt_student { get; set; }
   }

   public partial class xt_store_log
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      /// <summary>
      /// 1.注册 2.登录 3.修改资料
      /// </summary>
      [Required]
      [Display(Name = "类型")]
      public byte kind { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [ForeignKey("xt_store")]
      [Display(Name = "店铺")]
      public int store { get; set; }

      [StringLength(200)]
      [Display(Name = "备注")]
      public string remark { get; set; }

      public virtual xt_store xt_store { get; set; }
   }

   public partial class xt_area
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "名称")]
      public string name { get; set; }

      public virtual ICollection<xt_student_address> xt_student_address { get; set; }
   }

   public partial class xt_student_address
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Required]
      [ForeignKey("xt_area")]
      [Display(Name = "区域")]
      public Nullable<int> area { get; set; }

      [Required]
      [StringLength(20)]
      [Display(Name = "地址")]
      public string addr { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "收货人")]
      public string receiver { get; set; }

      [Required]
      [StringLength(13)]
      [Display(Name = "电话")]
      [DataType(DataType.PhoneNumber)]
      public string phone { get; set; }

      [StringLength(100)]
      [Display(Name = "备注")]
      public string remark { get; set; }

      [Display(Name = "是否默认")]
      public bool is_default { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "账号")]
      public int student { get; set; }

      public virtual xt_area xt_area { get; set; }
      public virtual xt_student xt_student { get; set; }
   }

   public partial class xt_stuinfo_db
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Display(Name = "学号")]
      public int sno { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "姓名")]
      public string sname { get; set; }

   }

   public partial class xt_stu_certification
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int id { get; set; }

      [Display(Name = "学号")]
      public int sno { get; set; }

      [Required]
      [StringLength(10)]
      [Display(Name = "姓名")]
      public string sname { get; set; }

      [StringLength(200)]
      [DataType(DataType.ImageUrl)]
      [Display(Name = "证件照")]
      public string voucher { get; set; }

      [DefaultValue(true)]
      [ScaffoldColumn(false)]
      [Display(Name = "是否通过核验")]
      public bool is_pass { get; set; }

      [StringLength(200)]
      [Display(Name = "结论")]
      public string result { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "创建时间")]
      [ScaffoldColumn(false)]
      public DateTime create_at { get; set; }

      [DataType(DataType.DateTime)]
      [Display(Name = "核验时间")]
      [ScaffoldColumn(false)]
      public Nullable<DateTime> valid_at { get; set; }

      [ForeignKey("xt_student")]
      [Display(Name = "学生")]
      public int student { get; set; }

      public virtual xt_student xt_student { get; set; }
   }

   /* end setting */

}
