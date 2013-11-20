namespace PerformersEval.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserProfile", newName: "FIN_PE_UserProfile");
            RenameTable(name: "dbo.Performer", newName: "FIN_PE_Performer");
            RenameTable(name: "dbo.Vendor", newName: "FIN_PE_Vendor");
            RenameTable(name: "dbo.SubW9", newName: "FIN_PE_SubW9");
            RenameTable(name: "dbo.Invoice", newName: "FIN_PE_Invoice");
            RenameTable(name: "dbo.FormsStatus", newName: "FIN_PE_FormStatus");
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_FullName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_Addr1", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_State", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_Zip", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.FIN_PE_Vendor", "Contact_Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Contact_Phone", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.FIN_PE_Vendor", "Contact_Email", c => c.String(nullable: false, maxLength: 25));
            DropColumn("dbo.FIN_PE_Vendor", "Vendor_NonLocal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FIN_PE_Vendor", "Vendor_NonLocal", c => c.Boolean(nullable: false));
            AlterColumn("dbo.FIN_PE_Vendor", "Contact_Email", c => c.String(maxLength: 25));
            AlterColumn("dbo.FIN_PE_Vendor", "Contact_Phone", c => c.String(maxLength: 25));
            AlterColumn("dbo.FIN_PE_Vendor", "Contact_Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_Zip", c => c.String(maxLength: 10));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_State", c => c.String(maxLength: 2));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_City", c => c.String(maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_Addr1", c => c.String(maxLength: 50));
            AlterColumn("dbo.FIN_PE_Vendor", "Vendor_FullName", c => c.String(maxLength: 50));
            RenameTable(name: "dbo.FIN_PE_FormStatus", newName: "FormsStatus");
            RenameTable(name: "dbo.FIN_PE_Invoice", newName: "Invoice");
            RenameTable(name: "dbo.FIN_PE_SubW9", newName: "SubW9");
            RenameTable(name: "dbo.FIN_PE_Vendor", newName: "Vendor");
            RenameTable(name: "dbo.FIN_PE_Performer", newName: "Performer");
            RenameTable(name: "dbo.FIN_PE_UserProfile", newName: "UserProfile");
        }
    }
}
