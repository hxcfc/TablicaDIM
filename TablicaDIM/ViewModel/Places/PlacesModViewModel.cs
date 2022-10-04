using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Places
{
    public class PlacesModViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        private readonly List<string> placesnamelist;
        public string Title { get; set; } = "Modyfikowanie stanowiska";
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
        private Visibility _visChangeName;
        public Visibility VisChangeName
        {
            get => _visChangeName;
            set => SetProperty(ref _visChangeName, value);
        }
        private Visibility _visModIf;
        public Visibility VisModIf
        {
            get => _visModIf;
            set => SetProperty(ref _visModIf, value);
        }
        public RelayCommand SubmitCommand { get; }
        public RelayCommand SubmitChangeCommand { get; }
        public RelayCommand BackCommand { get; }
        public PlacesModViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitChangeCommand = new RelayCommand(ModPlace, CanSubmit);
            SubmitCommand = new RelayCommand(ModPlacePage);
            BackCommand = new RelayCommand(BackPage);
            DataAssigment(managmentshopviewmodel);
            UpdateData();
            List<TblPlace> TblPlaces = new();
            TblPlaces = Context.TblPlaces.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            placesnamelist = new List<string>();
            VisDataGrid = Visibility.Visible;
            VisChangeName = Visibility.Collapsed;
            VisModIf = Visibility.Collapsed;
            foreach (var place in TblPlaces)
            {
                placesnamelist.Add(place.PlaceName);
            }
        }
        public void UpdateData()
        {
            var query = Context.TblPlaces.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId);
            ContextToDatagrid = query.ToList<object?>();
        }
        private async void ModPlacePage()
        {
            PlaceName = SelectedPlace.PlaceName;
            VisDataGrid = Visibility.Collapsed;
            VisChangeName = Visibility.Visible;
            if (SelectedPlace.ModWho != null)
            {
                VisModIf = Visibility.Visible;
            }
        }
        public void BackPage()
        {
            SelectedPlace = null;
            SelectedItem = false;
            VisDataGrid = Visibility.Visible;
            VisChangeName = Visibility.Collapsed;
            VisModIf = Visibility.Collapsed;
        }
        private async void ModPlace()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Stanowisko zmodyfikowane.");
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            TblPlace var = new();
            var = SelectedPlace;
            var.PlaceName = PlaceName;
            var.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
            var.ModWhen = DateTime.Now;
            Context.Entry(Context.TblPlaces.Where(d => d.PlaceId == SelectedPlace.PlaceId).Where(d => d.ShopId == LoggedPerson.ShopId).First()).CurrentValues.SetValues(var);
            Context.SaveChanges();
            UpdateData();
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            return true;
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PlaceName))
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
                case nameof(PlaceName):
                    if (string.IsNullOrWhiteSpace(PlaceName))
                    {
                        _ValidationErrorsByProperty[nameof(PlaceName)] = new List<object> { "Nazwa stanowiska jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlaceName)));
                    }
                    else if ((PlaceName != SelectedPlace.PlaceName) && placesnamelist.Contains(PlaceName.ToString().ToUpper()))
                    {
                        _ValidationErrorsByProperty[nameof(PlaceName)] = new List<object> { "Nazwa stanowiska jest już zajęta." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlaceName)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(PlaceName)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlaceName)));
                    }
                    break;
            }
            SubmitChangeCommand.NotifyCanExecuteChanged();
        }
    }
}

