using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using TablicaDIM.OtherClasses;


namespace TablicaDIM.ViewModel
{
    internal class MessageShowOkViewModel : ViewModelBase
    {
        public DialogSession DialogSession { get; }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        public RelayCommand CloseTrueCommand { get; }
        public MessageShowOkViewModel(string text)
        {
            CloseTrueCommand = new RelayCommand(CloseTrue);
            Text = text;
        }
        public MessageShowOkViewModel(string text, DialogSession dialogSession)
        {
            CloseTrueCommand = new RelayCommand(CloseTrue);
            Text = text;
            DialogSession = dialogSession ?? throw new ArgumentNullException(nameof(dialogSession));
        }
        private async void CloseTrue()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

    }
}
