using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int BoardColumnId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsFavorite { get; set; }
        // URL for an attached image (optional)
        public string ImageUrl { get; set; }
        public virtual BoardColumn BoardColumn { get; set; }
    }
}
