using System;
using System.Windows.Input;

namespace CodeForcesCurling.ViewModel
{
    public class SimpleDelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action commandAction;

        public SimpleDelegateCommand(Action commandAction)
        {
            this.commandAction = commandAction;
        }

        public bool CanExecute(object parameter)
        {
            return commandAction != null;
        }

        public void Execute(object parameter)
        {
            commandAction.Invoke();
        }
    }

}
