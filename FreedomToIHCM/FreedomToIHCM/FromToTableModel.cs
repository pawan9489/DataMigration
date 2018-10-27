using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FreedomToIHCM
{
    public class FromToTableModel : INotifyPropertyChanged
    {
        private static int count = 0;

        private static int incrementCount()
        {
            return count++;
        }
        public FromToTableModel()
        {
            incrementCount();
        }

        public static int decrementCount()
        {
            return count--;
        }

        public static int getCount()
        {
            return count;
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set {
                _id = value;
                OnNotifyPropertyChanged("Id");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _fromDatabase;

        public string FromDatabase
        {
            get { return _fromDatabase; }
            set { _fromDatabase = value; }
        }

        private string _toDatabase;

        public string ToDatabase
        {
            get { return _toDatabase; }
            set { _toDatabase = value; }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }


        public static ObservableCollection<FromToTableModel> GetData()
        {
            return new ObservableCollection<FromToTableModel> {
                new FromToTableModel() {
                    Id = getCount(),
                    FromDatabase = "",
                    ToDatabase = "",
                    Status = ""
                }
            };
        }
    }
}
