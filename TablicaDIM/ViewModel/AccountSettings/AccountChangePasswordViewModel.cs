using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel.AccountSettings
{
    public class AccountChangePasswordViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zmiana hasła pracownika";
        public RelayCommand SubmitCommand { get; }
        public AccountChangePasswordViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ChangePassword, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            BadNameOrPass = Visibility.Collapsed;
            ResetErrorAndValues();
        }
        private async void ChangePassword()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Hasło pracownika zmienione.");
                }
                else
                {
                    BadNameOrPass = Visibility.Visible;
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            if (ProtectedData.VerifyHashedPassword(LoggedPerson.Password, Password))
            {
                TblPerson var = new();
                var = LoggedPerson;
                var.Password = ProtectedData.HashPassword(NewPassword);
                var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
                var.ModWhen = DateTime.Now;
                Context.Entry(Context.TblPersons.Where(d => d.Login == LoggedPerson.Login).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                ManagmentShopViewModel.SelectHomeView();
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                return !HasErrors;
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
                case nameof(NewPassword):
                    if (string.IsNullOrWhiteSpace(NewPassword))
                    {
                        _ValidationErrorsByProperty[nameof(NewPassword)] = new List<object> { "Nowe hasło pracownika nie może być pusty." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewPassword)));
                    }
                    else if (NewPassword.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(NewPassword)] = new List<object> { "Nowe hasło pracownika musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewPassword)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(NewPassword)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewPassword)));
                        BadNameOrPass = Visibility.Collapsed;
                    }
                    break;
                case nameof(ConfirmPassword):
                    if (string.IsNullOrWhiteSpace(ConfirmPassword))
                    {
                        _ValidationErrorsByProperty[nameof(ConfirmPassword)] = new List<object> { "Powtórzonie nowego hasła jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                    }
                    else if (ConfirmPassword != NewPassword)
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
                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Hasło jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Password)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                        BadNameOrPass = Visibility.Collapsed;
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}

