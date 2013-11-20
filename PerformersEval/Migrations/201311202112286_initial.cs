namespace PerformersEval.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FIN_PE_FormStatus", "FormsSent", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FIN_PE_FormStatus", "FormsSent", c => c.Boolean(nullable: false));
        }
    }
}
