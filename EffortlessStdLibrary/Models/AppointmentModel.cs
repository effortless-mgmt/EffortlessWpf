using System;
using System.Collections.Generic;
using System.Text;

namespace EffortlessStdLibrary.Models
{
    public class AppointmentModel : IEffortlessModel
    {
        public long Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public int Break { get; set; }
        public WorkPeriodModel WorkPeriod { get; set; }
        public UserModel Owner { get; set; }
    }
}
