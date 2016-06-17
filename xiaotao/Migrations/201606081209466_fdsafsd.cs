namespace xiaotao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fdsafsd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.dn_category",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 10),
                        intro = c.String(maxLength: 300),
                        create_at = c.DateTime(nullable: false),
                        pid = c.Int(),
                        verify = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dn_goods",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 100),
                        ori_img = c.String(maxLength: 200),
                        thumb_img = c.String(maxLength: 200),
                        is_anonymous = c.Boolean(nullable: false),
                        is_onsale = c.Boolean(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                        claim_type = c.Int(nullable: false),
                        claim_addr = c.String(maxLength: 50),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        category = c.Int(),
                        donor = c.Int(nullable: false),
                        detail = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dn_category", t => t.category)
                .ForeignKey("dbo.xt_student", t => t.donor, cascadeDelete: true)
                .Index(t => t.category)
                .Index(t => t.donor);
            
            CreateTable(
                "dbo.dn_claim",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        states = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        goods = c.Int(nullable: false),
                        student = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dn_goods", t => t.goods, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.student)
                .Index(t => t.goods)
                .Index(t => t.student);
            
            CreateTable(
                "dbo.xt_student",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        email = c.String(nullable: false, maxLength: 50),
                        pwd = c.String(nullable: false, maxLength: 50),
                        avatar = c.String(maxLength: 200),
                        sno = c.Int(),
                        sname = c.String(maxLength: 10),
                        voucher = c.String(maxLength: 200),
                        sex = c.Byte(),
                        phone = c.String(maxLength: 13),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        verify = c.Boolean(nullable: false),
                        states = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.dn_consult",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        goods = c.Int(nullable: false),
                        writer = c.Int(nullable: false),
                        content = c.String(maxLength: 400),
                        answer = c.Int(),
                        reply = c.String(maxLength: 400),
                        is_show = c.Boolean(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.dn_goods", t => t.goods, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.writer)
                .ForeignKey("dbo.xt_student", t => t.answer)
                .Index(t => t.goods)
                .Index(t => t.writer)
                .Index(t => t.answer);
            
            CreateTable(
                "dbo.es_consult",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        goods = c.Int(nullable: false),
                        writer = c.Int(nullable: false),
                        content = c.String(maxLength: 400),
                        ori_id = c.Int(),
                        ori_writer = c.Int(),
                        is_show = c.Boolean(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_goods", t => t.goods, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.writer)
                .ForeignKey("dbo.xt_student", t => t.ori_writer)
                .Index(t => t.goods)
                .Index(t => t.writer)
                .Index(t => t.ori_writer);
            
            CreateTable(
                "dbo.es_goods",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 120),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ori_img = c.String(maxLength: 300),
                        thumb_img = c.String(maxLength: 300),
                        is_new = c.Boolean(nullable: false),
                        is_onsale = c.Boolean(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        category = c.Int(),
                        seller = c.Int(nullable: false),
                        detail = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_category", t => t.category)
                .ForeignKey("dbo.xt_student", t => t.seller, cascadeDelete: true)
                .Index(t => t.category)
                .Index(t => t.seller);
            
            CreateTable(
                "dbo.es_category",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 20),
                        intro = c.String(maxLength: 300),
                        create_at = c.DateTime(nullable: false),
                        pid = c.Int(),
                        verify = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.es_order",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        is_pay = c.Boolean(nullable: false),
                        states = c.Byte(nullable: false),
                        receiver = c.String(nullable: false, maxLength: 10),
                        addr = c.String(nullable: false, maxLength: 50),
                        phone = c.String(nullable: false, maxLength: 13),
                        create_at = c.DateTime(nullable: false),
                        goods = c.Int(nullable: false),
                        buyer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.es_goods", t => t.goods, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.buyer)
                .Index(t => t.goods)
                .Index(t => t.buyer);
            
            CreateTable(
                "dbo.sp_cart_item",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product = c.Int(nullable: false),
                        number = c.Int(nullable: false),
                        student = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.sp_product", t => t.product, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.student, cascadeDelete: true)
                .Index(t => t.product)
                .Index(t => t.student);
            
            CreateTable(
                "dbo.sp_product",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        stock = c.Int(nullable: false),
                        ori_img = c.String(nullable: false, maxLength: 200),
                        thumb_img = c.String(maxLength: 200),
                        sales = c.Int(nullable: false),
                        is_onsale = c.Boolean(nullable: false),
                        is_delete = c.Boolean(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        store = c.Int(nullable: false),
                        category = c.Int(nullable: false),
                        brand = c.Int(nullable: false),
                        detail = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.sp_brand", t => t.brand, cascadeDelete: true)
                .ForeignKey("dbo.sp_category", t => t.category, cascadeDelete: true)
                .ForeignKey("dbo.xt_store", t => t.store)
                .Index(t => t.store)
                .Index(t => t.category)
                .Index(t => t.brand);
            
            CreateTable(
                "dbo.sp_brand",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 20),
                        verify = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.sp_category",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 20),
                        intro = c.String(maxLength: 200),
                        create_at = c.DateTime(nullable: false),
                        pid = c.Int(),
                        verify = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.sp_order_item",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        oid = c.Int(nullable: false),
                        product = c.Int(nullable: false),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        number = c.Int(nullable: false),
                        is_comment = c.Boolean(nullable: false),
                        rank = c.Int(),
                        comment = c.String(maxLength: 400),
                        is_reply = c.Boolean(nullable: false),
                        reply = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.sp_order", t => t.oid, cascadeDelete: true)
                .ForeignKey("dbo.sp_product", t => t.product, cascadeDelete: true)
                .Index(t => t.oid)
                .Index(t => t.product);
            
            CreateTable(
                "dbo.sp_order",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        is_pay = c.Boolean(nullable: false),
                        states = c.Int(nullable: false),
                        receiver = c.String(nullable: false, maxLength: 10),
                        addr = c.String(nullable: false, maxLength: 50),
                        phone = c.String(nullable: false, maxLength: 13),
                        create_at = c.DateTime(nullable: false),
                        pay_time = c.DateTime(),
                        deliver_time = c.DateTime(),
                        finish_time = c.DateTime(),
                        store = c.Int(nullable: false),
                        buyer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.xt_store", t => t.store, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.buyer, cascadeDelete: true)
                .Index(t => t.store)
                .Index(t => t.buyer);
            
            CreateTable(
                "dbo.xt_store",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        login_id = c.String(nullable: false, maxLength: 20),
                        login_pwd = c.String(nullable: false, maxLength: 50),
                        avatar = c.String(maxLength: 200),
                        name = c.String(nullable: false, maxLength: 100),
                        license = c.String(nullable: false, maxLength: 200),
                        intro = c.String(maxLength: 200),
                        shopkeeper = c.String(nullable: false, maxLength: 10),
                        id_no = c.String(nullable: false, maxLength: 20),
                        id_cart = c.String(nullable: false, maxLength: 200),
                        sex = c.Byte(),
                        email = c.String(nullable: false, maxLength: 50),
                        phone = c.String(nullable: false, maxLength: 20),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        verify = c.Boolean(nullable: false),
                        states = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.xt_store_log",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        kind = c.Byte(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        store = c.Int(nullable: false),
                        remark = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.xt_store", t => t.store, cascadeDelete: true)
                .Index(t => t.store);
            
            CreateTable(
                "dbo.xt_stu_certification",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sno = c.Int(nullable: false),
                        sname = c.String(nullable: false, maxLength: 10),
                        voucher = c.String(maxLength: 200),
                        is_pass = c.Boolean(nullable: false),
                        result = c.String(maxLength: 200),
                        create_at = c.DateTime(nullable: false),
                        valid_at = c.DateTime(),
                        student = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.xt_student", t => t.student, cascadeDelete: true)
                .Index(t => t.student);
            
            CreateTable(
                "dbo.xt_student_address",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        area = c.Int(nullable: false),
                        addr = c.String(nullable: false, maxLength: 20),
                        receiver = c.String(nullable: false, maxLength: 10),
                        phone = c.String(nullable: false, maxLength: 13),
                        remark = c.String(maxLength: 100),
                        is_default = c.Boolean(nullable: false),
                        student = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.xt_area", t => t.area, cascadeDelete: true)
                .ForeignKey("dbo.xt_student", t => t.student)
                .Index(t => t.area)
                .Index(t => t.student);
            
            CreateTable(
                "dbo.xt_area",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.xt_student_log",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        kind = c.Byte(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        student = c.Int(nullable: false),
                        remark = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.xt_student", t => t.student, cascadeDelete: true)
                .Index(t => t.student);
            
            CreateTable(
                "dbo.xt_admin",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        login_id = c.String(nullable: false, maxLength: 20),
                        login_pwd = c.String(nullable: false, maxLength: 50),
                        avatar = c.String(maxLength: 200),
                        real_name = c.String(maxLength: 10),
                        sex = c.Byte(),
                        email = c.String(maxLength: 50),
                        phone = c.String(maxLength: 13),
                        create_at = c.DateTime(nullable: false),
                        update_at = c.DateTime(),
                        role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.xt_role", t => t.role, cascadeDelete: true)
                .Index(t => t.role);
            
            CreateTable(
                "dbo.xt_role",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.xt_stuinfo_db",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sno = c.Int(nullable: false),
                        sname = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.xt_admin", "role", "dbo.xt_role");
            DropForeignKey("dbo.dn_goods", "donor", "dbo.xt_student");
            DropForeignKey("dbo.dn_claim", "student", "dbo.xt_student");
            DropForeignKey("dbo.xt_student_log", "student", "dbo.xt_student");
            DropForeignKey("dbo.xt_student_address", "student", "dbo.xt_student");
            DropForeignKey("dbo.xt_student_address", "area", "dbo.xt_area");
            DropForeignKey("dbo.xt_stu_certification", "student", "dbo.xt_student");
            DropForeignKey("dbo.sp_cart_item", "student", "dbo.xt_student");
            DropForeignKey("dbo.sp_cart_item", "product", "dbo.sp_product");
            DropForeignKey("dbo.sp_product", "store", "dbo.xt_store");
            DropForeignKey("dbo.sp_order_item", "product", "dbo.sp_product");
            DropForeignKey("dbo.sp_order_item", "oid", "dbo.sp_order");
            DropForeignKey("dbo.sp_order", "buyer", "dbo.xt_student");
            DropForeignKey("dbo.sp_order", "store", "dbo.xt_store");
            DropForeignKey("dbo.xt_store_log", "store", "dbo.xt_store");
            DropForeignKey("dbo.sp_product", "category", "dbo.sp_category");
            DropForeignKey("dbo.sp_product", "brand", "dbo.sp_brand");
            DropForeignKey("dbo.es_consult", "ori_writer", "dbo.xt_student");
            DropForeignKey("dbo.es_consult", "writer", "dbo.xt_student");
            DropForeignKey("dbo.es_consult", "goods", "dbo.es_goods");
            DropForeignKey("dbo.es_goods", "seller", "dbo.xt_student");
            DropForeignKey("dbo.es_order", "buyer", "dbo.xt_student");
            DropForeignKey("dbo.es_order", "goods", "dbo.es_goods");
            DropForeignKey("dbo.es_goods", "category", "dbo.es_category");
            DropForeignKey("dbo.dn_consult", "answer", "dbo.xt_student");
            DropForeignKey("dbo.dn_consult", "writer", "dbo.xt_student");
            DropForeignKey("dbo.dn_consult", "goods", "dbo.dn_goods");
            DropForeignKey("dbo.dn_claim", "goods", "dbo.dn_goods");
            DropForeignKey("dbo.dn_goods", "category", "dbo.dn_category");
            DropIndex("dbo.xt_admin", new[] { "role" });
            DropIndex("dbo.xt_student_log", new[] { "student" });
            DropIndex("dbo.xt_student_address", new[] { "student" });
            DropIndex("dbo.xt_student_address", new[] { "area" });
            DropIndex("dbo.xt_stu_certification", new[] { "student" });
            DropIndex("dbo.xt_store_log", new[] { "store" });
            DropIndex("dbo.sp_order", new[] { "buyer" });
            DropIndex("dbo.sp_order", new[] { "store" });
            DropIndex("dbo.sp_order_item", new[] { "product" });
            DropIndex("dbo.sp_order_item", new[] { "oid" });
            DropIndex("dbo.sp_product", new[] { "brand" });
            DropIndex("dbo.sp_product", new[] { "category" });
            DropIndex("dbo.sp_product", new[] { "store" });
            DropIndex("dbo.sp_cart_item", new[] { "student" });
            DropIndex("dbo.sp_cart_item", new[] { "product" });
            DropIndex("dbo.es_order", new[] { "buyer" });
            DropIndex("dbo.es_order", new[] { "goods" });
            DropIndex("dbo.es_goods", new[] { "seller" });
            DropIndex("dbo.es_goods", new[] { "category" });
            DropIndex("dbo.es_consult", new[] { "ori_writer" });
            DropIndex("dbo.es_consult", new[] { "writer" });
            DropIndex("dbo.es_consult", new[] { "goods" });
            DropIndex("dbo.dn_consult", new[] { "answer" });
            DropIndex("dbo.dn_consult", new[] { "writer" });
            DropIndex("dbo.dn_consult", new[] { "goods" });
            DropIndex("dbo.dn_claim", new[] { "student" });
            DropIndex("dbo.dn_claim", new[] { "goods" });
            DropIndex("dbo.dn_goods", new[] { "donor" });
            DropIndex("dbo.dn_goods", new[] { "category" });
            DropTable("dbo.xt_stuinfo_db");
            DropTable("dbo.xt_role");
            DropTable("dbo.xt_admin");
            DropTable("dbo.xt_student_log");
            DropTable("dbo.xt_area");
            DropTable("dbo.xt_student_address");
            DropTable("dbo.xt_stu_certification");
            DropTable("dbo.xt_store_log");
            DropTable("dbo.xt_store");
            DropTable("dbo.sp_order");
            DropTable("dbo.sp_order_item");
            DropTable("dbo.sp_category");
            DropTable("dbo.sp_brand");
            DropTable("dbo.sp_product");
            DropTable("dbo.sp_cart_item");
            DropTable("dbo.es_order");
            DropTable("dbo.es_category");
            DropTable("dbo.es_goods");
            DropTable("dbo.es_consult");
            DropTable("dbo.dn_consult");
            DropTable("dbo.xt_student");
            DropTable("dbo.dn_claim");
            DropTable("dbo.dn_goods");
            DropTable("dbo.dn_category");
        }
    }
}
