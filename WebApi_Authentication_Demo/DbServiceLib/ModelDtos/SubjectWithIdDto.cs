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
    /// Subject with pkid but without List<Student>
    /// </summary>
    public class SubjectWithIdDto
    {
        public int PkId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public Subject ToSubject()
        {
            return new Subject { PkId=PkId, Name = Name, Description = Description };
        }
    }
}
