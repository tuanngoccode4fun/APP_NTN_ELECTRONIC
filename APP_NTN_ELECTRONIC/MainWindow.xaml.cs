using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using Emgu.Util.TypeEnum;
using System;
using System.Collections.Generic;
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
using System.Drawing;
using System.IO;

namespace APP_NTN_ELECTRONIC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetFullView8_8(byte[] input)
        {
            int row = 0;
            int col = 0;
            StackPanelImage.Children.Clear();
            for (row = 0; row < 8; row++)
            {
                StackPanel stackPanelRow = new StackPanel();
                stackPanelRow.Orientation = Orientation.Horizontal;
                for (col = 0; col < 8; col++)
                {
                    byte blue = input[(col + row*8) * 3 + 0];
                    byte green = input[(col + row*8) * 3 + 1];
                    byte red = input[(col + row*8) * 3 + 2];
                    SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(red,green, blue));
                    Button button = new Button();
                    button.Width = 50;
                    button.Height = 50;
                    button.Background = brush;
                    stackPanelRow.Children.Add(button);
                }
                StackPanelImage.Children.Add(stackPanelRow);
            }
        }
        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
        private void bt_load_image_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                Image<Bgr, byte> originalImage = new Image<Bgr, byte>(imagePath);

                // Kích thước mới (chiều rộng và chiều cao) cho hình ảnh resize
                int newWidth = 8;
                int newHeight = 8;
                // Resize hình ảnh
                Image<Bgr, byte> resizedImage = originalImage.Resize(newWidth, newHeight, Emgu.CV.CvEnum.Inter.Linear);
                byte[] imageData = resizedImage.Bytes;
                imageControl.Source = ConvertBitmapToBitmapSource( resizedImage.ToBitmap()) ;
                GetFullView8_8(imageData);
            }
        }
    }
}
