using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Total_Commander.ViewModel;

namespace Total_Commander
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string[] drives = Environment.GetLogicalDrives();
        private readonly string[] forbidden_directories = {"Boot","Documents and Settings","Recovery","System Volume Information"};
        private readonly string[] forbidden_files = {"bootmgr","BOOTNXT","BOOTSECT.BAK"};

        private uint success_check_counter = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new IO_Object_ViewModel();

            if (drives.Length == 1)
            {
                GetObjects(drives[0], 1);
            }
            else
            {
                GetObjects(drives[0], 1);
                GetObjects(drives[1], 2);
            }
        }

        public void GetObjects(string path, int collection_num)
        {
            var viewmodel = (this.DataContext as IO_Object_ViewModel);
            if (collection_num == 1)
            {
                foreach (var item in Directory.GetDirectories(path))
                {
                    var obj = new Model.IO_Object(item);
                    if(obj.Object.Name[0] != '$')
                    {
                        foreach (var dir in forbidden_directories)
                        {
                            if (obj.Object.Name != dir)
                            {
                                success_check_counter++;
                            }
                        }
                        if(success_check_counter == forbidden_directories.Length)
                        {
                            viewmodel.First_Window_Items.Add(obj);
                        }
                        success_check_counter = 0;
                    }
                }
                foreach (var item in Directory.GetFiles(path))
                {
                    var obj = new Model.IO_Object(item);
                    if (!obj.Object.Name.Contains(".sys") && !obj.Object.Name.Contains(".tmp") && !obj.Object.Name.Contains(".log"))
                    {
                        foreach (var fil in forbidden_files)
                        {
                            if (obj.Object.Name != fil)
                            {
                                success_check_counter++;
                            }
                        }
                        if (success_check_counter == forbidden_files.Length)
                        {
                            viewmodel.First_Window_Items.Add(obj);
                        }
                        success_check_counter = 0;
                    }
                }
            }
            else if(collection_num == 2)
            {
                foreach (var item in Directory.GetDirectories(path))
                {
                    var obj = new Model.IO_Object(item);
                    if (obj.Object.Name[0] != '$')
                    {
                        foreach (var dir in forbidden_directories)
                        {
                            if (obj.Object.Name != dir)
                            {
                                success_check_counter++;
                            }
                        }
                        if (success_check_counter == forbidden_directories.Length)
                        {
                            viewmodel.Second_Window_Items.Add(obj);
                        }
                        success_check_counter = 0;
                    }
                }
                foreach (var item in Directory.GetFiles(path))
                {
                    var obj = new Model.IO_Object(item);
                    if (!obj.Object.Name.Contains(".sys") && !obj.Object.Name.Contains(".tmp") && !obj.Object.Name.Contains(".log"))
                    {
                        foreach (var fil in forbidden_files)
                        {
                            if (obj.Object.Name != fil)
                            {
                                success_check_counter++;
                            }
                        }
                        if (success_check_counter == forbidden_files.Length)
                        {
                            viewmodel.Second_Window_Items.Add(obj);
                        }
                        success_check_counter = 0;
                    }
                }
            }
        }
    }
}
