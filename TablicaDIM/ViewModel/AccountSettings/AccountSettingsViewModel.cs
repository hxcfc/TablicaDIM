using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel.AccountSettings
{
    internal class AccountSettingsViewModel : InputsViewModel, IMenuItem
    {
        public string Title { get; } = "Ustawienia pracownika";
        public static string TitleToMenu { get; } = "Ustawienia pracownika";
        private object? _selectedObject;
        public object? SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (SetProperty(ref _selectedObject, value))
                {
                    VMAccountChangeInf.SetData();
                    VMAccountChangePass.ResetErrorAndValues();
                }
            }
        }
        private AccountChangeInformationViewModel _vMAccountChangeInf;
        public AccountChangeInformationViewModel VMAccountChangeInf
        {
            get => _vMAccountChangeInf;
            set => SetProperty(ref _vMAccountChangeInf, value);
        }
        private AccountChangePasswordViewModel _vMAccountChangePass;
        public AccountChangePasswordViewModel VMAccountChangePass
        {
            get => _vMAccountChangePass;
            set => SetProperty(ref _vMAccountChangePass, value);
        }

        public AccountSettingsViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            DataAssigment(managmentshopviewmodel);
            VMAccountChangeInf = new AccountChangeInformationViewModel(ManagmentShopViewModel);
            VMAccountChangePass = new AccountChangePasswordViewModel(ManagmentShopViewModel);
            SelectedObject = VMAccountChangePass;
        }
    }
}
