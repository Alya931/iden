using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace iden.ViewModels
{
    public class RoleFormViewModel
    {
        [Required, StringLength(260)]
        public string Name { set; get; }
    }
}
