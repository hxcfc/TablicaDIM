using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Places
{
    public class PlacesViewModel : InputsViewModel, IMenuItem
    {
        public static string TitleToMenu { get; } = "Zarządzanie stanowiskami";
        public string Title { get; } = "Zarządzanie stanowiskami";
        private object? _selectedObject;
        public object? SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (SetProperty(ref _selectedObject, value))
                {
                    VMPlacesAdd.ResetErrorAndValues();
                    VMPlacesMod.ResetErrorAndValues();
                    VMPlacesMod.BackPage();
                    VMPlacesMod.UpdateData();
                    VMPlacesDel.BackPage();
                    VMPlacesDel.ResetErrorAndValues();
                    VMPlacesDel.UpdateData();
                }
            }
        }

        private PlacesAddViewModel _vMPlacesAdd;
        public PlacesAddViewModel VMPlacesAdd
        {
            get => _vMPlacesAdd;
            set => SetProperty(ref _vMPlacesAdd, value);
        }
        private PlacesModViewModel _vMPlacesMod;
        public PlacesModViewModel VMPlacesMod
        {
            get => _vMPlacesMod;
            set => SetProperty(ref _vMPlacesMod, value);
        }
        private PlacesDelViewModel _vMPlacesDel;
        public PlacesDelViewModel VMPlacesDel
        {
            get => _vMPlacesDel;
            set => SetProperty(ref _vMPlacesDel, value);
        }
        public PlacesViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            DataAssigment(managmentshopviewmodel);
            VMPlacesAdd = new PlacesAddViewModel(ManagmentShopViewModel);
            VMPlacesMod = new PlacesModViewModel(ManagmentShopViewModel);
            VMPlacesDel = new PlacesDelViewModel(ManagmentShopViewModel);
            SelectedObject = VMPlacesAdd;
        }
    }
}
