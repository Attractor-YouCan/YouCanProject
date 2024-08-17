using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YouCan.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AvatarUrl = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Disctrict = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrtInstructions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    QuestionsCount = table.Column<int>(type: "integer", nullable: true),
                    TimeInMin = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrtInstructions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrtTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrtLevel = table.Column<int>(type: "integer", nullable: true),
                    TimeForTestInMin = table.Column<int>(type: "integer", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrtTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    SubjectType = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Subjects_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Streak = table.Column<int>(type: "integer", nullable: false),
                    TotalExperience = table.Column<int>(type: "integer", nullable: false),
                    StudyMinutes = table.Column<TimeSpan>(type: "interval", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statistics_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserORTTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    OrtTestId = table.Column<int>(type: "integer", nullable: true),
                    PassedLevel = table.Column<int>(type: "integer", nullable: true),
                    PassedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Points = table.Column<int>(type: "integer", nullable: true),
                    PassedTimeInMin = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserORTTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserORTTests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserORTTests_OrtTests_OrtTestId",
                        column: x => x.OrtTestId,
                        principalTable: "OrtTests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subtopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    TopicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    SubTitle = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Lecture = table.Column<string>(type: "text", nullable: true),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: false),
                    LessonLevel = table.Column<int>(type: "integer", nullable: true),
                    SubtopicId = table.Column<int>(type: "integer", nullable: true),
                    SubjectId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lessons_Subtopics_SubtopicId",
                        column: x => x.SubtopicId,
                        principalTable: "Subtopics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SubtopicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLevels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLevels_Subtopics_SubtopicId",
                        column: x => x.SubtopicId,
                        principalTable: "Subtopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    LessonId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonModule_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GainingExperience = table.Column<int>(type: "integer", nullable: false),
                    TimeForTestInMin = table.Column<int>(type: "integer", nullable: true),
                    SubjectId = table.Column<int>(type: "integer", nullable: true),
                    LessonId = table.Column<int>(type: "integer", nullable: true),
                    OrtTestId = table.Column<int>(type: "integer", nullable: true),
                    OrtInstructionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tests_OrtInstructions_OrtInstructionId",
                        column: x => x.OrtInstructionId,
                        principalTable: "OrtInstructions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tests_OrtTests_OrtTestId",
                        column: x => x.OrtTestId,
                        principalTable: "OrtTests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tests_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    SubtopicId = table.Column<int>(type: "integer", nullable: true),
                    SubjectId = table.Column<int>(type: "integer", nullable: true),
                    PassedLevel = table.Column<int>(type: "integer", nullable: true),
                    LessonId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLessons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLessons_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLessons_Subtopics_SubtopicId",
                        column: x => x.SubtopicId,
                        principalTable: "Subtopics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Instruction = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    TestId = table.Column<int>(type: "integer", nullable: true),
                    Point = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassedQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassedQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassedQuestions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassedQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionReports_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "user", "USER" },
                    { 2, null, "manager", "MANAGER" },
                    { 3, null, "admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "OrtInstructions",
                columns: new[] { "Id", "Description", "QuestionsCount", "TestId", "TimeInMin" },
                values: new object[,]
                {
                    { 1, "Добро пожаловать на предметный тест по Математике. Тест состоит из 40 заданий, на выполнение которых отводится 80 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!", 40, null, 80 },
                    { 2, "Добро пожаловать на предметный тест по Аналогиям и дополнениям . Тест состоит из 30 заданий, на выполнение которых отводится 30 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!", 30, null, 70 },
                    { 3, "Добро пожаловать на предметный тест по Чтению и пониманию. Тест состоит из 30 заданий, на выполнение которых отводится 35 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!", 30, null, 60 },
                    { 4, "Добро пожаловать на предметный тест по Практической грамматике. Тест состоит из 30 заданий, на выполнение которых отводится 35 минут. Во время выполнения заданий Вы можете изменить выбранный вариант ответа, но сделать это можно только один раз для каждого задания. При исправлении новый (правильный) ответ будет представлен квадратиком. После прохождения теста Вы получите результат, в котором будет указано количество данных Вами правильных ответов и общий процент правильно и неправильно выполненных заданий. Желаем удачи!", 30, null, 50 }
                });

            migrationBuilder.InsertData(
                table: "OrtTests",
                columns: new[] { "Id", "Language", "OrtLevel", "TimeForTestInMin" },
                values: new object[,]
                {
                    { 1, "ru", 1, null },
                    { 2, "ru", 2, null },
                    { 3, "ru", 3, null },
                    { 4, "ru", 4, null },
                    { 5, "ru", 5, null },
                    { 6, "ru", 6, null },
                    { 7, "ru", 7, null },
                    { 8, "ru", 8, null },
                    { 9, "ru", 9, null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "ImageUrl", "Name", "ParentId", "SubjectType" },
                values: new object[,]
                {
                    { 1, "/topicImages/mathematics1icon.png", "Mathematics", null, 0 },
                    { 2, "/topicImages/grammarIcon.png", "Russian", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mathematics" },
                    { 2, "Russian" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "ImageUrl", "Name", "ParentId", "SubjectType" },
                values: new object[,]
                {
                    { 3, "/topicImages/mathematics1icon.png", "Mathematics1", 1, 1 },
                    { 4, "/topicImages/mathematics2icon.png", "Mathematics2", 1, 1 },
                    { 5, "/topicImages/analogyIcon.png", "Analogy", 2, 0 },
                    { 6, "/topicImages/grammarIcon.png", "Grammar", 2, 1 },
                    { 7, "/topicImages/readUnderstIcon.png", "Reading and Understanding", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Subtopics",
                columns: new[] { "Id", "ImageUrl", "Name", "TopicId" },
                values: new object[,]
                {
                    { 1, "/topicImages/mathematics1icon.png", "Mathematics1", 1 },
                    { 2, "/topicImages/mathematics2icon.png", "Mathematics2", 1 },
                    { 3, "/topicImages/analogyIcon.png", "Analogy", 2 },
                    { 4, "/topicImages/grammarIcon.png", "Grammar", 2 },
                    { 5, "/topicImages/readUnderstIcon.png", "Reading and Understanding", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "GainingExperience", "LessonId", "OrtInstructionId", "OrtTestId", "SubjectId", "TimeForTestInMin" },
                values: new object[] { 5, 0, null, 4, 1, 2, 30 });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "Description", "Lecture", "LessonLevel", "RequiredLevel", "SubTitle", "SubjectId", "SubtopicId", "Title", "VideoUrl" },
                values: new object[,]
                {
                    { 1, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 1, 0, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 2, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 2, 1, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 3, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 3, 2, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 4, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 4, 3, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 5, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 5, 4, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 6, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 6, 5, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 7, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 7, 6, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 8, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 8, 7, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 9, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 9, 8, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 10, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 10, 9, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 11, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 11, 10, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", null, 1, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 12, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 1, 0, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 13, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 2, 1, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 14, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 3, 2, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 15, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 4, 3, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 16, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 5, 4, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 17, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 6, 5, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 18, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 7, 6, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 19, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 8, 7, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 20, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 9, 8, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 21, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 10, 9, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" },
                    { 22, "понятие интеграла;\nзамена переменной при интегрировании и интегрирование по частям;\nприменение определённых интегралов в геометрии и физике;\nдифференциальные уравнения.", "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", 11, 10, "Интеграл — одно из важнейших понятий математического анализа, которое возникает при решении задач", 3, null, "Integrals", "/userImages/defProf-ProfileN=1.png" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Content", "Instruction", "IsPublished", "Point", "TestId", "Type", "UserId" },
                values: new object[,]
                {
                    { 16, "Что такое интеграл?", "Отвечайте на следующие вопросы", false, 3, 5, "general", null },
                    { 17, "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", "Отвечайте на следующие вопросы", false, 3, 5, "general", null },
                    { 21, "Птица : Гнездо", "Отметьте вариант, наиболее близкий к контрольной паре", false, 2, 5, "analogy", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "ImageUrl", "Name", "ParentId", "SubjectType" },
                values: new object[,]
                {
                    { 8, "/topicImages/analogyIcon.png", "Analogy", 5, 1 },
                    { 9, "/topicImages/readUnderstIcon.png", "Addition of Offers", 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "GainingExperience", "LessonId", "OrtInstructionId", "OrtTestId", "SubjectId", "TimeForTestInMin" },
                values: new object[,]
                {
                    { 3, 0, null, null, null, 6, null },
                    { 4, 0, null, 3, 1, 7, 30 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 65, false, 21, "Площадь под функции f(x)" },
                    { 66, false, 21, "интеграл от f(x) от 0 до x" },
                    { 67, true, 21, "Функция, производная которой равна f(x)" },
                    { 68, false, 21, "Функция, производная которой равна Y(f-k)" },
                    { 77, false, 16, "Площадь под функции f(x)" },
                    { 78, false, 16, "интеграл от f(x) от 0 до x" },
                    { 79, true, 16, "Функция, производная которой равна f(x)" },
                    { 80, false, 16, "Функция, производная которой равна Y(f-k)" },
                    { 81, false, 17, "Грамматика это чудо" },
                    { 82, false, 17, "Грамматика это таска" },
                    { 83, false, 17, "Грамматика это пипец" },
                    { 84, true, 17, "Грамматика это 5 по русскому" }
                });

            migrationBuilder.InsertData(
                table: "LessonModule",
                columns: new[] { "Id", "Content", "LessonId", "PhotoUrl", "Title" },
                values: new object[,]
                {
                    { 1, "1. ∫a dx = ax + C 2. ∫x^n dx = (x^(n+1))/(n+1) + C\r\n\r\n3. ∫e^x dx = e^x + C\r\n\r\n4. ∫sin(x) dx = -cos(x) + C\r\n\r\n5. ∫cos(x) dx = sin(x) + C", 12, null, "Основные формулы" },
                    { 2, "<p>1. ∫a dx = ax + C</p><p>2. ∫x^n dx = (x^(n+1))/(n+1) + C</p><p>3. ∫e^x dx = e^x + C</p><p>4. ∫sin(x) dx = -cos(x) + C</p><p>5. ∫cos(x) dx = sin(x) + C</p>", 12, null, "Основные формулы" },
                    { 3, "<p>1. ∫a dx = ax + C</p><p>2. ∫x^n dx = (x^(n+1))/(n+1) + C</p><p>3. ∫e^x dx = e^x + C</p><p>4. ∫sin(x) dx = -cos(x) + C</p><p>5. ∫cos(x) dx = sin(x) + C</p>", 12, "/studyImages/Screenshot_1.png", "Основные формулы" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Content", "Instruction", "IsPublished", "Point", "TestId", "Type", "UserId" },
                values: new object[,]
                {
                    { 7, "В каком слове вместо точек следует вставить букву с?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 8, "В каком предложении подчеркнутое слово можно заменить словом (высохший (-ая, -ее))?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 9, "В каком слове вместо точек следует вставить букву з?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 10, "В каком предложении подчеркнутое слово употреблено в правильной форме?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 11, "Задание, ... нами, не вызывает особых затруднений.", "Какое слово следует вставить вместо точек в предложение?", false, null, 3, null, null },
                    { 12, "Какое слово является синонимом к слову 'красивый'?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 13, "Какое слово является антонимом к слову 'высокий'?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 14, "Какое слово является глаголом?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 15, "Какое слово обозначает предмет?", "Выберите правильный вариант", false, null, 3, null, null },
                    { 18, "23 : 34", "Что больше?", false, 3, 4, "analogy", null },
                    { 19, "Что такое интеграл?", "Отвечайте на следующие вопросы", false, 3, 4, "general", null },
                    { 20, "Грамматика это___", "Отметьте вариант, наиболее близкий к контрольной паре", false, 2, 4, "general", null }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "Id", "GainingExperience", "LessonId", "OrtInstructionId", "OrtTestId", "SubjectId", "TimeForTestInMin" },
                values: new object[,]
                {
                    { 1, 0, 12, 1, 1, 1, 40 },
                    { 2, 0, 13, 2, 1, 5, 40 }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 13, true, 7, "ве...ти концерт" },
                    { 14, false, 7, "ве...ти груз" },
                    { 15, false, 7, "ни...кая температура" },
                    { 16, false, 7, "передвигались пол...ком" },
                    { 17, false, 8, "Лето будет жаркое и (сухое)." },
                    { 18, false, 8, "Надо купить пакет (сухого) молока." },
                    { 19, false, 8, "Матч закончился с (сухим) счетом." },
                    { 20, true, 8, "В ванной висит (сухое) полотенце" },
                    { 21, true, 9, "в...рыхлённый граблями" },
                    { 22, false, 9, "в...копанный огород" },
                    { 23, false, 9, "в...порхнувший с ветки" },
                    { 24, false, 9, "в...тавленный в текст" },
                    { 25, false, 10, "Слева стояли такие высокие здания, что мимо (их) проплывали облака." },
                    { 26, false, 10, "Впереди шла стройная женщина, а позади (её) бежал малыш." },
                    { 27, false, 10, "На углу улицы я увидел мальчика, возле (его) стояла корзина с цветами." },
                    { 28, true, 10, "Когда Мурат свернул на шоссе, то увидел, что навстречу (ему) медленно движется колонна машин." },
                    { 29, false, 11, "выполняющееся" },
                    { 30, true, 11, "выполняемое" },
                    { 31, false, 11, "выполненное" },
                    { 32, false, 11, "выполнявшееся" },
                    { 33, true, 12, "прекрасный" },
                    { 34, false, 12, "ужасный" },
                    { 35, false, 12, "быстрый" },
                    { 36, false, 12, "медленный" },
                    { 37, true, 13, "низкий" },
                    { 38, false, 13, "высокий" },
                    { 39, false, 13, "широкий" },
                    { 40, false, 13, "длинный" },
                    { 41, true, 14, "бежать" },
                    { 42, false, 14, "дерево" },
                    { 43, false, 14, "красный" },
                    { 44, false, 14, "медленно" },
                    { 45, true, 15, "стол" },
                    { 46, false, 15, "быстро" },
                    { 47, false, 15, "играть" },
                    { 48, false, 15, "красивый" },
                    { 61, false, 20, "A больше" },
                    { 62, true, 20, "B больше" },
                    { 63, false, 20, " оба равны" },
                    { 64, false, 20, "нельзя сравнить" },
                    { 69, false, 18, "Грамматика это чудо" },
                    { 70, false, 18, "Грамматика это таска" },
                    { 71, false, 18, "Грамматика это пипец" },
                    { 72, true, 18, "Грамматика это 5 по русскому" },
                    { 73, false, 19, "A больше" },
                    { 74, true, 19, "B больше" },
                    { 75, false, 19, " оба равны" },
                    { 76, false, 19, "нельзя сравнить" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Content", "Instruction", "IsPublished", "Point", "TestId", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, "23 : 34", "Что больше?", false, 2, 1, "analogy", null },
                    { 2, "Что такое интеграл?", "Отвечайте на следующие вопросы", false, 2, 1, "general", null },
                    { 3, "Грамматика это___", "Отметьте вариант, наиболее близкий к контрольной паре", false, 3, 1, "general", null },
                    { 4, "Птица : Гнездо", "Отметьте вариант, наиболее близкий к контрольной паре", false, 3, 2, "analogy", null },
                    { 5, "Что такое интеграл?", "Отвечайте на следующие вопросы", false, 2, 2, "general", null },
                    { 6, "Lorem Ipsum - это текст-\"рыба\", часто используемый в печати и вэб-дизайне. Lorem Ipsum является стандартной \"рыбой\" для текстов на латинице с начала XVI века. В то время некий безымянный печатник создал большую коллекцию размеров и форм шрифтов, используя Lorem Ipsum для распечатки образцов. Lorem Ipsum не только успешно пережил без заметных изменений пять веков, но и перешагнул в электронный дизайн.", "Отвечайте на следующие вопросы", false, 2, 2, "general", null }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "IsCorrect", "QuestionId", "Text" },
                values: new object[,]
                {
                    { 1, false, 1, "A больше" },
                    { 2, true, 1, "B больше" },
                    { 3, false, 1, " оба равны" },
                    { 4, false, 1, "нельзя сравнить" },
                    { 5, false, 2, "Площадь под функции f(x)" },
                    { 6, false, 2, "интеграл от f(x) от 0 до x" },
                    { 7, true, 2, "Функция, производная которой равна f(x)" },
                    { 8, false, 2, "Функция, производная которой равна Y(f-k)" },
                    { 9, false, 3, "Грамматика это чудо" },
                    { 10, false, 3, "Грамматика это таска" },
                    { 11, false, 3, "Грамматика это пипец" },
                    { 12, true, 3, "Грамматика это 5 по русскому" },
                    { 49, false, 4, "A больше" },
                    { 50, true, 4, "B больше" },
                    { 51, false, 4, " оба равны" },
                    { 52, false, 4, "нельзя сравнить" },
                    { 53, false, 5, "Площадь под функции f(x)" },
                    { 54, false, 5, "интеграл от f(x) от 0 до x" },
                    { 55, true, 5, "Функция, производная которой равна f(x)" },
                    { 56, false, 5, "Функция, производная которой равна Y(f-k)" },
                    { 57, false, 6, "Грамматика это чудо" },
                    { 58, false, 6, "Грамматика это таска" },
                    { 59, false, 6, "Грамматика это пипец" },
                    { 60, true, 6, "Грамматика это 5 по русскому" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonModule_LessonId",
                table: "LessonModule",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubtopicId",
                table: "Lessons",
                column: "SubtopicId");

            migrationBuilder.CreateIndex(
                name: "IX_PassedQuestions_QuestionId",
                table: "PassedQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PassedQuestions_UserId",
                table: "PassedQuestions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReports_QuestionId",
                table: "QuestionReports",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionReports_UserId",
                table: "QuestionReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_UserId",
                table: "Statistics",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ParentId",
                table: "Subjects",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtopics_TopicId",
                table: "Subtopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_LessonId",
                table: "Tests",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_OrtInstructionId",
                table: "Tests",
                column: "OrtInstructionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tests_OrtTestId",
                table: "Tests",
                column: "OrtTestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SubjectId",
                table: "Tests",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_LessonId",
                table: "UserLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_SubjectId",
                table: "UserLessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_SubtopicId",
                table: "UserLessons",
                column: "SubtopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessons_UserId",
                table: "UserLessons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLevels_SubtopicId",
                table: "UserLevels",
                column: "SubtopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLevels_UserId",
                table: "UserLevels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserORTTests_OrtTestId",
                table: "UserORTTests",
                column: "OrtTestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserORTTests_UserId",
                table: "UserORTTests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LessonModule");

            migrationBuilder.DropTable(
                name: "PassedQuestions");

            migrationBuilder.DropTable(
                name: "QuestionReports");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "UserLessons");

            migrationBuilder.DropTable(
                name: "UserLevels");

            migrationBuilder.DropTable(
                name: "UserORTTests");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "OrtInstructions");

            migrationBuilder.DropTable(
                name: "OrtTests");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Subtopics");

            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
