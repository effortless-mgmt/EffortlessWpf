using System;
using System.Collections.Generic;
using System.Text;

namespace EffortlessStdLibrary.Models
{
    public class RoleModel : IEffortlessModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
