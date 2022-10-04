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
    internal class AddActionViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public string Title { get; set; } = "Dodawanie akcji";
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
        private string? _startActionDate;
        public string? StartActionDate
        {
            get => _startActionDate;
            set => SetProperty(ref _startActionDate, value);
        }
        private string? _planActionDate;
        public string? PlanActionDate
        {
            get => _planActionDate;
            set => SetProperty(ref _planActionDate, value);
        }
        private ObservableCollection<string> _places;
        public ObservableCollection<string> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }
        private string? _placePick;
        public string? PlacePick
        {
            get => _placePick;
            set => SetProperty(ref _placePick, value);
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
        private ObservableCollection<string> _problems;
        public ObservableCollection<string> Problems
        {
            get => _problems;
            set => SetProperty(ref _problems, value);
        }
        private string? _problemPick;
        public string? ProblemPick
        {
            get => _problemPick;
            set
            {
                if (SetProperty(ref _problemPick, value))
                {
                    if (value == "EWO")
                    {
                        EwoNumberVis = Visibility.Visible;
                    }
                    else
                    {
                        NumberEvo = string.Empty;
                        _ValidationErrorsByProperty.Remove(nameof(NumberEvo));
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NumberEvo)));
                        EwoNumberVis = Visibility.Collapsed;
                    }
                }
            }
        }
        private string? _numberEvo;
        public string? NumberEvo
        {
            get => _numberEvo;
            set => SetProperty(ref _numberEvo, value);

        }
        private string? _causeText;
        public string? CauseText
        {
            get => _causeText;
            set => SetProperty(ref _causeText, value);

        }
        private string? _solveText;
        public string? SolveText
        {
            get => _solveText;
            set => SetProperty(ref _solveText, value);

        }
        private int _valuePick;
        public int ValuePick
        {
            get => _valuePick;
            set => SetProperty(ref _valuePick, value);

        }
        private string? _infoText;
        public string? InfoText
        {
            get => _infoText;
            set => SetProperty(ref _infoText, value);

        }
        private Visibility _ewoNumberVis;
        public Visibility EwoNumberVis
        {
            get => _ewoNumberVis;
            set => SetProperty(ref _ewoNumberVis, value);

        }
        public RelayCommand AddCommand { get; }

        public AddActionViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            AddCommand = new RelayCommand(AddAction, CanAddSubmit);
            DataAssigment(managmentshopviewmodel);
            EwoNumberVis = Visibility.Collapsed;
            Places = new();
            Persons = new();
            Problems = new();
            ActualyDate = DateTime.Now;
            MinDate = new DateTime(ActualyDate.Year - 1, 1, 1);
            MaxDate = new DateTime(ActualyDate.Year + 1, 12, 31);
            GetPlaces();
            GetPersons();
            GetProblems();
            ClearData();
        }
        private bool CanAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(StartActionDate) && !string.IsNullOrWhiteSpace(PlanActionDate) && !string.IsNullOrWhiteSpace(PlacePick) && !string.IsNullOrWhiteSpace(PersonPick) && !string.IsNullOrWhiteSpace(ProblemPick)
                && (((!string.IsNullOrWhiteSpace(NumberEvo)) && (ProblemPick == "EWO")) || ((string.IsNullOrWhiteSpace(NumberEvo)) && (ProblemPick != "EWO"))) && !string.IsNullOrWhiteSpace(CauseText) && !string.IsNullOrWhiteSpace(SolveText))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async void AddAction()
        {
            if (!HasErrors)
            {
                bool result = await ValidateLogin();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Akcja dodana.");
                }
            }
        }
        private async Task<bool> ValidateLogin()
        {
            string conProb;
            if (ProblemPick == "EWO")
            {
                conProb = ProblemPick + NumberEvo;
            }
            else
            {
                conProb = ProblemPick;
            }

            TblDataGrid var = new()
            {
                AddWhen = ActualyDate,
                AddWho = LoggedPerson.Surname + " " + LoggedPerson.Name,
                DateCreateAction = DateTime.Parse(StartActionDate),
                DatePlaningAction = DateTime.Parse(PlanActionDate),
                PlaceId = Context.TblPlaces.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.PlaceName == PlacePick).First().PlaceId,
                PersonId = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => (d.Surname + " " + d.Name) == PersonPick).First().PersonId,
                Problem = conProb,
                Cause = CauseText,
                Solve = SolveText,
                Step = ValuePick,
                Info = InfoText,
                ShopId = SelectedShopFromFirstWindow.ShopId,
                EndAction = false
            };
            Context.TblDataGrids.Add(var);
            Context.SaveChanges();
            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
            DialogHost.CloseDialogCommand.Execute(true, null);
            return true;
        }
        private void GetPlaces()
        {
            Places.Clear();
            var con = Context.TblPlaces.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).OrderBy(d => d.PlaceName).ToList();
            foreach (var place in con)
            {
                Places.Add(place.PlaceName);
            }
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
                var con = Context.TblPersons.Where(b => b.ShopId == SelectedShopFromFirstWindow.ShopId).OrderBy(d => d.Surname).ToList();
                foreach (var person in con)
                {
                    Persons.Add(person.Surname + " " + person.Name);
                }
            }
        }
        private void GetProblems()
        {
            Problems.Clear();
            Problems.Add("EWO");
            Problems.Add("Czynność planowana");
            Problems.Add("Pozostałe");
        }

        public void ClearData()
        {
            StartActionDate = string.Empty;
            PlanActionDate = string.Empty;
            PlacePick = string.Empty;
            PersonPick = string.Empty;
            ProblemPick = string.Empty;
            NumberEvo = string.Empty;
            CauseText = string.Empty;
            SolveText = string.Empty;
            ValuePick = 0;
            InfoText = string.Empty;
            _ValidationErrorsByProperty.Remove(nameof(StartActionDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
            _ValidationErrorsByProperty.Remove(nameof(PlanActionDate));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
            _ValidationErrorsByProperty.Remove(nameof(PlacePick));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlacePick)));
            _ValidationErrorsByProperty.Remove(nameof(PersonPick));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PersonPick)));
            _ValidationErrorsByProperty.Remove(nameof(ProblemPick));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ProblemPick)));
            _ValidationErrorsByProperty.Remove(nameof(NumberEvo));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NumberEvo)));
            _ValidationErrorsByProperty.Remove(nameof(CauseText));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
            _ValidationErrorsByProperty.Remove(nameof(SolveText));
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(SolveText)));
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
                case nameof(StartActionDate):
                    if (string.IsNullOrWhiteSpace(StartActionDate))
                    {
                        _ValidationErrorsByProperty[nameof(StartActionDate)] = new List<object> { "Data rozpoczęcia jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
                    }
                    else if (!((DateTime.TryParse(StartActionDate, culture, styles, out dateResult))))
                    {
                        _ValidationErrorsByProperty[nameof(StartActionDate)] = new List<object> { "Data rozpoczęcia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(StartActionDate)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
                    }
                    break;
                case nameof(PlanActionDate):
                    if (string.IsNullOrWhiteSpace(PlanActionDate))
                    {
                        _ValidationErrorsByProperty[nameof(PlanActionDate)] = new List<object> { "Data planowanego zakończenia jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
                    }
                    else if (!((DateTime.TryParse(PlanActionDate, culture, styles, out dateResult1))))
                    {
                        _ValidationErrorsByProperty[nameof(PlanActionDate)] = new List<object> { "Data planowanego zakończenia powinna mieć foramt dd.MM.yyyy." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
                    }

                    else if (_ValidationErrorsByProperty.Remove(nameof(PlanActionDate)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
                    }
                    break;
                case nameof(PlacePick):
                    if (string.IsNullOrWhiteSpace(PlacePick))
                    {
                        _ValidationErrorsByProperty[nameof(PlacePick)] = new List<object> { "Stanowisko jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlacePick)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(PlacePick)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlacePick)));
                    }
                    break;
                case nameof(PersonPick):
                    if (string.IsNullOrWhiteSpace(PersonPick))
                    {
                        _ValidationErrorsByProperty[nameof(PersonPick)] = new List<object> { "Osoba odpowiedzialna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PersonPick)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(PersonPick)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PersonPick)));
                    }
                    break;
                case nameof(ProblemPick):
                    if (string.IsNullOrWhiteSpace(ProblemPick))
                    {
                        _ValidationErrorsByProperty[nameof(ProblemPick)] = new List<object> { "Problem jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ProblemPick)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(ProblemPick)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(ProblemPick)));
                    }
                    break;
                case nameof(NumberEvo):
                    if (string.IsNullOrWhiteSpace(NumberEvo) && (ProblemPick == "EWO"))
                    {
                        _ValidationErrorsByProperty[nameof(NumberEvo)] = new List<object> { "Numer EWO jest wymagany." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NumberEvo)));
                    }
                    else if (!Regex.IsMatch(NumberEvo, "^[0-9]{1,20}?$") && (ProblemPick == "EWO"))
                    {
                        _ValidationErrorsByProperty[nameof(NumberEvo)] = new List<object> { "Zły format. Przykładowy format 000." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NumberEvo)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(NumberEvo)) && (ProblemPick == "EWO"))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(NumberEvo)));
                    }
                    break;
                case nameof(CauseText):
                    if (string.IsNullOrWhiteSpace(CauseText))
                    {
                        _ValidationErrorsByProperty[nameof(CauseText)] = new List<object> { "Przyczyna jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
                    }

                    else if (!Regex.IsMatch(CauseText, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(CauseText)] = new List<object> { "Przyczyna musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
                    }
                    else if (CauseText.Length <= 5)
                    {
                        _ValidationErrorsByProperty[nameof(CauseText)] = new List<object> { "Przyczyna musi mieć więcej niż 5 znaków." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
                    }
                    else if (CauseText.Last().ToString() != ".")
                    {
                        _ValidationErrorsByProperty[nameof(CauseText)] = new List<object> { "Przyczyna musi się kończyć znakiem kropki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(CauseText)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
                    }
                    break;
                case nameof(SolveText):
                    if (string.IsNullOrWhiteSpace(SolveText))
                    {
                        _ValidationErrorsByProperty[nameof(SolveText)] = new List<object> { "Rozwiązanie jest wymagane." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(CauseText)));
                    }

                    else if (!Regex.IsMatch(SolveText, "^[A-ZŻŹĆĄŚĘÓŁŃ]"))
                    {
                        _ValidationErrorsByProperty[nameof(SolveText)] = new List<object> { "Rozwiązanie musi się zaczynać z wielkiej litery." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(SolveText)));
                    }
                    else if (SolveText.Length <= 5)
                    {
                        _ValidationErrorsByProperty[nameof(SolveText)] = new List<object> { "Rozwiązanie musi mieć więcej niż 5 znaków." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(SolveText)));
                    }
                    else if (SolveText.Last().ToString() != ".")
                    {
                        _ValidationErrorsByProperty[nameof(SolveText)] = new List<object> { "Rozwiązanie musi się kończyć znakiem kropki." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(SolveText)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(SolveText)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(SolveText)));
                    }
                    break;
            }
            if ((DateTime.TryParse(StartActionDate, culture, styles, out dateResult)) && (DateTime.TryParse(PlanActionDate, culture, styles, out dateResult1)))
            {

                if (DateTime.Parse(StartActionDate) > DateTime.Parse(PlanActionDate))
                {
                    _ValidationErrorsByProperty[nameof(StartActionDate)] = new List<object> { "Data zakończenia jest przed datą rozpoczęcia." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
                    _ValidationErrorsByProperty[nameof(PlanActionDate)] = new List<object> { "Data zakończenia jest przed datą rozpoczęcia." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
                }
                else if ((DateTime.Parse(StartActionDate) < MinDate) || (DateTime.Parse(StartActionDate) > MaxDate))
                {
                    _ValidationErrorsByProperty[nameof(StartActionDate)] = new List<object> { "Data rozpoczęcia musi być w ciągu roku." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
                }
                else if ((DateTime.Parse(PlanActionDate) < MinDate) || (DateTime.Parse(PlanActionDate) > MaxDate))
                {
                    _ValidationErrorsByProperty[nameof(PlanActionDate)] = new List<object> { "Data zakończenia musi być w ciągu roku." };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
                }
                else
                {
                    _ValidationErrorsByProperty.Remove(nameof(StartActionDate));
                    _ValidationErrorsByProperty.Remove(nameof(PlanActionDate));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(StartActionDate)));
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PlanActionDate)));
                }
            }
            AddCommand.NotifyCanExecuteChanged();
        }
    }
}
