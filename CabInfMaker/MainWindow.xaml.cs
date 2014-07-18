using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinForm = System.Windows.Forms;
using System.IO;
using System.Threading;

namespace CabInfMaker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private CabInfControl _control = new CabInfControl();
        private void applyDesFolder()
        {
            try
            {
                _control.DesFolder = desFolderTextBox.Text;
                desFilesListBox.Items.Clear();
                foreach (var fileItem in _control.FileList)
                {
                    desFilesListBox.Items.Add(fileItem.Name);
                }
            } catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            applyDesBuuton.IsEnabled = false;
        }
        private void setDesFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new WinForm.FolderBrowserDialog();
            var dlgResult = openFileDlg.ShowDialog();
            if (dlgResult == WinForm.DialogResult.OK)
            {
                desFolderTextBox.Text = openFileDlg.SelectedPath;
                applyDesFolder();
            }
        }

        private void preViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(desFolderTextBox.Text))
            {
                infContentTextBox.Text = "无效的目录值";
                return;
            }
            if (!Directory.Exists(desFolderTextBox.Text))
            {
                infContentTextBox.Text = "目录不存在";
                return;
            }
            preViewButton.IsEnabled = false;
            new Thread(()=>{
                String inf;
                try {
                    inf = _control.MakeInf();
                } catch (Exception err) {
                    inf = err.Message;
                }
                this.Dispatcher.Invoke(new Action(()=>{
                    infContentTextBox.Text = inf;
                    preViewButton.IsEnabled = true;
                }));
            }).Start();
        }

        private void makeInfButton_Click(object sender, RoutedEventArgs e)
        {
            String str = infContentTextBox.Text;
            if (String.IsNullOrEmpty(desFolderTextBox.Text))
            {
                MessageBox.Show("请先设置目标目录");
                return;
            }
            if (!Directory.Exists(desFolderTextBox.Text))
            {
                MessageBox.Show("无效的目录");
                return;
            }
            String desFolder = desFolderTextBox.Text;
            String desInfPath;
            if (desFolder[desFolder.Length - 1] == '\\')
            {
                desInfPath = desFolder + "cabInfo.inf";
            }
            else
            {
                desInfPath = desFolder + "\\cabInfo.inf";
            }
            if (String.IsNullOrEmpty(str))
            {
                _control.WriteToFile(desInfPath);
            }
            else
            {
                File.WriteAllText(desInfPath, str);
            }
        }

        private void applyDesBuuton_Click(object sender, RoutedEventArgs e)
        {
            applyDesFolder();

        }

        private void desFolderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            applyDesBuuton.IsEnabled = true;
        }

        private void desFilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (desFilesListBox.SelectedIndex == -1) return;
            var item = _control.FileList[desFilesListBox.SelectedIndex];
            filePathTextBox.Text = item.Name;
            if (!String.IsNullOrEmpty(item.Clsid))
            {
                clsidTextBox.Text = item.Clsid;
                clsidCheckBox.IsChecked = true;
            }
            else
            {
                clsidTextBox.Text = "";
                clsidCheckBox.IsChecked = false;
            }
            registerCheckBox.IsChecked = item.NeedRegister;
        }

        private void registerCheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            if (desFilesListBox.SelectedIndex == -1) return;
            var item = _control.FileList[desFilesListBox.SelectedIndex];
            item.NeedRegister = registerCheckBox.IsChecked.Value;
        }

        private void clsidCheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            if (desFilesListBox.SelectedIndex == -1) return;
            var item = _control.FileList[desFilesListBox.SelectedIndex];
            if (clsidCheckBox.IsChecked.HasValue && clsidCheckBox.IsChecked.Value)
            {
                if (CabInfControl.IsGuid(clsidTextBox.Text))
                {
                    item.Clsid = clsidTextBox.Text;
                }
                else
                {
                    MessageBox.Show("无效的GUID值");
                    clsidCheckBox.IsChecked = false;
                }
            }
            else
            {
                item.Clsid = null;
            }
        }

        private void runIExpressButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExpress.exe");
        }
    }
}
