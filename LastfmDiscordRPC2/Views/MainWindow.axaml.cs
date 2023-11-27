using System;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Media;
using LastfmDiscordRPC2.Models;
using LastfmDiscordRPC2.ViewModels;
using LastfmDiscordRPC2.ViewModels.Panes;

namespace LastfmDiscordRPC2.Views;

public partial class MainWindow : Window
{
    private static readonly Color WindowsBackground = Color.FromArgb(0xD0, 0x00, 0x00, 0x00);
    private static readonly Color UnixBackground = Color.FromRgb(0x00, 0x00, 0x00);
    
    public MainWindow(MainViewModel dataContext)
    {
        InitializeComponent();
        
        DataContext = dataContext;
        Background = Utilities.OS == OSPlatform.Windows ? 
            new SolidColorBrush(WindowsBackground) : new SolidColorBrush(UnixBackground);
        
        foreach (IPaneViewModel viewModel in dataContext.Children)
        {
            Type viewModelType = viewModel.GetType();
            string elementName = viewModelType.ToString().Split('.').Last().Replace("ViewModel", "");

            Control? control = this.FindControl<Control>(elementName);
            if (control != null)
            {
                control.DataContext = viewModel;
            }
        }
    }
}