using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Total_Commander.Model;
using Total_Commander;
using System.Windows;

namespace Total_Commander.ViewModel
{
    class IO_Object_ViewModel : INotifyPropertyChanged
    {
        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_


        private ObservableCollection<IO_Object> first_window_items = new ObservableCollection<IO_Object>();

        public ObservableCollection<IO_Object> First_Window_Items { get { return this.first_window_items; } set { this.first_window_items = value; OnPropertyChanged("First_Window_Items"); } }


        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_


        private ObservableCollection<IO_Object> second_window_items = new ObservableCollection<IO_Object>();

        public ObservableCollection<IO_Object> Second_Window_Items { get { return this.second_window_items; } set { this.second_window_items = value; OnPropertyChanged("Second_Window_Items"); } }


        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_



        private ObservableCollection<string> drives = new ObservableCollection<string>(Environment.GetLogicalDrives());

        public ObservableCollection<string> Drives { get { return drives; } set { drives = value; OnPropertyChanged("Drives"); } }


        private string first_selected_drive;

        public string First_Selected_Drive
        {
            get { return first_selected_drive; }
            set {
                OnPropertyChanged("First_Selected_Drive");
                try
                {
                    (App.Current.MainWindow as MainWindow).GetObjects(value,1,false);
                    first_selected_drive = value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }; 
            }
        }

        private string second_selected_drive;

        public string Second_Selected_Drive
        {
            get { return second_selected_drive; }
            set {
                OnPropertyChanged("Second_Selected_Drive");
                try
                {
                    (App.Current.MainWindow as MainWindow).GetObjects(value, 2,false);
                    second_selected_drive = value;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        //-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_
    }
}
