using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class RelayParameterCommand : ICommand
    {
        private Action<object> mAction;
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayParameterCommand(Action<object> action)
        {
            mAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction(parameter);
        }
    }
}
