using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TablicaDIM.ViewModel;

namespace TablicaDIM.OtherClasses
{
    public class InputsViewModel : ViewModelBase
    {
        private object _inputsVMCon;
        public object InputsVMCon
        {
            get => _inputsVMCon;
            set => SetProperty(ref _inputsVMCon, value);
        }
        private string? _shopName;
        public virtual string ShopName
        {
            get
            {
                if (_shopName != null)
                {
                    return _shopName;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _shopName, value);
        }
        private string? _surname;
        public virtual string Surname
        {
            get
            {
                if (_surname != null)
                {
                    return _surname;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _surname, value);
        }
        private string? _name;
        public virtual string Name
        {
            get
            {
                if (_name != null)
                {
                    return _name;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _name, value);
        }
        private string? _placeName;
        public virtual string PlaceName
        {
            get
            {
                if (_placeName != null)
                {
                    return _placeName;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _placeName, value);
        }
        private string? _login;
        public virtual string Login
        {
            get
            {
                if (_login != null)
                {
                    return _login;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _login, value);
        }
        private string? _email;
        public virtual string Email
        {
            get
            {
                if (_email != null)
                {
                    return _email;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _email, value);
        }
        private string? _password;
        public virtual string Password
        {
            get
            {
                if (_password != null)
                {
                    return _password;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _password, value);
        }
        private string? _newPassword;
        public virtual string NewPassword
        {
            get
            {
                if (_newPassword != null)
                {
                    return _newPassword;
                }
                else
                {
                    return string.Empty;
                }
            }
            set => SetProperty(ref _newPassword, value);
        }
        private string? _confirmpassword;
        public virtual string ConfirmPassword
        {
            get
            {
                if (_confirmpassword != null)
                {
                    return _confirmpassword;
                }
                else
                {
                    return string.Empty;
                }
            }

            set => SetProperty(ref _confirmpassword, value);
        }

        public double _chartMTTR;
        public virtual double ChartMTTR
        {
            get => _chartMTTR;
            set => SetProperty(ref _chartMTTR, value);
        }
        public double _minchartMTTR;
        public virtual double MinChartMTTR
        {
            get => _minchartMTTR;
            set => SetProperty(ref _minchartMTTR, value);
        }
        public double _maxchartMTTR;
        public virtual double MaxChartMTTR
        {
            get => _maxchartMTTR;
            set => SetProperty(ref _maxchartMTTR, value);
        }
        private double _chartMTBF;
        public virtual double ChartMTBF
        {
            get => _chartMTBF;

            set => SetProperty(ref _chartMTBF, value);
        }
        private double _minchartMTBF;
        public virtual double MinChartMTBF
        {
            get => _minchartMTBF;

            set => SetProperty(ref _minchartMTBF, value);
        }
        private double _maxchartMTBF;
        public virtual double MaxChartMTBF
        {
            get => _maxchartMTBF;

            set => SetProperty(ref _maxchartMTBF, value);
        }
        private double _chartPercentageOfBreakDowns;
        public virtual double ChartPercentageOfBreakDowns
        {
            get => _chartPercentageOfBreakDowns;

            set => SetProperty(ref _chartPercentageOfBreakDowns, value);
        }
        private double _minChartPercentageOfBreakDowns;
        public virtual double MinChartPercentageOfBreakDowns
        {
            get => _minChartPercentageOfBreakDowns;

            set => SetProperty(ref _minChartPercentageOfBreakDowns, value);
        }
        private double _maxChartPercentageOfBreakDowns;
        public virtual double MaxChartPercentageOfBreakDowns
        {
            get => _maxChartPercentageOfBreakDowns;

            set => SetProperty(ref _maxChartPercentageOfBreakDowns, value);
        }
        private int _chartcountOfBreakDowns;
        public virtual int ChartCountOfBreakDowns
        {
            get => _chartcountOfBreakDowns;

            set => SetProperty(ref _chartcountOfBreakDowns, value);
        }
        private int _minChartCountOfBreakDowns;
        public virtual int MinChartCountOfBreakDowns
        {
            get => _minChartCountOfBreakDowns;

            set => SetProperty(ref _minChartCountOfBreakDowns, value);
        }
        private int _maxChartCountOfBreakDowns;
        public virtual int MaxChartCountOfBreakDowns
        {
            get => _maxChartCountOfBreakDowns;

            set => SetProperty(ref _maxChartCountOfBreakDowns, value);
        }
        private Visibility _badNameOrPass;
        public virtual Visibility BadNameOrPass
        {
            get => _badNameOrPass;
            set => SetProperty(ref _badNameOrPass, value);
        }
        private Visibility _permisionMaster;
        public virtual Visibility PermisionMaster
        {
            get => _permisionMaster;
            set => SetProperty(ref _permisionMaster, value);
        }
        private Visibility _permisionTechnical;
        public virtual Visibility PermisionTechnical
        {
            get => _permisionTechnical;
            set => SetProperty(ref _permisionTechnical, value);
        }
        public bool HasErrors => _ValidationErrorsByProperty.Any();
        public readonly Dictionary<string, List<object>> _ValidationErrorsByProperty = new();
        public virtual void RemoveError()
        {
            _ValidationErrorsByProperty.Clear();
        }
        public IEnumerable GetErrors(string propertyName)
        {
            if (_ValidationErrorsByProperty.TryGetValue(propertyName, out List<object> errors))
            {
                return errors;
            }
            return Array.Empty<object>();
        }
        public virtual void ClearAllValues()
        {
            ShopName = string.Empty;
            Surname = string.Empty;
            Name = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            Email = string.Empty;
        }
        public virtual void ResetErrorAndValues()
        {
            ClearAllValues();
            RemoveError();
        }
        public void DataAssigment(ManagmentShopViewModel managmentshopviewmodel)
        {
            InputsVMCon = this;
            Context = managmentshopviewmodel.Context;
            SelectedShopFromFirstWindow = managmentshopviewmodel.SelectedShopFromFirstWindow;
            LoggedPerson = managmentshopviewmodel.LoggedPerson;
            BoundMessageQueue = managmentshopviewmodel.BoundMessageQueue;
            ManagmentShopViewModel = managmentshopviewmodel;
        }
    }
}
