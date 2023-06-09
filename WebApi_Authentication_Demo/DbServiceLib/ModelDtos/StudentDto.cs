﻿using DbServiceLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbServiceLib.ModelDtos
{
    public class StudentDto
    {

        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Gender is required.")]
        public bool Gender { get; set; }
        [Range(0,150)]
        public int Age { get; set; }

        public Student ToStudent()
        {
            return new Student {Name = Name, Gender = Gender, Age = Age };
        }
        public Student ToStudent(int pkid)
        {
            return new Student {PkId=pkid, Name = Name, Gender=Gender, Age=Age};
        }
    }
}
