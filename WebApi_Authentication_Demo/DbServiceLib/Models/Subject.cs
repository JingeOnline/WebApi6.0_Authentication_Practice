using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DbServiceLib.Models
{
    public class Subject
    {
        public int PkId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //这里使用virtual是为了启用延迟加载（Lazy Loading）功能，这样在访问Students属性时，相关的Student数据会在需要时才从数据库中加载，
        public virtual List<Student> Students { get; set; }
    }
}
