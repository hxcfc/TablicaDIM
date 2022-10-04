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
    public class ShopGraphViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zmiana wartości wskaźników obszaru";
        public RelayCommand SubmitCommand { get; }

        private string? _minpercentageOfBreakDowns;
        public string MinPercentageOfBreakDowns
        {
            get
            {
                if (_minpercentageOfBreakDowns != null)
                {
                    return _minpercentageOfBreakDowns;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _minpercentageOfBreakDowns, value);
        }
        private string? _maxpercentageOfBreakDowns;
        public string MaxPercentageOfBreakDowns
        {
            get
            {
                if (_maxpercentageOfBreakDowns != null)
                {
                    return _maxpercentageOfBreakDowns;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _maxpercentageOfBreakDowns, value);
        }
        private string? _minCountOfBreakDowns;
        public string MinCountOfBreakDowns
        {
            get
            {
                if (_minCountOfBreakDowns != null)
                {
                    return _minCountOfBreakDowns;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _minCountOfBreakDowns, value);
        }
        private string? _maxCountOfBreakDowns;
        public string MaxCountOfBreakDowns
        {
            get
            {
                if (_maxCountOfBreakDowns != null)
                {
                    return _maxCountOfBreakDowns;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _maxCountOfBreakDowns, value);
        }
        private string? _minChartMTBF;
        public string MinChartMTBF
        {
            get
            {
                if (_minChartMTBF != null)
                {
                    return _minChartMTBF;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _minChartMTBF, value);
        }
        private string? _maxChartMTBF;
        public string MaxChartMTBF
        {
            get
            {
                if (_maxChartMTBF != null)
                {
                    return _maxChartMTBF;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _maxChartMTBF, value);
        }
        private string? _minChartMTTR;
        public string MinChartMTTR
        {
            get
            {
                if (_minChartMTTR != null)
                {
                    return _minChartMTTR;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _minChartMTTR, value);
        }
        private string? _maxChartMTTR;
        public string MaxChartMTTR
        {
            get
            {
                if (_maxChartMTTR != null)
                {
                    return _maxChartMTTR;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _maxChartMTTR, value);
        }
        public ShopGraphViewModel(ManagmentShopViewModel managmentshopviewmodel)
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
                    BoundMessageQueue.Enqueue("Wskaźniki ustawione.");
                }
            }
        }
        public void UpdateData()
        {
            MinPercentageOfBreakDowns = SelectedShopFromFirstWindow.MinChartPercentOfBreakdown.ToString();
            MaxPercentageOfBreakDowns = SelectedShopFromFirstWindow.MaxChartPercentOfBreakdown.ToString();
            MinCountOfBreakDowns = SelectedShopFromFirstWindow.MinChartCoutOfBreakdown.ToString();
            MaxCountOfBreakDowns = SelectedShopFromFirstWindow.MaxChartCoutOfBreakdown.ToString();
            MinChartMTBF = SelectedShopFromFirstWindow.MinChartMtbf.ToString();
            MaxChartMTBF = SelectedShopFromFirstWindow.MaxChartMtbf.ToString();
            MinChartMTTR = SelectedShopFromFirstWindow.MinChartMttr.ToString();
            MaxChartMTTR = SelectedShopFromFirstWindow.MaxChartMttr.ToString();
        }
        private async Task<bool> ValidateLogin()
        {
            TblShop shopNew = new();
            shopNew = SelectedShopFromFirstWindow;
            shopNew.MinChartPercentOfBreakdown = (Convert.ToDouble(MinPercentageOfBreakDowns.ToString()));
            shopNew.MaxChartPercentOfBreakdown = (Convert.ToDouble(MaxPercentageOfBreakDowns.ToString()));
            shopNew.MinChartCoutOfBreakdown = (Convert.ToInt32(MinCountOfBreakDowns.ToString()));
            shopNew.MaxChartCoutOfBreakdown = (Convert.ToInt32(MaxCountOfBreakDowns.ToString()));
            shopNew.MinChartMtbf = (Convert.ToDouble(MinChartMTBF.ToString()));
            shopNew.MaxChartMtbf = (Convert.ToDouble(MaxChartMTBF.ToString()));
            shopNew.MinChartMttr = (Convert.ToDouble(MinChartMTTR.ToString()));
            shopNew.MaxChartMttr = (Convert.ToDouble(MaxChartMTTR.ToString()));
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
            if (!string.IsNullOrWhiteSpace(MinPercentageOfBreakDowns) && !string.IsNullOrWhiteSpace(MaxPercentageOfBreakDowns) &&
                !string.IsNullOrWhiteSpace(MinCountOfBreakDowns) && !string.IsNullOrWhiteSpace(MaxCountOfBreakDowns) &&
                !string.IsNullOrWhiteSpace(MinChartMTBF) && !string.IsNullOrWhiteSpace(MaxChartMTBF) &&
                !string.IsNullOrWhiteSpace(MinChartMTTR) && !string.IsNullOrWhiteSpace(MaxChartMTTR))
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
                case nameof(MinPercentageOfBreakDowns):
                    if (string.IsNullOrWhiteSpace(MinPercentageOfBreakDowns))
                    {
                        _ValidationErrorsByProperty[nameof(MinPercentageOfBreakDowns)] = new List<object> { "Wartość minimalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinPercentageOfBreakDowns)));
                    }
                    else if (!Regex.IsMatch(MinPercentageOfBreakDowns, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MinPercentageOfBreakDowns)] = new List<object> { "Wartość minimalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinPercentageOfBreakDowns)));
                    }
                    else if ((MinPercentageOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MinPercentageOfBreakDowns, "^[0-9]{1,6}?$") &&
                         (MaxPercentageOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MaxPercentageOfBreakDowns, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinPercentageOfBreakDowns.ToString()) >= (Convert.ToDouble(MaxPercentageOfBreakDowns.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MinPercentageOfBreakDowns)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinPercentageOfBreakDowns)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MinPercentageOfBreakDowns)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinPercentageOfBreakDowns)));
                    }
                    break;
                case nameof(MaxPercentageOfBreakDowns):
                    if (string.IsNullOrWhiteSpace(MaxPercentageOfBreakDowns))
                    {
                        _ValidationErrorsByProperty[nameof(MaxPercentageOfBreakDowns)] = new List<object> { "Wartość maksymalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxPercentageOfBreakDowns)));
                    }
                    else if (!Regex.IsMatch(MaxPercentageOfBreakDowns, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MaxPercentageOfBreakDowns)] = new List<object> { "Wartość maksymalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxPercentageOfBreakDowns)));
                    }
                    else if ((MinPercentageOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MinPercentageOfBreakDowns, "^[0-9]{1,6}?$") &&
                         (MaxPercentageOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MaxPercentageOfBreakDowns, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinPercentageOfBreakDowns.ToString()) >= (Convert.ToDouble(MaxPercentageOfBreakDowns.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MaxPercentageOfBreakDowns)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxPercentageOfBreakDowns)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MaxPercentageOfBreakDowns)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxPercentageOfBreakDowns)));
                    }
                    break;

                case nameof(MinCountOfBreakDowns):
                    if (string.IsNullOrWhiteSpace(MinCountOfBreakDowns))
                    {
                        _ValidationErrorsByProperty[nameof(MinCountOfBreakDowns)] = new List<object> { "Wartość minimalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinCountOfBreakDowns)));
                    }
                    else if (!Regex.IsMatch(MinCountOfBreakDowns, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MinCountOfBreakDowns)] = new List<object> { "Wartość minimalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinCountOfBreakDowns)));
                    }
                    else if ((MinCountOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MinCountOfBreakDowns, "^[0-9]{1,6}?$") &&
                         (MaxCountOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MaxCountOfBreakDowns, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinCountOfBreakDowns.ToString()) >= (Convert.ToDouble(MaxCountOfBreakDowns.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MinCountOfBreakDowns)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinCountOfBreakDowns)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MinCountOfBreakDowns)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinCountOfBreakDowns)));
                    }
                    break;
                case nameof(MaxCountOfBreakDowns):
                    if (string.IsNullOrWhiteSpace(MaxCountOfBreakDowns))
                    {
                        _ValidationErrorsByProperty[nameof(MaxCountOfBreakDowns)] = new List<object> { "Wartość maksymalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxCountOfBreakDowns)));
                    }
                    else if (!Regex.IsMatch(MaxCountOfBreakDowns, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MaxCountOfBreakDowns)] = new List<object> { "Wartość maksymalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxCountOfBreakDowns)));
                    }
                    else if ((MinCountOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MinCountOfBreakDowns, "^[0-9]{1,6}?$") &&
                         (MaxCountOfBreakDowns.ToString() != String.Empty) && Regex.IsMatch(MaxCountOfBreakDowns, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinCountOfBreakDowns.ToString()) >= (Convert.ToDouble(MaxCountOfBreakDowns.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MaxCountOfBreakDowns)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxCountOfBreakDowns)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MaxCountOfBreakDowns)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxCountOfBreakDowns)));
                    }
                    break;

                case nameof(MinChartMTBF):
                    if (string.IsNullOrWhiteSpace(MinChartMTBF))
                    {
                        _ValidationErrorsByProperty[nameof(MinChartMTBF)] = new List<object> { "Wartość minimalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTBF)));
                    }
                    else if (!Regex.IsMatch(MinChartMTBF, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MinChartMTBF)] = new List<object> { "Wartość minimalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTBF)));
                    }
                    else if ((MinChartMTBF.ToString() != String.Empty) && Regex.IsMatch(MinChartMTBF, "^[0-9]{1,6}?$") &&
                         (MaxChartMTBF.ToString() != String.Empty) && Regex.IsMatch(MaxChartMTBF, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinChartMTBF.ToString()) >= (Convert.ToDouble(MaxChartMTBF.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MinChartMTBF)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTBF)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MinChartMTBF)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTBF)));
                    }
                    break;
                case nameof(MaxChartMTBF):
                    if (string.IsNullOrWhiteSpace(MaxChartMTBF))
                    {
                        _ValidationErrorsByProperty[nameof(MaxChartMTBF)] = new List<object> { "Wartość maksymalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTBF)));
                    }
                    else if (!Regex.IsMatch(MaxChartMTBF, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MaxChartMTBF)] = new List<object> { "Wartość maksymalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTBF)));
                    }
                    else if ((MinChartMTBF.ToString() != String.Empty) && Regex.IsMatch(MinChartMTBF, "^[0-9]{1,6}?$") &&
                         (MaxChartMTBF.ToString() != String.Empty) && Regex.IsMatch(MaxChartMTBF, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinChartMTBF.ToString()) >= (Convert.ToDouble(MaxChartMTBF.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MaxChartMTBF)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTBF)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MaxChartMTBF)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTBF)));
                    }
                    break;

                case nameof(MinChartMTTR):
                    if (string.IsNullOrWhiteSpace(MinChartMTTR))
                    {
                        _ValidationErrorsByProperty[nameof(MinChartMTTR)] = new List<object> { "Wartość minimalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTTR)));
                    }
                    else if (!Regex.IsMatch(MinChartMTTR, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MinChartMTTR)] = new List<object> { "Wartość minimalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTTR)));
                    }
                    else if ((MinChartMTTR.ToString() != String.Empty) && Regex.IsMatch(MinChartMTTR, "^[0-9]{1,6}?$") &&
                         (MaxChartMTTR.ToString() != String.Empty) && Regex.IsMatch(MaxChartMTTR, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinChartMTTR.ToString()) >= (Convert.ToDouble(MaxChartMTTR.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MinChartMTTR)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTTR)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MinChartMTTR)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MinChartMTTR)));
                    }
                    break;
                case nameof(MaxChartMTTR):
                    if (string.IsNullOrWhiteSpace(MaxChartMTTR))
                    {
                        _ValidationErrorsByProperty[nameof(MaxChartMTTR)] = new List<object> { "Wartość maksymalna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTTR)));
                    }
                    else if (!Regex.IsMatch(MaxChartMTTR, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(MaxChartMTTR)] = new List<object> { "Wartość maksymalna musi być całkowita." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTTR)));
                    }
                    else if ((MinChartMTTR.ToString() != String.Empty) && Regex.IsMatch(MinChartMTTR, "^[0-9]{1,6}?$") &&
                         (MaxChartMTTR.ToString() != String.Empty) && Regex.IsMatch(MaxChartMTTR, "^[0-9]{1,6}?$") &&
                        ((Convert.ToDouble(MinChartMTTR.ToString()) >= (Convert.ToDouble(MaxChartMTTR.ToString())))))
                    {
                        _ValidationErrorsByProperty[nameof(MaxChartMTTR)] = new List<object> { "Cel min musi być mniejszy od max." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTTR)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(MaxChartMTTR)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(MaxChartMTTR)));
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}


