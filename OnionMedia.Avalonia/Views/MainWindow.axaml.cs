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
        public bool Debug => true;
#else
        public bool Debug => false;
#endif
    }
}