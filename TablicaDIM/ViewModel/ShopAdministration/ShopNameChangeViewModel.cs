using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel.ShopAdministration
{
    internal class ShopNameChangeViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zmiana nazwy obszaru";
        public RelayCommand SubmitCommand { get; }
        private readonly List<string> shopNameList;
        public ShopNameChangeViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ChangeNameShop, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            BadNameOrPass = Visibility.Collapsed;
            List<TblShop> TblShops = new();
            TblShops = Context.TblShops.ToList();
            shopNameList = new List<string>();
            foreach (var shop in TblShops)
            {
                shopNameList.Add(shop.ShopName);
            }
            ResetErrorAndValues();
        }
        private async void ChangeNameShop()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    _ValidationErrorsByProperty.Clear();
                    BoundMessageQueue.Enqueue("Nazwa obszaru zmieniona.");
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
                TblShop var = new();
                var = SelectedShopFromFirstWindow;
                var.ShopName = ShopName;
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
            if (!string.IsNullOrWhiteSpace(ShopName) && !string.IsNullOrWhiteSpace(Password))
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
                case nameof(ShopName):
                    if (string.IsNullOrWhiteSpace(ShopName))
                    {
                        _ValidationErrorsByProperty[nameof(ShopName)] = new List<object> { "Nazwa obszaru jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ShopName)));
                    }
                    else if (!Regex.IsMatch(ShopName, "^[A-ZŁĆŚĘŃÓŹ]"))
                    {
                        _ValidationErrorsByProperty[nameof(ShopName)] = new List<object> { "Nazwa obszaru musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ShopName)));
                    }
                    else if (ShopName.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(ShopName)] = new List<object> { "Nazwa obszaru musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ShopName)));
                    }
                    else if (shopNameList.Contains(ShopName.ToString()))
                    {
                        _ValidationErrorsByProperty[nameof(ShopName)] = new List<object> { "Istnieje już taka nazwa obszaru." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ShopName)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(ShopName)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ShopName)));
                    }
                    BadNameOrPass = Visibility.Collapsed;
                    break;
                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Hasło administratora jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Password)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    BadNameOrPass = Visibility.Collapsed;
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}

