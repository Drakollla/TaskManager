using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Models;

namespace TaskManagerAPI.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    await context.Database.MigrateAsync();

                    if (await userManager.Users.AnyAsync())
                        return;

                    var adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        FirstName = "Admin",
                        LastName = "User",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(adminUser, "Password123!");

                    var bobUser = new User
                    {
                        UserName = "bob",
                        Email = "bob@example.com",
                        FirstName = "Bob",
                        LastName = "Smith",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(bobUser, "Password123!");

                    var workCat = new Category { Id = Guid.NewGuid(), Name = "Работа", UserId = adminUser.Id };
                    var healthCat = new Category { Id = Guid.NewGuid(), Name = "Здоровье", UserId = adminUser.Id };

                    var homeCat = new Category { Id = Guid.NewGuid(), Name = "Личное", UserId = bobUser.Id };
                    var studyCat = new Category { Id = Guid.NewGuid(), Name = "Обучение", UserId = bobUser.Id };

                    await context.Categories.AddRangeAsync(workCat, healthCat, homeCat, studyCat);

                    var urgentTag = new Tag { Id = Guid.NewGuid(), Name = "Срочно", ColorHex = "#FF0000", UserId = adminUser.Id };
                    var bugTag = new Tag { Id = Guid.NewGuid(), Name = "Баг", ColorHex = "#FFA500", UserId = adminUser.Id };
                    var featureTag = new Tag { Id = Guid.NewGuid(), Name = "Фича", ColorHex = "#0000FF", UserId = adminUser.Id };

                    var ideaTag = new Tag { Id = Guid.NewGuid(), Name = "Идея", ColorHex = "#008000", UserId = bobUser.Id };
                    var laterTag = new Tag { Id = Guid.NewGuid(), Name = "Потом", ColorHex = "#808080", UserId = bobUser.Id };

                    await context.Tags.AddRangeAsync(urgentTag, bugTag, featureTag, ideaTag, laterTag);

                    var tasks = new List<WorkTask>
                    {
                        new WorkTask
                        {
                            Id = Guid.NewGuid(),
                            Title = "Пофиксить критическую ошибку авторизации",
                            Description = "Пользователи не могут войти, падает 500.",
                            CreatedAt = DateTime.UtcNow.AddDays(-5),
                            DueDate = DateTime.UtcNow.AddDays(-1),
                            Priority = Priority.High,
                            Status = WorkTaskStatus.Todo,
                            CategoryId = workCat.Id, 
                            UserId = adminUser.Id,   
                            Tags = new List<Tag> { urgentTag, bugTag }
                        },
                        new WorkTask
                        {
                            Id = Guid.NewGuid(),
                            Title = "Записаться к стоматологу",
                            Description = "Зуб ноет",
                            CreatedAt = DateTime.UtcNow.AddHours(-2),
                            DueDate = DateTime.UtcNow.AddHours(4),
                            Priority = Priority.High,
                            Status = WorkTaskStatus.Todo,
                            CategoryId = healthCat.Id,
                            UserId = adminUser.Id,
                            Tags = new List<Tag> { urgentTag }
                        },
                        new WorkTask
                        {
                            Id = Guid.NewGuid(),
                            Title = "Реализовать Пагинацию в API",
                            Description = "Сделать PagedList",
                            CreatedAt = DateTime.UtcNow.AddDays(-1),
                            DueDate = DateTime.UtcNow.AddDays(2),
                            Priority = Priority.Medium,
                            Status = WorkTaskStatus.InProgress,
                            CategoryId = workCat.Id,
                            UserId = adminUser.Id,
                            Tags = new List<Tag> { featureTag }
                        },

                        new WorkTask
                        {
                            Id = Guid.NewGuid(),
                            Title = "Купить продукты на неделю",
                            Description = "Молоко, яйца, хлеб",
                            CreatedAt = DateTime.UtcNow,
                            DueDate = DateTime.UtcNow.AddDays(1),
                            Priority = Priority.Low,
                            Status = WorkTaskStatus.Todo,
                            CategoryId = homeCat.Id,
                            UserId = bobUser.Id,  
                            Tags = new List<Tag>()
                        },
                        new WorkTask
                        {
                            Id = Guid.NewGuid(),
                            Title = "Изучить Docker и Kubernetes",
                            Description = "Посмотреть курсы",
                            CreatedAt = DateTime.UtcNow.AddDays(-10),
                            DueDate = DateTime.UtcNow.AddMonths(1),
                            Priority = Priority.Medium,
                            Status = WorkTaskStatus.Todo,
                            CategoryId = studyCat.Id,
                            UserId = bobUser.Id,      
                            Tags = new List<Tag> { ideaTag, laterTag }
                        },
                        new WorkTask
                        {
                            Id = Guid.NewGuid(),
                            Title = "Настроить Git репозиторий",
                            Description = "Инициализация",
                            CreatedAt = DateTime.UtcNow.AddDays(-20),
                            DueDate = DateTime.UtcNow.AddDays(-15),
                            Priority = Priority.Low,
                            Status = WorkTaskStatus.Done,
                            CategoryId = studyCat.Id,
                            UserId = bobUser.Id,
                            Tags = new List<Tag> { ideaTag }
                        }
                    };

                    await context.WorkTasks.AddRangeAsync(tasks);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}