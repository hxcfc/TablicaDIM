using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Persons
{
    public class PersonsAddViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        private readonly List<string> loginList;
        private readonly List<string> emaillist;
        private string _surname;
        public override string Surname
        {
            get => _surname;
            set
            {
                if (SetProperty(ref _surname, value))
                {
                    LoginAndEmailCreator();
                }
            }
        }
        private string _name;
        public override string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    LoginAndEmailCreator();
                }
            }
        }
        private int _selectedPermission;
        public int SelectedPermission
        {
            get => _selectedPermission;
            set => SetProperty(ref _selectedPermission, value);
        }
        public string Title { get; set; } = "Dodawanie pracownika";
        public RelayCommand SubmitCommand { get; }

        public PersonsAddViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(AddUser, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            SelectedPermission = -1;
            RemoveError();
            ClearAllValues();
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
        private async void AddUser()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Pracownik dodany.");
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            TblPerson var = new()
            {
                Name = Name,
                Surname = Surname,
                Login = Login,
                Email = Email,
                AddWho = LoggedPerson.Name + " " + LoggedPerson.Surname,
                AddWhen = DateTime.Now,
                FirstLogin = true,
                PermisionId = 1,
                Password = ProtectedData.HashPassword(Login),
                ShopId = SelectedShopFromFirstWindow.ShopId
            };
            if (SelectedPermission == 0)
            {
                var.PermisionId = 2;

            }
            else if (SelectedPermission == 1)
            {
                var.PermisionId = 3;

            }
            else if (SelectedPermission == 2)
            {
                var.PermisionId = 4;
            }
            else
            {
                SelectedPermission = -1;
            }
            Context.TblPersons.Add(var);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            return true;
        }
        private void LoginAndEmailCreator()
        {
            if ((Name is not null) && (Surname is not null))
            {
                if ((Name.Length > 0) && (Surname.Length > 0))
                {
                    Login = LoginNormalize.Normalize(Name[..1].ToLower() + Surname).ToLower();
                    Email = LoginNormalize.Normalize(Name + "." + Surname + "@test.com").ToLower();
                }
            }
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Email) && SelectedPermission != -1)
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
                    else if (loginList.Contains(Login.ToString().ToUpper()))
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
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email administratora jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    else if ((emaillist.Contains(Email.ToString().ToUpper())))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email jest już zajęty." };
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
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}

