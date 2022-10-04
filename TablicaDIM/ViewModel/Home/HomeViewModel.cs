using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using TablicaDIM.View;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel.Home
{
    public class HomeViewModel : InputsViewModel, IMenuItem
    {
        public string Title { get; } = "Strona główna";
        public static string TitleToMenu { get; } = "Strona główna";
        public SeriesCollection Percent_of_breakdown { get; set; }

        private DispatcherTimer _liveUpdate;
        public DispatcherTimer LiveUpdate
        {
            get => _liveUpdate;
            set => SetProperty(ref _liveUpdate, value);
        }
        private bool _automaticalRefresh;
        public bool AutomaticalRefresh
        {
            get => _automaticalRefresh;
            set
            {
                if (SetProperty(ref _automaticalRefresh, value))
                {
                    if (value)
                    {
                        LiveUpdate.Start();
                        TextToSearch = string.Empty;
                        OnlyEndActions = false;
                        OnlyAfterDateActions = false;
                    }
                    else
                    {
                        LiveUpdate.Stop();
                    }
                }
            }
        }
        private bool _onlyEndActions;
        public bool OnlyEndActions
        {
            get => _onlyEndActions;
            set
            {
                if (SetProperty(ref _onlyEndActions, value))
                {
                    if (value)
                    {
                        AutomaticalRefresh = false;
                        OnlyAfterDateActions = false;
                        VisiblityOfEndAction = Visibility.Visible;
                        DateToDatesAfterDates = DateTime.MaxValue;
                        BoolToDatesEnd = true;
                        TextToSearch = string.Empty;
                        UpdateData();
                    }
                    else
                    {
                        VisiblityOfEndAction = Visibility.Collapsed;
                        DateToDatesAfterDates = DateTime.MaxValue;
                        TextToSearch = string.Empty;
                        BoolToDatesEnd = false;
                        UpdateData();
                    }
                }
            }
        }
        private bool _OnlyAfterDateActions;
        public bool OnlyAfterDateActions
        {
            get => _OnlyAfterDateActions;
            set
            {
                if (SetProperty(ref _OnlyAfterDateActions, value))
                {
                    if (value)
                    {
                        AutomaticalRefresh = false;
                        OnlyEndActions = false;
                        BoolToDatesEnd = false;
                        TextToSearch = string.Empty;
                        DateToDatesAfterDates = DateTime.Now.Date;
                        UpdateData();
                    }
                    else
                    {
                        DateToDatesAfterDates = DateTime.MaxValue;
                        TextToSearch = string.Empty;
                        BoolToDatesEnd = false;
                        UpdateData();
                    }
                }
            }
        }
        private string _textToSearch;
        public string TextToSearch
        {
            get => _textToSearch;
            set
            {
                if (SetProperty(ref _textToSearch, value))
                {
                    if (value != null)
                    {
                        if (value.Length > 0)
                        {
                            AutomaticalRefresh = false;
                            DataSearchActions();
                        }
                        else
                        {
                            UpdateData();
                        }
                    }

                }
            }
        }
        private Visibility _visiblityOfEndAction;
        public Visibility VisiblityOfEndAction
        {
            get => _visiblityOfEndAction;
            set => SetProperty(ref _visiblityOfEndAction, value);
        }
        private Visibility _visiblityCanMod;
        public Visibility VisiblityCanMod
        {
            get => _visiblityCanMod;
            set => SetProperty(ref _visiblityCanMod, value);
        }
        private int _heightOfActions;
        public int HeightOfActions
        {
            get => _heightOfActions;
            set => SetProperty(ref _heightOfActions, value);
        }
        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    if (value != null)
                    {
                        string temp = value.ToString();
                        string findWord = "DataId =";
                        int indexFirstWord = temp.IndexOf(findWord);
                        string tempSecond = temp.Substring(indexFirstWord);
                        string[] tempArray = tempSecond.Split(", DateCreateAction =");
                        string[] tempArraySecond = tempArray[0].Split("= ");
                        TblDataGrid tempSelect = Context.TblDataGrids.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.DataId == Int32.Parse(tempArraySecond[1].ToString())).First();
                        if (LoggedPerson.PermisionId == 4)
                        {
                            if (tempSelect.EndAction == false)
                            {
                                if (tempSelect.PersonId == LoggedPerson.PersonId)
                                {
                                    VisiblityCanMod = Visibility.Visible;
                                }
                                else
                                {
                                    VisiblityCanMod = Visibility.Collapsed;
                                }
                            }
                            else
                            {
                                VisiblityCanMod = Visibility.Collapsed;
                            }

                        }
                        else if ((LoggedPerson.PermisionId >= 1) && (LoggedPerson.PermisionId <= 3))
                        {
                            VisiblityCanMod = Visibility.Visible;
                        }
                        else
                        {
                            VisiblityCanMod = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        VisiblityCanMod = Visibility.Collapsed;
                    }
                }
            }
        }
        private string? _chartPercentageOfBreakDownstxt;
        public string ChartPercentageOfBreakDownstxt
        {
            get
            {
                if (_chartPercentageOfBreakDownstxt != null)
                {
                    return _chartPercentageOfBreakDownstxt;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartPercentageOfBreakDownstxt, value);
        }
        private ChartValues<double> _arrayPercentage;
        public ChartValues<double> ArrayPercentage
        {
            get => _arrayPercentage;
            set => SetProperty(ref _arrayPercentage, value);
        }
        private string _firstNextDate;
        public string FirstNextDate
        {
            get => _firstNextDate;
            set => SetProperty(ref _firstNextDate, value);
        }
        private string _secondNextDate;
        public string SecondNextDate
        {
            get => _secondNextDate;
            set => SetProperty(ref _secondNextDate, value);
        }
        private ObservableCollection<string> _avaiblePersons;
        public ObservableCollection<string> AvaiblePersons
        {
            get => _avaiblePersons;
            set => SetProperty(ref _avaiblePersons, value);
        }
        private string _weekendNumber;
        public string WeekendNumber
        {
            get => _weekendNumber;
            set => SetProperty(ref _weekendNumber, value);
        }

        private ObservableCollection<TblChart> _objectToVal;
        public ObservableCollection<TblChart?> ObjectToVal
        {
            get => _objectToVal;
            set => SetProperty(ref _objectToVal, value);
        }

        private Task _taskCheckIn;
        public Task TaskCheckIn
        {
            get => _taskCheckIn;
            set => SetProperty(ref _taskCheckIn, value);
        }
        private int _officeAlarm;
        public int OfficeAlarm
        {
            get => _officeAlarm;
            set => SetProperty(ref _officeAlarm, value);
        }
        private int _officeWarning;
        public int OfficeWarning
        {
            get => _officeWarning;
            set => SetProperty(ref _officeWarning, value);
        }
        private int _technicalAlarm;
        public int TechnicalAlarm
        {
            get => _technicalAlarm;
            set => SetProperty(ref _technicalAlarm, value);
        }
        private int _technicalWarning;
        public int TechnicalWarning
        {
            get => _technicalWarning;
            set => SetProperty(ref _technicalWarning, value);
        }

        private ObservableCollection<TblPerson> _tblPersons;
        public ObservableCollection<TblPerson> TblPersons
        {
            get => _tblPersons;
            set => SetProperty(ref _tblPersons, value);
        }

        private ObservableCollection<TblUnavailable> _tblUnavailables;
        public ObservableCollection<TblUnavailable> TblUnavailables
        {
            get => _tblUnavailables;
            set => SetProperty(ref _tblUnavailables, value);
        }
        private ObservableCollection<TblDataGrid> _tblDatagrids;
        public ObservableCollection<TblDataGrid> TblDatagrids
        {
            get => _tblDatagrids;
            set => SetProperty(ref _tblDatagrids, value);
        }
        private ObservableCollection<TblPlace> _tblPlaces;
        public ObservableCollection<TblPlace> TblPlaces
        {
            get => _tblPlaces;
            set => SetProperty(ref _tblPlaces, value);
        }
        private ObservableCollection<TblShiftInWeekend> _tblShiftInWeekend;
        public ObservableCollection<TblShiftInWeekend> TblShiftInWeekends
        {
            get => _tblShiftInWeekend;
            set => SetProperty(ref _tblShiftInWeekend, value);
        }
        private ObservableCollection<TblPersonInWeekend> _tblPersonInWeekends;
        public ObservableCollection<TblPersonInWeekend> TblPersonInWeekends
        {
            get => _tblPersonInWeekends;
            set => SetProperty(ref _tblPersonInWeekends, value);
        }
        private ObservableCollection<object> _dataToContext;
        public ObservableCollection<object> DataToContext
        {
            get => _dataToContext;
            set => SetProperty(ref _dataToContext, value);
        }
        private DateTime _dateToDatesAfterDates;
        public DateTime DateToDatesAfterDates
        {
            get => _dateToDatesAfterDates;
            set => SetProperty(ref _dateToDatesAfterDates, value);
        }
        private bool _boolToDatesEnd;
        public bool BoolToDatesEnd
        {
            get => _boolToDatesEnd;
            set => SetProperty(ref _boolToDatesEnd, value);
        }
        private bool _isWeekendWork;
        public bool IsWeekendWork
        {
            get => _isWeekendWork;
            set
            {
                if (SetProperty(ref _isWeekendWork, value))
                {
                    if (value)
                    {
                        VisiblityOfWeekendWork = Visibility.Visible;
                        HeightOfActions = 300;
                    }
                    else
                    {
                        VisiblityOfWeekendWork = Visibility.Collapsed;
                        HeightOfActions = 550;

                    }

                }
            }
        }
        private Visibility _visiblityOfWeekendWork;
        public Visibility VisiblityOfWeekendWork
        {
            get => _visiblityOfWeekendWork;
            set => SetProperty(ref _visiblityOfWeekendWork, value);
        }
        private ObservableCollection<string> _stringPeoples;
        public ObservableCollection<string> StringPeoples
        {
            get => _stringPeoples;
            set => SetProperty(ref _stringPeoples, value);
        }
        private ObservableCollection<Visibility> _visiblityOfWork;
        public ObservableCollection<Visibility> VisiblityOfWork
        {
            get => _visiblityOfWork;
            set => SetProperty(ref _visiblityOfWork, value);
        }

        public RelayCommand AddActionCommand { get; }
        public RelayCommand ClearClickCommand { get; }
        public RelayCommand OpenGraphCommand { get; }
        public RelayCommand ModActionCommand { get; }
        private RelayCommand<string> _plusCommand;

        public RelayCommand<string> PlusCommand
        {
            get { return _plusCommand ?? (_plusCommand = new RelayCommand<string>(PlusPerson)); }
            set { _plusCommand = value; }
        }
        private RelayCommand<string> _minusCommand;

        public RelayCommand<string> MinusCommand
        {
            get { return _minusCommand ?? (_minusCommand = new RelayCommand<string>(MinusPerson)); }
            set { _minusCommand = value; }
        }



        public HomeViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            AddActionCommand = new RelayCommand(OpenAddWindow);
            ClearClickCommand = new RelayCommand(ClearClick);
            OpenGraphCommand = new RelayCommand(OpenGraph);
            ModActionCommand = new RelayCommand(OpenModWindow);
            DataAssigment(managmentshopviewmodel);
            CreateVars();
            TaskCheckIn = CheckIn();
            if (TaskCheckIn.Status == TaskStatus.Created)
                TaskCheckIn.Start();

            TextToSearch = string.Empty;
            AutomaticalRefresh = true;
            OnlyEndActions = false;
            OnlyAfterDateActions = false;
            VisiblityOfEndAction = Visibility.Collapsed;
            VisiblityCanMod = Visibility.Collapsed;
            VisiblityOfWeekendWork = Visibility.Collapsed;
            HeightOfActions = 550;
        }

        private async void UpdateData()
        {
            TblDatagrids.Clear();
            TblPlaces.Clear();
            TblPersons.Clear();
            DataToContext.Clear();
            Context = new DimTabContext();
            await CheckIn();
        }
        private async void ClearClick()
        {
            SelectedItem = null;
        }
        private async void OpenAddWindow()
        {
            AutomaticalRefresh = false;
            var viewModel = new AddActionViewModel(ManagmentShopViewModel);
            await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            AutomaticalRefresh = true;
            OnlyAfterDateActions = false;
            OnlyEndActions = false;
            TextToSearch = string.Empty;
            UpdateData();

        }
        private async void OpenModWindow()
        {
            AutomaticalRefresh = false;
            var viewModel = new ModActionViewModel(ManagmentShopViewModel, SelectedItem);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            AutomaticalRefresh = true;
            OnlyAfterDateActions = false;
            OnlyEndActions = false;
            TextToSearch = string.Empty;
            UpdateData();
        }
        private void DataSearchActions()
        {
            DataToContext.Clear();
            Context = new DimTabContext();
            if (OnlyEndActions)
            {
                var query = from b in Context.Set<TblDataGrid>()
                            join p in Context.Set<TblPerson>() on b.PersonId equals p.PersonId
                            join d in Context.Set<TblPlace>() on b.PlaceId equals d.PlaceId
                            where b.ShopId == SelectedShopFromFirstWindow.ShopId
                            where b.EndAction == BoolToDatesEnd
                            where b.DatePlaningAction < DateToDatesAfterDates
                            where EF.Functions.Like(b.Problem, "%" + TextToSearch + "%") || EF.Functions.Like(d.PlaceName, "%" + TextToSearch + "%") || EF.Functions.Like(b.Cause, "%" + TextToSearch + "%") || EF.Functions.Like(b.Solve, "%" + TextToSearch + "%") || EF.Functions.Like(p.Surname, "%" + TextToSearch + "%") || EF.Functions.Like(p.Name, "%" + TextToSearch + "%")
                            orderby b.DatePlaningAction ascending
                            select new { b.DataId, b.DateCreateAction, b.DatePlaningAction, d.PlaceName, b.Problem, b.Cause, b.Solve, FullName = p.Surname + " " + p.Name, p.PersonId, b.AddWho, b.AddWhen, b.ModWho, b.ModWhen, b.DateEndAction, b.EndAction, b.Step, b.Info };
                foreach (var item in query)
                    DataToContext.Add(item);
            }
            else if (OnlyAfterDateActions)
            {
                var query = from b in Context.Set<TblDataGrid>()
                            join p in Context.Set<TblPerson>() on b.PersonId equals p.PersonId
                            join d in Context.Set<TblPlace>() on b.PlaceId equals d.PlaceId
                            where b.ShopId == SelectedShopFromFirstWindow.ShopId
                            where b.EndAction == BoolToDatesEnd
                            where b.DatePlaningAction < DateToDatesAfterDates
                            where EF.Functions.Like(b.Problem, "%" + TextToSearch + "%") || EF.Functions.Like(d.PlaceName, "%" + TextToSearch + "%") || EF.Functions.Like(b.Cause, "%" + TextToSearch + "%") || EF.Functions.Like(b.Solve, "%" + TextToSearch + "%") || EF.Functions.Like(p.Surname, "%" + TextToSearch + "%") || EF.Functions.Like(p.Name, "%" + TextToSearch + "%")
                            orderby b.DatePlaningAction ascending
                            select new { b.DataId, b.DateCreateAction, b.DatePlaningAction, d.PlaceName, b.Problem, b.Cause, b.Solve, FullName = p.Surname + " " + p.Name, p.PersonId, b.AddWho, b.AddWhen, b.ModWho, b.ModWhen, b.DateEndAction, b.EndAction, b.Step, b.Info };
                foreach (var item in query)
                    DataToContext.Add(item);
            }
            else
            {
                var query = from b in Context.Set<TblDataGrid>()
                            join p in Context.Set<TblPerson>() on b.PersonId equals p.PersonId
                            join d in Context.Set<TblPlace>() on b.PlaceId equals d.PlaceId
                            where b.EndAction == BoolToDatesEnd
                            where b.DatePlaningAction < DateToDatesAfterDates
                            where EF.Functions.Like(b.Problem, "%" + TextToSearch + "%") || EF.Functions.Like(d.PlaceName, "%" + TextToSearch + "%") || EF.Functions.Like(b.Cause, "%" + TextToSearch + "%") || EF.Functions.Like(b.Solve, "%" + TextToSearch + "%") || EF.Functions.Like(p.Surname, "%" + TextToSearch + "%") || EF.Functions.Like(p.Name, "%" + TextToSearch + "%")
                            orderby b.DatePlaningAction ascending
                            select new { b.DataId, b.DateCreateAction, b.DatePlaningAction, d.PlaceName, b.Problem, b.Cause, b.Solve, FullName = p.Surname + " " + p.Name, p.PersonId, b.AddWho, b.AddWhen, b.ModWho, b.ModWhen, b.DateEndAction, b.EndAction, b.Step, b.Info };
                foreach (var item in query)
                    DataToContext.Add(item);
            }
        }
        private void CreateVars()
        {
            AvaiblePersons = new();
            ArrayPercentage = new();
            ObjectToVal = new();
            TblUnavailables = new();
            TblPersons = new();
            TblPlaces = new();
            TblShiftInWeekends = new();
            DataToContext = new();
            StringPeoples = new();
            VisiblityOfWork = new();
            TblPersonInWeekends = new();
            for (int i = 0; i < 17; i++)
            {
                StringPeoples.Add("");
                VisiblityOfWork.Add(Visibility.Collapsed);
                AvaiblePersons.Add("0");
            }
            DateToDatesAfterDates = DateTime.MaxValue;
            BoolToDatesEnd = false;
            LiveUpdate = new();
            LiveUpdate.Interval = TimeSpan.FromSeconds(5);
            LiveUpdate.Tick += delegate
            {
                UpdateData();
            };
        }
        private async Task CheckIn()
        {
            await CreateGraph();
            await CreateAvaiblePersons();
            await CreateWeekendWork();
            await CreateDataToDatagrid();
        }
        private async Task CreateWeekendWork()
        {
            int week = GetWeekNumber(DateTime.Now);
            WeekendNumber = week.ToString();
            TblShiftInWeekends = GetShiftInWeekendFromDb();
            TblPersonInWeekends = GetPersonsInWEekendFromDb();

            int index = 0;
            int indexVis = 0;

            IsWeekendWork = false;
            foreach (var item in TblShiftInWeekends)
            {
                if (item.Count > 0)
                {
                    IsWeekendWork = true;
                    // Shift day index 0 - 1zm sat, 1 - 2 zm sat, 2 - 3 zm sat, 3 - off sat
                    // 4 - 1 zm sun, 5 - 2 zm sun, 6 - 3 zm sun, 7 - off sun
                    List<TblPersonInWeekend> listOfNames = new();
                    string stringOfNames = "";
                    int personsCout = 0;
                    listOfNames = TblPersonInWeekends.Where(d => d.ShiftDayIndex == index).ToList();
                    foreach (var person in listOfNames)
                    {
                        if (personsCout > 0)
                            stringOfNames += ", ";
                        var objectPerson = TblPersons.Where(d => d.PersonId == person.PersonId).First();
                        stringOfNames += objectPerson.Surname + " " + objectPerson.Name + " - " + person.Reason;
                        personsCout++;
                    }
                    if (personsCout > 0)
                        stringOfNames.Remove(stringOfNames.Count() - 2, 2);
                    if (stringOfNames == "")
                        StringPeoples[index] = "Miejsca: " + personsCout + "/" + item.Count.ToString();
                    else
                        StringPeoples[index] = stringOfNames + System.Environment.NewLine + "Miejsca: " + personsCout + "/" + item.Count.ToString();
                    bool PersonWorkSat = false;
                    bool PersonWorkSun = false;
                    bool PersonWorkInThisDay = false;

                    if (LoggedPerson.PermisionId == 4)
                    {
                        var checkingOfWorkSat = TblPersonInWeekends.Where(d => d.PersonId == LoggedPerson.PersonId).Where(d => d.ShiftDayIndex < 4).ToList();
                        var checkingOfWorkSun = TblPersonInWeekends.Where(d => d.PersonId == LoggedPerson.PersonId).Where(d => d.ShiftDayIndex > 4).ToList();
                        var checkingOfWorkToday = TblPersonInWeekends.Where(d => d.PersonId == LoggedPerson.PersonId).Where(d => d.ShiftDayIndex == index).ToList();
                        if (checkingOfWorkSat.Count > 0)
                            PersonWorkSat = true;
                        if (checkingOfWorkSun.Count > 0)
                            PersonWorkSun = true;
                        if (checkingOfWorkToday.Count > 0)
                            PersonWorkInThisDay = true;
                    }
                    if (personsCout < item.Count)
                    {
                        if(((index<4)&& (LoggedPerson.PermisionId == 4)&& PersonWorkSat == false)|| ((LoggedPerson.PermisionId < 4 ) && (LoggedPerson.PermisionId >0)) )
                            VisiblityOfWork[indexVis] = Visibility.Visible;
                        else if (((index > 4) && (LoggedPerson.PermisionId == 4) && PersonWorkSun == false) || ((LoggedPerson.PermisionId < 4) && (LoggedPerson.PermisionId > 0)))
                            VisiblityOfWork[indexVis] = Visibility.Visible;
                        else
                            VisiblityOfWork[indexVis] = Visibility.Collapsed;
                    }
                    else
                    {
                        VisiblityOfWork[indexVis] = Visibility.Collapsed;
                    }
                    indexVis++;
                    if (personsCout >0)
                    {
                        if (((LoggedPerson.PermisionId == 4) && PersonWorkInThisDay == true) || ((LoggedPerson.PermisionId < 4) && (LoggedPerson.PermisionId > 0)))
                            VisiblityOfWork[indexVis] = Visibility.Visible;
                        else
                            VisiblityOfWork[indexVis] = Visibility.Collapsed;
                    }
                    else
                        VisiblityOfWork[indexVis] = Visibility.Collapsed;
                    indexVis++;
                }
                else
                {
                    StringPeoples[index] = "Brak planu.";
                    VisiblityOfWork[indexVis] = Visibility.Collapsed;
                    indexVis++;
                    VisiblityOfWork[indexVis] = Visibility.Collapsed;
                    indexVis++;
                }
                index++;
            }
        }

        private async Task CreateDataToDatagrid()
        {
            TblDatagrids = GetDataGridFromDb();
            TblPlaces = GetPlacesFromDb();
            var query = (from b in TblDatagrids
                         join p in TblPersons on b.PersonId equals p.PersonId
                         join d in TblPlaces on b.PlaceId equals d.PlaceId
                         where b.ShopId == SelectedShopFromFirstWindow.ShopId
                         where b.EndAction == BoolToDatesEnd
                         where b.DatePlaningAction < DateToDatesAfterDates
                         orderby b.DatePlaningAction ascending
                         select new { b.DataId, b.DateCreateAction, b.DatePlaningAction, d.PlaceName, b.Problem, b.Cause, b.Solve, FullName = p.Surname + " " + p.Name, p.PersonId, b.AddWho, b.AddWhen, b.ModWho, b.ModWhen, b.DateEndAction, b.EndAction, b.Step, b.Info }).ToList();
            foreach (var item in query)
                DataToContext.Add(item);
        }
        private ObservableCollection<TblDataGrid> GetDataGridFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblDataGrid>();
            var list = new List<TblDataGrid> { /* ... */ };
            list = Context.TblDataGrids.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        private static int GetWeekNumber(DateTime time)
        {
            GregorianCalendar cal = new GregorianCalendar();
            int week = cal.GetWeekOfYear(time, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            return week;
        }
        private ObservableCollection<TblPerson> GetPersonsFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblPerson>();
            var list = new List<TblPerson> { /* ... */ };
            list = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        private ObservableCollection<TblPlace> GetPlacesFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblPlace>();
            var list = new List<TblPlace> { /* ... */ };
            list = Context.TblPlaces.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }

        private ObservableCollection<TblPersonInWeekend> GetPersonsInWEekendFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblPersonInWeekend>();
            var list = new List<TblPersonInWeekend> { /* ... */ };
            list = Context.TblPersonInWeekends.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.WeekNumber == Int32.Parse(WeekendNumber)).
                Where(d => d.YearNumber == DateTime.Now.Year).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        private ObservableCollection<TblShiftInWeekend> GetShiftInWeekendFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblShiftInWeekend>();
            var list = new List<TblShiftInWeekend> { /* ... */ };
            list = Context.TblShiftInWeekends.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.WeekNumber == Int32.Parse(WeekendNumber)).
                Where(d => d.YearNumber == DateTime.Now.Year).OrderBy(d => d.IdshopInWeekend).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        private ObservableCollection<TblUnavailable> GetUnavailableFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblUnavailable>();
            var list = new List<TblUnavailable> { /* ... */ };
            list = Context.TblUnavailables.ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        private int CheckPersonsInShift(ObservableCollection<TblPerson> tblPersons, ObservableCollection<TblUnavailable> TblUnavailables, int shift, DateTime date)
        {
            int result = 0;

            string newDate = date.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            var temp = tblPersons.Where(d => d.Shift == shift).ToList();
            foreach (var person in temp)
            {
                var tempHolidayList = TblUnavailables.Where(d => d.PersonId == person.PersonId)
                    .Where(d => d.AbsentFrom <= DateTime.Parse(newDate) && d.AbsentTo >= DateTime.Parse(newDate)).Where(d => d.Accepted == true).ToList();
                if (tempHolidayList.Count == 0)
                {
                    result++;
                }
            }
            return result;
        }
        private async Task CreateAvaiblePersons()
        {
            TechnicalAlarm = SelectedShopFromFirstWindow.TechnicalAlarm;
            TechnicalWarning = SelectedShopFromFirstWindow.TechnicalWarning;
            OfficeAlarm = SelectedShopFromFirstWindow.OfficeAlarm;
            OfficeWarning = SelectedShopFromFirstWindow.OfficeWarning;
            TblPersons.Clear();
            TblUnavailables.Clear();
            TblPersons = GetPersonsFromDb();
            TblUnavailables = GetUnavailableFromDb();
            ObservableCollection<DateTime> days = new();
            ObservableCollection<DayOfWeek> daysOfWeek = new();
            for (int i = 0; i < 4; i++)
            {
                days.Add(DateTime.Now.AddDays(i));
                daysOfWeek.Add(DateTime.Now.AddDays(i).DayOfWeek);
            }
            FirstNextDate = days[2].ToShortDateString();
            SecondNextDate = days[3].ToShortDateString();
            DayOfWeek satDay = DayOfWeek.Saturday;
            DayOfWeek sunDay = DayOfWeek.Sunday;
            string weekend = "Weekend";
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                if ((daysOfWeek[i] == satDay) || (daysOfWeek[i] == sunDay))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        AvaiblePersons[index] = weekend;
                        index++;
                    }
                }
                else if (GetWeekNumber(days[i]) != GetWeekNumber(days[0]))
                {
                    AvaiblePersons[index] = (CheckPersonsInShift(TblPersons, TblUnavailables, 2, days[i])).ToString();
                    index++;
                    AvaiblePersons[index] = (CheckPersonsInShift(TblPersons, TblUnavailables, 3, days[i])).ToString();
                    index++;
                    AvaiblePersons[index] = (CheckPersonsInShift(TblPersons, TblUnavailables, 1, days[i])).ToString();
                    index++;
                    AvaiblePersons[index] = (CheckPersonsInShift(TblPersons, TblUnavailables, 4, days[i])).ToString();
                    index++;
                }
                else
                {
                    for (int shift = 1; shift < 5; shift++)
                    {
                        AvaiblePersons[index] = (CheckPersonsInShift(TblPersons, TblUnavailables, shift, days[i])).ToString();
                        index++;
                    }
                }
            }
        }
        private async void OpenGraph()
        {
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectGraphView();
        }
        private ObservableCollection<TblChart> GetChartsFromDb()
        {
            var GetDataFromContext = new ObservableCollection<TblChart>();
            var list = new List<TblChart> { /* ... */ };
            list = Context.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Year == DateTime.Now.Year).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        private async Task CreateGraph()
        {
            ArrayPercentage.Clear();
            ObjectToVal.Clear();
            ChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.ChartPercentOfBreakdown;
            MinChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.MinChartPercentOfBreakdown;
            MaxChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.MaxChartPercentOfBreakdown;

            foreach (var chart in GetChartsFromDb())
                ObjectToVal.Add(chart);

            ArrayPercentage.Add(0);

            if (ObjectToVal.Count > 0)
            {
                for (int i = 0; i < 52; i++)
                {
                    ArrayPercentage.Add(Convert.ToDouble(ObjectToVal[i].PercentOfBreakdown));
                }
            }
            else
            {
                for (int i = 0; i < 52; i++)
                {
                    ArrayPercentage.Add(0d);
                }
            }

            Percent_of_breakdown = new();
            Percent_of_breakdown.Clear();
            Percent_of_breakdown.Add(new ColumnSeries
            {

                Values = ArrayPercentage,
                DataLabels = false,
                Stroke = Brushes.Orange,
                Fill = Brushes.Orange,
                Title = "Procent awaryjności [%]",
                StrokeThickness = 4,

            });
            Percent_of_breakdown.Add(new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(0)
                    },
                DataLabels = false,
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Title = "Cel procentu awaryjności = " + ChartPercentageOfBreakDowns + "[%]",
                StrokeThickness = 0,
            });

        }
        private async void PlusPerson(string value)
        {
            var viewModel = new AddPersonToWeekendViewModel(ManagmentShopViewModel, value, true);
            await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");

            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            UpdateData();
        }
        private async void MinusPerson(string value)
        {
            var viewModel = new AddPersonToWeekendViewModel(ManagmentShopViewModel, value, false);
            await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");

            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            UpdateData();
        }
    }
}