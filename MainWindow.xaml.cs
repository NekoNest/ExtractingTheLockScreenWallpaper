using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ExtraTheLockScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AssetsDir =
            "C:\\Users\\%s\\AppData\\Local\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\\LocalState\\Assets";

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!CheckDirectory(TextBox.Text)) return;
            if (BeginExtra(TextBox.Text))
            {
                MessageBox.Show("提取成功！");
            }
        }

        private bool CheckDirectory(string path)
        {
            Console.WriteLine("用户的路径是：" + path);
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("路径不能为空");
                return false;
            }
            
            if (Directory.Exists(TextBox.Text)) return true;
            Console.WriteLine("文件夹不存在");
            new AlertWindow.Builder()
                .SetMessage("文件夹不存在，需要创建吗？")
                .SetPositiveButton("好的", (o, args, window) =>
                {
                    try
                    {
                        Directory.CreateDirectory(TextBox.Text);
                        if (BeginExtra(TextBox.Text))
                        {
                            MessageBox.Show("提取成功！");
                        }
                        window.Close();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("文件夹创建失败");
                        Console.WriteLine(exception.StackTrace);
                        MessageBox.Show("文件夹创建失败");
                        window.Close();
                    }
                })
                .SetNegativeButton("不行", OnDirNotExistsAlertCancel)
                .Create()
                .Show();
            return false;
        }

        private bool BeginExtra(string path)
        {
            var userName = Environment.UserName;
            var wallpaperDirPath = AssetsDir.Replace("%s", userName);
            var wallpaperDir = new DirectoryInfo(wallpaperDirPath);
            var wallpapers = wallpaperDir.GetFiles();
            try
            {
                foreach (var wallpaper in wallpapers)
                {
                    CopyFile(wallpaper, path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show("提取时出现错误，可能是文件已存在或文件名冲突\n会生成的文件名类似：\"2020-09-18 20-19-29.356.jpg\"\n请检查文件目录下是否有类似文件名的文件");
                return false;
            }

            return true;
        }

        private void CopyFile(FileInfo file, string targetDir)
        {
            var modifiedTime = file.LastWriteTime.ToString("yyyy-MM-dd HH-mm-ss.fff") + ".jpg"; 
            var targetFile = targetDir + "\\" + modifiedTime; 
            file.CopyTo(targetFile);
        }
        
        private void OnDirNotExistsAlertCancel(object sender, RoutedEventArgs e, AlertWindow window)
        {
            Console.WriteLine("用户取消了");
            window.Close();
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CopyToClipBoard(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText("https://github.com/NekoNest/ExtractingTheLockScreenWallpaper");
            MessageBox.Show("已复制到剪贴板");
        }
    }
}