using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel
{
    internal class WorkInWeekendViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public static string TitleToMenu { get; } = "Praca w weekend";
        public string Title { get; } = "Praca w weekend";

        private int _actuallyWeekNumber;
        public int ActuallyWeekNumber
        {
            get => _actuallyWeekNumber;
            set => SetProperty(ref _actuallyWeekNumber, value);
        }
        private int _actuallyYearNumber;
        public int ActuallyYearNumber
        {
            get => _actuallyYearNumber;
            set => SetProperty(ref _actuallyYearNumber, value);
        }
        private string _shiftSelect;
        public string ShiftSelect
        {
            get => _shiftSelect;
            set => SetProperty(ref _shiftSelect, value);
        }
        private ObservableCollection<TblShiftInWeekend> _readTable;
        public ObservableCollection<TblShiftInWeekend> ReadTable
        {
            get => _readTable;
            set => SetProperty(ref _readTable, value);
        }
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
        public RelayCommand DeleteCommand { get; }
        public WorkInWeekendViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            DeleteCommand = new RelayCommand(DeleteWorkers);
            ReadTable = new();
            DataAssigment(managmentshopviewmodel);
            GregorianCalendar cal = new GregorianCalendar();
            ActuallyWeekNumber = cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            ActuallyYearNumber = DateTime.Now.Year;
            if (CheckIfExist(ActuallyWeekNumber, ActuallyYearNumber))
            {
                CreateWeek(ActuallyWeekNumber, ActuallyYearNumber);
                ReadTable = ReadWeek(ActuallyWeekNumber, ActuallyYearNumber);
            }
            else
            {
                ReadTable = ReadWeek(ActuallyWeekNumber, ActuallyYearNumber);
            }
        }
        private void DeleteWorkers()
        {
            ObservableCollection<TblShiftInWeekend> newValues = new();
            newValues = ReadTable;
            int index = 0;
            foreach (var item in newValues)
            {
                item.Count = 0;
                Context.Entry(ReadTable[index]).CurrentValues.SetValues(newValues[index]);
                index++;
            }
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
        }
        private void UpdateData()
        {
            DimTabContext newData = new();
            Context = newData;
            ReadTable = ReadWeek(ActuallyWeekNumber, ActuallyYearNumber);
        }
        private void PlusPerson(string value)
        {
            TblShiftInWeekend newValueTotable = new();
            newValueTotable = ReadTable[Int32.Parse(value)];
            newValueTotable.Count += 1;
            Context.Entry(ReadTable[Int32.Parse(value)]).CurrentValues.SetValues(newValueTotable);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            UpdateData();
        }
        private void MinusPerson(string value)
        {
            TblShiftInWeekend newValueTotable = new();
            newValueTotable = ReadTable[Int32.Parse(value)];
            if (newValueTotable.Count > 0)
                newValueTotable.Count -= 1;
            Context.Entry(ReadTable[Int32.Parse(value)]).CurrentValues.SetValues(newValueTotable);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            UpdateData();
        }

        private bool CheckIfExist(int weekNumber, int yearNumber)
        {
            List<TblShiftInWeekend> objectToVal = new();
            objectToVal = Context.TblShiftInWeekends.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.YearNumber == yearNumber)
                .Where(d => d.WeekNumber == weekNumber).ToList();
            if (objectToVal.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void CreateWeek(int weekNumber, int yearNumber)
        {
            List<TblShiftInWeekend> newVal = new();
            int index = 0;
            for (int j = 6; j < 8; j++)
            {
                for (int i = 1; i < 5; i++)
                {
                    newVal.Add(new TblShiftInWeekend() { ShopId = SelectedShopFromFirstWindow.ShopId, WeekNumber = weekNumber, YearNumber = yearNumber, DayNumber = j, Shift = i, Count = 0 });
                    Context.TblShiftInWeekends.Add(newVal[index]);
                    index++;
                }
            }
            Context.SaveChanges();

        }
        private ObservableCollection<TblShiftInWeekend> ReadWeek(int weekNumber, int yearNumber)
        {
            var GetDataFromContext = new ObservableCollection<TblShiftInWeekend>();
            var list = new List<TblShiftInWeekend> { /* ... */ };
            list = Context.TblShiftInWeekends.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.WeekNumber == weekNumber).
                Where(d => d.YearNumber == yearNumber).OrderBy(d => d.IdshopInWeekend).ToList();
            foreach (var item in list)
                GetDataFromContext.Add(item);
            return GetDataFromContext;
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            Validate(e.PropertyName);
        }
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void Validate(string changedPropertyName)
        {
        }
    }
}
