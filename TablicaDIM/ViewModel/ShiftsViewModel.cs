using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;

namespace TablicaDIM.ViewModel
{
    public class ShiftsViewModel : InputsViewModel, INotifyDataErrorInfo, IMenuItem
    {
        public static string TitleToMenu { get; } = "Zmiany pracowników";
        public string Title { get; } = "Zmiany pracowników";
        private List<TblPerson> _shift1Context;
        public List<TblPerson> Shift1Context
        {
            get => _shift1Context;
            set => SetProperty(ref _shift1Context, value);
        }
        private TblPerson _shift1Selected;
        public TblPerson? Shift1Selected
        {
            get => _shift1Selected;
            set
            {
                if (SetProperty(ref _shift1Selected, value))
                {
                    if (value != null)
                    {
                        Shift2Selected = null;
                        Shift3Selected = null;
                        Shift4Selected = null;
                    }
                }
            }
        }
        private TblPerson _shift2Selected;
        public TblPerson? Shift2Selected
        {
            get => _shift2Selected;
            set
            {
                if (SetProperty(ref _shift2Selected, value))
                {
                    if (value != null)
                    {
                        Shift1Selected = null;
                        Shift3Selected = null;
                        Shift4Selected = null;
                    }
                }
            }
        }
        private TblPerson _shift3Selected;
        public TblPerson? Shift3Selected
        {
            get => _shift3Selected;
            set
            {
                if (SetProperty(ref _shift3Selected, value))
                {
                    if (value != null)
                    {
                        Shift2Selected = null;
                        Shift1Selected = null;
                        Shift4Selected = null;
                    }
                }
            }
        }
        private TblPerson _shift4Selected;
        public TblPerson? Shift4Selected
        {
            get => _shift4Selected;
            set
            {
                if (SetProperty(ref _shift4Selected, value))
                {
                    if (value != null)
                    {
                        Shift2Selected = null;
                        Shift3Selected = null;
                        Shift1Selected = null;
                    }
                }
            }
        }
        private TblPerson _shift1SelectedAdd;
        public TblPerson? Shift1SelectedAdd
        {
            get => _shift1SelectedAdd;
            set
            {
                if (SetProperty(ref _shift1SelectedAdd, value))
                {
                    if (value != null)
                    {
                        Shift2SelectedAdd = null;
                        Shift3SelectedAdd = null;
                        Shift4SelectedAdd = null;
                    }
                }
            }
        }
        private TblPerson _shift2SelectedAdd;
        public TblPerson? Shift2SelectedAdd
        {
            get => _shift2SelectedAdd;
            set
            {
                if (SetProperty(ref _shift2SelectedAdd, value))
                {
                    if (value != null)
                    {
                        Shift1SelectedAdd = null;
                        Shift3SelectedAdd = null;
                        Shift4SelectedAdd = null;
                    }
                }
            }
        }
        private TblPerson _shift3SelectedAdd;
        public TblPerson? Shift3SelectedAdd
        {
            get => _shift3SelectedAdd;
            set
            {
                if (SetProperty(ref _shift3SelectedAdd, value))
                {
                    if (value != null)
                    {
                        Shift2SelectedAdd = null;
                        Shift1SelectedAdd = null;
                        Shift4SelectedAdd = null;
                    }
                }
            }
        }
        private TblPerson _shift4SelectedAdd;
        public TblPerson? Shift4SelectedAdd
        {
            get => _shift4SelectedAdd;
            set
            {
                if (SetProperty(ref _shift4SelectedAdd, value))
                {
                    if (value != null)
                    {
                        Shift2SelectedAdd = null;
                        Shift3SelectedAdd = null;
                        Shift1SelectedAdd = null;
                    }
                }
            }
        }

        private List<TblPerson> _shift2Context;
        public List<TblPerson> Shift2Context
        {
            get => _shift2Context;
            set => SetProperty(ref _shift2Context, value);
        }
        private List<TblPerson> _shift3Context;
        public List<TblPerson> Shift3Context
        {
            get => _shift3Context;
            set => SetProperty(ref _shift3Context, value);
        }
        private List<TblPerson> _shift4Context;
        public List<TblPerson> Shift4Context
        {
            get => _shift4Context;
            set => SetProperty(ref _shift4Context, value);
        }
        private List<TblPerson> _shift0Context;
        public List<TblPerson> Shift0Context
        {
            get => _shift0Context;
            set => SetProperty(ref _shift0Context, value);
        }
        private string _officeAlarmStr;
        public string OfficeAlarmStr
        {
            get => _officeAlarmStr;
            set => SetProperty(ref _officeAlarmStr, value);
        }
        private string _officeWarningStr;
        public string OfficeWarningStr
        {
            get => _officeWarningStr;
            set => SetProperty(ref _officeWarningStr, value);
        }
        private string _technicalAlarmStr;
        public string TechnicalAlarmStr
        {
            get => _technicalAlarmStr;
            set => SetProperty(ref _technicalAlarmStr, value);
        }
        private string _technicalWarningStr;
        public string TechnicalWarningStr
        {
            get => _technicalWarningStr;
            set => SetProperty(ref _technicalWarningStr, value);
        }

        public RelayCommand SubmitCommand { get; }
        public RelayCommand MinusCommand { get; }
        public RelayCommand AddCommand { get; }

        public ShiftsViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            SubmitCommand = new RelayCommand(SetValues, CanSubmit);
            MinusCommand = new RelayCommand(SetZeroValues);
            AddCommand = new RelayCommand(SetAddValues);

            DataAssigment(managmentshopviewmodel);
            SetShifts();
            ClearSelect();
        }
        private void ClearSelect()
        {
            Shift1Selected = null;
            Shift2Selected = null;
            Shift3Selected = null;
            Shift4Selected = null;
            Shift1SelectedAdd = null;
            Shift2SelectedAdd = null;
            Shift3SelectedAdd = null;
            Shift4SelectedAdd = null;
        }
        private void SetShifts()
        {
            DimTabContext con = new();
            Context = con;
            OfficeAlarmStr = Context.TblShops.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First().OfficeAlarm.ToString();
            OfficeWarningStr = Context.TblShops.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First().OfficeWarning.ToString();
            TechnicalAlarmStr = Context.TblShops.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First().TechnicalAlarm.ToString();
            TechnicalWarningStr = Context.TblShops.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First().TechnicalWarning.ToString();
            Shift1Context = new();
            Shift1Context.Clear();
            Shift1Context = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Shift == 1).OrderBy(d => d.Surname).ToList();
            Shift2Context = new();
            Shift2Context.Clear();
            Shift2Context = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Shift == 2).OrderBy(d => d.Surname).ToList();
            Shift3Context = new();
            Shift3Context.Clear();
            Shift3Context = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Shift == 3).OrderBy(d => d.Surname).ToList();
            Shift4Context = new();
            Shift4Context.Clear();
            Shift4Context = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Shift == 4).OrderBy(d => d.Surname).ToList();
            Shift0Context = new();
            Shift0Context.Clear();
            Shift0Context = Context.TblPersons.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).Where(d => d.Shift == 0 || d.Shift == null).OrderBy(d => d.Surname).ToList();
        }
        private bool CanSubmit()
        {
            if ((!string.IsNullOrWhiteSpace(TechnicalWarningStr)) && (!string.IsNullOrWhiteSpace(TechnicalAlarmStr)) && (!string.IsNullOrWhiteSpace(OfficeWarningStr)) && (!string.IsNullOrWhiteSpace(OfficeAlarmStr)))
            {
                return !HasErrors;
            }
            else
            {
                return false;
            }
        }
        private async void SetAddValues()
        {
            if (Shift1SelectedAdd != null)
            {
                TblPerson temp = new();
                temp = Shift1SelectedAdd;
                temp.Shift = 1;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift1SelectedAdd.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            else if (Shift2SelectedAdd != null)
            {
                TblPerson temp = new();
                temp = Shift2SelectedAdd;
                temp.Shift = 2;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift2SelectedAdd.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            else if (Shift3SelectedAdd != null)
            {
                TblPerson temp = new();
                temp = Shift3SelectedAdd;
                temp.Shift = 3;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift3SelectedAdd.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            else if (Shift4SelectedAdd != null)
            {
                TblPerson temp = new();
                temp = Shift4SelectedAdd;
                temp.Shift = 4;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift4SelectedAdd.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            DialogHost.CloseDialogCommand.Execute(null, null);

        }
        private async void SetZeroValues()
        {
            if (Shift1Selected != null)
            {
                TblPerson temp = new();
                temp = Shift1Selected;
                temp.Shift = null;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift1Selected.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            else if (Shift2Selected != null)
            {
                TblPerson temp = new();
                temp = Shift2Selected;
                temp.Shift = null;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift2Selected.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            else if (Shift3Selected != null)
            {
                TblPerson temp = new();
                temp = Shift3Selected;
                temp.Shift = null;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift3Selected.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }
            else if (Shift4Selected != null)
            {
                TblPerson temp = new();
                temp = Shift4Selected;
                temp.Shift = null;
                Context.Entry(Context.TblPersons.Where(d => d.PersonId == Shift4Selected.PersonId).First()).CurrentValues.SetValues(temp);
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                SetShifts();
                ClearSelect();
            }

        }
        private async void SetValues()
        {
            if (!HasErrors)
            {
                bool result = await ValidateSets();
                if (result)
                {
                    BoundMessageQueue.Enqueue("Wskaźniki ustawione.");
                }
            }
        }
        private async Task<bool> ValidateSets()
        {
            TblShop exist = new();
            exist = Context.TblShops.Where(d => d.ShopId == SelectedShopFromFirstWindow.ShopId).First();
            TblShop tempnew = new();
            tempnew = exist;
            tempnew.OfficeAlarm = Int32.Parse(OfficeAlarmStr);
            tempnew.OfficeWarning = Int32.Parse(OfficeWarningStr);
            tempnew.TechnicalAlarm = Int32.Parse(TechnicalAlarmStr);
            tempnew.TechnicalWarning = Int32.Parse(TechnicalWarningStr);
            Context.Entry(exist).CurrentValues.SetValues(tempnew);
            Context.SaveChanges();

            ManagmentShopViewModel.Update();
            ManagmentShopViewModel.SelectHomeView();
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
                case nameof(TechnicalWarningStr):
                    if (string.IsNullOrWhiteSpace(TechnicalWarningStr))
                    {
                        _ValidationErrorsByProperty[nameof(TechnicalWarningStr)] = new List<object> { "Wartość jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TechnicalWarningStr)));
                    }
                    else if (!Regex.IsMatch(TechnicalWarningStr, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(TechnicalWarningStr)] = new List<object> { "Zły format. Przykładowy format 000" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TechnicalWarningStr)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(TechnicalWarningStr)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TechnicalWarningStr)));
                    }
                    break;
                case nameof(TechnicalAlarmStr):
                    if (string.IsNullOrWhiteSpace(TechnicalAlarmStr))
                    {
                        _ValidationErrorsByProperty[nameof(TechnicalAlarmStr)] = new List<object> { "Wartość jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TechnicalAlarmStr)));
                    }
                    else if (!Regex.IsMatch(TechnicalAlarmStr, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(TechnicalAlarmStr)] = new List<object> { "Zły format. Przykładowy format 000" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TechnicalAlarmStr)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(TechnicalAlarmStr)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(TechnicalAlarmStr)));
                    }
                    break;
                case nameof(OfficeWarningStr):
                    if (string.IsNullOrWhiteSpace(OfficeWarningStr))
                    {
                        _ValidationErrorsByProperty[nameof(OfficeWarningStr)] = new List<object> { "Wartość jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(OfficeWarningStr)));
                    }
                    else if (!Regex.IsMatch(OfficeWarningStr, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(OfficeWarningStr)] = new List<object> { "Zły format. Przykładowy format 000" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(OfficeWarningStr)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(OfficeWarningStr)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(OfficeWarningStr)));
                    }
                    break;
                case nameof(OfficeAlarmStr):
                    if (string.IsNullOrWhiteSpace(OfficeAlarmStr))
                    {
                        _ValidationErrorsByProperty[nameof(OfficeAlarmStr)] = new List<object> { "Wartość jest wymagana." };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(OfficeAlarmStr)));
                    }
                    else if (!Regex.IsMatch(OfficeAlarmStr, "^[0-9]{1,6}?$"))
                    {
                        _ValidationErrorsByProperty[nameof(OfficeAlarmStr)] = new List<object> { "Zły format. Przykładowy format 000" };
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(OfficeAlarmStr)));
                    }
                    else if (_ValidationErrorsByProperty.Remove(nameof(OfficeAlarmStr)))
                    {
                        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(OfficeAlarmStr)));
                    }
                    break;
            }
            SubmitCommand.NotifyCanExecuteChanged();
        }
    }
}

