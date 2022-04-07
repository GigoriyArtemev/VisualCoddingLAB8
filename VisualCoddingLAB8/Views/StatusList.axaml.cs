using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using VisualCoddingLAB8.ViewModels;
using System;
namespace VisualCoddingLAB8.Views
{
    public partial class StatusList : UserControl
    {
        public StatusList()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void ClickButton_Load(object sender, RoutedEventArgs e)
        {
            var taskGetPath = new OpenFileDialog().ShowAsync((Window)this.Parent);
            string[]? path = taskGetPath.Result;
            var context = this.Parent.DataContext as MainWindowViewModel;
            if (path != null)
            {
                context.ReadDataFromFile(string.Join(@"/", path));
            }
            context.RefreshWindow();
        }
        private async void ClickButton_Save(object sender, RoutedEventArgs e)
        {
            var taskGetPath = new SaveFileDialog().ShowAsync((Window)this.Parent);
            try
            {
                string? path = await taskGetPath;
                var context = this.Parent.DataContext as MainWindowViewModel;
                if (path != null && path != "")
                {
                    context.WriteDataToFile(path);
                }
                context.RefreshWindow();
            }
            catch
            {
                return;
            }
        }
        private void ClickButton_About(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogWindow = new About();
                dialogWindow.ShowDialog((Window)this.VisualRoot);
            }
            catch
            {
                return;
            }
        }
        private void ClickButton_Exit(object sender, RoutedEventArgs e)
        {
            var s = this.Parent as MainWindow;
            s.Close();
        }
        private void ClickButton_AddToDo(object sender, RoutedEventArgs e) => MainWindowViewModel.AddToDo();
        private void ClickButton_AddDoing(object sender, RoutedEventArgs e) => MainWindowViewModel.AddDoing();
        private void ClickButton_AddDone(object sender, RoutedEventArgs e) => MainWindowViewModel.AddDone();
    }
}
