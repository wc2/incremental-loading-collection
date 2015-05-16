using UniversalWindowsPlatformToolkit.IncrementalLoadingCollection;
using Windows.UI.Xaml.Controls;

namespace IncrementalLoadingCollection.SampleApp
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            var dataSource = new GeneratingDataSource();
            var collection = new IncrementalLoadingCollection<int>(dataSource);

            ListView.ItemsSource = collection;
        }
    }
}
