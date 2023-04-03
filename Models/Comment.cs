using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        [Display(Name = "Created at")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last updated at")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }

        public int TopicId { get; set; }
        public Topic? Topic { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
