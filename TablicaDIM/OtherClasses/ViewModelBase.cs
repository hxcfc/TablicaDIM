using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using TablicaDIM.DBModels;
using TablicaDIM.ViewModel;

namespace TablicaDIM.OtherClasses
{
    public class ViewModelBase : ObservableObject
    {
        private DimTabContext? _context;
        public virtual DimTabContext Context
        {
            get
            {
                if (_context != null)
                {
                    return _context;
                }
                else
                {
                    return new DimTabContext();
                }
            }
            set => SetProperty(ref _context, value);
        }
        private TblShop? _selectedShopFromFirstWindow;
        public virtual TblShop SelectedShopFromFirstWindow
        {
            get
            {
                if (_selectedShopFromFirstWindow != null)
                {
                    return _selectedShopFromFirstWindow;
                }
                else
                {
                    return new TblShop();
                }
            }
            set => SetProperty(ref _selectedShopFromFirstWindow, value);
        }
        private TblPerson? _loggedPerson;
        public virtual TblPerson LoggedPerson
        {
            get
            {
                if (_loggedPerson != null)
                {
                    return _loggedPerson;
                }
                else
                {
                    return new TblPerson();
                }
            }
            set => SetProperty(ref _loggedPerson, value);
        }
        private SnackbarMessageQueue? _boundMessageQueue;
        public virtual SnackbarMessageQueue BoundMessageQueue
        {
            get
            {
                if (_boundMessageQueue != null)
                {
                    return _boundMessageQueue;
                }
                else
                {
                    return new SnackbarMessageQueue();
                }
            }
            set => SetProperty(ref _boundMessageQueue, value);
        }
        private ManagmentShopViewModel? _magnagmentShopViewModel;
        public virtual ManagmentShopViewModel ManagmentShopViewModel
        {
            get
            {
                if (_magnagmentShopViewModel != null)
                {
                    return _magnagmentShopViewModel;
                }
                else
                {
                    return new ManagmentShopViewModel(new TblShop(), new DimTabContext());
                }
            }
            set => SetProperty(ref _magnagmentShopViewModel, value);
        }
    }
}
