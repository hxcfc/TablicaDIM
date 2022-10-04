using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel
{
    internal class LoginViewModel : InputsViewModel, INotifyDataErrorInfo
    {

        private Visibility _inactiveShop;
        public Visibility InactiveShop
        {
            get => _inactiveShop;
            set => SetProperty(ref _inactiveShop, value);
        }
        public RelayCommand SubmitCommand { get; }
        public LoginViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(LogIn, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            BadNameOrPass = Visibility.Collapsed;
            InactiveShop = Visibility.Collapsed;
        }
        private bool CanSubmit()
        {
            if (!(string.IsNullOrWhiteSpace(Login)) && !(string.IsNullOrWhiteSpace(Password)))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async void LogIn()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    if (SelectedShopFromFirstWindow.ShopInactive == false)
                    {
                        LoggedPerson = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(b => b.Login == Login).First();
                        DialogHost.CloseDialogCommand.Execute(true, null);
                    }
                    else
                    {
                        if (Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(b => b.Login == Login).Where(d => d.PermisionId == 1).Count() > 0)
                        {
                            LoggedPerson = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(b => b.Login == Login).First();
                            DialogHost.CloseDialogCommand.Execute(true, null);
                        }
                        else
                        {
                            ClearAllValues();
                            RemoveError();
                            InactiveShop = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    ClearAllValues();
                    BadNameOrPass = Visibility.Visible;
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            List<TblPerson> shopuser = new();
            shopuser = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(b => b.Login == Login).ToList();

            if ((shopuser.Count > 0) && (ProtectedData.VerifyHashedPassword(shopuser.First().Password, Password)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            Validate(e.PropertyName);
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void Validate(string changedPropertyName)
        {
            switch (changedPropertyName)
            {
                case nameof(Login):
                    if (string.IsNullOrWhiteSpace(Login))
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login pracownika nie może być pusty." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Login)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    InactiveShop = Visibility.Collapsed;
                    BadNameOrPass = Visibility.Collapsed;
                    break;
                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Hasło pracownika nie może być puste." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Password)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    InactiveShop = Visibility.Collapsed;
                    BadNameOrPass = Visibility.Collapsed;
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}
