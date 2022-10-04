using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel.Holidays
{
    public class HolidaysCalendarViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Kalendarz urlopów";
        private List<int> _personIDToDatagrid;
        public List<int> PersonIDToDatagrid
        {
            get => _personIDToDatagrid;
            set => SetProperty(ref _personIDToDatagrid, value);
        }
        private int _daysInMonth;
        public int DaysInMonth
        {
            get => _daysInMonth;
            set => SetProperty(ref _daysInMonth, value);
        }
        private string _month;
        public string Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }
        private int _rowCount;
        public int RowCount
        {
            get => _rowCount;
            set => SetProperty(ref _rowCount, value);
        }
        private int _yourDaysOfHolidays;
        public int YourDaysOfHolidays
        {
            get => _yourDaysOfHolidays;
            set => SetProperty(ref _yourDaysOfHolidays, value);
        }
        private int _daysToAccept;
        public int DaysToAccept
        {
            get => _daysToAccept;
            set => SetProperty(ref _daysToAccept, value);
        }
        private int _daysOfSelectedDates;
        public int DaysOfSelectedDates
        {
            get => _daysOfSelectedDates;
            set => SetProperty(ref _daysOfSelectedDates, value);
        }
        private int _daysOfSelectedDatesMod;
        public int DaysOfSelectedDatesMod
        {
            get => _daysOfSelectedDatesMod;
            set => SetProperty(ref _daysOfSelectedDatesMod, value);
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    DaysInMonth = DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month);
                    SelectedMonth();
                    ColumnsDays.Clear();
                    var culture = new System.Globalization.CultureInfo("pl-PL");
                    DateTime SelectedDateNewDay = DateTime.Parse(SelectedDate.Year.ToString() + "-" + SelectedDate.Month.ToString() + "-01");
                    for (int i = 0; i < DaysInMonth; i++)
                    {
                        string firstWord = SelectedDateNewDay.AddDays(i).ToString().Substring(0, 2) + " - ";
                        string secondWord = culture.DateTimeFormat.GetDayName(SelectedDateNewDay.AddDays(i).DayOfWeek).ToString().Substring(0, 2) + ".";
                        string createdText = firstWord + char.ToUpper(secondWord[0]) + secondWord.Substring(1);

                        if ((SelectedDate.Year == DateTime.Now.Year) && (SelectedDate.Month == DateTime.Now.Month) && (SelectedDateNewDay.AddDays(i).Day == DateTime.Now.Day))
                        {
                            ColumnsDays.Add(createdText + "               TODAY");
                        }
                        else if ((ListOfStops.Contains(SelectedDateNewDay.AddDays(i))) && (SelectedDateNewDay.AddDays(i).DayOfWeek != DayOfWeek.Sunday) && (SelectedDateNewDay.AddDays(i).DayOfWeek != DayOfWeek.Saturday))
                        {
                            ColumnsDays.Add(createdText + "               POSTOJ");
                        }
                        else
                        {
                            ColumnsDays.Add(createdText);
                        }
                    }
                    ManagmentShopViewModel.Update();
                    UpdateDataInPersons();

                }

            }
        }
        private ObservableCollection<string> _columnsDays;
        public ObservableCollection<string> ColumnsDays
        {
            get => _columnsDays;
            set => SetProperty(ref _columnsDays, value);
        }
        private List<string> _reasonsOfHolidays;
        public List<string> ReasonsOfHolidays
        {
            get => _reasonsOfHolidays;
            set => SetProperty(ref _reasonsOfHolidays, value);
        }
        private ObservableCollection<PersonHoliday> _personDays;
        public ObservableCollection<PersonHoliday> PersonDays
        {
            get => _personDays;
            set => SetProperty(ref _personDays, value);
        }
        private bool _isOpenPopout;
        public bool IsOpenPopout
        {
            get => _isOpenPopout;
            set
            {
                if (SetProperty(ref _isOpenPopout, value))
                {
                    if (value == true)
                    {
                        HolidaysListSelect = null;
                        HolidayMod = false;
                    }
                }
            }
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
        private string? _startHolidayDateMod;
        public string? StartHolidayDateMod
        {
            get => _startHolidayDateMod;
            set => SetProperty(ref _startHolidayDateMod, value);
        }
        private string? _endHolidayDateMod;
        public string? EndHolidayDateMod
        {
            get => _endHolidayDateMod;
            set => SetProperty(ref _endHolidayDateMod, value);
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
        private string _reasonHolidayMod;
        public string ReasonHolidayMod
        {
            get => _reasonHolidayMod;
            set => SetProperty(ref _reasonHolidayMod, value);
        }
        private bool _HolidayMod;
        public bool HolidayMod
        {
            get => _HolidayMod;
            set => SetProperty(ref _HolidayMod, value);
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
        private List<DateTime> _listOfBadDates;
        public List<DateTime> ListOfBadDates
        {
            get => _listOfBadDates;
            set => SetProperty(ref _listOfBadDates, value);
        }
        private List<DateTime> _listOfStops;
        public List<DateTime> ListOfStops
        {
            get => _listOfStops;
            set => SetProperty(ref _listOfStops, value);
        }
        private Visibility _holidaysReqList;
        public Visibility HolidaysReqList
        {
            get => _holidaysReqList;
            set => SetProperty(ref _holidaysReqList, value);
        }
        private Visibility _holidaysReqModList;
        public Visibility HolidaysReqModList
        {
            get => _holidaysReqModList;
            set => SetProperty(ref _holidaysReqModList, value);
        }
        private Visibility _backDelete;
        public Visibility BackDelete
        {
            get => _backDelete;
            set => SetProperty(ref _backDelete, value);
        }
        private List<TblUnavailable> _holidaysList;
        public List<TblUnavailable> HolidaysList
        {
            get => _holidaysList;
            set => SetProperty(ref _holidaysList, value);
        }
        private TblUnavailable? _holidaysListSelect;
        public TblUnavailable? HolidaysListSelect
        {
            get => _holidaysListSelect;
            set
            {
                if (SetProperty(ref _holidaysListSelect, value))
                {
                    if (value != null)
                    {
                        StartHolidayDate = string.Empty;
                        EndHolidayDate = string.Empty;
                        ReasonHoliday = "Urlop";
                        HolidaysReqModList = Visibility.Visible;
                        StartHolidayDateMod = value.AbsentFrom.ToShortDateString();
                        EndHolidayDateMod = value.AbsentTo.ToShortDateString();
                        ReasonHolidayMod = value.Reason;
                        DaysOfSelectedDatesMod = (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod));

                        ClearErrors();
                        if ((LoggedPerson.PermisionId == 3) || (LoggedPerson.PermisionId == 4))
                        {
                            if (value.AbsentFrom.Year == DateTime.Now.Year)
                            {
                                if (value.ToDelete == true)
                                {
                                    ClearErrors();
                                    CanMod = false;
                                    CanDel = false;
                                    BackDelete = Visibility.Visible;
                                    HolidayMod = false;
                                }
                                else
                                {
                                    if (value.Accepted == true)
                                    {
                                        ClearErrors();
                                        HolidayMod = false;
                                        CanMod = false;
                                        CanDel = true;
                                        BackDelete = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        ClearErrors();
                                        HolidayMod = true;
                                        CanMod = true;
                                        CanDel = true;
                                        BackDelete = Visibility.Collapsed;

                                    }

                                }
                            }
                            else
                            {
                                ClearErrors();
                                HolidayMod = false;
                                CanMod = false;
                                CanDel = true;
                                BackDelete = Visibility.Collapsed;
                            }

                        }
                        else
                        {
                            if (HolidaysListSelect.AbsentTo.Year == DateTime.Now.Year)
                            {
                                ClearErrors();
                                HolidayMod = true;
                                CanMod = true;
                                CanDel = true;
                            }
                            else
                            {
                                ClearErrors();
                                HolidayMod = false;
                                CanMod = false;
                                CanDel = true;
                            }

                        }

                    }
                    else
                    {
                        StartHolidayDate = string.Empty;
                        EndHolidayDate = string.Empty;
                        ReasonHoliday = "Urlop";
                        StartHolidayDateMod = string.Empty;
                        EndHolidayDateMod = string.Empty;
                        ReasonHolidayMod = "Urlop";
                        ClearErrors();
                        BackDelete = Visibility.Collapsed;
                        HolidaysReqModList = Visibility.Collapsed;
                        CanDel = false;
                        CanMod = false;
                        HolidayMod = false;
                    }
                }

            }
        }

        private bool _canMod;
        public bool CanMod
        {
            get => _canMod;
            set => SetProperty(ref _canMod, value);
        }
        private bool _canDel;
        public bool CanDel
        {
            get => _canDel;
            set => SetProperty(ref _canDel, value);
        }
        public RelayCommand MonthLeftCommand { get; }
        public RelayCommand MonthRightCommand { get; }
        public RelayCommand OpenPopoutCommand { get; }
        public RelayCommand SubmitCommand { get; }
        public RelayCommand ClearClickCommand { get; }
        public RelayCommand ModCommand { get; }
        public RelayCommand DelCommand { get; }
        public RelayCommand BackDelCommand { get; }


        public HolidaysCalendarViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            MonthLeftCommand = new RelayCommand(ChangeLeftMonth);
            SubmitCommand = new RelayCommand(AddHoliday, CanSubmit);
            ModCommand = new RelayCommand(ModHoliday, CanModCom);
            DelCommand = new RelayCommand(DelHoliday);
            MonthRightCommand = new RelayCommand(ChangeRightMonth);
            OpenPopoutCommand = new RelayCommand(OpenPopout);
            ClearClickCommand = new RelayCommand(ClearClick);
            BackDelCommand = new RelayCommand(BackDeleteHoliday);
            DataAssigment(managmentshopviewmodel);
            PersonDays = new();
            ColumnsDays = new();
            ReasonsOfHolidays = new();
            ListOfStops = new();
            ListOfBadDates = new();
            ReasonHoliday = "Urlop";
            ActualyDate = DateTime.Now;
            SelectedDate = ActualyDate;
            MinDate = new DateTime(ActualyDate.Year, 1, 1);
            MaxDate = new DateTime(ActualyDate.Year, 12, 31);
            IsOpenPopout = false;
            HolidayMod = false;
            HolidaysReqList = Visibility.Collapsed;
            HolidaysReqModList = Visibility.Collapsed;
            BackDelete = Visibility.Collapsed;
            CreateReasonList();
            NewData();
        }
        private void CheckDaysInUnacctep()
        {
            DaysToAccept = 0;
            if (!string.IsNullOrEmpty(LoggedPerson.Login))
            {
                YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;
            }
            else
            {
                YourDaysOfHolidays = 0;
            }

            var temp = Context.TblUnavailables.Where(d => d.PersonId == LoggedPerson.PersonId).ToList();
            foreach (var day in temp)
            {
                if (day.Reason == "Urlop")
                {
                    if (day.Accepted == false)
                    {
                        DaysToAccept += day.DaysCount;
                    }
                }
            }
        }
        private async void ClearClick()
        {
            HolidaysListSelect = null;
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
        private void OpenPopout()
        {
            IsOpenPopout = true;
            DaysOfSelectedDates = 0;
            StartHolidayDate = string.Empty;
            EndHolidayDate = string.Empty;
            ReasonHoliday = "Urlop";
            StartHolidayDateMod = string.Empty;
            EndHolidayDateMod = string.Empty;
            ReasonHolidayMod = "Urlop";
            ClearErrors();
        }
        private void ClearErrors()
        {
            _ValidationErrorsByProperty.Remove(nameof(StartHolidayDate));
            _ValidationErrorsByProperty.Remove(nameof(EndHolidayDate));
            _ValidationErrorsByProperty.Remove(nameof(StartHolidayDateMod));
            _ValidationErrorsByProperty.Remove(nameof(EndHolidayDateMod));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
        }
        private async void AddHoliday()
        {
            if (!HasErrors)
            {
                if (NextYearDate == false)
                {
                    if ((ReasonHoliday == "Urlop") && (LoggedPerson.FreeDays < (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate))))
                    {
                        BoundMessageQueue.Enqueue("Masz za mało dni urlopu. Wniosek nie złożony.");
                    }
                    else
                    {
                        bool result = await ValidateAdd();
                        if (result)
                        {
                            BoundMessageQueue.Enqueue("Wniosek złożony.");
                        }
                    }
                }
                else
                {
                    bool result = await ValidateAddNextYear();
                    if (result)
                    {
                        BoundMessageQueue.Enqueue("Wniosek złożony.");
                    }
                }

            }
        }
        private async Task<bool> ValidateAddNextYear()
        {
            if ((LoggedPerson.PermisionId == 1) || (LoggedPerson.PermisionId == 2))
            {
                NormalAddHolidayAccepted();
            }
            else
            {
                NormalAddHoliday();
            }
            ManagmentShopViewModel.Update();
            YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;

            NewData();
            IsOpenPopout = false;
            return true;
        }
        private async Task<bool> ValidateAdd()
        {
            if ((LoggedPerson.PermisionId == 1) || (LoggedPerson.PermisionId == 2))
            {

                NormalAddHolidayAccepted();
                if (ReasonHoliday == "Urlop")
                {
                    TblPerson temp = new();
                    temp = LoggedPerson;
                    temp.FreeDays = LoggedPerson.FreeDays - (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate));
                    Context.Entry(Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First()).CurrentValues.SetValues(temp);
                    Context.SaveChanges();

                }
            }
            else
            {
                NormalAddHoliday();
            }
            ManagmentShopViewModel.Update();
            NewData();
            YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;

            IsOpenPopout = false;
            return true;
        }
        private async void ModHoliday()
        {
            if (!HasErrors)
            {
                if (HolidaysListSelect.Accepted == true)
                {
                    if ((ReasonHoliday == "Urlop") && (LoggedPerson.FreeDays +
                        (int)GetBusinessDays(DateTime.Parse(HolidaysListSelect.AbsentFrom.ToShortDateString()), DateTime.Parse(HolidaysListSelect.AbsentTo.ToShortDateString())) < (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod))))
                    {
                        BoundMessageQueue.Enqueue("Masz za mało dni urlopu. Wniosek nie złożony.");
                    }
                    else
                    {
                        bool result = await ValidateMod();
                        if (result)
                        {
                            if ((LoggedPerson.PermisionId == 1) || (LoggedPerson.PermisionId == 2))
                            {
                                BoundMessageQueue.Enqueue("Wolne zmodyfikowane.");
                            }
                            else
                            {
                                BoundMessageQueue.Enqueue("Wniosek złożony.");
                            }
                        }
                    }
                }
                else
                {
                    if ((ReasonHoliday == "Urlop") && (LoggedPerson.FreeDays < (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod))))
                    {
                        BoundMessageQueue.Enqueue("Masz za mało dni urlopu. Wniosek nie złożony.");
                    }
                    else
                    {
                        bool result = await ValidateMod();
                        if (result)
                        {
                            if ((LoggedPerson.PermisionId == 1) || (LoggedPerson.PermisionId == 2))
                            {
                                BoundMessageQueue.Enqueue("Wolne zmodyfikowane.");
                            }
                            else
                            {
                                BoundMessageQueue.Enqueue("Wniosek złożony.");
                            }
                        }
                    }
                }


            }
        }
        private async Task<bool> ValidateMod()
        {
            if ((LoggedPerson.PermisionId == 1) || (LoggedPerson.PermisionId == 2))
            {
                if (HolidaysListSelect.Reason == "Urlop")
                {
                    TblPerson temp = new();
                    temp = LoggedPerson;
                    if (ReasonHolidayMod == "Urlop")
                    {
                        temp.FreeDays = LoggedPerson.FreeDays + (int)GetBusinessDays(DateTime.Parse(HolidaysListSelect.AbsentFrom.ToShortDateString()), DateTime.Parse(HolidaysListSelect.AbsentTo.ToShortDateString())) - (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod));
                    }
                    else
                    {
                        temp.FreeDays = LoggedPerson.FreeDays + (int)GetBusinessDays(DateTime.Parse(HolidaysListSelect.AbsentFrom.ToShortDateString()), DateTime.Parse(HolidaysListSelect.AbsentTo.ToShortDateString()));
                    }

                    Context.Entry(Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First()).CurrentValues.SetValues(temp);
                    Context.SaveChanges();

                }
                else
                {
                    TblPerson temp = new();
                    temp = LoggedPerson;
                    if (ReasonHolidayMod == "Urlop")
                    {
                        temp.FreeDays = LoggedPerson.FreeDays - (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod));
                    }
                    else
                    {
                        temp.FreeDays = LoggedPerson.FreeDays;
                    }

                    Context.Entry(Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First()).CurrentValues.SetValues(temp);
                    Context.SaveChanges();

                }
                NormalModHolidayAccepted();

            }
            else
            {
                NormalModHoliday();
            }
            ManagmentShopViewModel.Update();
            YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;

            NewData();
            IsOpenPopout = false;
            return true;
        }
        private async void BackDeleteHoliday()
        {
            TblUnavailable temp = new();
            temp = HolidaysListSelect;
            temp.ToDelete = false;
            Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidaysListSelect.UnavailableId).First()).CurrentValues.SetValues(temp);
            Context.SaveChanges();
            BackDelete = Visibility.Collapsed;
            ManagmentShopViewModel.Update();
            NewData();
        }
        private async void DelHoliday()
        {
            string texttomsg = String.Format("Czy napewno chcesz usunąć wolne?");
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {
                if (HolidaysListSelect.Accepted == false)
                {
                    NormalDelHoliday();

                }
                else if ((LoggedPerson.PermisionId == 1) || (LoggedPerson.PermisionId == 2))
                {
                    if (HolidaysListSelect.AbsentTo.Year == DateTime.Now.Year)
                    {
                        if (HolidaysListSelect.Reason == "Urlop")
                        {
                            TblPerson temp = new();
                            temp = LoggedPerson;
                            temp.FreeDays = LoggedPerson.FreeDays + (int)GetBusinessDays(DateTime.Parse(HolidaysListSelect.AbsentFrom.ToShortDateString()), DateTime.Parse(HolidaysListSelect.AbsentTo.ToShortDateString()));
                            Context.Entry(Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First()).CurrentValues.SetValues(temp);
                            Context.SaveChanges();
                            NormalDelHoliday();
                        }
                        else
                        {
                            NormalDelHoliday();
                        }
                    }
                    else
                    {
                        NormalDelHoliday();
                    }

                }
                else
                {
                    bool result = await ValidateDel();
                    if (result)
                    {
                        BoundMessageQueue.Enqueue("Wniosek usnięcia urlopu złożony.");
                    }
                }

            }
        }
        private async Task<bool> ValidateDel()
        {
            TblUnavailable temp = new();
            temp = HolidaysListSelect;
            temp.ToDelete = true;
            Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidaysListSelect.UnavailableId).First()).CurrentValues.SetValues(temp);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            NewData();
            return true;

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
        public void NewData()
        {
            DimTabContext con = new();
            Context = con;
            ManagmentShopViewModel.Update();
            if (!string.IsNullOrEmpty(LoggedPerson.Login))
            {
                YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;
            }

            ListOfStops.Clear();
            ListOfBadDates.Clear();
            ListOfStop();
            ListOfUnavaibleDates();
            CheckDaysInUnacctep();
            UpdateDataInPersons();
            CheckIfHolidayReq();
            SelectedDate = MinDate;
            SelectedDate = ActualyDate;
        }
        private void NormalDelHoliday()
        {
            Context.TblUnavailables.Remove(Context.TblUnavailables.Where(d => d.UnavailableId == HolidaysListSelect.UnavailableId).First());
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            NewData();
            BoundMessageQueue.Enqueue("Wniosek usnięty.");
        }
        private void NormalModHoliday()
        {
            TblUnavailable var = new()
            {
                UnavailableId = HolidaysListSelect.UnavailableId,
                PersonId = HolidaysListSelect.PersonId,
                AbsentFrom = DateTime.Parse(StartHolidayDateMod),
                AbsentTo = DateTime.Parse(EndHolidayDateMod),
                DataOfSend = ActualyDate,
                Accepted = false,
                DaysCount = (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod)),
                Reason = ReasonHolidayMod
            };
            Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidaysListSelect.UnavailableId).First()).CurrentValues.SetValues(var);
            Context.SaveChanges();
        }
        private void NormalModHolidayAccepted()
        {
            TblUnavailable var = new()
            {
                UnavailableId = HolidaysListSelect.UnavailableId,
                PersonId = HolidaysListSelect.PersonId,
                AbsentFrom = DateTime.Parse(StartHolidayDateMod),
                AbsentTo = DateTime.Parse(EndHolidayDateMod),
                DataOfSend = ActualyDate,
                Accepted = true,
                DaysCount = (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod)),
                Reason = ReasonHolidayMod
            };
            Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidaysListSelect.UnavailableId).First()).CurrentValues.SetValues(var);
            Context.SaveChanges();
        }
        private void NormalAddHoliday()
        {
            TblUnavailable var = new()
            {
                PersonId = LoggedPerson.PersonId,
                AbsentFrom = DateTime.Parse(StartHolidayDate),
                AbsentTo = DateTime.Parse(EndHolidayDate),
                DataOfSend = ActualyDate,
                Accepted = false,
                DaysCount = (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate)),
                Reason = ReasonHoliday
            };
            Context.TblUnavailables.Add(var);
            Context.SaveChanges();
        }
        private void NormalAddHolidayAccepted()
        {
            TblUnavailable var = new()
            {
                PersonId = LoggedPerson.PersonId,
                AbsentFrom = DateTime.Parse(StartHolidayDate),
                AbsentTo = DateTime.Parse(EndHolidayDate),
                DataOfSend = ActualyDate,
                Accepted = true,
                DaysCount = (int)GetBusinessDays(DateTime.Parse(StartHolidayDate), DateTime.Parse(EndHolidayDate)),
                Reason = ReasonHoliday
            };
            Context.TblUnavailables.Add(var);
            Context.SaveChanges();
        }
        private void UpdateDataInPersons()
        {
            RowCount = 0;
            PersonDays.Clear();
            var con = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).OrderBy(d => d.Surname).ToList();
            foreach (var Person in con)
            {
                RowCount++;
                PersonDays.Add(new PersonHoliday(Context, SelectedShopFromFirstWindow.ShopId, SelectedDate.Year, SelectedDate.Month, Person.PersonId));
            }
        }
        private void ListOfStop()
        {
            var con = Context.TblHolidays.Where(d => d.ItsFreeDay == false).ToList();
            foreach (var dates in con)
            {
                for (var dt = dates.DateFrom; dt <= dates.DateTo; dt = dt.AddDays(1))
                {
                    ListOfStops.Add(dt);
                }
            }
        }
        private void ListOfUnavaibleDates()
        {
            var con = Context.TblUnavailables.Where(d => d.PersonId == LoggedPerson.PersonId).ToList();
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
        private void ChangeLeftMonth()
        {

            if (SelectedDate.Year == DateTime.Now.Year)
            {

                if (SelectedDate.Month > 1)
                {
                    SelectedDate = SelectedDate.AddMonths(-1);
                }
            }
            else
            {
                if (SelectedDate.Month == 1)
                {
                    SelectedDate = SelectedDate.AddMonths(-1);
                }
            }
            ClearClick();
            DimTabContext con = new();
            Context = con;
            ManagmentShopViewModel.Update();
            if (!string.IsNullOrEmpty(LoggedPerson.Login))
            {
                YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;
            }
        }
        private void ChangeRightMonth()
        {
            if (SelectedDate.Year == DateTime.Now.Year)
            {
                if (SelectedDate.Month < 13)
                {
                    SelectedDate = SelectedDate.AddMonths(1);
                }
            }
            else
            {
                if (SelectedDate.Month < 13)
                {
                    SelectedDate = SelectedDate.AddMonths(0);
                }
            }
            ClearClick();
            DimTabContext con = new();
            Context = con;
            ManagmentShopViewModel.Update();
            if (!string.IsNullOrEmpty(LoggedPerson.Login))
            {
                YourDaysOfHolidays = Context.TblPersons.Where(d => d.PersonId == LoggedPerson.PersonId).First().FreeDays;
            }
        }
        private void CheckIfHolidayReq()
        {
            var temp = Context.TblUnavailables.Where(d => d.PersonId == LoggedPerson.PersonId).OrderBy(d => d.Accepted).ToList();
            if (temp.Count > 0)
            {
                HolidaysList = new();
                HolidaysList.Clear();
                HolidaysList = temp;
                HolidaysReqList = Visibility.Visible;
            }
            else
            {
                HolidaysReqList = Visibility.Collapsed;
            }
        }
        private void SelectedMonth()
        {
            switch (SelectedDate.Month.ToString())
            {
                case "1":
                    Month = "Styczeń";
                    break;
                case "2":
                    Month = "Luty";
                    break;
                case "3":
                    Month = "Marzec";
                    break;
                case "4":
                    Month = "Kwiecień";
                    break;
                case "5":
                    Month = "Maj";
                    break;
                case "6":
                    Month = "Czerwiec";
                    break;
                case "7":
                    Month = "Lipiec";
                    break;
                case "8":
                    Month = "Sierpień";
                    break;
                case "9":
                    Month = "Wrzesień";
                    break;
                case "10":
                    Month = "Październik";
                    break;
                case "11":
                    Month = "Listopad";
                    break;
                case "12":
                    Month = "Grudzień";
                    break;
                case "13":
                    Month = "Styczeń";
                    break;
            }
        }
        private bool CanSubmit()
        {
            if ((!string.IsNullOrWhiteSpace(EndHolidayDate) && !string.IsNullOrWhiteSpace(EndHolidayDate) && !string.IsNullOrWhiteSpace(ReasonHoliday)) && HolidayMod == false)
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private bool CanModCom()
        {
            if ((!string.IsNullOrWhiteSpace(EndHolidayDateMod) && !string.IsNullOrWhiteSpace(EndHolidayDateMod) && !string.IsNullOrWhiteSpace(ReasonHolidayMod)) && HolidayMod == true && CanMod == true)
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
            CultureInfo culture = CultureInfo.CreateSpecificCulture("pl-PL");
            DateTimeStyles styles = DateTimeStyles.None;
            DateTime dateResult;
            DateTime dateResult1;
            DateTime dateResult3;
            DateTime dateResult4;
            switch (changedPropertyName)
            {
                case nameof(StartHolidayDate):
                    if (string.IsNullOrWhiteSpace(StartHolidayDate) && HolidayMod == false)
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    else if (!((DateTime.TryParse(StartHolidayDate, culture, styles, out dateResult))) && HolidayMod == false)
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDate)] = new List<object> { "Data rozpoczęcia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(StartHolidayDate)) && HolidayMod == false)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }
                    break;
                case nameof(EndHolidayDate):
                    if (string.IsNullOrWhiteSpace(EndHolidayDate) && HolidayMod == false)
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    else if (!((DateTime.TryParse(EndHolidayDate, culture, styles, out dateResult1))) && HolidayMod == false)
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDate)] = new List<object> { "Data zakończenia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(EndHolidayDate)) && HolidayMod == false)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDate)));
                    }
                    break;
                case nameof(StartHolidayDateMod):
                    if (string.IsNullOrWhiteSpace(StartHolidayDateMod) && HolidayMod == true)
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDateMod)] = new List<object> { "Data rozpoczęcia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                    }
                    else if (!((DateTime.TryParse(StartHolidayDateMod, culture, styles, out dateResult))) && HolidayMod == true)
                    {
                        _ValidationErrorsByProperty[nameof(StartHolidayDateMod)] = new List<object> { "Data rozpoczęcia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(StartHolidayDateMod)) && HolidayMod == true)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                    }
                    break;
                case nameof(EndHolidayDateMod):
                    if (string.IsNullOrWhiteSpace(EndHolidayDateMod) && HolidayMod == true)
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDateMod)] = new List<object> { "Data zakończenia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                    }
                    else if (!((DateTime.TryParse(EndHolidayDateMod, culture, styles, out dateResult1))) && HolidayMod == true)
                    {
                        _ValidationErrorsByProperty[nameof(EndHolidayDateMod)] = new List<object> { "Data zakończenia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(EndHolidayDateMod)) && HolidayMod == true)
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                    }
                    break;
            }
            if ((DateTime.TryParse(StartHolidayDate, culture, styles, out dateResult)) && (DateTime.TryParse(EndHolidayDate, culture, styles, out dateResult1)) && HolidayMod == false)
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
            else if ((DateTime.TryParse(StartHolidayDateMod, culture, styles, out dateResult3)) && (DateTime.TryParse(EndHolidayDateMod, culture, styles, out dateResult4)) && HolidayMod == true)
            {
                if (HolidaysListSelect != null)
                {
                    if (((DateTime.Parse(StartHolidayDateMod) != HolidaysListSelect.AbsentFrom) || (DateTime.Parse(EndHolidayDateMod) != HolidaysListSelect.AbsentTo)) && HolidayMod == true)
                    {
                        List<DateTime> ListDates2 = new();
                        ListDates2.Clear();
                        for (var dt = (DateTime.Parse(StartHolidayDateMod)); dt <= DateTime.Parse(EndHolidayDateMod); dt = dt.AddDays(1))
                        {
                            ListDates2.Add(dt);
                        }
                        if (DateTime.Parse(StartHolidayDateMod) > DateTime.Parse(EndHolidayDateMod))
                        {
                            _ValidationErrorsByProperty[nameof(StartHolidayDateMod)] = new List<object> { "Data zakończenia jest przed datą rozpoczęcia." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                            _ValidationErrorsByProperty[nameof(EndHolidayDateMod)] = new List<object> { "Data zakończenia jest przed datą rozpoczęcia." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                        }
                        else if (ListOfBadDates.Contains((DateTime.Parse(StartHolidayDateMod))))
                        {
                            _ValidationErrorsByProperty[nameof(StartHolidayDateMod)] = new List<object> { "Data rozpoczęcia jest już w innym wniosku/święcie." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                        }
                        else if (ListOfBadDates.Contains((DateTime.Parse(EndHolidayDateMod))))
                        {
                            _ValidationErrorsByProperty[nameof(EndHolidayDateMod)] = new List<object> { "Data zakończenia jest już w innym wniosku/święcie." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                        }
                        else if (ListDates2.Intersect(ListOfBadDates).Any())
                        {
                            _ValidationErrorsByProperty[nameof(StartHolidayDateMod)] = new List<object> { "Data pokrywa się w innym wniosku/święcie." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                            _ValidationErrorsByProperty[nameof(EndHolidayDateMod)] = new List<object> { "Data pokrywa się w innym wniosku/święcie." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                        }
                        else if ((DateTime.Parse(StartHolidayDateMod) < MinDate) || (DateTime.Parse(StartHolidayDateMod) > MaxDate))
                        {
                            _ValidationErrorsByProperty[nameof(StartHolidayDateMod)] = new List<object> { "Data rozpoczęcia musi być w tym roku." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                        }
                        else if ((DateTime.Parse(EndHolidayDateMod) < MinDate) || (DateTime.Parse(EndHolidayDateMod) > MaxDate))
                        {
                            _ValidationErrorsByProperty[nameof(EndHolidayDateMod)] = new List<object> { "Data zakończenia musi być w tym roku." };
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                        }
                        else
                        {
                            _ValidationErrorsByProperty.Remove(nameof(StartHolidayDateMod));
                            _ValidationErrorsByProperty.Remove(nameof(EndHolidayDateMod));
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                            DaysOfSelectedDatesMod = (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod));
                        }
                    }
                    else
                    {
                        _ValidationErrorsByProperty.Remove(nameof(StartHolidayDateMod));
                        _ValidationErrorsByProperty.Remove(nameof(EndHolidayDateMod));
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                        DaysOfSelectedDatesMod = (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod));
                    }
                }
                else
                {
                    _ValidationErrorsByProperty.Remove(nameof(StartHolidayDateMod));
                    _ValidationErrorsByProperty.Remove(nameof(EndHolidayDateMod));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartHolidayDateMod)));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EndHolidayDateMod)));
                    DaysOfSelectedDatesMod = (int)GetBusinessDays(DateTime.Parse(StartHolidayDateMod), DateTime.Parse(EndHolidayDateMod));

                }

            }
            SubmitCommand.NotifyCanExecuteChanged();
            ModCommand.NotifyCanExecuteChanged();
        }
    }
}

