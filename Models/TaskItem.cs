using System.ComponentModel.DataAnnotations;
namespace TaskManager.Models
{
    public class TaskItem
    { 
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public string userId { get; set; } = string.Empty;
    }
}
