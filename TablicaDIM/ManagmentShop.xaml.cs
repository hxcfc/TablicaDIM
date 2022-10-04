using System.Windows;
using System.Windows.Input;
using TablicaDIM.DBModels;
using TablicaDIM.ViewModel;

namespace TablicaDIM
{
    /// <summary>
    /// Interaction logic for ManagmentShop.xaml
    /// </summary>
    public partial class ManagmentShop : Window
    {
        public ManagmentShop(TblShop selectedshop, DimTabContext context)
        {
            DataContext = new ManagmentShopViewModel(selectedshop, context);
            InitializeComponent();
        }
        // In future need to replace below code to MVVM.
        //                            ||
        //                            ||
        //                            \/
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
