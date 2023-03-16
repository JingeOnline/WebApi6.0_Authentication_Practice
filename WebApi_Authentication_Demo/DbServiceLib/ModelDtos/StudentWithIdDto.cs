using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib.ModelDtos
{
    /// <summary>
    /// Student with Id but without List<Subject>
    /// </summary>
    public class StudentWithIdDto
    {
        public int Pkid { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public bool Gender { get; set; }
        [Range(0, 150)]
        public int Age { get; set; }

        public Student ToStudent()
        {
            return new Student { PkId=Pkid, Name = Name, Gender = Gender, Age = Age };
        }
    }
}
