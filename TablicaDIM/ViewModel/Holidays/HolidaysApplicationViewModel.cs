using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TablicaDIM.DBModels;
using TablicaDIM.OtherClasses;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;


namespace TablicaDIM.ViewModel.Holidays
{
    public class HolidaysApplicationViewModel : InputsViewModel, IMenuItem
    {
        public string Title { get; set; } = "Zarządzanie wnioskami pracowników";
        public List<object?> _contextToDatagrid;
        public List<object?> ContextToDatagrid
        {
            get => _contextToDatagrid;
            set => SetProperty(ref _contextToDatagrid, value);
        }
        private object? _holidaysListSelect;
        public object? HolidaysListSelect
        {
            get => _holidaysListSelect;
            set
            {
                if (SetProperty(ref _holidaysListSelect, value))
                {
                    if (value != null)
                    {
                        VisOption = Visibility.Visible;
                    }
                    else
                    {
                        VisOption = Visibility.Collapsed;
                    }

                }
            }
        }
        private Visibility _visOption;
        public Visibility VisOption
        {
            get => _visOption;
            set => SetProperty(ref _visOption, value);
        }
        public RelayCommand AcceptCommand { get; }
        public RelayCommand DeclineCommand { get; }

        public HolidaysApplicationViewModel(ManagmentShopViewModel managmentshopviewmodel)
        {
            AcceptCommand = new RelayCommand(AcceptClick);
            DeclineCommand = new RelayCommand(DeclineClick);
            DataAssigment(managmentshopviewmodel);
            UpdateData();
            VisOption = Visibility.Collapsed;
        }
        private void NewData()
        {
            DimTabContext dimnew = new();
            Context = dimnew;
        }
        public void UpdateData()
        {
            NewData();
            VisOption = Visibility.Collapsed;
            var query = from b in Context.Set<TblUnavailable>()
                        join p in Context.Set<TblPerson>() on b.PersonId equals p.PersonId
                        where ((b.Accepted == false) || (b.ToDelete == true))
                        orderby b.AbsentFrom ascending
                        select new { b.UnavailableId, p.Surname, p.Name, b.PersonId, AbsentFrom = b.AbsentFrom.ToShortDateString(), AbsentTo = b.AbsentTo.ToShortDateString(), b.DaysCount, b.Reason, DataOfSend = b.DataOfSend.ToShortDateString(), b.ToDelete, b.Accepted };
            ContextToDatagrid = query.ToList<object?>();
        }
        private async void AcceptClick()
        {
            //Get data about holiday
            string tempString = HolidaysListSelect.ToString().Replace("{", " ").Replace("}", " ").Trim();
            string tempStringWithoutSpaces = String.Concat(tempString.Where(c => !Char.IsWhiteSpace(c)));
            string[] strlist = tempStringWithoutSpaces.Split(",");
            int HolidayUnvID = Int32.Parse(strlist[0].Substring(strlist[0].IndexOf("=") + 1));
            int HolidayPersonID = Int32.Parse(strlist[3].Substring(strlist[3].IndexOf("=") + 1));
            DateTime HolidayAbsentFrom = DateTime.Parse(strlist[4].Substring(strlist[4].IndexOf("=") + 1));
            DateTime HolidayAbsentTo = DateTime.Parse(strlist[5].Substring(strlist[5].IndexOf("=") + 1));
            int HolidayDaysCout = Int32.Parse(strlist[6].Substring(strlist[6].IndexOf("=") + 1));
            string HolidayReason = strlist[7].Substring(strlist[7].IndexOf("=") + 1);
            DateTime HolidayAbsentSend = DateTime.Parse(strlist[8].Substring(strlist[8].IndexOf("=") + 1));
            bool HolidayToDelete = Boolean.Parse(strlist[9].Substring(strlist[9].IndexOf("=") + 1));
            if (HolidayToDelete == false)
            {
                //check reason, if 'urlop' check avaible days
                if (HolidayReason != "Urlop")
                {
                    TblUnavailable var = new()
                    {
                        UnavailableId = HolidayUnvID,
                        PersonId = HolidayPersonID,
                        AbsentFrom = HolidayAbsentFrom,
                        AbsentTo = HolidayAbsentTo,
                        DataOfSend = HolidayAbsentSend,
                        Accepted = true,
                        DaysCount = HolidayDaysCout,
                        Reason = HolidayReason,
                        ToDelete = false
                    };
                    Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First()).CurrentValues.SetValues(var);
                    Context.SaveChanges();
                    ManagmentShopViewModel.Update();
                    UpdateData();
                    BoundMessageQueue.Enqueue("Wniosek nadania wolnego zaakceptowany.");
                }
                else
                {
                    if ((HolidayAbsentFrom.Year != DateTime.Now.Year))
                    {
                        TblUnavailable var = new()
                        {
                            UnavailableId = HolidayUnvID,
                            PersonId = HolidayPersonID,
                            AbsentFrom = HolidayAbsentFrom,
                            AbsentTo = HolidayAbsentTo,
                            DataOfSend = HolidayAbsentSend,
                            Accepted = true,
                            DaysCount = HolidayDaysCout,
                            Reason = HolidayReason,
                            ToDelete = false
                        };
                        Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First()).CurrentValues.SetValues(var);
                        Context.SaveChanges();
                        ManagmentShopViewModel.Update();
                        UpdateData();
                        BoundMessageQueue.Enqueue("Wniosek nadania wolnego zaakceptowany.");
                    }
                    else
                    {
                        //check days of person
                        var tempPerson = Context.TblPersons.Where(d => d.PersonId == HolidayPersonID).First();
                        if (tempPerson.FreeDays < HolidayDaysCout)
                        {
                            BoundMessageQueue.Enqueue("Pracownik ma za mało urlopu.");
                        }
                        else
                        {
                            TblUnavailable var = new()
                            {
                                UnavailableId = HolidayUnvID,
                                PersonId = HolidayPersonID,
                                AbsentFrom = HolidayAbsentFrom,
                                AbsentTo = HolidayAbsentTo,
                                DataOfSend = HolidayAbsentSend,
                                Accepted = true,
                                DaysCount = HolidayDaysCout,
                                Reason = HolidayReason,
                                ToDelete = false
                            };
                            Context.Entry(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First()).CurrentValues.SetValues(var);
                            Context.SaveChanges();
                            TblPerson varPerson = new();
                            varPerson = Context.TblPersons.Where(d => d.PersonId == HolidayPersonID).First();
                            int tempDays = varPerson.FreeDays - HolidayDaysCout;
                            varPerson.FreeDays = tempDays;
                            Context.Entry(Context.TblPersons.Where(d => d.PersonId == HolidayPersonID).First()).CurrentValues.SetValues(varPerson);
                            Context.SaveChanges();
                            ManagmentShopViewModel.Update();
                            UpdateData();
                            BoundMessageQueue.Enqueue("Wniosek nadania wolnego zaakceptowany.");
                        }
                    }

                }
            }
            else
            {
                if (HolidayReason != "Urlop")
                {
                    Context.TblUnavailables.Remove(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First());
                    Context.SaveChanges();
                    ManagmentShopViewModel.Update();
                    UpdateData();
                    BoundMessageQueue.Enqueue("Wniosek usunięcia wolnego zaakceptowany.");
                }
                else
                {
                    if (HolidayAbsentFrom.Year != DateTime.Now.Year)
                    {
                        Context.TblUnavailables.Remove(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First());
                        Context.SaveChanges();
                        ManagmentShopViewModel.Update();
                        UpdateData();
                        BoundMessageQueue.Enqueue("Wniosek usunięcia wolnego zaakceptowany.");
                    }
                    else
                    {
                        TblPerson varPerson = new();
                        varPerson = Context.TblPersons.Where(d => d.PersonId == HolidayPersonID).First();
                        int tempDays = varPerson.FreeDays + HolidayDaysCout;
                        varPerson.FreeDays = tempDays;
                        Context.Entry(Context.TblPersons.Where(d => d.PersonId == HolidayPersonID).First()).CurrentValues.SetValues(varPerson);
                        Context.SaveChanges();
                        Context.TblUnavailables.Remove(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First());
                        Context.SaveChanges();
                        ManagmentShopViewModel.Update();
                        UpdateData();
                        BoundMessageQueue.Enqueue("Wniosek usunięcia wolnego zaakceptowany.");
                    }

                }
            }
        }
        private async void DeclineClick()
        {
            string tempString = HolidaysListSelect.ToString().Replace("{", " ").Replace("}", " ").Trim();
            string tempStringWithoutSpaces = String.Concat(tempString.Where(c => !Char.IsWhiteSpace(c)));
            string[] strlist = tempStringWithoutSpaces.Split(",");
            int HolidayUnvID = Int32.Parse(strlist[0].Substring(strlist[0].IndexOf("=") + 1));
            string texttomsg = String.Format("Czy napewno chcesz usunąć wniosek?");
            var viewModel = new MessageShowYesNoViewModel(texttomsg);
            var resultdialog = await MDIXDialogHost.Show(viewModel, "SecondDialogHostId");
            if ((bool)resultdialog)
            {
                Context.TblUnavailables.Remove(Context.TblUnavailables.Where(d => d.UnavailableId == HolidayUnvID).First());
                Context.SaveChanges();
                ManagmentShopViewModel.Update();
                UpdateData();
                BoundMessageQueue.Enqueue("Wniosek usnięty.");
            }
        }
    }
}
