using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Holidays
{
    public class PersonHoliday : InputsViewModel
    {
        public int _personID;
        public int PersonID
        {
            get => _personID;
            set => SetProperty(ref _personID, value);
        }
        public string _personName;
        public string PersonName
        {
            get => _personName;
            set => SetProperty(ref _personName, value);
        }
        private ObservableCollection<string> _columnsDaysOfPerson;
        public ObservableCollection<string> ColumnsDaysOfPerson
        {
            get => _columnsDaysOfPerson;
            set => SetProperty(ref _columnsDaysOfPerson, value);
        }
        private int _selectedShop;
        public int SelectedShop
        {
            get => _selectedShop;
            set => SetProperty(ref _selectedShop, value);
        }
        private int _selectedMonth;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set => SetProperty(ref _selectedMonth, value);
        }
        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set => SetProperty(ref _selectedYear, value);
        }
        private int _daysInMonth;
        public int DaysInMonth
        {
            get => _daysInMonth;
            set => SetProperty(ref _daysInMonth, value);
        }
        public PersonHoliday(DimTabContext context, int selectedshop, int selectedyear, int selectedmonth, int personid)
        {
            Context = context;
            SelectedShop = selectedshop;
            PersonID = personid;
            SelectedMonth = selectedmonth;
            SelectedYear = selectedyear;
            ColumnsDaysOfPerson = new();
            DaysInMonth = DateTime.DaysInMonth(selectedyear, selectedmonth);
            List<DateTime> ListOfSanta = new();
            var con = Context.TblHolidays.Where(d => d.ItsFreeDay == true).ToList();
            foreach (var dates in con)
            {
                for (var dt = dates.DateFrom; dt <= dates.DateTo; dt = dt.AddDays(1))
                {
                    ListOfSanta.Add(dt);
                }
            }
            for (int i = 0; i < DaysInMonth; i++)
            {
                var culture = new System.Globalization.CultureInfo("pl-PL");
                DateTime SelectedDateNewDay = DateTime.Parse(selectedyear.ToString() + "-" + selectedmonth.ToString() + "-01");

                string firstWord = SelectedDateNewDay.AddDays(i).ToString().Substring(0, 2) + " ";
                string secondWord = culture.DateTimeFormat.GetDayName(SelectedDateNewDay.AddDays(i).DayOfWeek).ToString().Substring(0, 2) + ".";
                string createdText = firstWord + char.ToUpper(secondWord[0]) + secondWord.Substring(1);
                if (ListOfSanta.Contains(SelectedDateNewDay.AddDays(i)) && (SelectedDateNewDay.AddDays(i).DayOfWeek != DayOfWeek.Sunday) && (SelectedDateNewDay.AddDays(i).DayOfWeek != DayOfWeek.Saturday))
                {
                    ColumnsDaysOfPerson.Add("Święto");
                }
                else
                {
                    ColumnsDaysOfPerson.Add(createdText);
                }
            }
            var holidayList = Context.TblUnavailables.Where(d => d.PersonId == PersonID).ToList();
            foreach (var holiday in holidayList)
            {
                if ((holiday.AbsentFrom.Month == selectedmonth) && (holiday.AbsentTo.Month == selectedmonth) && (holiday.AbsentFrom.Year == SelectedYear))
                {
                    for (int i = holiday.AbsentFrom.Day; i <= holiday.AbsentTo.Day; i++)
                    {
                        if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("So")))
                        {
                            if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                            {
                                if (holiday.Accepted == true)
                                {
                                    ColumnsDaysOfPerson[i - 1] = holiday.Reason;
                                }
                                else
                                {
                                    ColumnsDaysOfPerson[i - 1] = "Wniosek";
                                }
                            }
                        }
                    }
                }
                if ((holiday.AbsentFrom.Month == selectedmonth) && (holiday.AbsentTo.Month > selectedmonth) && (holiday.AbsentFrom.Year == SelectedYear))
                {
                    for (int i = holiday.AbsentFrom.Day; i <= DaysInMonth; i++)
                    {
                        if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("So")))
                        {
                            if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                            {
                                if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                                {
                                    if (holiday.Accepted == true)
                                    {
                                        ColumnsDaysOfPerson[i - 1] = holiday.Reason;
                                    }
                                    else
                                    {
                                        ColumnsDaysOfPerson[i - 1] = "Wniosek";
                                    }
                                }
                            }
                        }
                    }
                }
                if ((holiday.AbsentFrom.Month < selectedmonth) && ((holiday.AbsentTo.Month == selectedmonth)) && (holiday.AbsentFrom.Year == SelectedYear))
                {
                    for (int i = 1; i <= holiday.AbsentTo.Day; i++)
                    {
                        if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("So")))
                        {
                            if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                            {
                                if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                                {
                                    if (holiday.Accepted == true)
                                    {
                                        ColumnsDaysOfPerson[i - 1] = holiday.Reason;
                                    }
                                    else
                                    {
                                        ColumnsDaysOfPerson[i - 1] = "Wniosek";
                                    }
                                }
                            }
                        }
                    }
                }
                if ((holiday.AbsentFrom.Month < selectedmonth) && ((holiday.AbsentTo.Month > selectedmonth)) && (holiday.AbsentFrom.Year == SelectedYear))
                {
                    for (int i = 1; i <= DaysInMonth; i++)
                    {
                        if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("So")))
                        {
                            if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                            {
                                if (!(ColumnsDaysOfPerson[i - 1].ToString().Contains("Ni")))
                                {
                                    if (holiday.Accepted == true)
                                    {
                                        ColumnsDaysOfPerson[i - 1] = holiday.Reason;
                                    }
                                    else
                                    {
                                        ColumnsDaysOfPerson[i - 1] = "Wniosek";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            PersonName = Context.TblPersons.Where(d => d.ShopId == SelectedShop).Where(d => d.PersonId == PersonID).First().Surname + " " + Context.TblPersons.Where(d => d.ShopId == SelectedShop).Where(d => d.PersonId == PersonID).First().Name;
        }
    }
}
