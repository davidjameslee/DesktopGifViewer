using Microsoft.Win32;
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

namespace DesktopGifViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double maxHeight = SystemParameters.PrimaryScreenHeight;
        double maxWidth = SystemParameters.PrimaryScreenWidth;
        double minHeight = 100;
        double minWidth = 100;

        double upScale = 1.25;
        double downScale = 0.8;

        public MainWindow()
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.PrimaryScreenHeight;
            this.MaxWidth = SystemParameters.PrimaryScreenWidth;
            this.SizeToContent = SizeToContent.WidthAndHeight;

            ImageSource placeholderSource = WpfAnimatedGif.ImageBehavior.GetAnimatedSource(gif);
            gif.Height = placeholderSource.Height;
            gif.Width = placeholderSource.Width;
        }

        //Menu Handlers
        private void Menu_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                // Check filesize is under 5mb
                long filesize = new FileInfo(dialog.FileName).Length;
                if (filesize < 5000000)
                {
                    LoadGif(dialog.FileName);
                }
            }
        }

        private void Menu_Flip_Click(object sender, RoutedEventArgs e)
        {
            if (DockPanel.GetDock(menu) == Dock.Top)
            {
                DockPanel.SetDock(menu, Dock.Bottom);
            }
            else
            {
                DockPanel.SetDock(menu, Dock.Top);
            }
        }

        private void Menu_ScaleUp_Click(object sender, RoutedEventArgs e)
        {
            ScaleGif(upScale);
        }

        private void Menu_ScaleDown_Click(object sender, RoutedEventArgs e)
        {
            ScaleGif(downScale);
        }

        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Image Handlers
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Show/Hide menu
            if (e.ClickCount == 2)
            {
                menu.Visibility = menu.IsVisible ? Visibility.Collapsed : Visibility.Visible;
            }
            // Move Window
            else
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
        }

        //Private Helper Functions
        private void LoadGif(String source)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(source);
            image.EndInit();

            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(gif, image);
            gif.Height = image.PixelHeight;
            gif.Width = image.PixelWidth;

            if (image.PixelHeight < minHeight || image.PixelWidth < minWidth)
            {
                double scaleFactor = Math.Max(image.PixelHeight / maxHeight, image.PixelWidth / maxWidth);
                ScaleGif(scaleFactor);
            }
            if (image.PixelHeight > maxHeight || image.PixelWidth > maxWidth)
            {
                double scaleFactor = Math.Min(image.PixelHeight / maxHeight, image.PixelWidth / maxWidth);
                ScaleGif(scaleFactor);
            }
        }

        private bool ScaleGif(double scaleFactor)
        {
            double newHeight = scaleFactor * gif.Height;
            double newWidth = scaleFactor * gif.Width;
            if (newHeight > minHeight && newHeight < maxHeight &&
                newWidth > minWidth && newWidth < maxWidth)
            {
                gif.Height = newHeight;
                gif.Width = newWidth;
                return true;
            }
            return false;
        }
    }
}
