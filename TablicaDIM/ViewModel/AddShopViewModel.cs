using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel
{
    public class AddShopViewModel : InputsViewModel, INotifyDataErrorInfo
    {
        public DialogSession DialogSession { get; set; }
        public RelayCommand SubmitCommand { get; }
        private readonly List<string> shopNameList;
        public AddShopViewModel(DimTabContext context)
        {
            SubmitCommand = new RelayCommand(AddShop, CanSubmit);
            Context = context;
            ObservableCollection<TblShop> TblShops = new();
            TblShops = Context.TblShops.Local.ToObservableCollection();
            shopNameList = new List<string>();
            foreach (var shop in TblShops)
            {
                shopNameList.Add(shop.ShopName);
            }
        }
        private async void AddShop()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    DialogSession.UpdateContent(new MessageShowOkViewModel("Dodawanie nowego obszaru powiodło się.", DialogSession));
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            TblShop newShop = new() { ShopName = ShopName, AddWhen = DateTime.Now, AddWho = WindowsIdentity.GetCurrent().Name.ToString() };
            Context.TblShops.Add(newShop);
            Context.SaveChanges();
            int shopId = Context.TblShops.Where(b => b.ShopName == ShopName).First().ShopId;
            TblPerson newPerson = new() { Login = Login, Name = Name, Surname = Surname, Password = ProtectedData.HashPassword(Password), ShopId = shopId, Email = Email, FirstLogin = false, AddWho = WindowsIdentity.GetCurrent().Name.ToString(), AddWhen = DateTime.Now, PermisionId = 1 };
            Context.TblPersons.Add(newPerson);
            Context.SaveChanges();
            string whoAdd = Context.TblPersons.Where(b => b.Login == Login).Where(b => b.ShopId == shopId).First().Name + " " + Context.TblPersons.Where(b => b.Login == Login).Where(b => b.ShopId == shopId).First().Surname;
            List<TblChart> listOfChart = new();
            for (int i = 0; i < 52; i++)
            {
                listOfChart.Add(new() { ShopId = shopId, AddWho = whoAdd, AddWhen = DateTime.Now, NumberOfWeek = i + 1, Year = DateTime.Now.Year });
                Context.TblCharts.Add(listOfChart[i]);
            }
            Context.SaveChanges();
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            return true;
        }
        private bool CanSubmit()
        {
            if (!(string.IsNullOrWhiteSpace(ShopName)) && !(string.IsNullOrWhiteSpace(Login)) && !(string.IsNullOrWhiteSpace(Password)) && !(string.IsNullOrWhiteSpace(ConfirmPassword)))
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
                    break;
                case nameof(Login):
                    if (string.IsNullOrWhiteSpace(Login))
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login administratora jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    else if (Login.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login administratora musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Login)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    break;
                case nameof(Name):
                    if (string.IsNullOrWhiteSpace(Name))
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię administratora jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (!Regex.IsMatch(Name, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię administratora musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (!Regex.IsMatch(Name, @"^[A-ZŻŹĆĄŚĘÓŁŃa-zżźćąśęółń \-?]+$"))
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię posiada złe znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (Name.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię administratora musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Name)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    break;
                case nameof(Surname):
                    if (string.IsNullOrWhiteSpace(Surname))
                    {
                        _ValidationErrorsByProperty[nameof(Surname)] = new List<object> { "Nazwisko administratora jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Surname)));
                    }
                    else if (!Regex.IsMatch(Surname, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(Surname)] = new List<object> { "Nazwisko musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Surname)));
                    }
                    else if (!Regex.IsMatch(Surname, @"^[A-ZŻŹĆĄŚĘÓŁŃa-zżźćąśęółń \-?]+$"))
                    {
                        _ValidationErrorsByProperty[nameof(Surname)] = new List<object> { "Nazwisko posiada złe znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Surname)));
                    }
                    else if (Surname.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Surname)] = new List<object> { "Nazwisko administratora musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Surname)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Surname)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Surname)));
                    }
                    break;
                case nameof(Email):
                    if (string.IsNullOrWhiteSpace(Email))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email administratora jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    else if (!Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email administratora musi wyglądać xx@xx.pl." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Email)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    break;
                case nameof(Password):
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Hasło administratora jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (Password.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Password)] = new List<object> { "Hasło administratora musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(Password)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
                    }
                    break;
                case nameof(ConfirmPassword):
                    if (string.IsNullOrWhiteSpace(ConfirmPassword))
                    {
                        _ValidationErrorsByProperty[nameof(ConfirmPassword)] = new List<object> { "Powtórzenie hasła administratora jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                    }
                    else if (Password != ConfirmPassword)
                    {
                        _ValidationErrorsByProperty[nameof(ConfirmPassword)] = new List<object> { "Hasła nie są takie same." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ConfirmPassword)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ConfirmPassword)));
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}
