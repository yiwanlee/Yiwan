using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCoreNull.Models
{
    public class EmployeeEditViewModel
    {
        [Required, MaxLength(64), MinLength(5, ErrorMessage = "最小长度5")]
        public string Name { get; set; }
    }
}
