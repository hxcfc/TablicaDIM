using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel
{
    public class GraphsViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public static string TitleToMenu { get; } = "Wskaźniki";
        public string Title { get; } = "Wskaźniki";
        public SeriesCollection Percent_of_breakdown { get; set; }
        public SeriesCollection MTTR { get; set; }
        public SeriesCollection MTBF { get; set; }
        public List<int> _weekTake;
        public List<int> WeeKTake
        {
            get => _weekTake;
            set => SetProperty(ref _weekTake, value);
        }
        public int _selectedWeek;
        public int SelectedWeek
        {
            get => _selectedWeek;
            set
            {
                if (SetProperty(ref _selectedWeek, value))
                {
                    if ((value != null) && (value > 0) && (value < 55))
                    {
                        int weekNum = value;
                        TblChart selectedValues = new();
                        if (Context.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.NumberOfWeek == weekNum).Where(d => d.Year == DateTime.Now.Year).Any())
                        {
                            selectedValues = Context.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.NumberOfWeek == weekNum).Where(d => d.Year == DateTime.Now.Year).First();
                            ChartPercentageOfBreakDownstxt = selectedValues.PercentOfBreakdown.ToString();
                            ChartMTTRtxt = selectedValues.Mttr.ToString();
                            ChartMTBFtxt = selectedValues.Mtbf.ToString();
                            ChartCountOfBreakDownstxt = selectedValues.CoutOfBreakdown.ToString();
                        }
                        else
                        {
                            ChartPercentageOfBreakDownstxt = "0";
                            ChartMTTRtxt = "0";
                            ChartMTBFtxt = "0";
                            ChartCountOfBreakDownstxt = "0";
                        }
                    }
                }
            }
        }
        public string? _chartMTTRtxt;
        public string ChartMTTRtxt
        {
            get
            {
                if (_chartMTTRtxt != null)
                {
                    return _chartMTTRtxt;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartMTTRtxt, value);
        }
        private string? _chartMTBFtxt;
        public string ChartMTBFtxt
        {
            get
            {
                if (_chartMTBFtxt != null)
                {
                    return _chartMTBFtxt;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartMTBFtxt, value);
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

        private string? _chartCountOfBreakDownstxt;
        public string ChartCountOfBreakDownstxt
        {
            get
            {
                if (_chartCountOfBreakDownstxt != null)
                {
                    return _chartCountOfBreakDownstxt;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _chartCountOfBreakDownstxt, value);
        }

        private Visibility _onlyThisYear;
        public Visibility OnlyThisYear
        {
            get => _onlyThisYear;
            set => SetProperty(ref _onlyThisYear, value);
        }
        private bool _isOpenPopout;
        public bool IsOpenPopout
        {
            get => _isOpenPopout;
            set => SetProperty(ref _isOpenPopout, value);
        }
        private bool _isOpenPopoutChangeYear;
        public bool IsOpenPopoutChangeYear
        {
            get => _isOpenPopoutChangeYear;
            set => SetProperty(ref _isOpenPopoutChangeYear, value);
        }
        private ChartValues<double> _arrayPercentage;
        public ChartValues<double> ArrayPercentage
        {
            get => _arrayPercentage;
            set => SetProperty(ref _arrayPercentage, value);
        }
        private ChartValues<double> _arrayMTTR;
        public ChartValues<double> ArrayMTTR
        {
            get => _arrayMTTR;
            set => SetProperty(ref _arrayMTTR, value);
        }
        private ChartValues<double> _arrayMTBF;
        public ChartValues<double> ArrayMTBF
        {
            get => _arrayMTBF;
            set => SetProperty(ref _arrayMTBF, value);
        }
        private ChartValues<int> _arrayCout;
        public ChartValues<int> ArrayCout
        {
            get => _arrayCout;
            set => SetProperty(ref _arrayCout, value);
        }
        private ObservableCollection<string> _years;
        public ObservableCollection<string> Years
        {
            get => _years;
            set => SetProperty(ref _years, value);
        }
        private string? _yearPick;
        public string? YearPick
        {
            get => _yearPick;
            set => SetProperty(ref _yearPick, value);
        }
        private int _valueToMove;
        public int ValueToMove
        {
            get => _valueToMove;
            set => SetProperty(ref _valueToMove, value);
        }
        public RelayCommand OpenPopoutCommand { get; }
        public RelayCommand OpenPopoutChangeYearCommand { get; }
        public RelayCommand SubmitCommand { get; }
        public RelayCommand ClearCommand { get; }

        public GraphsViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(ModWeeks, CanSubmit);
            OpenPopoutCommand = new RelayCommand(OpenPopout);
            OpenPopoutChangeYearCommand = new RelayCommand(OpenPopoutChangeYear);
            ClearCommand = new RelayCommand(ClearUser, CanSelect);
            DataAssigment(managmentshopviewmodel);
            Years = new();
            WeeKTake = new();
            for (int i = 1; i < 53; i++)
            {
                WeeKTake.Add(i);
            }
            if (managmentshopviewmodel.PermisionMaster == Visibility.Visible)
            {
                ValueToMove = 260;
            }
            else
            {
                ValueToMove = 35;
            }
            YearPick = null;
            OnlyThisYear = Visibility.Visible;
            IsOpenPopout = false;
            IsOpenPopoutChangeYear = false;
            //Charts
            MinChartMTTR = SelectedShopFromFirstWindow.MinChartMttr;
            MaxChartMTTR = SelectedShopFromFirstWindow.MaxChartMttr;
            ChartMTTR = SelectedShopFromFirstWindow.ChartMttr;
            MinChartMTBF = SelectedShopFromFirstWindow.MinChartMtbf;
            MaxChartMTBF = SelectedShopFromFirstWindow.MaxChartMtbf;
            ChartMTBF = SelectedShopFromFirstWindow.ChartMtbf;
            ChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.ChartPercentOfBreakdown;
            MinChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.MinChartPercentOfBreakdown;
            MaxChartPercentageOfBreakDowns = SelectedShopFromFirstWindow.MaxChartPercentOfBreakdown;
            MinChartCountOfBreakDowns = SelectedShopFromFirstWindow.MinChartCoutOfBreakdown;
            MaxChartCountOfBreakDowns = SelectedShopFromFirstWindow.MaxChartCoutOfBreakdown;
            ChartCountOfBreakDowns = SelectedShopFromFirstWindow.ChartCoutOfBreakdown;
            //Series
            ArrayPercentage = new ChartValues<double>();
            ArrayMTTR = new ChartValues<double>();
            ArrayMTBF = new ChartValues<double>();
            ArrayCout = new ChartValues<int>();
            UpdateSelectData();
            LiveCharts();
        }
        private void DataToCharts()
        {
            DimTabContext NewTab = new();
            List<TblChart> objectToVal = new();
            objectToVal = NewTab.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Year == DateTime.Now.Year).ToList();
            if (objectToVal.Count == 0)
            {
                List<TblChart> listOfChart = new();
                for (int i = 0; i < 52; i++)
                {
                    listOfChart.Add(new() { ShopId = SelectedShopFromFirstWindow.ShopId, AddWho = WindowsIdentity.GetCurrent().Name.ToString(), AddWhen = DateTime.Now, NumberOfWeek = i + 1, Year = DateTime.Now.Year });
                    Context.TblCharts.Add(listOfChart[i]);
                    Context.SaveChanges();
                }
                objectToVal = NewTab.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Year == DateTime.Now.Year).ToList();
                ArrayPercentage.Clear();
                ArrayMTTR.Clear();
                ArrayMTBF.Clear();
                ArrayCout.Clear();
                ArrayPercentage.Add(0d);
                ArrayMTTR.Add(0d);
                ArrayMTBF.Add(0d);
                ArrayCout.Add(0);

                for (int i = 0; i < 52; i++)
                {
                    ArrayPercentage.Add(Convert.ToDouble(objectToVal[i].PercentOfBreakdown));
                    ArrayMTTR.Add(Convert.ToDouble(objectToVal[i].Mttr));
                    ArrayMTBF.Add(Convert.ToDouble(objectToVal[i].Mtbf));
                    ArrayCout.Add(Convert.ToInt32(objectToVal[i].CoutOfBreakdown));
                }
            }
            else
            {
                ArrayPercentage.Clear();
                ArrayMTTR.Clear();
                ArrayMTBF.Clear();
                ArrayCout.Clear();
                ArrayPercentage.Add(0d);
                ArrayMTTR.Add(0d);
                ArrayMTBF.Add(0d);
                ArrayCout.Add(0);

                for (int i = 0; i < 52; i++)
                {
                    ArrayPercentage.Add(Convert.ToDouble(objectToVal[i].PercentOfBreakdown));
                    ArrayMTTR.Add(Convert.ToDouble(objectToVal[i].Mttr));
                    ArrayMTBF.Add(Convert.ToDouble(objectToVal[i].Mtbf));
                    ArrayCout.Add(Convert.ToInt32(objectToVal[i].CoutOfBreakdown));
                }
            }


        }
        private void LiveCharts()
        {
            DataToCharts();
            Percent_of_breakdown = new();
            Percent_of_breakdown.Clear();
            Percent_of_breakdown.Add(new ColumnSeries
            {
                Values = ArrayPercentage,
                DataLabels = true,
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
            MTTR = new();
            MTTR.Clear();
            MTTR.Add(new ColumnSeries
            {
                Values = ArrayMTTR,
                DataLabels = true,
                Title = "MTTR [Min]",
                StrokeThickness = 4,
            });
            MTTR.Add(
            new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(0)
                    },
                DataLabels = false,
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Title = "Cel MTTR = " + ChartMTTR + "[Min]",
                StrokeThickness = 0,
            });
            MTBF = new();
            MTBF.Clear();
            MTBF.Add(new ColumnSeries
            {
                Values = ArrayCout,
                ScalesYAt = 0,
                DataLabels = true,
                Stroke = Brushes.Orange,
                Fill = Brushes.Orange,
                Title = "Ilość awarii [#]",
                StrokeThickness = 4,
            });
            MTBF.Add(new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(0)
                    },
                ScalesYAt = 0,
                DataLabels = false,
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
                Title = "Cel ilości awarii = " + ChartCountOfBreakDowns + " [#]",
                StrokeThickness = 0,
            });
            MTBF.Add(new LineSeries
            {
                Values = ArrayMTBF,
                ScalesYAt = 1,
                DataLabels = true,
                LineSmoothness = 0,
                PointGeometrySize = 0,
                PointGeometry = DefaultGeometries.Square,
                PointForeground = Brushes.SpringGreen,
                Stroke = Brushes.SpringGreen,
                Fill = Brushes.Transparent,
                Title = "MTBF [H]",
                StrokeThickness = 4,
            });
            MTBF.Add(new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(0)
                    },
                ScalesYAt = 1,
                DataLabels = false,
                PointGeometry = DefaultGeometries.Square,

                Stroke = Brushes.Green,
                Fill = Brushes.Green,
                Title = "Cel MTBF = " + ChartMTBF + " [H]",
                StrokeThickness = 0,
            });
        }
        private async void OpenPopout()
        {
            UpdateSelectData();
            IsOpenPopout = true;
        }
        private async void OpenPopoutChangeYear()
        {
            UpdateSelectData();
            IsOpenPopoutChangeYear = true;
        }

        private async void UpdateSelectData()
        {
            var d = DateTime.Now;
            CultureInfo cul = CultureInfo.CurrentCulture;
            int weekNum = cul.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            SelectedWeek = weekNum;
            var temp = Context.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).ToList();
            foreach (var tempItem in temp)
            {
                if (!Years.Contains(tempItem.Year.ToString()))
                {
                    Years.Add(tempItem.Year.ToString());
                }
            }
            RemoveError();
            ClearAllValues();
        }
        private async void ModWeeks()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Wskaźnik ustawiony.");
                }
            }
        }
        public async void ClearUser()
        {

            DimTabContext NewTab = new();
            List<TblChart> objectToVal = new();

            objectToVal = NewTab.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Year == Int32.Parse(YearPick)).ToList();
            if (objectToVal.Count > 0)
            {
                ArrayPercentage.Clear();
                ArrayMTTR.Clear();
                ArrayMTBF.Clear();
                ArrayCout.Clear();
                ArrayPercentage.Add(0d);
                ArrayMTTR.Add(0d);
                ArrayMTBF.Add(0d);
                ArrayCout.Add(0);

                for (int i = 0; i < 52; i++)
                {
                    ArrayPercentage.Add(Convert.ToDouble(objectToVal[i].PercentOfBreakdown));
                    ArrayMTTR.Add(Convert.ToDouble(objectToVal[i].Mttr));
                    ArrayMTBF.Add(Convert.ToDouble(objectToVal[i].Mtbf));
                    ArrayCout.Add(Convert.ToInt32(objectToVal[i].CoutOfBreakdown));
                }
                if (ManagmentShopViewModel.PermisionMaster == Visibility.Visible)
                {
                    if (DateTime.Now.Year == Int32.Parse(YearPick))
                    {
                        OnlyThisYear = Visibility.Visible;
                        ValueToMove = 260;
                    }
                    else
                    {
                        OnlyThisYear = Visibility.Collapsed;
                        ValueToMove = 35;
                    }
                }
                else
                {
                    OnlyThisYear = Visibility.Collapsed;
                    ValueToMove = 35;
                }
                string msg = String.Format("Wyświetlane są wskaźniki z {0} roku.", YearPick);
                BoundMessageQueue.Enqueue(msg);

            }
            else
            {
                BoundMessageQueue.Enqueue("Brak wykresów z tamtego roku.");
            }
            IsOpenPopoutChangeYear = false;
        }
        private bool CanSubmit()
        {
            if (!string.IsNullOrWhiteSpace(ChartMTTRtxt) && !string.IsNullOrWhiteSpace(ChartMTBFtxt) && !string.IsNullOrWhiteSpace(ChartPercentageOfBreakDownstxt) && !string.IsNullOrWhiteSpace(ChartCountOfBreakDownstxt))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private bool CanSelect()
        {
            if (YearPick != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<bool> ValidateLogin()
        {

            TblChart exist = new();
            exist = Context.TblCharts.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.NumberOfWeek == SelectedWeek).Where(d => d.Year == DateTime.Now.Year).First();
            TblChart ChartNew = new()
            {
                Mttr = Convert.ToDouble(ChartMTTRtxt),
                Mtbf = Convert.ToDouble(ChartMTBFtxt),
                Year = DateTime.Now.Year,
                PercentOfBreakdown = Convert.ToDouble(ChartPercentageOfBreakDownstxt),
                CoutOfBreakdown = Convert.ToInt32(ChartCountOfBreakDownstxt),
                ShopId = SelectedShopFromFirstWindow.ShopId,
                NumberOfWeek = SelectedWeek,
                AddWho = exist.AddWho,
                AddWhen = exist.AddWhen,
                ModWhen = DateTime.Now,
                ModWho = LoggedPerson.Name + " " + LoggedPerson.Surname,
                ChartId = exist.ChartId
            };
            Context.Entry(exist).CurrentValues.SetValues(ChartNew);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            IsOpenPopout = false;
            DataToCharts();
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
            switch (changedPropertyName)
            {
                case nameof(ChartMTTRtxt):
                    if (string.IsNullOrWhiteSpace(ChartMTTRtxt))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTTRtxt)] = new List<object> { "MTTR jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTRtxt)));
                    }
                    else if (!Regex.IsMatch(ChartMTTRtxt, "^[0-9]{1,6}([,][0-9]{1,2})?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTTRtxt)] = new List<object> { "Zły format. Przykładowy format 000,00" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTRtxt)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartMTTRtxt)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTRtxt)));
                    }
                    break;
                case nameof(ChartMTBFtxt):
                    if (string.IsNullOrWhiteSpace(ChartMTBFtxt))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTBFtxt)] = new List<object> { "MTBF jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTTRtxt)));
                    }
                    else if (!Regex.IsMatch(ChartMTBFtxt, "^[0-9]{1,6}([,][0-9]{1,2})?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartMTBFtxt)] = new List<object> { "Zły format. Przykładowy format 000,00" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTBFtxt)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartMTBFtxt)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartMTBFtxt)));
                    }
                    break;
                case nameof(ChartPercentageOfBreakDownstxt):
                    if (string.IsNullOrWhiteSpace(ChartPercentageOfBreakDownstxt))
                    {
                        _ValidationErrorsByProperty[nameof(ChartPercentageOfBreakDownstxt)] = new List<object> { "Procent awaryjności jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartPercentageOfBreakDownstxt)));
                    }
                    else if (!Regex.IsMatch(ChartPercentageOfBreakDownstxt, "^[0-9]{1,6}([,][0-9]{1,2})?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartPercentageOfBreakDownstxt)] = new List<object> { "Zły format. Przykładowy format 000,00" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartPercentageOfBreakDownstxt)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartPercentageOfBreakDownstxt)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartPercentageOfBreakDownstxt)));
                    }
                    break;
                case nameof(ChartCountOfBreakDownstxt):
                    if (string.IsNullOrWhiteSpace(ChartCountOfBreakDownstxt))
                    {
                        _ValidationErrorsByProperty[nameof(ChartCountOfBreakDownstxt)] = new List<object> { "Ilość awarii jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartCountOfBreakDownstxt)));
                    }
                    else if (!Regex.IsMatch(ChartCountOfBreakDownstxt, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(ChartCountOfBreakDownstxt)] = new List<object> { "Zły format. Przykładowy format 000." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartCountOfBreakDownstxt)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ChartCountOfBreakDownstxt)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ChartCountOfBreakDownstxt)));
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
            ClearCommand.NotifyCanExecuteChanged();
        }
    }
}

