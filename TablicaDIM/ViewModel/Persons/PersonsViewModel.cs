using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Persons
{
    public class PersonsViewModel : InputsViewModel, IMenuItem
    {
        public static string TitleToMenu { get; } = "Zarządzanie pracownikami";
        public string Title { get; } = "Zarządzanie pracownikami";
        private object? _selectedObject;
        public object? SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (SetProperty(ref _selectedObject, value))
                {
                    VMPersonsAdd.ResetErrorAndValues();
                    VMPersonsMod.ResetErrorAndValues();
                    VMPersonsMod.BackPage();
                    VMPersonsMod.UpdateData();
                    VMPersonsDel.BackPage();
                    VMPersonsDel.ResetErrorAndValues();
                    VMPersonsDel.UpdateData();
                }
            }
        }

        private PersonsAddViewModel _vMPersonsAdd;
        public PersonsAddViewModel VMPersonsAdd
        {
            get => _vMPersonsAdd;
            set => SetProperty(ref _vMPersonsAdd, value);
        }
        private PersonsModViewModel _vMPersonsMod;
        public PersonsModViewModel VMPersonsMod
        {
            get => _vMPersonsMod;
            set => SetProperty(ref _vMPersonsMod, value);
        }
        private PersonsDelViewModel _vMPersonsDel;
        public PersonsDelViewModel VMPersonsDel
        {
            get => _vMPersonsDel;
            set => SetProperty(ref _vMPersonsDel, value);
        }
        public PersonsViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            DataAssigment(managmentshopviewmodel);
            VMPersonsAdd = new PersonsAddViewModel(ManagmentShopViewModel);
            VMPersonsMod = new PersonsModViewModel(ManagmentShopViewModel);
            VMPersonsDel = new PersonsDelViewModel(ManagmentShopViewModel);
            SelectedObject = VMPersonsAdd;
        }
    }
}
