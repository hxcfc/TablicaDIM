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
    public class FreeDaysManagmentViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Zarządzanie wolnym pracowników";
        private List<object?> _contextToDatagrid;
        public List<object?> ContextToDatagrid
        {
            get => _contextToDatagrid;
            set => SetProperty(ref _contextToDatagrid, value);
        }
        private List<TblUnavailable?> _holidaysToDatagrid;
        public List<TblUnavailable?> HolidaysToDatagrid
        {
            get => _holidaysToDatagrid;
            set => SetProperty(ref _holidaysToDatagrid, value);
        }
        private TblUnavailable? _selectedHoliday;
        public TblUnavailable? SelectedHoliday
        {
            get => _selectedHoliday;
            set
            {
                if (SetProperty(ref _selectedHoliday, value))
                {
                    if (value != null)
                    {
                        SelectedHolidayBool = true;
                    }
                    else
                    {
                        SelectedHolidayBool = false;
                    }
                }
            }
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
                        ListOfBadDates.Clear();
                        SelectedItem = true;
                        CheckHolidaysLeft();
                        ListOfUnavaibleDates();
                    }
                }
            }
        }


        private string? _newDaysValue;
        public string NewDaysValue
        {
            get
            {
                if (_newDaysValue != null)
                {
                    return _newDaysValue;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _newDaysValue, value);
        }
        private bool _selectedItem;
        public bool SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        private bool _selectedHolidayBool;
        public bool SelectedHolidayBool
        {
            get => _selectedHolidayBool;
            set => SetProperty(ref _selectedHolidayBool, value);
        }
        private bool _popoutOpen;
        public bool PopoutOpen
        {
            get => _popoutOpen;
            set => SetProperty(ref _popoutOpen, value);
        }
        private bool _popoutHolidayOpen;
        public bool PopoutHolidayOpen
        {
            get => _popoutHolidayOpen;
            set => SetProperty(ref _popoutHolidayOpen, value);
        }
        private Visibility _chooseItem;
        public Visibility ChooseItem
        {
            get => _chooseItem;
            set => SetProperty(ref _chooseItem, value);
        }
        private Visibility _gridView;
        public Visibility GridView
        {
            get => _gridView;
            set => SetProperty(ref _gridView, value);
        }
        private int _daysURLOP;
        public int DaysURLOP
        {
            get => _daysURLOP;
            set => SetProperty(ref _daysURLOP, value);
        }
        private int _daysCHOROBA;
        public int DaysCHOROBA
        {
            get => _daysCHOROBA;
            set => SetProperty(ref _daysCHOROBA, value);
        }
        private int _daysKREW;
        public int DaysKREW
        {
            get => _daysKREW;
            set => SetProperty(ref _daysKREW, value);
        }
        private int _daysODBIORKA;
        public int DaysODBIORKA
        {
            get => _daysODBIORKA;
            set => SetProperty(ref _daysODBIORKA, value);
        }
        private int _daysOPIEKA;
        public int DaysOPIEKA
        {
            get => _daysOPIEKA;
            set => SetProperty(ref _daysOPIEKA, value);
        }
        private int _daysPOSTOJ;
        public int DaysPOSTOJ
        {
            get => _daysPOSTOJ;
            set => SetProperty(ref _daysPOSTOJ, value);
        }
        private int _daysSZKOLENIE;
        public int DaysSZKOLENIE
        {
            get => _daysSZKOLENIE;
            set => SetProperty(ref _daysSZKOLENIE, value);
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
        private string _reasonHoliday;
        public string ReasonHoliday
        {
            get => _reasonHoliday;
            set => SetProperty(ref _reasonHoliday, value);
        }
        private List<DateTime> _listOfBadDates;
        public List<DateTime> ListOfBadDates
        {
            get => _listOfBadDates;
            set => SetProperty(ref _listOfBadDates, value);
        }
        private List<string> _reasonsOfHolidays;
        public List<string> ReasonsOfHolidays
        {
            get => _reasonsOfHolidays;
            set => SetProperty(ref _reasonsOfHolidays, value);
        }
        private int _daysOfSelectedDates;
        public int DaysOfSelectedDates
        {
            get => _daysOfSelectedDates;
            set => SetProperty(ref _daysOfSelectedDates, value);
        }
        private bool _nextYearDate;
        public bool NextYearDate
        {
            get => _nextYearDate;
            set
            {
                if (SetProperty(ref _nextYearDate, value))
                {
                    if (value == false)
                    {
                        MinDate = new DateTime(ActualyDate.Year, 1, 1);
                        MaxDate = new DateTime(ActualyDate.Year, 12, 31);
                    }
                    else
                    {
                        MinDate = new DateTime(ActualyDate.Year + 1, 1, 1);
                        MaxDate = new DateTime(ActualyDate.Year + 1, 1, 31);
                    }
                }
            }
        }
        public RelayCommand ModClickCommand { get; }
        public RelayCommand DelClickCommand { get; }
        public RelayCommand BackClickCommand { get; }
        public RelayCommand ResetDaysCoutCommand { get; }
        public RelayCommand DeleteAllHolidaysOptionCommand { get; }
        public RelayCommand OpemPopClickCommand { get; }
        public RelayCommand OpemPopHolidayClickCommand { get; }
        public RelayCommand DeleteAllHolidaysForPersonOptionCommand { get; }
        public RelayCommand DeleteAllHolidaysForThisYearPersonOptionCommand { get; }
        public RelayCommand SubmitDaysCommand { get; }
        public RelayCommand SubmitHolidayCommand { get; }


        public FreeDaysManagmentViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            ModClickCommand = new RelayCommand(ModClick);
            DelClickCommand = new RelayCommand(DelClick);
            BackClickCommand = new RelayCommand(BackClick);
            ResetDaysCoutCommand = new RelayCommand(ResetDaysCoutClick);
            DeleteAllHolidaysOptionCommand = new RelayCommand(DeleteAllHolidaysClick);
            DeleteAllHolidaysForThisYearPersonOptionCommand = new RelayCommand(DeleteAllHolidaysForThisYearPersonClick);
            DeleteAllHolidaysForPersonOptionCommand = new RelayCommand(DeleteAllHolidaysForPersonClick);
            OpemPopClickCommand = new RelayCommand(OpemPopClick);
            OpemPopHolidayClickCommand = new RelayCommand(OpemPopHolidayClick);
            SubmitDaysCommand = new RelayCommand(ModDays, CanSubmit);
            SubmitHolidayCommand = new RelayCommand(AddDays, CanAddSubmit);
            SelectedHolidayBool = false;
            SelectedItem = false;
            PopoutOpen = false;
            PopoutHolidayOpen = false;
            ReasonsOfHolidays = new();
            ListOfBadDates = new();
            ActualyDate = DateTime.Now;
            MinDate = new DateTime(ActualyDate.Year, 1, 1);
            MaxDate = new DateTime(ActualyDate.Year, 12, 31);
            DataAssigment(managmentshopviewmodel);
            GridView = Visibility.Visible;
            ChooseItem = Visibility.Collapsed;
            NewData();
            CreateReasonList();
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
        private void CreateReasonList()
        {
            ReasonsOfHolidays.Add("Choroba");
            ReasonsOfHolidays.Add("Krew");
            ReasonsOfHolidays.Add("Odbiórka");
            ReasonsOfHolidays.Add("Opieka");
            ReasonsOfHolidays.Add("Postój");
            ReasonsOfHolidays.Add("Szkolenie");
            ReasonsOfHolidays.Add("Urlop");
        }
        private async void OpemPopClick()
        {
            NewDaysValue = SelectedPerson.FreeDays.ToString();
            PopoutOpen = true;
            ClearErrors();

        }
        private async void OpemPopHolidayClick()
        {
            StartHolidayDate = null;
            EndHolidayDate = null;
            ReasonHoliday = "Urlop";
            DaysOfSelectedDates = 0;
            NextYearDate = false;
            PopoutHolidayOpen = true;
            ClearErrors();
        }
        private async void DelClick()
        {
            string texttomsg = String.Format("Czy napewno chcesz usunąć to wolne? {0}Pamiętaj, że nie da się tego cofnąć.", Environment.NewLine);
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {

                var temppersonlistav = Context.TblUnavailables.Where(d => d.PersonId == SelectedHoliday.PersonId).Where(d => d.UnavailableId == SelectedHoliday.UnavailableId).First();
                if (temppersonlistav.Reason == "Urlop")
                {
                    var tempPerson = Context.TblPersons.Where(d => d.PersonId == SelectedHoliday.PersonId).First();
                    int daysCout = tempPerson.FreeDays;
                    TblPerson ToChange = new();
                    ToChange = tempPerson;
                    ToChange.FreeDays = daysCout + temppersonlistav.DaysCount;
                    Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedHoliday.PersonId).First()).CurrentValues.SetValues(ToChange);
                    Context.SaveChanges();
                }

                Context.Remove(temppersonlistav);
                Context.SaveChanges();
                BoundMessageQueue.Enqueue("Wolne usunięte.");

                ManagmentShopViewModel.Update();
                NewData();
            }
        }
        private async void ModClick()
        {
            CheckTableHolidays();
            GridView = Visibility.Collapsed;
            ChooseItem = Visibility.Visible;
        }
        private async void BackClick()
        {
            SelectedItem = false;
            SelectedPerson = null;
            GridView = Visibility.Visible;
            ChooseItem = Visibility.Collapsed;
        }
        private void CheckHolidaysLeft()
        {
            DaysURLOP = 0;
            DaysCHOROBA = 0;
            DaysKREW = 0;
            DaysODBIORKA = 0;
            DaysOPIEKA = 0;
            DaysPOSTOJ = 0;
            DaysSZKOLENIE = 0;
            var temp = Context.TblUnavailables.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.AbsentFrom.Year == DateTime.Now.Year).ToList();
            foreach (var tempItem in temp)
            {
                if (tempItem.Reason == "Urlop")
                {
                    DaysURLOP += tempItem.DaysCount;
                }
                else if (tempItem.Reason == "Choroba")
                {
                    DaysCHOROBA += tempItem.DaysCount;
                }
                else if (tempItem.Reason == "Krew")
                {
                    DaysKREW += tempItem.DaysCount;
                }
                else if (tempItem.Reason == "Odbiórka")
                {
                    DaysODBIORKA += tempItem.DaysCount;
                }
                else if (tempItem.Reason == "Opieka")
                {
                    DaysOPIEKA += tempItem.DaysCount;
                }
                else if (tempItem.Reason == "Postój")
                {
                    DaysPOSTOJ += tempItem.DaysCount;
                }
                else
                {
                    DaysSZKOLENIE += tempItem.DaysCount;
                }
            }
        }
        public void NewData()
        {
            DimTabContext con = new();
            Context = con;
            PopoutOpen = false;

            BackClick();
            var query = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId);
            ContextToDatagrid = query.OrderBy(d => d.Surname).ToList<object?>();
        }
        private void CheckTableHolidays()
        {
            DimTabContext con = new();
            Context = con;
            var query = Context.TblUnavailables.Where(d => d.PersonId == SelectedPerson.PersonId);
            HolidaysToDatagrid = query.ToList<TblUnavailable?>();
        }
        private async void ResetDaysCoutClick()
        {
            string texttomsg = String.Format("Czy napewno chcesz ustawić wszystkim użytkownikom 26 dni urlopu? {0}Pamiętaj, że nie da się tego cofnąć.", Environment.NewLine);
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {
                var temppersonlist = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
                foreach (var tempItem in temppersonlist)
                {
                    TblPerson temp = new();
                    temp = tempItem;
                    temp.FreeDays = 26;
                    Context.Entry(Context.TblPersons.Where(d => d.PersonId == tempItem.PersonId).First()).CurrentValues.SetValues(temp);
                    Context.SaveChanges();

                }
                NewData();
                BoundMessageQueue.Enqueue("Ustawiono wszystkim 26 dni urlopu.");
                ManagmentShopViewModel.SelectHomeView();
                ManagmentShopViewModel.Update();
            }
        }
        private async void DeleteAllHolidaysClick()
        {
            string texttomsg = String.Format("Czy napewno chcesz usunąć wszystkim użytkownikom wolne? {0}Pamiętaj, że nie da się tego cofnąć.", Environment.NewLine);
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {
                var temppersonlist = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
                foreach (var tempItem in temppersonlist)
                {
                    var temppersonlistav = Context.TblUnavailables.Where(d => d.PersonId == tempItem.PersonId).ToList();
                    foreach (var tempItemav in temppersonlistav)
                    {
                        Context.Remove(tempItemav);
                        Context.SaveChanges();
                    }
                }
                NewData();
                BoundMessageQueue.Enqueue("Usunięto wszystkim wolne.");
                ManagmentShopViewModel.SelectHomeView();
                ManagmentShopViewModel.Update();
            }
        }
        private async void DeleteAllHolidaysForPersonClick()
        {
            string texttomsg = String.Format("Czy napewno chcesz usunąć wolne aktualnie wybranemu pracownikowi? {0}Pamiętaj, że nie da się tego cofnąć.", Environment.NewLine);
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {

                var temppersonlistav = Context.TblUnavailables.Where(d => d.PersonId == SelectedPerson.PersonId).ToList();
                foreach (var tempItemav in temppersonlistav)
                {
                    Context.Remove(tempItemav);
                    Context.SaveChanges();
                }
                BoundMessageQueue.Enqueue("Wolne usunięte.");

                ManagmentShopViewModel.Update();
                NewData();
            }
        }

        private void ListOfUnavaibleDates()
        {
            var con = Context.TblUnavailables.Where(d => d.PersonId == SelectedPerson.PersonId).OrderBy(d => d.AbsentFrom).ToList();
            foreach (var dates in con)
            {
                for (var dt = dates.AbsentFrom; dt <= dates.AbsentTo; dt = dt.AddDays(1))
                {
                    ListOfBadDates.Add(dt);
                }
            }
            var con2 = Context.TblHolidays.Where(d => d.ItsFreeDay == true).ToList();
            foreach (var dates in con2)
            {
                for (var dt = dates.DateFrom; dt <= dates.DateTo; dt = dt.AddDays(1))
                {
                    ListOfBadDates.Add(dt);
                }
            }
        }
        private async void DeleteAllHolidaysForThisYearPersonClick()
        {
            string texttomsg = String.Format("Czy napewno chcesz usunąć wolne aktualnie wybranemu pracownikowi w tym roku? {0}Pamiętaj, że nie da się tego cofnąć.", Environment.NewLine);
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {

                var temppersonlistav = Context.TblUnavailables.Where(d => d.PersonId == SelectedPerson.PersonId).Where(d => d.AbsentFrom.Year == DateTime.Now.Year).ToList();
                foreach (var tempItemav in temppersonlistav)
                {
                    Context.Remove(tempItemav);
                    Context.SaveChanges();
                }
                BoundMessageQueue.Enqueue("Wolne usunięte.");

                ManagmentShopViewModel.Update();
                NewData();
            }
        }
        private void ClearErrors()
        {
            _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
            _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
        }
        private async void ModDays()
        {
            if (!HasErrors)
            {
                bool result = await ValidateMod();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Liczba dni urlopu ustawiona.");
                }
            }
        }
        private async void AddDays()
        {
            if (!HasErrors)
            {

                if (NextYearDate == false)
                {
                    if ((ReasonHoliday == "Urlop") && (SelectedPerson.FreeDays < (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate))))
                    {
                        BoundMessageQueue.Enqueue("Za mało dni urlopu. Wniosek nie złożony.");
                    }
                    else
                    {
                        bool result = await ValidateAdd();
                        if (result)
                        {
                            BoundMessageQueue.Enqueue("Wolne dodane.");
                        }
                    }
                }
                else
                {
                    bool result = await ValidateAddNextYear();
                    if (result)
                    {
                        BoundMessageQueue.Enqueue("Wolne dodane.");
                    }
                }
            }
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(NewDaysValue) && PopoutHolidayOpen == false)
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private bool CanAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(EndHolidayDate) && !string.IsNullOrWhiteSpace(EndHolidayDate) && !string.IsNullOrWhiteSpace(ReasonHoliday) && PopoutHolidayOpen == true)
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async Task<bool> ValidateMod()
        {
            var tempPerson = Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).First();
            TblPerson ToChange = new();
            ToChange = tempPerson;
            ToChange.FreeDays = Int32.Parse(NewDaysValue);
            Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).First()).CurrentValues.SetValues(ToChange);
            Context.SaveChanges();
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            PopoutOpen = false;
            BackClick();

            NewData();
            return true;
        }
        private async Task<bool> ValidateAddNextYear()
        {
            TblUnavailable var = new()
            {
                PersonId = SelectedPerson.PersonId,
                AbsentFrom = DateTime.Parse(StartHolidayDate),
                AbsentTo = DateTime.Parse(EndHolidayDate),
                DataOfSend = ActualyDate,
                Accepted = true,
                DaysCount = (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate)),
                Reason = ReasonHoliday
            };
            Context.TblUnavailables.Add(var);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();

            NewData();
            BackClick();

            PopoutHolidayOpen = false;

            return true;
        }
        private async Task<bool> ValidateAdd()
        {
            if (ReasonHoliday == "Urlop")
            {
                TblPerson temp = new();
                temp = SelectedPerson;
                temp.FreeDays = SelectedPerson.FreeDays - (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate));
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == SelectedPerson.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();

            }
            TblUnavailable var = new()
            {
                PersonId = SelectedPerson.PersonId,
                AbsentFrom = DateTime.Parse(StartHolidayDate),
                AbsentTo = DateTime.Parse(EndHolidayDate),
                DataOfSend = ActualyDate,
                Accepted = true,
                DaysCount = (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate)),
                Reason = ReasonHoliday
            };
            Context.TblUnavailables.Add(var);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            NewData();
            BackClick();
            PopoutHolidayOpen = false;

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
                    if (string.IsNullOrWhiteSpace(StartHolidayDate) && PopoutHolidayOpen == true)
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    else if (!((DateTime.TryParse(StartHolidayDate, culture, styles, out dateResult))) && PopoutHolidayOpen == true)
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(StartHolidayDate)) && PopoutHolidayOpen == true)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    break;
                case nameof(EndHolidayDate):
                    if (string.IsNullOrWhiteSpace(EndHolidayDate) && PopoutHolidayOpen == true)
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else if (!((DateTime.TryParse(EndHolidayDate, culture, styles, out dateResult1))) && PopoutHolidayOpen == true)
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(EndHolidayDate)) && PopoutHolidayOpen == true)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    break;
                case nameof(NewDaysValue):
                    if (string.IsNullOrWhiteSpace(NewDaysValue) && PopoutHolidayOpen == false)
                    {
                        _ValidationErrorsByProperty[nameof(NewDaysValue)] = new List<object> { "Ilość dni jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewDaysValue)));
                    }
                    else if (!Regex.IsMatch(NewDaysValue, "^[0-9]{1,6}?$") && PopoutHolidayOpen == false)
                    {
                        _ValidationErrorsByProperty[nameof(NewDaysValue)] = new List<object> { "Zły format. Przykładowy format 000." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewDaysValue)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(NewDaysValue)) && PopoutHolidayOpen == false)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NewDaysValue)));
                    }
                    break;
            }
            if ((DateTime.TryParse(StartHolidayDate, culture, styles, out dateResult)) && (DateTime.TryParse(EndHolidayDate, culture, styles, out dateResult1)) && PopoutHolidayOpen == true)
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
                else if (ListOfBadDates.Contains((DateTime.Parse(StartHolidayDate))))
                {
                    _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia jest już w innym wniosku/święcie." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                }
                else if (ListOfBadDates.Contains((DateTime.Parse(EndHolidayDate))))
                {
                    _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia jest już w innym wniosku/święcie." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                }
                else if (ListDates.Intersect(ListOfBadDates).Any())
                {
                    _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data pokrywa się w innym wniosku/święcie." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data pokrywa się w innym wniosku/święcie." };
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
                else
                {
                    _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
                    _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    DaysOfSelectedDates = (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate));
                }
            }
            SubmitDaysCommand.NotifyCanExecuteChanged();
            SubmitHolidayCommand.NotifyCanExecuteChanged();
        }
    }
}
