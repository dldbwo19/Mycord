﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Mycord_WPF
{
    public class RelayCommand : ICommand
    {
        private Action mAction;


        public event EventHandler CanExecuteChanged = (sender, e) => { };


        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            mAction();
        }

    }
}
