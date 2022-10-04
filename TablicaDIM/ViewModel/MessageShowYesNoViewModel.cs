using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel
{
    internal class MessageShowYesNoViewModel : ViewModelBase
    {
        public DialogSession DialogSession { get; }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        public RelayCommand CloseCommand { get; }
        public RelayCommand CloseTrueCommand { get; }

        public MessageShowYesNoViewModel(string text)
        {
            CloseCommand = new RelayCommand(Close);
            CloseTrueCommand = new RelayCommand(CloseTrue);
            Text = text;
        }
        public MessageShowYesNoViewModel(string text, DialogSession dialogSession)
        {
            CloseCommand = new RelayCommand(Close);
            CloseTrueCommand = new RelayCommand(CloseTrue);
            Text = text;
            DialogSession = dialogSession ?? throw new ArgumentNullException(nameof(dialogSession));
        }
        private async void Close()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }
        private async void CloseTrue()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }
    }
}
