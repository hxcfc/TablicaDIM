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

namespace TablicaDIM.ViewModel.AccountSettings
{
    public class AccountChangeInformationViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        private readonly List<string> emaillist;
        private readonly List<string> loginList;
        public string Title { get; set; } = "Zmiana danych pracownika";
        public RelayCommand SubmitCommand { get; }
        public AccountChangeInformationViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ChangeInformation, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            SetData();
            BadNameOrPass = Visibility.Collapsed;
            List<TblPerson> TblPerson = new();
            TblPerson = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            loginList = new List<string>();
            emaillist = new List<string>();

            foreach (var person in TblPerson)
            {
                loginList.Add(person.Login.ToUpper());
                emaillist.Add(person.Email.ToUpper());
            }
        }
        public void SetData()
        {
            Name = LoggedPerson.Name;
            Surname = LoggedPerson.Surname;
            Login = LoggedPerson.Login;
            Email = LoggedPerson.Email;
        }
        private async void ChangeInformation()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Dane pracownika zmienione.");
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
                var.Name = Name;
                var.Surname = Surname;
                var.Login = Login;
                var.Email = Email;
                var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
                var.ModWhen = DateTime.Now;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
                Context.SaveChanges();
                ManagmentShopViewModel.WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
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
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
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
                case nameof(Login):
                    if (string.IsNullOrWhiteSpace(Login))
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    else if (Login.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login musi mieć minimum 3 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    else if ((Login != LoggedPerson.Login) && (loginList.Contains(Login.ToString().ToUpper())))
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login jest już zajęty." };
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
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (!Regex.IsMatch(Name, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (!Regex.IsMatch(Name, @"^[A-ZŻŹĆĄŚĘÓŁŃa-zżźćąśęółń \-?]+$"))
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię posiada złe znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Name)));
                    }
                    else if (Name.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Name)] = new List<object> { "Imię musi mieć minimum 3 znaki." };
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
                        _ValidationErrorsByProperty[nameof(Surname)] = new List<object> { "Nazwisko jest wymagane." };
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
                        _ValidationErrorsByProperty[nameof(Surname)] = new List<object> { "Nazwisko musi mieć minimum 3 znaki." };
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
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    else if (!Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email musi wyglądać xx@xx.pl." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    else if ((Email != LoggedPerson.Email) && (emaillist.Contains(Email.ToString().ToUpper())))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email jest już zajęty." };
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

