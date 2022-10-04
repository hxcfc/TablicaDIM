using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel.Home
{
    internal class AddPersonToWeekendViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Dodawanie akcji";
        private ObservableCollection<string> _reasons;
        public ObservableCollection<string> Reasons
        {
            get => _reasons;
            set => SetProperty(ref _reasons, value);
        }
        private string _reasonPick;
        public string ReasonPick
        {
            get => _reasonPick;
            set => SetProperty(ref _reasonPick, value);
        }

        private ObservableCollection<string> _persons;
        public ObservableCollection<string> Persons
        {
            get => _persons;
            set => SetProperty(ref _persons, value);
        }
        private string? _personPick;
        public string? PersonPick
        {
            get => _personPick;
            set => SetProperty(ref _personPick, value);
        }
        private int _week;
        public int Week
        {
            get => _week;
            set => SetProperty(ref _week, value);
        }
        private string _textOfReason;

        public string TextOfReason
        {
            get => _textOfReason;
            set => SetProperty(ref _textOfReason, value);
        }
        private string _parameter;

        public string Parameter
        {
            get => _parameter;
            set => SetProperty(ref _parameter, value);
        }
        private Visibility _addVis;
        public Visibility AddVis
        {
            get => _addVis;
            set => SetProperty(ref _addVis, value);
        }
        private Visibility _minusVis;
        public Visibility MinusVis
        {
            get => _minusVis;
            set => SetProperty(ref _minusVis, value);
        }
        public RelayCommand CloseCommand { get; }
        public RelayCommand CloseAddCommand { get; }
        public RelayCommand CloseMinusCommand { get; }

        public AddPersonToWeekendViewModel(ManagmentShopViewModel managmentshopviewmodel, string parameter, bool add)
        {
            CloseCommand = new RelayCommand(Close);
            CloseAddCommand = new RelayCommand(CloseAdd, CanAddSubmit);
            CloseMinusCommand = new RelayCommand(CloseMinus, CanMinusSubmit);
            Reasons = new();
            Persons = new();
            DataAssigment(managmentshopviewmodel);
            Parameter = parameter;
            GregorianCalendar cal = new GregorianCalendar();
            Week = cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            if (add == true)
            {
                AddVis = Visibility.Visible;
                MinusVis = Visibility.Collapsed;
                CreateReasons();
                GetPersons();
            }
            else
            {
                AddVis = Visibility.Collapsed;
                MinusVis = Visibility.Visible;
                GetPersonsToDelete();
            }

        }
        private bool CanAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PersonPick) && (!string.IsNullOrWhiteSpace(ReasonPick) || !string.IsNullOrEmpty(TextOfReason)))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private bool CanMinusSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PersonPick))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async void Close()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }
        private async void CloseAdd()
        {
            if (!HasErrors)
            {
                bool result = await AddPerson();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Pracownik dodany.");
                }
            }
        }
        private async Task<bool> AddPerson()
        {
            TblPersonInWeekend newValueTotable = new();
            newValueTotable.WeekNumber = Week;
            newValueTotable.YearNumber = DateTime.Now.Year;
            newValueTotable.ShopId = SelectedShopFromFirstWindow.ShopId;
            newValueTotable.Reason = TextOfReason;
            newValueTotable.ShiftDayIndex = Int32.Parse(Parameter);
            var idPerson = Context.TblPersons.Where(d => d.Surname + " " + d.Name == PersonPick).Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First();
            newValueTotable.PersonId = idPerson.PersonId;
            
            Context.TblPersonInWeekends.Add(newValueTotable);
            Context.SaveChanges();
            DialogHost.CloseDialogCommand.Execute(true, null);
            return true;
        }
        private async void CloseMinus()
        {
            if (!HasErrors)
            {
                bool result = await MinusPerson();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Pracownik usuniety.");
                }
            }
        }
        private async Task<bool> MinusPerson()
        {
            var idPerson = Context.TblPersons.Where(d => d.Surname + " " + d.Name == PersonPick).Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First();
            int PersonId = idPerson.PersonId;

            
            Context.Remove(Context.TblPersonInWeekends.Where(d=>d.ShiftDayIndex== Int32.Parse(Parameter)).Where(d => d.WeekNumber == Week).Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PersonId == PersonId).Where(d => d.YearNumber == DateTime.Now.Year).First());
            Context.SaveChanges();
            DialogHost.CloseDialogCommand.Execute(true, null);
            return true;
        }
        private void GetPersons()
        {
            Persons.Clear();
            if (LoggedPerson.PermisionId == 4)
            {
                var con = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(b => b.PersonId == LoggedPerson.PersonId).ToList();
                foreach (var person in con)
                {
                    Persons.Add(person.Surname + " " + person.Name);
                }
            }
            else
            {
                if (Int32.Parse(Parameter) < 4)
                {
                    var con = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).OrderBy(d => d.Surname).ToList();
                    var checkWhichCant = Context.TblPersonInWeekends.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.WeekNumber == Week).
                        Where(d => d.YearNumber == DateTime.Now.Year).Where(d => d.ShiftDayIndex < 4 ).ToList();
                    foreach (var person in con)
                    {
                        bool exist = false;
                        foreach (var person2 in checkWhichCant)
                        {
                            if (person.PersonId == person2.PersonId)
                            {
                                exist = true;
                            }
                        }
                        if (exist == false)
                        {
                            Persons.Add(person.Surname + " " + person.Name);
                        }
                    }
                }else {
                    var con = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).OrderBy(d => d.Surname).ToList();
                    var checkWhichCant = Context.TblPersonInWeekends.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.WeekNumber == Week).
                        Where(d => d.YearNumber == DateTime.Now.Year).Where(d => d.ShiftDayIndex > 4).ToList();
                    foreach (var person in con)
                    {
                        bool exist = false;
                        foreach (var person2 in checkWhichCant)
                        {
                            if (person.PersonId == person2.PersonId)
                            {
                                exist = true;
                            }
                        }
                        if (exist == false)
                        {
                            Persons.Add(person.Surname + " " + person.Name);
                        }
                    }
                }
               
            }
        }
        private void GetPersonsToDelete()
        {
           
            Persons.Clear();
            if (LoggedPerson.PermisionId == 4)
            {
                var con = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(b => b.PersonId == LoggedPerson.PersonId).ToList();
                foreach (var person in con)
                {
                    Persons.Add(person.Surname + " " + person.Name);
                }
            }
            else
            {
                var con = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).OrderBy(d => d.Surname).ToList();
                var checkWhichCant = Context.TblPersonInWeekends.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.WeekNumber == Week).
                    Where(d => d.YearNumber == DateTime.Now.Year).Where(d => d.ShiftDayIndex == Int32.Parse(Parameter)).ToList();
                foreach (var person in con)
                {
                    bool exist = false;
                    foreach (var person2 in checkWhichCant)
                    {
                        if (person.PersonId == person2.PersonId)
                        {
                            exist = true;
                        }
                    }
                    if (exist == true)
                    {
                        Persons.Add(person.Surname + " " + person.Name);
                    }
                }
            }
        }
        private void CreateReasons()
        {
            Reasons.Clear();
            Reasons.Add("Prace zlecone");
            Reasons.Add("Wsparcie produkcji");
            Reasons.Add("Rozruch");
            Reasons.Add("Inny powód");
            Reasons.Add("Wpisz powód");
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
                case nameof(TextOfReason):
                    if (string.IsNullOrWhiteSpace(TextOfReason))
                    {
                        _ValidationErrorsByProperty[nameof(TextOfReason)] = new List<object> { "Podanie powodu jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TextOfReason)));
                    }
                    else if (!Regex.IsMatch(TextOfReason, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(TextOfReason)] = new List<object> { "Powód musi zaczynać się z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TextOfReason)));
                    }
                    else if (TextOfReason.Length <= 3)
                    {
                        _ValidationErrorsByProperty[nameof(TextOfReason)] = new List<object> { "Powód musi mieć więcej niż 3 znaków." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TextOfReason)));
                    }
                    else if (!Regex.IsMatch(TextOfReason, @"^[A-ZŻŹĆĄŚĘÓŁŃa-zżźćąśęółń ?]+$"))
                    {
                        _ValidationErrorsByProperty[nameof(TextOfReason)] = new List<object> { "Powód posiada złe znaki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TextOfReason)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(TextOfReason)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TextOfReason)));
                    }
                    break;
            }
            CloseAddCommand.NotifyCanExecuteChanged();
            CloseMinusCommand.NotifyCanExecuteChanged();
        }
    }

}
