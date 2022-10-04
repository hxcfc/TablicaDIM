using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel.Holidays
{
    public class HolidaysManagmentViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zarządzanie świętami i postojem";
        public List<object?> _contextToDatagrid;
        public List<object?> ContextToDatagrid
        {
            get => _contextToDatagrid;
            set => SetProperty(ref _contextToDatagrid, value);
        }
        private DateTime _actualyDate;
        public DateTime ActualyDate
        {
            get => _actualyDate;
            set => SetProperty(ref _actualyDate, value);
        }
        private DateTime _minDate;
        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value);
        }
        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value);
        }
        private string? _startHolidayDate;
        public string? StartHolidayDate
        {
            get => _startHolidayDate;
            set => SetProperty(ref _startHolidayDate, value);
        }
        private string? _endHolidayDate;
        public string? EndHolidayDate
        {
            get => _endHolidayDate;
            set => SetProperty(ref _endHolidayDate, value);
        }
        private string? _countOfDays;
        public string? CountOfDays
        {
            get => _countOfDays;
            set => SetProperty(ref _countOfDays, value);
        }
        private string? _nameHoliday;
        public string? NameHoliday
        {
            get => _nameHoliday;
            set => SetProperty(ref _nameHoliday, value);
        }

        private bool _isOpenPopout;
        public bool IsOpenPopout
        {
            get => _isOpenPopout;
            set => SetProperty(ref _isOpenPopout, value);

        }

        private bool _itsFreeDay;
        public bool ItsFreeDay
        {
            get => _itsFreeDay;
            set => SetProperty(ref _itsFreeDay, value);

        }
        private List<DateTime> _listOfBadDates;
        public List<DateTime> ListOfBadDates
        {
            get => _listOfBadDates;
            set => SetProperty(ref _listOfBadDates, value);
        }
        private TblHoliday? _selectedItem;
        public TblHoliday? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    if (value != null)
                    {
                        SelectedItemClone = value;
                        IsSelectedItem = true;
                        HorizonAligm = 25;
                        VertiAligm = 675;
                    }
                    else
                    {
                        IsSelectedItem = false;
                        HorizonAligm = -190;
                        VertiAligm = 5;
                    }
                }
            }
        }
        private TblHoliday? _selectedItemClone;
        public TblHoliday? SelectedItemClone
        {
            get => _selectedItemClone;
            set => SetProperty(ref _selectedItemClone, value);

        }
        private Visibility _visiblityOfAdd;
        public Visibility VisiblityOfAdd
        {
            get => _visiblityOfAdd;
            set => SetProperty(ref _visiblityOfAdd, value);
        }
        private Visibility _visiblityOfMod;
        public Visibility VisiblityOfMod
        {
            get => _visiblityOfMod;
            set => SetProperty(ref _visiblityOfMod, value);
        }
        private bool _isSelectedItem;
        public bool IsSelectedItem
        {
            get => _isSelectedItem;
            set => SetProperty(ref _isSelectedItem, value);
        }
        private bool? _isMod;
        public bool? IsMod
        {
            get => _isMod;
            set
            {
                if (SetProperty(ref _isMod, value))
                {
                    if (value != null)
                    {
                        if (value == false)
                        {
                            VisiblityOfAdd = Visibility.Visible;
                            VisiblityOfMod = Visibility.Hidden;
                        }
                        else
                        {
                            VisiblityOfAdd = Visibility.Hidden;
                            VisiblityOfMod = Visibility.Visible;
                        }
                    }

                }
            }
        }
        private int _horizonAligm;
        public int HorizonAligm
        {
            get => _horizonAligm;
            set => SetProperty(ref _horizonAligm, value);
        }
        private int _vertiAligm;
        public int VertiAligm
        {
            get => _vertiAligm;
            set => SetProperty(ref _vertiAligm, value);
        }
        public RelayCommand OpenPopoutCommand { get; }
        public RelayCommand OpenModPopoutCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand ModCommand { get; }
        public RelayCommand DelCommand { get; }
        public RelayCommand ChangeYearCommand { get; }
        public HolidaysManagmentViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            AddCommand = new RelayCommand(AddAction, CanAddSubmit);
            DelCommand = new RelayCommand(DelAction);
            ModCommand = new RelayCommand(ModAction, CanAddSubmit);
            OpenPopoutCommand = new RelayCommand(UpdateSelectedData);
            OpenModPopoutCommand = new RelayCommand(UpdateModSelectedData);
            ChangeYearCommand = new RelayCommand(UpdateYear);
            SelectedItemClone = new();
            IsSelectedItem = false;
            HorizonAligm = -190;
            VertiAligm = 5;
            DataAssigment(managmentshopviewmodel);
            ListOfBadDates = new();
            UpdateData();
            IsOpenPopout = false;
            IsMod = null;
            ActualyDate = DateTime.Now;
            MinDate = new DateTime(ActualyDate.Year, 1, 1);
            MaxDate = new DateTime(ActualyDate.Year + 1, 1, 31);
            ListOfUnavaibleDates();
        }
        private void ListOfUnavaibleDates()
        {
            ListOfBadDates.Clear();
            var con = Context.TblHolidays.ToList();
            if (con.Count > 0)
            {
                foreach (var dates in con)
                {
                    for (var dt = dates.DateFrom; dt <= dates.DateTo; dt = dt.AddDays(1))
                    {
                        ListOfBadDates.Add(dt);
                    }
                }
            }
        }
        private async void OpenPopout()
        {
            IsOpenPopout = true;
        }
        private async void UpdateYear()
        {
            var cont = Context.TblHolidays.ToList();
            foreach (var date in cont)
            {
                TblHoliday var = new();
                var = date;
                var.DateFrom = new DateTime(ActualyDate.Year, date.DateFrom.Month, date.DateFrom.Day);
                var.DateTo = new DateTime(ActualyDate.Year, date.DateTo.Month, date.DateTo.Day);
                Context.Entry(Context.TblHolidays.Where(d => d.HolidayId == date.HolidayId).First()).CurrentValues.SetValues(var);
                Context.SaveChanges();
            }
            BoundMessageQueue.Enqueue("Zmieniono rok w wolnych na aktualny. Pamiętaj, by sprawdzić święta ruchome.");
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            UpdateData();
        }
        private void UpdateSelectedData()
        {
            CountOfDays = "0";
            NameHoliday = string.Empty;
            StartHolidayDate = string.Empty;
            EndHolidayDate = string.Empty;
            ItsFreeDay = false;
            _ValidationErrorsByProperty.Remove(nameof(NameHoliday));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NameHoliday)));
            _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
            _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
            IsMod = false;
            SelectedItem = null;
            OpenPopout();
        }
        private void UpdateModSelectedData()
        {
            NameHoliday = SelectedItemClone.Name;
            StartHolidayDate = SelectedItemClone.DateFrom.ToString();
            EndHolidayDate = SelectedItemClone.DateTo.ToString();
            CountOfDays = ((int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate))).ToString();
            ItsFreeDay = SelectedItemClone.ItsFreeDay;
            _ValidationErrorsByProperty.Remove(nameof(NameHoliday));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NameHoliday)));
            _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
            _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
            IsMod = true;
            OpenPopout();
        }
        private void NewData()
        {
            DimTabContext dimnew = new();
            Context = dimnew;
        }
        public void UpdateData()
        {
            NewData();
            var query = Context.TblHolidays.OrderBy(d => d.DateFrom);
            if (query != null)
            {
                ContextToDatagrid = query.ToList<object?>();
            }
            ListOfUnavaibleDates();

        }
        public static double GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday)
            {
                calcBusinessDays--;
            }

            if (startD.DayOfWeek == DayOfWeek.Sunday)
            {
                calcBusinessDays--;
            }

            return calcBusinessDays;
        }
        private bool CanAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(NameHoliday) && !string.IsNullOrWhiteSpace(StartHolidayDate) && !string.IsNullOrWhiteSpace(EndHolidayDate))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async void DelAction()
        {
            string texttomsg = String.Format("Czy napewno chcesz usunąć wolne {0}?{1}Pamiętaj, że wpływa to na wszystkie obszary.", SelectedItem.Name, Environment.NewLine);
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {
                bool result = await ValidateDel();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Wolne usunięte.");
                }
            }
        }
        private async void AddAction()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Wolne dodane.");
                }
            }
        }
        private async void ModAction()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Wolne zmieniono.");
                }
            }
        }
        private async Task<bool> ValidateDel()
        {
            TblHoliday var = new();
            var = Context.TblHolidays.Where(d => d.Name == SelectedItem.Name).Where(d => d.HolidayId == SelectedItem.HolidayId).First();
            Context.TblHolidays.Remove(var);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            UpdateData();
            IsOpenPopout = false;
            return true;
        }
        private async Task<bool> ValidateLogin()
        {
            if (IsMod == false)
            {
                TblHoliday var = new()
                {
                    DateFrom = DateTime.Parse(StartHolidayDate),
                    DateTo = DateTime.Parse(EndHolidayDate),
                    ItsFreeDay = ItsFreeDay,
                    Name = NameHoliday
                };
                Context.TblHolidays.Add(var);
                IsOpenPopout = false;
            }
            else
            {
                TblHoliday var = new()
                {
                    HolidayId = SelectedItemClone.HolidayId,
                    DateFrom = DateTime.Parse(StartHolidayDate),
                    DateTo = DateTime.Parse(EndHolidayDate),
                    ItsFreeDay = ItsFreeDay,
                    Name = NameHoliday
                };
                Context.Entry(SelectedItemClone).CurrentValues.SetValues(var);
                IsOpenPopout = false;
            }
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            UpdateData();

            return true;

        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            Validate(e.PropertyName);
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void Validate(string changedPropertyName)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("pl-PL");
            DateTimeStyles styles = DateTimeStyles.None;
            DateTime dateResult;
            DateTime dateResult1;
            switch (changedPropertyName)
            {
                case nameof(StartHolidayDate):
                    if (string.IsNullOrWhiteSpace(StartHolidayDate))
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    else if (!((DateTime.TryParse(StartHolidayDate, culture, styles, out dateResult))))
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(StartHolidayDate)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    break;
                case nameof(EndHolidayDate):
                    if (string.IsNullOrWhiteSpace(EndHolidayDate))
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else if (!((DateTime.TryParse(EndHolidayDate, culture, styles, out dateResult1))))
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(EndHolidayDate)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    break;
                case nameof(NameHoliday):
                    if (string.IsNullOrWhiteSpace(NameHoliday))
                    {
                        _ValidationErrorsByProperty[nameof(NameHoliday)] = new List<object> { "Nazwa jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NameHoliday)));
                    }

                    else if (!Regex.IsMatch(NameHoliday, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(NameHoliday)] = new List<object> { "Nazwa musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NameHoliday)));
                    }
                    else if (NameHoliday.Length <= 4)
                    {
                        _ValidationErrorsByProperty[nameof(NameHoliday)] = new List<object> { "Przyczyna musi mieć więcej niż 4 znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NameHoliday)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(NameHoliday)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NameHoliday)));
                    }
                    break;
            }
            if ((DateTime.TryParse(StartHolidayDate, culture, styles, out dateResult)) && (DateTime.TryParse(EndHolidayDate, culture, styles, out dateResult1)))
            {
                if ((DateTime.Parse(StartHolidayDate) == SelectedItemClone.DateFrom) && (DateTime.Parse(EndHolidayDate) == SelectedItemClone.DateTo))
                {
                    CountOfDays = ((int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate))).ToString();
                    _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
                    _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                }
                else
                {
                    List<DateTime> ListDates = new();
                    ListDates.Clear();
                    for (var dt = (DateTime.Parse(StartHolidayDate)); dt <= DateTime.Parse(EndHolidayDate); dt = dt.AddDays(1))
                    {
                        ListDates.Add(dt);
                    }
                    if (DateTime.Parse(StartHolidayDate) > DateTime.Parse(EndHolidayDate))
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data zakończenia jest przed datą rozpoczęcia." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia jest przed datą rozpoczęcia." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else if ((DateTime.Parse(StartHolidayDate) < MinDate) || (DateTime.Parse(StartHolidayDate) > MaxDate))
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia musi być w tym roku." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    else if ((DateTime.Parse(EndHolidayDate) < MinDate) || (DateTime.Parse(EndHolidayDate) > MaxDate))
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia musi być w tym roku." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else if (ListOfBadDates.Contains((DateTime.Parse(StartHolidayDate))))
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia jest już w innym wniosku." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    else if (ListOfBadDates.Contains((DateTime.Parse(EndHolidayDate))))
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia jest już w innym wniosku." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else if (ListDates.Intersect(ListOfBadDates).Any())
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data pokrywa się w innym wniosku." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data pokrywa się w innym wniosku." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else
                    {
                        CountOfDays = ((int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate))).ToString();
                        _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
                        _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                }

            }
            AddCommand.NotifyCanExecuteChanged();
            ModCommand.NotifyCanExecuteChanged();
        }
    }
}
