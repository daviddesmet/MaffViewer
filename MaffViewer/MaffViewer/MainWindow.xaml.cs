namespace MaffViewer
{
    using System.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool _defaultWindowStarted;

        public MainWindow()
        {
            InitializeComponent();

            if (!_defaultWindowStarted)
                DataContext = new MainViewModel();//MainViewModel.Initialize();

            _defaultWindowStarted = true;
        }
    }
}
