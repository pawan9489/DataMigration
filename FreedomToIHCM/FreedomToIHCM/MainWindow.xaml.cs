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

            // Read XML on Load
            Table.FromXML("S:\\Projects\\DataMigration\\DatabaseMetadata\\DatabaseMetadata\\bin\\Debug\\tableParents.xml");
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
            // Make Sure that all from and To Databases are Filled and then Run the Process
            var canStartProcess = true;
            foreach(var row in ts)
            {
                if(row.FromDatabase == "" || row.ToDatabase == "")
                {
                    canStartProcess = false;
                }
            }
            if(! canStartProcess)
            {
                MessageBox.Show("Fill out the Missing Database names in the Table");
            } else
            {
                RunProcess.Run(ts.ToList());
            }
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
