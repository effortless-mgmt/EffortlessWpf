using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortlessWpf.ViewModels
{
    public interface IGenericDataViewModel : IScreen
    {
        Task LoadItemsAsync();
    }
}
