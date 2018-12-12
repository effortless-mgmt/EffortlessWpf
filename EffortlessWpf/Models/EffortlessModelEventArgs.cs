using EffortlessStdLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.Models
{
    public class EffortlessModelEventArgs : EventArgs
    {
        public IEffortlessModel Model { get; set; }

        public EffortlessModelEventArgs(IEffortlessModel model)
        {
            Model = model;
        }
    }
}
