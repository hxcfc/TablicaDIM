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
    internal class FirstLoginViewModel : InputsViewModel, INotifyDataErrorInfo
    {
        public RelayCommand SubmitCommand { get; }
        public FirstLoginViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ChangePassword, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            BadNameOrPass = Visibility.Collapsed;
        }
        private bool CanSubmit()
        {
            if (!(string.IsNullOrWhiteSpace(Password)) && !(string.IsNullOrWhiteSpace(ConfirmPassword)))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async void ChangePassword()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    DialogHost.CloseDialogCommand.Execute(true, null);
                }
                else
                {
                    BadNameOrPass = Visibility.Visible;
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            TblPerson var = new();
            var = LoggedPerson;
            var.FirstLogin = false;
            var.Password = ProtectedData.HashPassword(Password);
            var.ModWhen = DateTime.Now;
            var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
            Context.Entry(Context.TblPersons.Where(d => d.Login == LoggedPerson.Login).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
            Context.SaveChanges();
            return true;
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
                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Nowe hasło użytkownika nie może być pusty." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (Password.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Nowe hasło użytkownika musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Password)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                        BadNameOrPass = Visibility.Collapsed;
                    }
                    break;
                case nameof(ConfirmPassword):
                    if (string.IsNullOrWhiteSpace(ConfirmPassword))
                    {
                        _ValidationErrorsByProperty[nameof(ConfirmPassword)] = new List<object> { "Powtórzonie nowego hasła jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                    }
                    else if (ConfirmPassword != Password)
                    {
                        _ValidationErrorsByProperty[nameof(ConfirmPassword)] = new List<object> { "Hasła nie są takie same." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ConfirmPassword)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                        BadNameOrPass = Visibility.Collapsed;
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}
