using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set;}
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set;}
        public string? Content { get; set; }
        public string? Category { get; set; }
    }
}
