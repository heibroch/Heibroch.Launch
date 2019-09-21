using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Heibroch.Launch
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> _action;

        public ActionCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _action(parameter);

        public event EventHandler CanExecuteChanged;
    }
}
