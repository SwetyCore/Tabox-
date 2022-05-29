using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tabox__.Converters
{
    class IconGetter : IValueConverter
    {
        private ImageSource ToBitmapSourceA(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                //Bitmap img = System.Drawing.Icon.ExtractAssociatedIcon(value.ToString()).ToBitmap();

                return new ImageBrush(ToBitmapSourceA(IconHelper.GetIcon(value.ToString())));
                //return new 
            }
            catch (Exception ex)
            {
                if (value == null)
                {
                    return new ImageBrush();

                }
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    if (File.Exists(value.ToString()))
                    {

                        //Bitmap img = System.Drawing.Icon.ExtractAssociatedIcon(value.ToString()).ToBitmap();
                        System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(value.ToString());
                        var ico = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new System.Windows.Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                        return new ImageBrush(ico);

                    }
                }
                //throw;
                return new ImageBrush();

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
