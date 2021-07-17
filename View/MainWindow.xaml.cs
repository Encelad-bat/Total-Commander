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
using Total_Commander.Model;
using Total_Commander.ViewModel;

namespace Total_Commander
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //==========================================================================================================================
        #region =======================================================VARIABLES============================================================

        private readonly string temp_directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Total Commander by Ncelad";
        private string buffer_file_path;

        private readonly string[] drives = Environment.GetLogicalDrives();
        private readonly string[] forbidden_directories = {"Boot","Documents and Settings","Recovery","System Volume Information", "Config.Msi"};
        private readonly string[] forbidden_files = {"bootmgr","BOOTNXT","BOOTSECT.BAK"};

        private uint success_check_counter = 0;
        #endregion
        //==========================================================================================================================
        #region ====================================================MAIN CONSTRUCTOR========================================================

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new IO_Object_ViewModel();

            if (drives.Length == 1)
            {
                GetObjects(drives[0], 1,false);
            }
            else
            {
                GetObjects(drives[0], 1,false);
                GetObjects(drives[1], 2,false);
                (this.DataContext as IO_Object_ViewModel).First_Selected_Drive = drives[0];
                (this.DataContext as IO_Object_ViewModel).Second_Selected_Drive = drives[1];
            }

            this.First_Window_TextBox.KeyDown += First_Window_TextBox_KeyDown;
            this.Second_Window_TextBox.KeyDown += Second_Window_TextBox_KeyDown;

            this.First_Window_ListView.MouseDoubleClick += First_Window_ListView_MouseDoubleClick;
            this.Second_Window_ListView.MouseDoubleClick += Second_Window_ListView_MouseDoubleClick;

            Directory.CreateDirectory(this.temp_directory);
        }

        #endregion
        //==========================================================================================================================
        #region ========================================================EVENTS==============================================================

        private void First_Window_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string path = (sender as TextBox).Text;
                try
                {
                    GetObjects(path, 1,true);
                }
                catch (Exception ex)
                {
                    (sender as TextBox).Text = this.First_Window_ComboBox.SelectedValue.ToString();
                    MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Second_Window_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string path = (sender as TextBox).Text;
                try
                {
                    GetObjects(path, 2,true);
                }
                catch(Exception ex)
                {
                    (sender as TextBox).Text = this.Second_Window_ComboBox.SelectedValue.ToString();
                    MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void First_Window_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(((sender as ListView).SelectedItem as IO_Object).Object.ToString() != ".."){
                    this.First_Window_TextBox.Text = ((sender as ListView).SelectedItem as IO_Object).Object.FullName;
                    GetObjects(((sender as ListView).SelectedItem as IO_Object).Object.FullName, 1, true);
                }
                else
                {
                    string[] prev_path = this.First_Window_TextBox.Text.Split('\\');
                    if (prev_path.Length > 2)
                    {
                        this.First_Window_TextBox.Text = this.First_Window_TextBox.Text.Replace('\\' + prev_path[prev_path.Length - 1], "");
                        GetObjects(this.First_Window_TextBox.Text, 1, true);
                    }
                    else
                    {
                        this.First_Window_TextBox.Text = this.First_Window_TextBox.Text.Replace(prev_path[prev_path.Length - 1], "");
                        GetObjects(this.First_Window_TextBox.Text, 1, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Second_Window_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (((sender as ListView).SelectedItem as IO_Object).Object.ToString() != "..")
                {
                    this.Second_Window_TextBox.Text = ((sender as ListView).SelectedItem as IO_Object).Object.FullName;
                    GetObjects(((sender as ListView).SelectedItem as IO_Object).Object.FullName, 2, true);
                }
                else
                {
                    string[] prev_path = this.Second_Window_TextBox.Text.Split('\\');
                    if (prev_path.Length > 2)
                    {
                        this.Second_Window_TextBox.Text = this.Second_Window_TextBox.Text.Replace('\\' + prev_path[prev_path.Length - 1], "");
                        GetObjects(this.Second_Window_TextBox.Text, 2, true);
                    }
                    else
                    {
                        this.Second_Window_TextBox.Text = this.Second_Window_TextBox.Text.Replace(prev_path[prev_path.Length - 1], "");
                        GetObjects(this.Second_Window_TextBox.Text, 2, false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cut_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.First_Window_ListView.SelectedItem != null)
                {
                    var file_path = (this.First_Window_ListView.SelectedItem as IO_Object).Path;
                    var file_fullname = file_path.Split('\\');
                    this.buffer_file_path = this.temp_directory + $"\\{file_fullname[file_fullname.Length - 1]}";
                    File.Copy(file_path, this.buffer_file_path);
                    File.Delete(file_path);


                    var update_path = this.First_Window_TextBox.Text;
                    if (update_path.Length > 1)
                    {
                        GetObjects(update_path, 1, true);
                    }
                    else
                    {
                        GetObjects(update_path, 1, false);
                    }
                }
                else if (this.Second_Window_ListView.SelectedItem != null)
                {
                    var file_path = (this.Second_Window_ListView.SelectedItem as IO_Object).Path;
                    var file_fullname = file_path.Split('\\');
                    this.buffer_file_path = this.temp_directory + $"\\{file_fullname[file_fullname.Length - 1]}";
                    File.Copy(file_path, this.buffer_file_path);
                    File.Delete(file_path);


                    var update_path = this.Second_Window_TextBox.Text;
                    if (update_path.Length > 2)
                    {
                        GetObjects(update_path, 2, true);
                    }
                    else
                    {
                        GetObjects(update_path, 2, false);
                    }
                }
                else
                {
                    MessageBox.Show("Nothing to cut!", "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void copy_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.First_Window_ListView.SelectedItem != null)
                {
                    var file_path = (this.First_Window_ListView.SelectedItem as IO_Object).Path;
                    var file_fullname = file_path.Split('\\');
                    this.buffer_file_path = this.temp_directory + $"\\{file_fullname[file_fullname.Length - 1]}";
                    File.Copy(file_path, this.buffer_file_path);


                    var update_path = this.First_Window_TextBox.Text;
                    if (update_path.Length > 1)
                    {
                        GetObjects(update_path, 1, true);
                    }
                    else
                    {
                        GetObjects(update_path, 1, false);
                    }
                }
                else if (this.Second_Window_ListView.SelectedItem != null)
                {
                    var file_path = (this.Second_Window_ListView.SelectedItem as IO_Object).Path;
                    var file_fullname = file_path.Split('\\');
                    this.buffer_file_path = this.temp_directory + $"\\{file_fullname[file_fullname.Length - 1]}";
                    File.Copy(file_path, this.buffer_file_path);


                    var update_path = this.Second_Window_TextBox.Text;
                    if (update_path.Length > 2)
                    {
                        GetObjects(update_path, 2, true);
                    }
                    else
                    {
                        GetObjects(update_path, 2, false);
                    }
                }
                else
                {
                    MessageBox.Show("Nothing to copy!", "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void paste_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.First_Window_ListView.SelectedItem != null)
                {
                    string[] buffer_file_fullname = this.buffer_file_path.Split('\\');
                    File.Copy(this.buffer_file_path, this.First_Window_TextBox.Text + buffer_file_fullname[buffer_file_fullname.Length-1]);


                    var update_path = this.First_Window_TextBox.Text;
                    if (update_path.Length > 1)
                    {
                        GetObjects(update_path, 1, true);
                    }
                    else
                    {
                        GetObjects(update_path, 1, false);
                    }
                }
                else if (this.Second_Window_ListView.SelectedItem != null)
                {
                    string[] buffer_file_fullname = this.buffer_file_path.Split('\\');
                    File.Copy(this.buffer_file_path, this.Second_Window_TextBox.Text + buffer_file_fullname[buffer_file_fullname.Length - 1]);


                    var update_path = this.Second_Window_TextBox.Text;
                    if (update_path.Length > 1)
                    {
                        GetObjects(update_path, 2, true);
                    }
                    else
                    {
                        GetObjects(update_path, 2, false);
                    }
                }
                else
                {
                    MessageBox.Show("Nowhere to paste!", "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Operation failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        //==========================================================================================================================
        #region =====================================================MAIN LOAD FUNC=========================================================

        public void GetObjects(string path, int collection_num, bool is_inner)
        {
            var viewmodel = (this.DataContext as IO_Object_ViewModel);

            //=============================================================
            #region =================FIRST WINDOW OBJECTS GETTING==================
            if (collection_num == 1)
            {
                viewmodel.First_Window_Items.Clear();
                if (is_inner)
                {
                    viewmodel.First_Window_Items.Add(IO_Object.Inner_Directory);
                }

                //=============================================================

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

                //=============================================================

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
            #endregion
            //=============================================================
            #region =================SECOND WINDOW OBJECTS GETTING=================
            else if (collection_num == 2)
            {
                viewmodel.Second_Window_Items.Clear();
                if (is_inner)
                {
                    viewmodel.Second_Window_Items.Add(IO_Object.Inner_Directory);
                }

                //=============================================================

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

                //=============================================================

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
            #endregion
            //=============================================================
        }

        #endregion
        //==========================================================================================================================
    }
}
