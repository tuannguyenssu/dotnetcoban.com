using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreRelationshipTest.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManyToManyPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManyToManyPosts", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "ManyToManyTags",
                columns: table => new
                {
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManyToManyTags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "OneToManyBlogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneToManyBlogs", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "OneToOneBlogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneToOneBlogs", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_ManyToManyPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "ManyToManyPosts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_ManyToManyTags_TagId",
                        column: x => x.TagId,
                        principalTable: "ManyToManyTags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OneToManyPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneToManyPosts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_OneToManyPosts_OneToManyBlogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "OneToManyBlogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OneToOneBlogImages",
                columns: table => new
                {
                    BlogImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneToOneBlogImages", x => x.BlogImageId);
                    table.ForeignKey(
                        name: "FK_OneToOneBlogImages_OneToOneBlogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "OneToOneBlogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OneToManyPosts_BlogId",
                table: "OneToManyPosts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_OneToOneBlogImages_BlogId",
                table: "OneToOneBlogImages",
                column: "BlogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneToManyPosts");

            migrationBuilder.DropTable(
                name: "OneToOneBlogImages");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "OneToManyBlogs");

            migrationBuilder.DropTable(
                name: "OneToOneBlogs");

            migrationBuilder.DropTable(
                name: "ManyToManyPosts");

            migrationBuilder.DropTable(
                name: "ManyToManyTags");
        }
    }
}
