using System.Windows;
using System.Windows.Input;
using TablicaDIM.DBModels;
using TablicaDIM.ViewModel;
using Microsoft.Toolkit.Mvvm.Input;



namespace TablicaDIM
{
    public partial class SelectShop : Window
    {


        public SelectShop()
        {
            InitializeComponent();
            DataContext = new SelectShopViewModel(new DimTabContext());
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
