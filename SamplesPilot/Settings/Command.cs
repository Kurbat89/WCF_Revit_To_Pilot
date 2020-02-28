using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PilotSamples.Settings
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _func;

        public Command(Action func)
        {
            _func = func;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _func.Invoke();
        }
    }
}
