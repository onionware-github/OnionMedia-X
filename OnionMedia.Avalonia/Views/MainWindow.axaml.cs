using Avalonia.Controls;
using OnionMedia.Avalonia.ViewModels;

namespace OnionMedia.Avalonia.Views
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
#if DEBUG
        bool Debug => true;
#else
        bool Debug => false;
#endif
    }
}