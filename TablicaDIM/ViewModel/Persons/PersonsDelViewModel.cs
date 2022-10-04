using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace TablicaDIM.ViewModel.Persons
{
    public class PersonsDelViewModel : InputsViewModel, IMenuItem
    {
        public string Title { get; set; } = "Usuwanie pracownika";
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
        public RelayCommand SubmitCommand { get; }
        public PersonsDelViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(DeleteUser);
            DataAssigment(managmentshopviewmodel);
            UpdateData();
        }
        public async void DeleteUser()
        {
            bool result = await ResetPasswordValidateLogin();
            if (result)
            {
                string texttomsg = String.Format("Czy napewno chcesz usunąć pracownika {0} {1}?{2}Pamiętaj, że aby usunąć pracownika nie może on mieć żadnych akcji.", SelectedPerson.Name, SelectedPerson.Surname, Environment.NewLine);
                var viewModel = new MessageShowYesNoViewModel(texttomsg);
                var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
                if ((bool)resultdialog)
                {
                    if (Context.TblDataGrids.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == SelectedPerson.ShopId).Count() == 0)
                    {
                        if (LoggedPerson.Login == SelectedPerson.Login)
                        {
                            ManagmentShopViewModel.LogOut();
                        }
                        else
                        {
                            ManagmentShopViewModel.WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
                            ManagmentShopViewModel.Update();
                        }
                        Context.Remove(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.ShopId == LoggedPerson.ShopId).First());
                        Context.SaveChanges();
                        ManagmentShopViewModel.SelectHomeView();
                        BoundMessageQueue.Enqueue("Pracownik usunięty.");
                    }
                    else
                    {
                        string textbad = String.Format("Wybrany użytkownik posiada związane ze sobą akcje!");
                        BoundMessageQueue.Enqueue(textbad);
                        BackPage();
                    }
                }
                else
                {
                    BackPage();
                }
            }
        }
        public void BackPage()
        {
            SelectedPerson = null;
            SelectedItem = false;
        }
        private async Task<bool> ResetPasswordValidateLogin()
        {

            return true;

        }

        public void UpdateData()
        {
            var query = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PermisionId > 1);
            ContextToDatagrid = query.ToList<object?>();
        }
    }
}

