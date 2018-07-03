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
using System.Windows.Shapes;

namespace Microsoft.CognitiveServices.SpeechRecognition
{
    /// <summary>
    /// Interaction logic for Subtitles.xaml
    /// </summary>
    public partial class Subtitles : Window
    {
        double detectWidthChanges;
        Boolean initiallySized = false;
        public Subtitles()
        {
            InitializeComponent();

            //Center screen
            this.Top = 0;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double windowWidth = this.Width;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            detectWidthChanges = this.ActualWidth;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void showShadow(object sender, MouseEventArgs e)
        {
            this.opacityBrush.Opacity = 0.75;
        }

        private void hideShadow(object sender, MouseEventArgs e)
        {
            this.opacityBrush.Opacity = 0;
        }

        private void allowFormToMove(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                this.DragMove();
        }

        private void resizeText(object sender, MouseWheelEventArgs e)
        {
            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return;
            double lastWidth = this.ActualWidth;
            textA.FontSize += e.Delta > 0 ? 1 : -1;
            textB.FontSize += e.Delta > 0 ? 1 : -1;
        }

        public static double PointsToPixels(double points)
        {
            return points * (96.0 / 72.0);
        }

        private void recenterElement(object sender, SizeChangedEventArgs e)
        {
            // if (initiallySized)
            //     this.Left += (e.PreviousSize.Width - e.NewSize.Width) + 1;
            // else initiallySized = true;
        }

        //Re-center automatically
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            //Calculate half of the offset to move the form


            if (initiallySized)
            {
                //if (sizeInfo.HeightChanged)
                //    this.Top += (sizeInfo.PreviousSize.Height - sizeInfo.NewSize.Height) / 2;

                if (sizeInfo.WidthChanged)
                    this.Left += (sizeInfo.PreviousSize.Width - sizeInfo.NewSize.Width) / 2;
            }
            else initiallySized = true;

        }

        public void updateText(String text)
        {
            textA.Text = text;
            textB.Text = text;
        }
    }
}

