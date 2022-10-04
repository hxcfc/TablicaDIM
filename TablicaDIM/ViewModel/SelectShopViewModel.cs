using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using TablicaDIM.View.Persons;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace TablicaDIM.ViewModel
{
    public class SelectShopViewModel : ViewModelBase
    {
        public List<TblShop> _tblShops;
        public List<TblShop> TblShops
        {
            get => _tblShops;
            set => SetProperty(ref _tblShops, value);
        }
        public List<TblUserSelectedShop> _tblUserSelectedShops;
        public List<TblUserSelectedShop> TblUserSelectedShops
        {
            get => _tblUserSelectedShops;
            set => SetProperty(ref _tblUserSelectedShops, value);
        }
        private TblShop _selectedShopFromListBox;
        public TblShop SelectedShopFromListBox
        {
            get => _selectedShopFromListBox;

            set
            {
                if (value != null)
                {
                    if (SetProperty(ref _selectedShopFromListBox, value))
                    {
                        OpenSelectedShop();
                    }
                }
            }
        }
        private bool _isSelectedShopCheckBox;
        public bool IsSelectedShopCheckBox
        {
            get => _isSelectedShopCheckBox;
            set => SetProperty(ref _isSelectedShopCheckBox, value);
        }
        private bool _showInactive;
        public bool ShowInactive
        {
            get => _showInactive;
            set => SetProperty(ref _showInactive, value);
        }
        private bool _isSelectedAutomaticRefCheckBox;
        public bool IsSelectedAutomaticRefCheckBox
        {
            get => _isSelectedAutomaticRefCheckBox;
            set
            {
                if (value != null)
                {
                    if (SetProperty(ref _isSelectedAutomaticRefCheckBox, value))
                    {
                        if (value == true)
                        {
                            StatusVis = Visibility.Visible;
                            DispatcherTimer.Start();
                            CanClickBool = true;
                        }
                        else
                        {
                            DispatcherTimer.Stop();
                            StatusVis = Visibility.Hidden;
                            CanClickBool = true;
                        }
                    }
                }
            }
        }
        private Visibility _statusVis;
        public Visibility StatusVis
        {
            get => _statusVis;
            set => SetProperty(ref _statusVis, value);
        }
        private string _statusString;
        public string StatusString
        {
            get => _statusString;
            set => SetProperty(ref _statusString, value);
        }
        private Brush _buttonArchiv;
        public Brush ButtonArchiv
        {
            get => _buttonArchiv;
            set => SetProperty(ref _buttonArchiv, value);
        }
        private DispatcherTimer _dispatcherTimer;
        public DispatcherTimer DispatcherTimer
        {
            get => _dispatcherTimer;
            set => SetProperty(ref _dispatcherTimer, value);
        }
        private int _counterToCheckVersion;
        public int CounterToCheckVersion
        {
            get => _counterToCheckVersion;
            set => SetProperty(ref _counterToCheckVersion, value);
        }
        private bool _canClickBool;
        public bool CanClickBool
        {
            get => _canClickBool;
            set => SetProperty(ref _canClickBool, value);

        }
        private Task _taskCheckIn;
        public Task TaskCheckIn
        {
            get => _taskCheckIn;
            set => SetProperty(ref _taskCheckIn, value);
        }
        private string _userPCNameWithCPUID;
        public string UserPCNameWithCPUID
        {
            get => _userPCNameWithCPUID;
            set => SetProperty(ref _userPCNameWithCPUID, value);
        }
        // Commands
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand ShowAddShopFormCommand { get; }
        public RelayCommand ShowInactiveShopCommand { get; }
        public SelectShopViewModel(DimTabContext context)
        {
            AddResources();
            //Commands
            CloseWindowCommand = new RelayCommand(CloseWindow);
            ShowAddShopFormCommand = new RelayCommand(ShowAddShopForm);
            ShowInactiveShopCommand = new RelayCommand(ShowInactiveShop);
            TblUserSelectedShops = new();
            TblShops = new();

            //Data of page
            Context = context;
            ShowInactive = false;
            ButtonArchiv = new SolidColorBrush(Colors.White);
            StatusVis = Visibility.Visible;
            CanClickBool = false;
            string accentedStr = WindowsIdentity.GetCurrent().Name.ToString() + getCPUId();
            byte[] tempBytes;
            System.Text.EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);
            UserPCNameWithCPUID = asciiStr;
            //Timer of autoupdate
            DispatcherTimer = new();
            DispatcherTimer.Tick += new EventHandler(AutoUpdate_Tick);
            DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            CounterToCheckVersion = 0;
            //First load funcions

            TaskCheckIn = CheckIn();
            if (TaskCheckIn.Status == TaskStatus.Created)
                TaskCheckIn.Start();

        }
        private async void AddResources()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/View/DialogTemplates/SelectShopDialogTemplate.xaml")
            });
        }

        private void ShowInactiveShop()
        {
            if (ShowInactive == true)
            {
                ShowInactive = false;
                ButtonArchiv = new SolidColorBrush(Colors.White);
            }
            else
            {
                ShowInactive = true;
                ButtonArchiv = new SolidColorBrush(Colors.LimeGreen);
            }
            TblShops.Clear();
            CheckIn();
        }

        private void AutoUpdate_Tick(object sender, EventArgs e)
        {
            DimTabContext context = new();
            Context = context;

            if (CounterToCheckVersion <= 10)
            {
                ShopList();
                CounterToCheckVersion++;
            }
            else
            {
                CheckIn();
                CounterToCheckVersion = 0;
            }
        }

        private async Task CheckIn()
        {
            CanClickBool = false;
            await ConnectChecker();
            await Task.Delay(10);
            await VersionChecker();
            await Task.Delay(10);
            await DateChecker();
            await Task.Delay(10);
            await ChangeShifts();
            await ShopList();
            if (DispatcherTimer.IsEnabled == false)
            {
                IsSelectedAutomaticRefCheckBox = true;
            }
            await StatusOfCheckBox();
        }

        private async Task ConnectChecker()
        {
            StatusString = "Sprawdzanie połączenia z bazą danych..";
            bool result = await Context.Database.CanConnectAsync();
            if (!result)
            {
                MessageBox.Show("Brak połączenia z bazą danych. Spróbuj ponownie później. Jeśli problem nie ustępuje - powiadom o tym programiste.", "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWindow();
            }
        }
        private async Task VersionChecker()
        {
            StatusString = "Sprawdzanie wersji aplikacji..";
            int appVersion = 21;
            List<TblAppInfo> checkVersionInDB = new();
            checkVersionInDB = await CheckVersionOfAppInDB();
            if (checkVersionInDB.Count != 0)
            {
                if (checkVersionInDB[0].AppVersion != appVersion)
                {
                    var Result = MessageBox.Show("Wersja Twojej aplikacji nie jest już obsługiwana. Chcesz pobrać nową?", "Masz starą wersję aplikacji", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (Result == MessageBoxResult.Yes)
                        CloseWindow();
                    else if (Result == MessageBoxResult.No)
                        CloseWindow();
                }
            }
            else
            {
                await CreateNewRowWithVersion(appVersion);
            }
        }
        private async Task<DateTime> DateFromNetwork()
        {
            List<DateTime> timeFromNetworkDateTime = new();
            List<string> listOfServers = new();
            listOfServers.Add("129.6.15.28");
            listOfServers.Add("129.6.15.29");
            listOfServers.Add("132.163.97.1");
            listOfServers.Add("132.163.97.2");
            listOfServers.Add("132.163.96.1");
            listOfServers.Add("132.163.96.2");
            DateTime maxTimeFromNetworkDateTime = new();
            foreach (string serverString in listOfServers)
            {
                TcpClient client = new();
                try
                {
                    client.Connect(serverString, 13);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Caught exception {e.GetType()}.{Environment.NewLine}{e.Message}.");
                }
                if (client.Connected == true)
                {
                    using (var streamReader = new StreamReader(client.GetStream()))
                    {
                        var response = streamReader.ReadToEnd();
                        var utcDateTimeString = (!string.IsNullOrEmpty(response)) ? response.Substring(7, 17) : "wrongdate";
                        if (utcDateTimeString != "wrongdate")
                        {
                            timeFromNetworkDateTime.Add(DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal));
                        }
                        else
                        {
                            timeFromNetworkDateTime.Add(DateTime.MinValue);
                        }
                    }
                }

            }
            foreach (DateTime dateTime in timeFromNetworkDateTime)
            {
                if (maxTimeFromNetworkDateTime < dateTime)
                {
                    maxTimeFromNetworkDateTime = dateTime;
                }
            }
            return maxTimeFromNetworkDateTime;
        }
        private async Task DateChecker()
        {
            StatusString = "Sprawdzanie poprawności daty i godziny z serwera czasu..";
            DateTime maxTimeFromNetworkDateTime = new();
            maxTimeFromNetworkDateTime = await DateFromNetwork();
            DateTime NetworkDateTimeWithoutSeconds = new DateTime(maxTimeFromNetworkDateTime.Year, maxTimeFromNetworkDateTime.Month, maxTimeFromNetworkDateTime.Day, maxTimeFromNetworkDateTime.Hour, maxTimeFromNetworkDateTime.Minute, 0);

            DateTime SystemDateTime = new DateTime();
            SystemDateTime = DateTime.Now;
            DateTime SystemDateWithoutSecond = new DateTime(SystemDateTime.Year, SystemDateTime.Month, SystemDateTime.Day, SystemDateTime.Hour, SystemDateTime.Minute, 0);

            if ((NetworkDateTimeWithoutSeconds == DateTime.MinValue))
            {
                MessageBox.Show("Błąd podczas łączeniem z serwerem daty. Aplikacja wymaga połączenia internetowego.  Proszę napraw połączenie internetowe, by móc korzystać z aplikacji.", "Błąd połączenia z internetem", MessageBoxButton.OK, MessageBoxImage.Error);
                CloseWindow();
            }
            else
            {
                DateTime MinDateValue = NetworkDateTimeWithoutSeconds.Subtract(new TimeSpan(4, 0, 0));
                DateTime MaxDateValue = NetworkDateTimeWithoutSeconds.AddHours(4);
                if ((MinDateValue > SystemDateWithoutSecond) || (SystemDateWithoutSecond > MaxDateValue))
                {
                    MessageBox.Show("Błędna data systemu. Proszę zaktualizuj datę i godzinę, by móc korzystać z aplikacji.", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Error);
                    CloseWindow();
                }
            }
        }
        private async Task ChangeShifts()
        {
            StatusString = "Reorganizacja zmian pracowników..";
            await Task.Delay(50);
            GregorianCalendar cal = new GregorianCalendar();
            int week = cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            var rowDb = await CheckVersionOfAppInDB();
            int weekDb = rowDb[0].AppWeek;
            if (weekDb < week)
            {
                TblAppInfo weekToChange = new();
                weekToChange = rowDb[0];
                for (int i = 0; i < week - weekDb; i++)
                {
                    await PersonToPersonChangeShift();
                    weekToChange.AppWeek++;
                }
                Context.Entry(rowDb[0]).CurrentValues.SetValues(weekToChange);
                Context.SaveChanges();
            }
            else if (week == 1)
            {
                TblAppInfo weekToChange = new();
                weekToChange = rowDb[0];
                await PersonToPersonChangeShift();
                weekToChange.AppWeek = 1;
                Context.Entry(rowDb[0]).CurrentValues.SetValues(weekToChange);
                Context.SaveChanges();
            }
        }
        private async Task PersonToPersonChangeShift()
        {
            List<TblPerson> Persons = new();
            Persons = await GetPersonsFromDb();
            foreach (TblPerson person in Persons)
            {
                TblPerson editedPerson = new();
                editedPerson = person;
                if (person.Shift == 1)
                {
                    editedPerson.Shift = 11;
                }
                else if (person.Shift == 2)
                {
                    editedPerson.Shift = 22;
                }
                else if (person.Shift == 3)
                {
                    editedPerson.Shift = 33;
                }
                else if (person.Shift == 4)
                {
                    editedPerson.Shift = 44;
                }
                Context.Entry(person).CurrentValues.SetValues(editedPerson);
                Context.SaveChanges();
            }

            List<TblPerson> PersonsAfterChange = new();
            PersonsAfterChange = await GetPersonsFromDb();
            foreach (TblPerson person in PersonsAfterChange)
            {
                TblPerson editedPerson = new();
                editedPerson = person;
                if (person.Shift == 44)
                {
                    editedPerson.Shift = 4;
                }
                else if (person.Shift == 22)
                {
                    editedPerson.Shift = 1;
                }
                else if (person.Shift == 33)
                {
                    editedPerson.Shift = 2;
                }
                else if (person.Shift == 11)
                {
                    editedPerson.Shift = 3;
                }
                Context.Entry(person).CurrentValues.SetValues(editedPerson);
                Context.SaveChanges();
            }
        }

        private async Task<List<TblAppInfo>> CheckVersionOfAppInDB()
        {
            var GetDataFromContext = Context.TblAppInfos.ToList();
            return GetDataFromContext;
        }
        private async Task<List<TblPerson>> GetPersonsFromDb()
        {
            var GetDataFromContext = Context.TblPersons.ToList();
            return GetDataFromContext;
        }
        private async Task CreateNewRowWithVersion(int appVersion)
        {
            TblAppInfo newData = new();
            newData.AppVersion = appVersion;
            newData.AppWeek = 0;
            Context.Add(newData);
            Context.SaveChanges();
        }
        private async Task DoneAndClear()
        {
            StatusString = "Oczekiwanie na działanie użytkownika..";
            CanClickBool = true;

        }
        private async Task ShopList()
        {
            StatusString = "Pobieranie obszarów z bazy danych..";
            await Task.Delay(10);
            await DoneAndClear();
            if (ShowInactive == false)
            {
                TblShops = GetShopList();
            }
            else
            {
                TblShops = GetInactiveShopList();
            }
        }
        private Task<bool> CheckExistUserPCNameWithCPUID()
        {
            bool exist = false;
            foreach (var user in TblUserSelectedShops)
            {
                if (user.UserPc == UserPCNameWithCPUID)
                    exist = true;
            }
            return Task.FromResult(exist);
        }
        private Task<int> CheckIDShopIfExistUserPCNameWithCPUID()
        {
            int idUser = 0;
            foreach (var user in TblUserSelectedShops)
            {
                if (user.UserPc == UserPCNameWithCPUID)
                    if ((user.ShopId != null) && (user.ShopId != 0))
                        idUser = (int)user.ShopId;
            }
            return Task.FromResult(idUser);
        }

        private async Task StatusOfCheckBox()
        {
            TblUserSelectedShops = GetUserPCShopList();
            int shopID = await CheckIDShopIfExistUserPCNameWithCPUID();

            if (shopID != 0)
            {
                IsSelectedShopCheckBox = true;
                SelectedShopFromListBox = TblShops.Find(d => d.ShopId == shopID);
            }
            else
            {
                IsSelectedShopCheckBox = false;
            }
        }

        private async void ShopSelected()
        {
            bool exist = await CheckExistUserPCNameWithCPUID();

            if (IsSelectedShopCheckBox == true)
            {
                if (exist)
                {
                    TblUserSelectedShop tempValue = new();
                    TblUserSelectedShop tempValueEdited = new();
                    tempValue = TblUserSelectedShops.Find(d => d.UserPc == UserPCNameWithCPUID);
                    tempValueEdited = TblUserSelectedShops.Find(d => d.UserPc == UserPCNameWithCPUID);
                    tempValueEdited.ShopId = SelectedShopFromListBox.ShopId;
                    Context.Entry(tempValue).CurrentValues.SetValues(tempValueEdited);
                }
                else
                {
                    TblUserSelectedShop newValue = new() { UserPc = UserPCNameWithCPUID, ShopId = SelectedShopFromListBox.ShopId };
                    Context.TblUserSelectedShops.Add(newValue);
                }
            }
            else
            {
                if (exist)
                {
                    TblUserSelectedShop tempValue = new();
                    TblUserSelectedShop tempValueEdited = new();
                    tempValue = TblUserSelectedShops.Find(d => d.UserPc == UserPCNameWithCPUID);
                    tempValueEdited = TblUserSelectedShops.Find(d => d.UserPc == UserPCNameWithCPUID);
                    tempValueEdited.ShopId = null;
                    Context.Entry(tempValue).CurrentValues.SetValues(tempValueEdited);
                }
                else
                {
                    TblUserSelectedShop newValue = new() { UserPc = UserPCNameWithCPUID, ShopId = SelectedShopFromListBox.ShopId };
                    Context.TblUserSelectedShops.Add(newValue);
                }
            }
            Context.SaveChanges();
            
        }
        private void OpenSelectedShop()
        {
            ShopSelected();
            ManagmentShop managmentwindow = new(SelectedShopFromListBox, Context);
            managmentwindow.Show();
            DispatcherTimer.Stop();
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this)
                {
                    item.Close();
                }
            }
        }
        private async void ShowAddShopForm()
        {
            var viewModel = new AddShopViewModel(Context);
            await MDIXDialogHost.Show(viewModel, "RootDialogHostId", new DialogOpenedEventHandler((sender, args) =>
            {
                viewModel.DialogSession = args.Session;
            }));
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
        private List<TblShop> GetShopList()
        {
            var GetDataFromContext = Context.TblShops.Where(b => b.ShopId >= 0).Where(b => b.ShopInactive == false).OrderBy(b => b.ShopName).ToList();
            return GetDataFromContext;
        }
        private List<TblShop> GetInactiveShopList()
        {
            var GetDataFromContext = Context.TblShops.Where(b => b.ShopId >= 0).Where(b => b.ShopInactive == true).OrderBy(b => b.ShopName).ToList();
            return GetDataFromContext;
        }
        private List<TblUserSelectedShop> GetUserPCShopList()
        {
            var GetDataFromContext = Context.TblUserSelectedShops.ToList();
            return GetDataFromContext;
        }
        private void CloseWindow()
        {
            App.Current.Shutdown();
        }
    }

}

