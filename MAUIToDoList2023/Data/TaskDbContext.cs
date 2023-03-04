using MAUIToDoList2023.Models;
using Microsoft.EntityFrameworkCore;

namespace MAUIToDoList2023.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskItem> TaskItems { get; set; }

        public TaskDbContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            TaskItem task1 = new TaskItem()
            {
                TaskId = 1,
                Title = "PRG - úkolníček",
                Description = "Dodělat úkolníček pomocí MAUI s použitím SQLite.",
                EndDate = new DateTime(2024, 03, 03),
                Importance = Importance.High,
                IsDone = false
            };
            TaskItem task2 = new TaskItem()
            {
                TaskId = 2,
                Title = "WEB - API",
                EndDate = new DateTime(2023, 04, 10),
                Importance = Importance.Medium,
                IsDone = false
            };
            TaskItem task3 = new TaskItem()
            {
                TaskId = 3,
                Title = "ČJL - příprava test",
                Description = "májovci, ruchovci, lumírovci",
                EndDate = new DateTime(2024, 02, 26),
                Importance = Importance.Low,
                IsDone = true
            };
            TaskItem task4 = new TaskItem()
            {
                TaskId = 4,
                Title = "OPS - schéma",
                Description = "v Packet Traceru - statické směrování",
                EndDate = new DateTime(2023, 03, 25),
                Importance = Importance.High,
                IsDone = true
            };
            TaskItem task5 = new TaskItem()
            {
                TaskId = 5,
                Title = "MAT - limity",
                EndDate = new DateTime(2024, 03, 03),
                Importance = Importance.Medium,
                IsDone = false
            };
            
            mb.Entity<TaskItem>().HasData(task1);
            mb.Entity<TaskItem>().HasData(task2);
            mb.Entity<TaskItem>().HasData(task3);
            mb.Entity<TaskItem>().HasData(task4);
            mb.Entity<TaskItem>().HasData(task5);
        }
    }
}
