using MaterialDesignThemes.Wpf;
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
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel.Persons
{
    public class PersonsModViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        private readonly List<string> loginList;
        private readonly List<string> emaillist;
        public DialogSession DialogSession { get; set; }

        private List<object?> _contextToDatagrid;
        public List<object?> ContextToDatagrid
        {
            get => _contextToDatagrid;
            set => SetProperty(ref _contextToDatagrid, value);
        }
        private TblPerson? _selectedPerson;
        public TblPerson? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (SetProperty(ref _selectedPerson, value))
                {
                    if (value != null)
                    {
                        SelectedItem = true;
                        if (value.ModWho != null)
                        {
                            VisModIf = Visibility.Visible;
                        }
                    }
                    else
                    {
                        VisModIf = Visibility.Collapsed;
                    }
                }
            }
        }
        private bool _selectedItem;
        public bool SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        private Visibility _visDataGrid;
        public Visibility VisDataGrid
        {
            get => _visDataGrid;
            set => SetProperty(ref _visDataGrid, value);
        }
        private Visibility _visChangeInfo;
        public Visibility VisChangeInfo
        {
            get => _visChangeInfo;
            set => SetProperty(ref _visChangeInfo, value);
        }
        private Visibility _visChangePermisions;
        public Visibility VisChangePermisions
        {
            get => _visChangePermisions;
            set => SetProperty(ref _visChangePermisions, value);
        }
        private Visibility _visModIf;
        public Visibility VisModIf
        {
            get => _visModIf;
            set => SetProperty(ref _visModIf, value);
        }
        private int _selectedPermission;
        public int SelectedPermission
        {
            get => _selectedPermission;
            set
            {
                if (SetProperty(ref _selectedPermission, value))
                {
                    if (value == -1)
                    {
                        CanChangePermisions = false;
                    }
                    else
                    {
                        CanChangePermisions = true;
                    }
                }
            }
        }
        private bool _canChangePermisions;
        public bool CanChangePermisions
        {
            get => _canChangePermisions;
            set => SetProperty(ref _canChangePermisions, value);
        }

        public string Title { get; set; } = "Modyfikowanie pracownika";
        public RelayCommand ChangeInfoOptionCommand { get; }
        public RelayCommand ChangePermisionsOptionCommand { get; }
        public RelayCommand ResetPasswordOptionCommand { get; }
        public RelayCommand BackCommand { get; }
        public RelayCommand SubmitCommand { get; }
        public RelayCommand ChangePermisionsCommand { get; }
        public PersonsModViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            ChangeInfoOptionCommand = new RelayCommand(ChangeInfoPage);
            ChangePermisionsOptionCommand = new RelayCommand(ChangePermisionsPage);
            ChangePermisionsCommand = new RelayCommand(ChangePermisions);
            ResetPasswordOptionCommand = new RelayCommand(ResetPasword);
            SubmitCommand = new RelayCommand(ChangeInfo, CanSubmit);
            BackCommand = new RelayCommand(BackPage);
            DataAssigment(managmentshopviewmodel);
            InputsVMCon = this;
            _selectedPerson = null;
            SelectedItem = false;
            VisDataGrid = Visibility.Visible;
            VisChangeInfo = Visibility.Collapsed;
            VisChangePermisions = Visibility.Collapsed;
            VisModIf = Visibility.Collapsed;
            UpdateData();
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
        public void BackPage()
        {
            SelectedPerson = null;
            SelectedItem = false;
            VisDataGrid = Visibility.Visible;
            VisChangeInfo = Visibility.Collapsed;
            VisModIf = Visibility.Collapsed;
            VisChangePermisions = Visibility.Collapsed;
        }
        public async void ResetPasword()
        {
            bool result = await ResetPasswordValidateLogin();
            if (result)
            {
                string texttomsg = String.Format("Czy napewno chcesz zresetować hasło pracownika {0} {1}?{2}Hasłem użytkownika będzie jego login.", SelectedPerson.Name, SelectedPerson.Surname, Environment.NewLine);
                var viewModel = new MessageShowYesNoViewModel(texttomsg);
                var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
                if ((bool)resultdialog)
                {
                    TblPerson var = new();
                    var = SelectedPerson;
                    var.Password = ProtectedData.HashPassword(SelectedPerson.Login);
                    var.FirstLogin = true;
                    var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
                    var.ModWhen = DateTime.Now;
                    if (LoggedPerson.Login == SelectedPerson.Login)
                    {
                        ManagmentShopViewModel.LogOut();
                    }
                    Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
                    Context.SaveChanges();
                    ManagmentShopViewModel.WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
                    UpdateData();
                    ManagmentShopViewModel.Update();
                    ManagmentShopViewModel.SelectHomeView();
                    BoundMessageQueue.Enqueue("Hasło pracownika zresetowane.");
                }
                else
                {
                    BackPage();
                }
            }

        }
        private async Task<bool> ResetPasswordValidateLogin()
        {

            return true;
        }
        private async void ChangePermisions()
        {
            bool result = await ChangePermisionsValidateLogin();
            if (result)
            {
                BoundMessageQueue.Enqueue("Pracownik zmodyfikowany.");
            }
        }
        private async Task<bool> ChangePermisionsValidateLogin()
        {
            TblPerson var = new();
            var = SelectedPerson;
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
            var.ModWho = LoggedPerson.Login;
            var.ModWhen = DateTime.Now;
            if (LoggedPerson.Login == SelectedPerson.Login)
            {
                TblPerson var2 = new();
                var2 = SelectedPerson;
                if (SelectedPermission == 0)
                {
                    var2.PermisionId = 2;
                }
                else if (SelectedPermission == 1)
                {
                    var2.PermisionId = 3;
                }
                else if (SelectedPermission == 2)
                {
                    var2.PermisionId = 4;
                }
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var2);
                ManagmentShopViewModel.LogOut();
            }
            else
            {
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
            }
            Context.SaveChanges();
            UpdateData();
            ManagmentShopViewModel.WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            return true;
        }
        private void ChangeInfoPage()
        {
            Login = SelectedPerson.Login;
            Name = SelectedPerson.Name;
            Surname = SelectedPerson.Surname;
            Email = SelectedPerson.Email;
            VisDataGrid = Visibility.Collapsed;
            VisChangeInfo = Visibility.Visible;
            VisChangePermisions = Visibility.Collapsed;
            if (SelectedPerson.ModWho != null)
            {
                VisModIf = Visibility.Visible;
            }
        }
        private void ChangePermisionsPage()
        {
            Login = SelectedPerson.Login;
            Name = SelectedPerson.Name;
            Surname = SelectedPerson.Surname;
            Email = SelectedPerson.Email;
            if (SelectedPerson.PermisionId == 2)
            {
                SelectedPermission = 0;
            }
            else if (SelectedPerson.PermisionId == 3)
            {
                SelectedPermission = 1;
            }
            else if (SelectedPerson.PermisionId == 4)
            {
                SelectedPermission = 2;
            }
            else
            {
                SelectedPermission = -1;
            }
            VisDataGrid = Visibility.Collapsed;
            VisChangeInfo = Visibility.Collapsed;
            VisChangePermisions = Visibility.Visible;
            if (SelectedPerson.ModWho != null)
            {
                VisModIf = Visibility.Visible;
            }
        }
        public void UpdateData()
        {
            if (LoggedPerson.PermisionId == 1)
            {
                var query = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PermisionId > 1);
                ContextToDatagrid = query.ToList<object?>();
            }
            else if (LoggedPerson.PermisionId == 2)
            {
                var query = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PermisionId > 1);
                ContextToDatagrid = query.ToList<object?>();
            }
            else if (LoggedPerson.PermisionId == 3)
            {
                var query = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PermisionId > 2);
                ContextToDatagrid = query.ToList<object?>();
            }
        }
        private async void ChangeInfo()
        {
            if (!HasErrors)
            {
                bool result = await ChangeInfoValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Pracownik zmodyfikowany.");
                }
            }
        }
        private async Task<bool> ChangeInfoValidateLogin()
        {
            TblPerson var = new();
            var = SelectedPerson;
            var.Name = Name;
            var.Surname = Surname;
            var.Login = Login;
            var.Email = Email;
            var.ModWho = LoggedPerson.Login;
            var.ModWhen = DateTime.Now;
            if (LoggedPerson.Login == SelectedPerson.Login)
            {
                LoggedPerson.Name = Name;
                LoggedPerson.Surname = Surname;
                LoggedPerson.Login = Login;
                LoggedPerson.Email = Email;
            }
            Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
            Context.SaveChanges();
            UpdateData();
            ManagmentShopViewModel.WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            return true;
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Email))
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
                    else if ((Login != SelectedPerson.Login) && (loginList.Contains(Login.ToString().ToUpper())))
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login jest już zajęty." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Login)));
                    }
                    else if (Login.Length <= 2)
                    {
                        _ValidationErrorsByProperty[nameof(Login)] = new List<object> { "Login musi mieć minimum 3 znaki." };
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
                    else if (!Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email administratora musi wyglądać xx@xx.pl." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Email)));
                    }
                    else if ((Email != SelectedPerson.Email) && (emaillist.Contains(Email.ToString().ToUpper())))
                    {
                        _ValidationErrorsByProperty[nameof(Email)] = new List<object> { "Email jest już zajęty." };
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

