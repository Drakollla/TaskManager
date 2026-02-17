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

                    //var adminUser = new User
                    //{
                    //    UserName = "admin",
                    //    Email = "admin@example.com",
                    //    FirstName = "Admin",
                    //    LastName = "User",
                    //    EmailConfirmed = true
                    //};

                    //var result = await userManager.CreateAsync(adminUser, "Password123!");

                    //if (!result.Succeeded)
                    //    throw new Exception("Failed to create seed user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                    var adminUser = await userManager.FindByNameAsync("admin");

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            UserName = "admin",
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            LastName = "User",
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(adminUser, "Password123!");
                        if (!result.Succeeded)
                        {
                            throw new Exception("Не удалось создать админа: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }


                    var workCat = new Category { Id = Guid.NewGuid(), Name = "Работа" };
                    var homeCat = new Category { Id = Guid.NewGuid(), Name = "Личное" };
                    var studyCat = new Category { Id = Guid.NewGuid(), Name = "Обучение" };
                    var healthCat = new Category { Id = Guid.NewGuid(), Name = "Здоровье" };

                    await context.Categories.AddRangeAsync(workCat, homeCat, studyCat, healthCat);

                    var urgentTag = new Tag { Id = Guid.NewGuid(), Name = "Срочно", ColorHex = "#FF0000" };
                    var bugTag = new Tag { Id = Guid.NewGuid(), Name = "Баг", ColorHex = "#FFA500" };
                    var featureTag = new Tag { Id = Guid.NewGuid(), Name = "Фича", ColorHex = "#0000FF" };
                    var ideaTag = new Tag { Id = Guid.NewGuid(), Name = "Идея", ColorHex = "#008000" };
                    var laterTag = new Tag { Id = Guid.NewGuid(), Name = "Потом", ColorHex = "#808080" };

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
                        Description = "Зуб ноет, надо позвонить",
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
                        Description = "Нужно сделать PagedList и вернуть метаданные в заголовке",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        DueDate = DateTime.UtcNow.AddDays(2),
                        Priority = Priority.Medium,
                        Status = WorkTaskStatus.InProgress,
                        CategoryId = workCat.Id,
                        Tags = new List<Tag> { featureTag },
                        UserId = adminUser.Id
                    },

                    new WorkTask
                    {
                        Id = Guid.NewGuid(),
                        Title = "Купить продукты на неделю",
                        Description = "Молоко, яйца, хлеб, курица",
                        CreatedAt = DateTime.UtcNow,
                        DueDate = DateTime.UtcNow.AddDays(1),
                        Priority = Priority.Low,
                        Status = WorkTaskStatus.Todo,
                        CategoryId = homeCat.Id,
                        Tags = new List<Tag>(),
                        UserId = adminUser.Id
                    },

                    new WorkTask
                    {
                        Id = Guid.NewGuid(),
                        Title = "Изучить Docker и Kubernetes",
                        Description = "Посмотреть курсы, поднять кластер локально",
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        DueDate = DateTime.UtcNow.AddMonths(1),
                        Priority = Priority.Medium,
                        Status = WorkTaskStatus.Todo,
                        CategoryId = studyCat.Id,
                        UserId = adminUser.Id,
                        Tags = new List<Tag> { ideaTag, laterTag }
                    },

                    new WorkTask
                    {
                        Id = Guid.NewGuid(),
                        Title = "Настроить Git репозиторий",
                        Description = "Инициализация, .gitignore",
                        CreatedAt = DateTime.UtcNow.AddDays(-20),
                        DueDate = DateTime.UtcNow.AddDays(-15),
                        Priority = Priority.Low,
                        Status = WorkTaskStatus.Done,
                        CategoryId = workCat.Id,
                        UserId = adminUser.Id,
                        Tags = new List<Tag> { featureTag },
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
