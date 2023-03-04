using System.ComponentModel.DataAnnotations;

namespace MAUIToDoList2023.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool IsDone { get; set; } = false;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7);
        public Importance Importance { get; set; } = Importance.Low;
    }
}
