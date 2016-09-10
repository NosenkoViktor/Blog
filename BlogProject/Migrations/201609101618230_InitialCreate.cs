namespace BlogProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        CommentText = c.String(),
                        CommentTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Title = c.String(),
                        PostText = c.String(),
                        Like = c.Int(nullable: false),
                        Dislike = c.Int(nullable: false),
                        PostTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PostId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Age = c.Int(nullable: false),
                        Fone = c.String(),
                        Email = c.String(),
                        Skype = c.String(),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
