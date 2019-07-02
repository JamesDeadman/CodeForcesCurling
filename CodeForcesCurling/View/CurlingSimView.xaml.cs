using System.Windows;
using CodeForcesCurling.ViewModel;

namespace CodeForcesCurling.Views
{
    public partial class CurlingSimView : Window
    {
        public CurlingSimView()
        {
            InitializeComponent();
            DataContext = new CurlingSimViewModel();
        }
    }
}
