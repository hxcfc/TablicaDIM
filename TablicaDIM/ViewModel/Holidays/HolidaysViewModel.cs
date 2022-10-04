using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Holidays
{
    public class HolidaysViewModel : InputsViewModel, IMenuItem
    {
        public static string TitleToMenu { get; } = "Urlopy";
        public string Title { get; } = "Urlopy";
        private object? _selectedObject;
        public object? SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (SetProperty(ref _selectedObject, value))
                {
                    VMHolidaysCalendar.NewData();
                    VMHolidaysApplication.UpdateData();
                    VMFreeDaysManagment.NewData();
                    VMHolidaysManagment.UpdateData();

                }
            }
        }
        private HolidaysCalendarViewModel _vMHolidaysCalendar;
        public HolidaysCalendarViewModel VMHolidaysCalendar
        {
            get => _vMHolidaysCalendar;
            set => SetProperty(ref _vMHolidaysCalendar, value);
        }
        private FreeDaysManagmentViewModel _vMFreeDaysManagment;
        public FreeDaysManagmentViewModel VMFreeDaysManagment
        {
            get => _vMFreeDaysManagment;
            set => SetProperty(ref _vMFreeDaysManagment, value);
        }
        private HolidaysManagmentViewModel _vMHolidaysManagment;
        public HolidaysManagmentViewModel VMHolidaysManagment
        {
            get => _vMHolidaysManagment;
            set => SetProperty(ref _vMHolidaysManagment, value);
        }
        private HolidaysApplicationViewModel _vMHolidaysApplication;
        public HolidaysApplicationViewModel VMHolidaysApplication
        {
            get => _vMHolidaysApplication;
            set => SetProperty(ref _vMHolidaysApplication, value);
        }

        public HolidaysViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            DataAssigment(managmentshopviewmodel);
            VMHolidaysCalendar = new HolidaysCalendarViewModel(ManagmentShopViewModel);
            VMHolidaysApplication = new HolidaysApplicationViewModel(ManagmentShopViewModel);
            VMFreeDaysManagment = new FreeDaysManagmentViewModel(ManagmentShopViewModel);
            VMHolidaysManagment = new HolidaysManagmentViewModel(ManagmentShopViewModel);
            SelectedObject = VMHolidaysCalendar;
        }
    }
}
