using Domain.Models;

namespace Repository.Extensions
{
    public static class RepositoryWorkTaskExtensions
    {
        public static IQueryable<WorkTask> FilterWorkTasks(this IQueryable<WorkTask> tasks, DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue && !maxDate.HasValue)
                return tasks;

            var min = minDate ?? DateTime.MinValue;
            var max = maxDate ?? DateTime.MaxValue;

            return tasks.Where(t => t.DueDate >= min && t.DueDate <= max);
        }

        public static IQueryable<WorkTask> Search(this IQueryable<WorkTask> tasks, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return tasks;

            var lowerTerm = searchTerm.Trim().ToLower();

            return tasks.Where(t => t.Title.ToLower().Contains(lowerTerm));
        }
    }
}