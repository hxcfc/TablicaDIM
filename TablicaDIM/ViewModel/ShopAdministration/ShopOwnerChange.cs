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

    internal class ShopOwnerChange : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zmiana właściciela obszaru";
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
            set => SetProperty(ref _selectedPerson, value);
        }

        public RelayCommand SubmitCommand { get; }
        public ShopOwnerChange(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ChangeNameShop, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            UpdateData();
            BadNameOrPass = Visibility.Collapsed;
            ResetErrorAndValues();
        }
        private void UpdateData()
        {
            var query = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PermisionId > 1);
            ContextToDatagrid = query.ToList<object?>();
        }
        private async void ChangeNameShop()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    _ValidationErrorsByProperty.Clear();
                    BoundMessageQueue.Enqueue("Właściciel obszaru zmieniony.");
                }
                else
                {
                    BadNameOrPass = Visibility.Visible;
                }
            }
        }
        public override void ResetErrorAndValues()
        {
            SelectedPerson = null;
            RemoveError();
            ClearAllValues();
        }
        private async Task<bool> ValidateLogin()
        {
            if (ProtectedData.VerifyHashedPassword(LoggedPerson.Password, Password))
            {
                TblPerson var = new();
                var = SelectedPerson;
                var.ModWhen = DateTime.Now;
                var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
                var.PermisionId = 1;
                TblPerson var2 = new();
                var2 = LoggedPerson;
                var2.PermisionId = 2;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var2);
                Context.SaveChanges();
                ManagmentShopViewModel.WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
                ManagmentShopViewModel.LogOut();
                ManagmentShopViewModel.Update();
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Password) && SelectedPerson != null)
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

