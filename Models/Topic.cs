using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        [Display(Name = "Created at")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set;}

        [Display(Name = "Last updated at")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set;}
        public string? Content { get; set; }
        public string? Category { get; set; }

        public List<Comment>? Comments { get; set; }
        public int UserId { get; set; }
    }
}
