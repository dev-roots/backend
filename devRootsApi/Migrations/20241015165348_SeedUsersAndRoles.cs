using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace devRootsApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4936d6d7-420b-455c-b5eb-17254fafbb7a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71f7f2da-311e-4be5-ae22-980b980fc2df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2386e57-d94a-40dd-b716-14b2522829b0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "User", "USER" },
                    { "2", null, "Admin", "ADMIN" },
                    { "3", null, "Blogger", "BLOGGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePicture", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[,]
                {
                    { "0df10b5f-e080-4d43-9f87-5ba5f0bcc52e", 0, "a5853cbd-e89c-499f-b96d-cd0cbcd16aa7", new DateTime(2024, 10, 15, 17, 53, 47, 420, DateTimeKind.Local).AddTicks(4822), "rafagomezguillen03@gmail.com", false, false, null, "RAFAGOMEZGUILLEN03@GMAIL.COM", "RGOMEZ", "AQAAAAIAAYagAAAAEAYZhLYAzHSqHchxMJaH0IOvhvYoHoXs/1kWf/mZDOMcCyYxIEcjF/lQ7am0hZmjQg==", null, false, "https://www.gravatar.com/avatar/", "7aad0561-5bcf-4304-b05b-383f726eab54", false, new DateTime(2024, 10, 15, 17, 53, 47, 420, DateTimeKind.Local).AddTicks(4866), "rgomez" },
                    { "1dc4bb20-37b3-4fef-9b45-e428687a546f", 0, "dcda02f3-1802-48dd-ac69-23e10c0c438f", new DateTime(2024, 10, 15, 17, 53, 47, 487, DateTimeKind.Local).AddTicks(1163), "gafonsoudev@gmail.com", false, false, null, "GAFONSOUDEV@GMAIL.COM", "GAFONSO", "AQAAAAIAAYagAAAAEKAsjaaxzvZlMziVQC4Eru8LIIDkErjLXGgoEm96K+aro72hsW74UMBHH2ATkrNsKA==", null, false, "https://www.gravatar.com/avatar/", "7515d508-daf7-433d-920e-7b1906576696", false, new DateTime(2024, 10, 15, 17, 53, 47, 487, DateTimeKind.Local).AddTicks(1189), "gafonso" },
                    { "4f68cf3a-4372-49b2-b760-749c3cadb21d", 0, "b338c078-b619-4e35-b742-1935e612d087", new DateTime(2024, 10, 15, 17, 53, 47, 554, DateTimeKind.Local).AddTicks(6431), "andres.izbri@gmail.com", false, false, null, "ANDRES.IZBRI@GMAIL.COM", "AIZQUIERDO", "AQAAAAIAAYagAAAAEO3c9nm0su99Bjdys48Gg1rGmP03zchEiDGpty0/OUKnnFqgj1yrUpeMgB/xvBZpZg==", null, false, "https://www.gravatar.com/avatar/", "22dc2464-9064-486a-8c48-1087efbab1df", false, new DateTime(2024, 10, 15, 17, 53, 47, 554, DateTimeKind.Local).AddTicks(6476), "aizquierdo" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2", "0df10b5f-e080-4d43-9f87-5ba5f0bcc52e" },
                    { "2", "1dc4bb20-37b3-4fef-9b45-e428687a546f" },
                    { "2", "4f68cf3a-4372-49b2-b760-749c3cadb21d" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "0df10b5f-e080-4d43-9f87-5ba5f0bcc52e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "1dc4bb20-37b3-4fef-9b45-e428687a546f" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "4f68cf3a-4372-49b2-b760-749c3cadb21d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0df10b5f-e080-4d43-9f87-5ba5f0bcc52e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1dc4bb20-37b3-4fef-9b45-e428687a546f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4f68cf3a-4372-49b2-b760-749c3cadb21d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4936d6d7-420b-455c-b5eb-17254fafbb7a", null, "Admin", "ADMIN" },
                    { "71f7f2da-311e-4be5-ae22-980b980fc2df", null, "User", "USER" },
                    { "b2386e57-d94a-40dd-b716-14b2522829b0", null, "Blogger", "BLOGGER" }
                });
        }
    }
}
