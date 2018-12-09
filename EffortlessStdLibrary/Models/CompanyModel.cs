using System;
using System.Collections.Generic;
using System.Text;

namespace EffortlessStdLibrary.Models
{
    public class CompanyModel : IEffortlessModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Vat { get; set; }
        public int Pno { get; set; }
    }
}
