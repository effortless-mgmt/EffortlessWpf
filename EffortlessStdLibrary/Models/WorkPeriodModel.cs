using System;
using System.Collections.Generic;
using System.Text;

namespace EffortlessStdLibrary.Models
{
    public class WorkPeriodModel : IEffortlessModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public bool Active { get; set; }
        public IEnumerable<UserModel> AssignedUsers { get; set; }
        public IEnumerable<AppointmentModel> Appointments { get; set; }
        public DepartmentModel Department { get; set; }
    }
}
