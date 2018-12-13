using System;
using System.Collections.Generic;
using System.Text;

namespace EffortlessStdLibrary.Models
{
    public class DepartmentModel : IEffortlessModel
    {
        public long Id { get; set; }
        public int Pno { get; set; }
        public string Name { get; set; }
        public IList<WorkPeriodModel> WorkPeriods { get; set; }
        public AddressModel Address { get; set; }
    }
}
