using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FreedomToIHCM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<FromToTableModel> ts;
        public MainWindow()
        {
            InitializeComponent();
            ts = FromToTableModel.GetData();
            dataGrid.ItemsSource = ts;
        }

        private void addRow(object sender, RoutedEventArgs e)
        {
            ts.Add(new FromToTableModel() {
                Id = FromToTableModel.getCount(),
                FromDatabase = "",
                ToDatabase = "",
                Status = ""
            });
        }

        private void startProcess(object sender, RoutedEventArgs e)
        {

        }

        private void removeRow(object sender, RoutedEventArgs e)
        {
            var row_id = (dataGrid.SelectedItem as FromToTableModel).Id;
            FromToTableModel rowToRemove = (from r in ts where r.Id == row_id select r).SingleOrDefault();
            ts.Remove(rowToRemove);
            FromToTableModel.decrementCount();
            var index = 1;
            foreach(var r in ts)
            {
                r.Id = index;
                index++;
            }
        }

    }
}
