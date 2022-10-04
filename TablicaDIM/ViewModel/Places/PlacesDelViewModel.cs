using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel.Places
{
    public class PlacesDelViewModel : InputsViewModel, IMenuItem
    {
        public string Title { get; set; } = "Usuwanie stanowiska";
        private List<object?> _contextToDatagrid;
        public List<object?> ContextToDatagrid
        {
            get => _contextToDatagrid;
            set => SetProperty(ref _contextToDatagrid, value);
        }
        private TblPlace? _selectedPlace;
        public TblPlace? SelectedPlace
        {
            get => _selectedPlace;
            set
            {
                if (SetProperty(ref _selectedPlace, value))
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
        public PlacesDelViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(DelPlace);
            DataAssigment(managmentshopviewmodel);
            UpdateData();
        }
        public void UpdateData()
        {
            var query = Context.TblPlaces.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId);
            ContextToDatagrid = query.ToList<object?>();
        }
        public void BackPage()
        {
            SelectedPlace = null;
            SelectedItem = false;
        }
        private async void DelPlace()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    string texttomsg = String.Format("Czy napewno chcesz usunąć stanowisko {0}?{1}Pamiętaj, że aby usunąć stanowisko nie może ono mieć żadnych akcji.", SelectedPlace.PlaceName, Environment.NewLine);
                    var viewModel = new MessageShowYesNoViewModel(texttomsg);
                    var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
                    if ((bool)resultdialog)
                    {
                        if (Context.TblDataGrids.Where(d => d.PlaceId == SelectedPlace.PlaceId).Where(d => d.ShopId == SelectedPlace.ShopId).Count() == 0)
                        {

                            ManagmentShopViewModel.Update();
                            Context.Remove(Context.TblPlaces.Where(d => d.PlaceId == SelectedPlace.PlaceId).Where(d => d.ShopId == LoggedPerson.ShopId).First());
                            Context.SaveChanges();
                            ManagmentShopViewModel.SelectHomeView();
                            BoundMessageQueue.Enqueue("Stanowisko usunięte.");
                        }
                        else
                        {
                            string textbad = String.Format("Wybrane stanowisko posiada związane ze sobą akcje!");
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
        }
        private async Task<bool> ValidateLogin()
        {
            return true;
        }
    }
}

