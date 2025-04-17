using System.ComponentModel.DataAnnotations;
using static TaskManagementApp.Shared.Enum;

namespace TaskManagementApp.Models
{
    public class BoardColumn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BoardColumnType ColumnType { get; set; }
    }
}
