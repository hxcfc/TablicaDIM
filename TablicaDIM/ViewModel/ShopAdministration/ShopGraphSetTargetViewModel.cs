using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.ShopAdministration
{
    public class ShopGraphSetTargetViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zmiana celów wskaźników obszaru";
        public string? _chartMTTR;
        public string ChartMTTR
        {
            get
            {
                if (_chartMTTR != null)
                {
                    return _chartMTTR;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartMTTR, value);
        }
        private string? _chartMTBF;
        public string ChartMTBF
        {
            get
            {
                if (_chartMTBF != null)
                {
                    return _chartMTBF;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartMTBF, value);
        }
        private string? _chartPercentageOfBreakDowns;
        public string ChartPercentageOfBreakDowns
        {
            get
            {
                if (_chartPercentageOfBreakDowns != null)
                {
                    return _chartPercentageOfBreakDowns;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartPercentageOfBreakDowns, value);
        }

        private string? _chartCountOfBreakDowns;
        public string ChartCountOfBreakDowns
        {
            get
            {
                if (_chartCountOfBreakDowns != null)
                {
                    return _chartCountOfBreakDowns;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartCountOfBreakDowns, value);
        }


        public RelayCommand SubmitCommand { get; }

        public ShopGraphSetTargetViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ModTargets, CanSubmit);
            DataAssigment(managmentshopviewmodel);
            UpdateData();
        }
        private async void ModTargets()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Cele ustawione.");
                }
            }
        }
        public void UpdateData()
        {
            ChartMTTR = SelectedShopFromFirstWindow.ChartMttr.ToString();
            ChartMTBF = SelectedShopFromFirstWindow.ChartMtbf.ToString();
            ChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.ChartPercentOfBreakdown.ToString();
            ChartCountOfBreakDowns = SelectedShopFromFirstWindow.ChartCoutOfBreakdown.ToString();
        }
        private async Task<bool> ValidateLogin()
        {
            TblShop shopNew = new();
            shopNew = SelectedShopFromFirstWindow;
            shopNew.ChartMttr = (Convert.ToDouble(ChartMTTR));
            shopNew.ChartMtbf = (Convert.ToDouble(ChartMTBF));
            shopNew.ChartPercentOfBreakdown = (Convert.ToDouble(ChartPercentageOfBreakDowns));
            shopNew.ChartCoutOfBreakdown = (Convert.ToInt32(ChartCountOfBreakDowns));
            shopNew.ModWhen = DateTime.Now;
            shopNew.ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname;
            Context.Entry(SelectedShopFromFirstWindow).CurrentValues.SetValues(shopNew);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            return true;
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(ChartMTTR) && !string.IsNullOrWhiteSpace(ChartMTBF) && !string.IsNullOrWhiteSpace(ChartPercentageOfBreakDowns) && !string.IsNullOrWhiteSpace(ChartCountOfBreakDowns))
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
                case nameof(ChartMTTR):
                    if (string.IsNullOrWhiteSpace(ChartMTTR))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTTR)] = new List<object> { "Cel MTTR jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTR)));
                    }
                    else if (!Regex.IsMatch(ChartMTTR, "^[0-9]{1,6}([,][0-9]{1,2})?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTTR)] = new List<object> { "Zły format. Przykładowy format 000,00" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTR)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartMTTR)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTR)));
                    }
                    break;
                case nameof(ChartMTBF):
                    if (string.IsNullOrWhiteSpace(ChartMTBF))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTBF)] = new List<object> { "Cel MTBF jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTR)));
                    }
                    else if (!Regex.IsMatch(ChartMTBF, "^[0-9]{1,6}([,][0-9]{1,2})?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTBF)] = new List<object> { "Zły format. Przykładowy format 000,00" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTBF)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartMTBF)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTBF)));
                    }
                    break;
                case nameof(ChartPercentageOfBreakDowns):
                    if (string.IsNullOrWhiteSpace(ChartPercentageOfBreakDowns))
                    {
                        _ValidationErrorsByProperty[nameof(ChartPercentageOfBreakDowns)] = new List<object> { "Cel procentu awaryjności jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartPercentageOfBreakDowns)));
                    }
                    else if (!Regex.IsMatch(ChartPercentageOfBreakDowns, "^[0-9]{1,6}([,][0-9]{1,2})?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartPercentageOfBreakDowns)] = new List<object> { "Zły format. Przykładowy format 000,00" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartPercentageOfBreakDowns)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartPercentageOfBreakDowns)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartPercentageOfBreakDowns)));
                    }
                    break;
                case nameof(ChartCountOfBreakDowns):
                    if (string.IsNullOrWhiteSpace(ChartCountOfBreakDowns))
                    {
                        _ValidationErrorsByProperty[nameof(ChartCountOfBreakDowns)] = new List<object> { "Cel ilości awarii jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartCountOfBreakDowns)));
                    }
                    else if (!Regex.IsMatch(ChartCountOfBreakDowns, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartCountOfBreakDowns)] = new List<object> { "Zły format. Przykładowy format 000." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartCountOfBreakDowns)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartCountOfBreakDowns)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartCountOfBreakDowns)));
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}


