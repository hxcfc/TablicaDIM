using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using TablicaDIM.ViewModel.AccountSettings;
using TablicaDIM.ViewModel.Holidays;
using TablicaDIM.ViewModel.Home;
using TablicaDIM.ViewModel.Persons;
using TablicaDIM.ViewModel.Places;
using TablicaDIM.ViewModel.ShopAdministration;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel
{
    public class ManagmentShopViewModel : ViewModelBase
    {

        private double _maximumWidth;
        public double MaximumWidth
        {
            get => _maximumWidth;
            set => SetProperty(ref _maximumWidth, value);
        }
        private double _maximumHeight;
        public double MaximumHeight
        {
            get => _maximumHeight;
            set => SetProperty(ref _maximumHeight, value);
        }
        readonly DispatcherTimer AutoLogoutTimer = new();
        public ObservableCollection<string> MainMenuUpper { get; set; }
        public ObservableCollection<string> MainMenuLower { get; set; }
        private TblPerson _loggedPerson;
        public override TblPerson LoggedPerson
        {
            get => _loggedPerson;
            set
            {
                if (SetProperty(ref _loggedPerson, value))
                {
                    if (value.FirstLogin != null)
                    {
                        if ((bool)value.FirstLogin)
                        {
                            ShowFirstLoginForm();
                        }
                    }
                }
            }
        }
        private bool _isMainLeftMenuOpen;
        public bool IsMainLeftMenuOpen
        {
            get => _isMainLeftMenuOpen;
            set => SetProperty(ref _isMainLeftMenuOpen, value);
        }
        private object? _contentManagmentShop;
        public object? ContentManagmentShop
        {
            get => _contentManagmentShop;
            set => SetProperty(ref _contentManagmentShop, value);
        }
        private object? _selectedMainMenu;
        public object? SelectedMainMenu
        {
            get => _selectedMainMenu;
            set
            {
                if (SetProperty(ref _selectedMainMenu, value))
                {
                    // cant be case switch bcs of 'variable' cant be in case.
                    if (value == HomeViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new HomeViewModel(this);
                    }

                    else if (value == GraphsViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new GraphsViewModel(this);
                    }

                    else if (value == HolidaysViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new HolidaysViewModel(this);
                    }

                    else if (value == PersonsViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new PersonsViewModel(this);
                    }

                    else if (value == PlacesViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new PlacesViewModel(this);
                    }

                    else if (value == ShopAdministrationViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new ShopAdministrationViewModel(this);
                    }
                    else if (value == ShiftsViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new ShiftsViewModel(this);
                    }
                    else if (value == WorkInWeekendViewModel.TitleToMenu)
                    {
                        ContentManagmentShop = new WorkInWeekendViewModel(this);
                    }
                    else if (value == "Zaloguj się")
                    {
                        ShowLoginFormCommand();
                    }

                    else if (value == "Powrót do okna wyboru obszaru")
                    {
                        BackToSelectWindow();
                    }

                    else if (value == "Wyloguj się")
                    {
                        LogOut();
                    }

                    else if (value == "Ustawienia pracownika")
                    {
                        ContentManagmentShop = new AccountSettingsViewModel(this);
                    }

                    IsMainLeftMenuOpen = false;
                }
            }
        }
        private object? _selectedMainMenuLower;
        public object? SelectedMainMenuLower
        {
            get => _selectedMainMenuLower;
            set
            {
                if (SetProperty(ref _selectedMainMenuLower, value))
                {
                    switch (value)
                    {
                        case "Zaloguj się":
                            ShowLoginFormCommand();
                            break;
                        case "Powrót do okna wyboru obszaru":
                            BackToSelectWindow();
                            break;
                        case "Wyloguj się":
                            LogOut();
                            break;
                        case "Ustawienia użytkownika":
                            ContentManagmentShop = new AccountSettingsViewModel(this);
                            break;
                        default:
                            break;
                    }
                    IsMainLeftMenuOpen = false;
                }
            }
        }
        private string _actualTime;
        public string ActualTime
        {
            get => _actualTime;
            set => SetProperty(ref _actualTime, value);
        }
        private string _whosLogged;
        public string WhosLogged
        {
            get => _whosLogged;
            set => SetProperty(ref _whosLogged, value);
        }
        private int _loggedPersonId;
        public int LoggedPersonId
        {
            get => _loggedPersonId;
            set => SetProperty(ref _loggedPersonId, value);
        }
        private int _selectedShopId;
        public int SelectedShopId
        {
            get => _selectedShopId;
            set => SetProperty(ref _selectedShopId, value);
        }
        private Visibility _visiblityOfLogged;
        public Visibility VisiblityOfLogged
        {
            get => _visiblityOfLogged;
            set => SetProperty(ref _visiblityOfLogged, value);
        }
        private Visibility _isVisibleMax;
        public Visibility IsVisibleMax
        {
            get => _isVisibleMax;
            set => SetProperty(ref _isVisibleMax, value);
        }
        private Visibility _isVisibleNorm;
        public Visibility IsVisibleNorm
        {
            get => _isVisibleNorm;
            set => SetProperty(ref _isVisibleNorm, value);
        }
        private WindowState _managmentShopWindowState;
        public WindowState ManagmentShopWindowState
        {
            get => _managmentShopWindowState;
            set => SetProperty(ref _managmentShopWindowState, value);
        }
        private Visibility _isInactive;
        public Visibility IsInactive
        {
            get => _isInactive;
            set => SetProperty(ref _isInactive, value);
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
        private Visibility _permisionAdmin;
        public virtual Visibility PermisionAdmin
        {
            get => _permisionAdmin;
            set => SetProperty(ref _permisionAdmin, value);
        }
        private Visibility _permisionOwner;
        public virtual Visibility PermisionOwner
        {
            get => _permisionOwner;
            set => SetProperty(ref _permisionOwner, value);
        }
        private Visibility _permisionVisOnlyTechnicalAndMaster;
        public virtual Visibility PermisionVisOnlyTechnicalAndMaster
        {
            get => _permisionVisOnlyTechnicalAndMaster;
            set => SetProperty(ref _permisionVisOnlyTechnicalAndMaster, value);
        }

        private int _timerDown;
        public int TimerDown
        {
            get => _timerDown;
            set => SetProperty(ref _timerDown, value);
        }
        HomeViewModel VMHome;
        GraphsViewModel VMGraphs;
        HolidaysViewModel VMHolidays;
        PersonsViewModel VMPersons;
        PlacesViewModel VMPlaces;
        ShopAdministrationViewModel VMShopAdministration;
        AccountSettingsViewModel VMAccounts;
        WorkInWeekendViewModel VMWorkInWeekend;
        ShiftsViewModel VMShifts;
        //Commands
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand MinWindowCommand { get; }
        public RelayCommand NormWindowCommand { get; }
        public RelayCommand MaxWindowCommand { get; }
        public RelayCommand OpenHomePageCommand { get; }

        public ManagmentShopViewModel(TblShop selectedshop, DimTabContext context)
        {
            //Data of page
            Context = context;
            SelectedShopFromFirstWindow = selectedshop;
            BoundMessageQueue = new SnackbarMessageQueue();
            SelectedShopId = SelectedShopFromFirstWindow.ShopId;
            //Initiate
            LoggedPerson = new TblPerson();
            MainMenuUpper = new ObservableCollection<string>();
            MainMenuLower = new ObservableCollection<string>();
            if (SelectedShopFromFirstWindow.ShopInactive == true)
            {
                IsInactive = Visibility.Visible;
            }
            else
            {
                IsInactive = Visibility.Collapsed;
            }
            MaximumWidth = SystemParameters.MaximizedPrimaryScreenWidth - 9;
            MaximumHeight = SystemParameters.MaximizedPrimaryScreenHeight - 9;
            CreateStandardMenu();
            UnLoggedMenu();
            SelectHomeView();
            //Other
            ManagmentShopWindowState = WindowState.Maximized;
            //Visiblity of items in page
            IsVisibleNorm = Visibility.Visible;
            IsVisibleMax = Visibility.Collapsed;
            PermisionMaster = Visibility.Collapsed;
            PermisionTechnical = Visibility.Collapsed;
            PermisionAdmin = Visibility.Collapsed;
            PermisionOwner = Visibility.Collapsed;
            VisiblityOfLogged = Visibility.Collapsed;
            PermisionVisOnlyTechnicalAndMaster = Visibility.Collapsed;
            //Timer of actualty date
            DispatcherTimer LiveTime = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            LiveTime.Tick += delegate
            {
                ActuallyTimer();
                if (AutoLogoutTimer.IsEnabled)
                {
                    TimerDown -= 1;
                }
                if (TimerDown == 30)
                {
                    BoundMessageQueue.Enqueue("Nie wykryto ruchu przez dłuższy okres czasu. Po kolejnych 30 sekundach bez ruchu zostaniesz automatycznie wylogowana/y.", "Zamknij", () => { });
                }
            };
            LiveTime.Start();
            //Commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            MinWindowCommand = new RelayCommand(MinWindow);
            NormWindowCommand = new RelayCommand(NormWindow);
            MaxWindowCommand = new RelayCommand(MaxWindow);
            OpenHomePageCommand = new RelayCommand(SelectHomeView);
            //  Update();
            AutoLogoutTimer.Interval = TimeSpan.FromMinutes(5);

            AutoLogoutTimer.Tick += delegate
            {
                AutoLogoutAsync();
            };
            InputManager.Current.PostProcessInput += delegate (object s, ProcessInputEventArgs r)
            {
                if (r.StagingItem.Input is MouseButtonEventArgs || r.StagingItem.Input is KeyEventArgs)
                {
                    AutoLogoutTimer.Dispatcher.BeginInvoke(new Action(() => Startup()));
                    AutoLogoutTimer.Interval = TimeSpan.FromMinutes(5);
                }
            };

        }

        private void Startup()
        {
            TimerDown = (int)AutoLogoutTimer.Interval.TotalSeconds - 2;
        }

        private void CloseWindow()
        {
            Application.Current.Shutdown();
        }
        private void MinWindow()
        {
            ManagmentShopWindowState = WindowState.Minimized;
        }
        private void NormWindow()
        {
            ManagmentShopWindowState = WindowState.Normal;
            IsVisibleNorm = Visibility.Collapsed;
            IsVisibleMax = Visibility.Visible;
            MaximumWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MaximumHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        private void MaxWindow()
        {
            MaximumWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MaximumHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            ManagmentShopWindowState = WindowState.Maximized;
            IsVisibleNorm = Visibility.Visible;
            IsVisibleMax = Visibility.Collapsed;
        }
        private async void ShowLoginFormCommand()
        {
            var viewModel = new LoginViewModel(this);
            object dialogResult = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if (dialogResult is bool boolResult && boolResult)
            {
                LoggedPerson = viewModel.LoggedPerson;
                LoggedPersonId = LoggedPerson.PersonId;
                LogIn();
            }
            else
            {
                LogOutWithoutText();
            }
        }
        private async void ShowFirstLoginForm()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(300));
            var viewModel = new FirstLoginViewModel(this);
            object dialogResult = await DialogHost.Show(viewModel, "SecondDialogHostId");
            if (dialogResult is bool boolResult && boolResult)
            {
                BoundMessageQueue.Enqueue("Zmieniono hasło.", "Zamknij", () => { });
                LogInWithoutText();
            }
            else
            {
                LogOut();
            }
        }
        private void LogIn()
        {
            Update();
            BoundMessageQueue.Enqueue("Zalogowano pomyślnie.", "Zamknij", () => { });
            LogInWithoutText();
        }
        private void LogInWithoutText()
        {

            switch (LoggedPerson.PermisionId)
            {
                case 1:
                    // OWNER
                    CreateOwnerMenu();
                    PermisionAdmin = Visibility.Visible;
                    PermisionMaster = Visibility.Visible;
                    PermisionTechnical = Visibility.Visible;
                    PermisionOwner = Visibility.Visible;
                    PermisionVisOnlyTechnicalAndMaster = Visibility.Collapsed;
                    break;
                case 2:
                    //ADMIN
                    CreateAdminMenu();
                    PermisionAdmin = Visibility.Visible;
                    PermisionMaster = Visibility.Visible;
                    PermisionTechnical = Visibility.Visible;
                    PermisionOwner = Visibility.Collapsed;
                    PermisionVisOnlyTechnicalAndMaster = Visibility.Collapsed;

                    break;
                case 3:
                    //MISTRZ
                    CreateExtendMenu();
                    PermisionAdmin = Visibility.Collapsed;
                    PermisionMaster = Visibility.Visible;
                    PermisionTechnical = Visibility.Visible;
                    PermisionOwner = Visibility.Collapsed;
                    PermisionVisOnlyTechnicalAndMaster = Visibility.Visible;
                    break;
                case 4:
                    //TECHNIK
                    CreateUserMenu();
                    PermisionAdmin = Visibility.Collapsed;
                    PermisionMaster = Visibility.Collapsed;
                    PermisionTechnical = Visibility.Visible;
                    PermisionOwner = Visibility.Collapsed;
                    PermisionVisOnlyTechnicalAndMaster = Visibility.Visible;
                    break;
                default:
                    CreateStandardMenu();
                    PermisionAdmin = Visibility.Collapsed;
                    PermisionMaster = Visibility.Collapsed;
                    PermisionTechnical = Visibility.Collapsed;
                    PermisionOwner = Visibility.Collapsed;
                    PermisionVisOnlyTechnicalAndMaster = Visibility.Collapsed;

                    break;
            }
            LoggedMenu();
            SelectHomeView();
            AutoLogoutTimer.Start();
            WhosLogged = "Zalogowany jako: " + LoggedPerson.Name + " " + LoggedPerson.Surname;
            VisiblityOfLogged = Visibility.Visible;
        }
        private void CreateStandardMenu()
        {
            MainMenuUpper.Clear();
            MainMenuUpper.Add(HomeViewModel.TitleToMenu);
            MainMenuUpper.Add(GraphsViewModel.TitleToMenu);
            MainMenuUpper.Add(ShiftsViewModel.TitleToMenu);
            MainMenuUpper.Add(HolidaysViewModel.TitleToMenu);
        }
        private void CreateUserMenu()
        {
            CreateStandardMenu();
        }
        private void CreateExtendMenu()
        {
            CreateUserMenu();
            MainMenuUpper.Add(PersonsViewModel.TitleToMenu);
            MainMenuUpper.Add(WorkInWeekendViewModel.TitleToMenu);
        }
        private void CreateAdminMenu()
        {
            CreateExtendMenu();
            MainMenuUpper.Add(PlacesViewModel.TitleToMenu);
        }
        private void CreateOwnerMenu()
        {
            CreateAdminMenu();
            MainMenuUpper.Add(ShopAdministrationViewModel.TitleToMenu);
        }
        private void LoggedMenu()
        {
            MainMenuLower.Clear();
            MainMenuLower.Add("Ustawienia pracownika");
            MainMenuLower.Add("Wyloguj się");
            MainMenuLower.Add("Powrót do okna wyboru obszaru");
        }
        private void UnLoggedMenu()
        {
            MainMenuLower.Clear();
            MainMenuLower.Add("Zaloguj się");
            MainMenuLower.Add("Powrót do okna wyboru obszaru");
        }
        public void SelectHomeView()
        {
            SelectedMainMenu = MainMenuUpper[MainMenuUpper.IndexOf(HomeViewModel.TitleToMenu)];
            SelectedMainMenuLower = null;
        }
        public void SelectGraphView()
        {
            SelectedMainMenu = MainMenuUpper[MainMenuUpper.IndexOf(GraphsViewModel.TitleToMenu)];
            SelectedMainMenuLower = null;
        }
        private void BackToSelectWindow()
        {
            string accentedStr = WindowsIdentity.GetCurrent().Name.ToString() + getCPUId();
            byte[] tempBytes;
            System.Text.EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            string UserPCNameWithCPUID = asciiStr;
            List<TblUserSelectedShop> TblUserSelectedShops = new();
            TblUserSelectedShops = GetUserPCShopList();
            TblUserSelectedShop tempValue = new();
            TblUserSelectedShop tempValueEdited = new();
            tempValue = TblUserSelectedShops.Find(d => d.UserPc == UserPCNameWithCPUID);
            tempValueEdited = TblUserSelectedShops.Find(d => d.UserPc == UserPCNameWithCPUID);
            tempValueEdited.ShopId = null;
            Context.Entry(tempValue).CurrentValues.SetValues(tempValueEdited);
            Context.SaveChanges();
            SelectShop managmentwindow = new();
            managmentwindow.Show();
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this)
                {
                    item.Close();
                }
            }
        }
        private List<TblUserSelectedShop> GetUserPCShopList()
        {
            var GetDataFromContext = Context.TblUserSelectedShops.ToList();
            return GetDataFromContext;
        }
        private string getCPUId()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
        private void ActuallyTimer()
        {
            ActualTime = "Aktualna data i godzina: " + DateTime.Now.ToString();
        }
        public void Update()
        {
            DimTabContext dimTab = new();
            Context = dimTab;
            SelectedShopFromFirstWindow = dimTab.TblShops.Where(b => b.ShopId == SelectedShopId).First();
        }
        private async Task AutoLogoutAsync()
        {
            if (DialogHost.IsDialogOpen("SecondDialogHostId") == true)
            {
                DialogHost.Close("SecondDialogHostId", false);
            }
            SelectHomeView();

            var viewModel = new MessageShowOkViewModel("Przez brak aktywności zostałaś/eś automatycznie wylogowana/y.");
            AutoLogoutTimer.Stop();

            if (ManagmentShopWindowState != WindowState.Minimized)
            {
                await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            }

            BoundMessageQueue.Clear();
            BoundMessageQueue.Enqueue("Wylogowano pomyślnie.", "Zamknij", () => { });
            LogOutWithoutText();
        }

        public void LogOut()
        {
            if (DialogHost.IsDialogOpen("SecondDialogHostId") == true)
            {
                DialogHost.Close("SecondDialogHostId", false);
            }
            Update();
            BoundMessageQueue.Enqueue("Wylogowano pomyślnie.", "Zamknij", () => { });
            LogOutWithoutText();
        }
        private async void LogOutWithoutText()
        {
            //Clear data
            LoggedPerson = new TblPerson();
            AutoLogoutTimer.Stop();
            VisiblityOfLogged = Visibility.Collapsed;
            PermisionMaster = Visibility.Collapsed;
            PermisionTechnical = Visibility.Collapsed;
            PermisionAdmin = Visibility.Collapsed;
            PermisionOwner = Visibility.Collapsed;
            PermisionVisOnlyTechnicalAndMaster = Visibility.Collapsed;
            //Clear menus
            CreateStandardMenu();
            UnLoggedMenu();
            SelectHomeView();
        }
    }
}
