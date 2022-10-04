using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Places
{
    public class PlacesAddViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        private readonly List<string> placesnamelist;

        public string Title { get; set; } = "Dodawanie stanowiska";
        public RelayCommand SubmitCommand { get; }
        public PlacesAddViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(AddPlace, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            List<TblPlace> TblPlaces = new();
            TblPlaces = Context.TblPlaces.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            placesnamelist = new List<string>();
            foreach (var place in TblPlaces)
            {
                placesnamelist.Add(place.PlaceName.ToUpper());
            }
        }
        private async void AddPlace()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Stanowisko dodane.");
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            TblPlace var = new()
            {
                PlaceName = PlaceName,
                AddWho = LoggedPerson.Name + " " + LoggedPerson.Surname,
                AddWhen = DateTime.Now,
                ShopId = SelectedShopFromFirstWindow.ShopId
            };
            Context.TblPlaces.Add(var);
            Context.SaveChanges();
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
                    else if (placesnamelist.Contains(PlaceName.ToString().ToUpper()))
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
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}

