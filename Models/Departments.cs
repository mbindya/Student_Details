using System.ComponentModel.DataAnnotations;

namespace Student_Department.Models
{
    public class Departments
    {
        [Key]
        public int ID { get; set; }

        public string Department { get; set; }
    }
}
