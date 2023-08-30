namespace TaskManagementSystem.Model
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsFavorite { get; set; }
    }
}
