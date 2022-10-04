using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel.ShopAdministration
{
    internal class ShopInactivityChangeViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zmiana statusu obszaru";
        private bool isCheckedChangeActiviti;
        public bool IsCheckedChangeActiviti
        {
            get => isCheckedChangeActiviti;
            set => SetProperty(ref isCheckedChangeActiviti, value);
        }
        public RelayCommand SubmitCommand { get; }
        public ShopInactivityChangeViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ChangeActivitiShop, CanSubmit);
            IsCheckedChangeActiviti = false;
            BadNameOrPass = Visibility.Collapsed;
            DataAssigment(managmentshopviewmodel);
            BadNameOrPass = Visibility.Collapsed;
        }
        public override void ResetErrorAndValues()
        {
            IsCheckedChangeActiviti = false;
            RemoveError();
            ClearAllValues();
        }
        private async void ChangeActivitiShop()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Status obszaru zmieniony.");
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

            if (ProtectedData.VerifyHashedPassword(LoggedPerson.Password, Password))
            {
                TblShop var = new();
                var = SelectedShopFromFirstWindow;
                if (var.ShopInactive == false)
                {
                    var.ShopInactive = true;
                    ManagmentShopViewModel.IsInactive = Visibility.Visible;
                }
                else
                {
                    var.ShopInactive = false;
                    ManagmentShopViewModel.IsInactive = Visibility.Collapsed;
                }
                var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
                var.ModWhen = DateTime.Now;
                Context.Entry(Context.TblShops.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First()).CurrentValues.SetValues(var);
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
            if (IsCheckedChangeActiviti && !string.IsNullOrWhiteSpace(Password))
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
                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Hasło jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Password)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    BadNameOrPass = Visibility.Collapsed;
                    break;
                case nameof(IsCheckedChangeActiviti):
                    if (!IsCheckedChangeActiviti)
                    {
                        _ValidationErrorsByProperty[nameof(IsCheckedChangeActiviti)] = new List<object> { "Potwierdzenie checkboxa jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(IsCheckedChangeActiviti)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(IsCheckedChangeActiviti)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(IsCheckedChangeActiviti)));
                    }
                    BadNameOrPass = Visibility.Collapsed;
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}


