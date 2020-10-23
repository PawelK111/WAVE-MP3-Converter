using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;

namespace MP3_WAVEConverter
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// False - If WAVE -> MP3 is NOW!
        /// </summary>
        private static bool CheckConverter;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        List<string> filters = new List<string>()
            {
                "Plik MP3 (*.mp3)|*.mp3;",
                "Plik WAV (*.wav)|*.wav;"
            };

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void ConverterBtn_Click(object sender, RoutedEventArgs e)
        {
            Convert().Wait(100);
        }

        private void StreamBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConverter)
            {
                converterStream("MP3 -> WAVE", true, false);
                return;
            }
            converterStream("WAVE -> MP3", false, true);
        }

        private async Task Convert()
        {
            try
            {
                switch (CheckConverter)
                {
                    case false:
                        openFileDialog.Filter = filters[1];
                        if (openFileDialog.ShowDialog() == false) return;

                        saveFileDialog.Filter = filters[0];
                        if (saveFileDialog.ShowDialog() == false) return;
                        var bitRate = int.Parse(QualityBox.Text);
                        await FileConverter.ToMP3(openFileDialog.FileName, saveFileDialog.FileName, bitRate);
                        break;

                    case true:
                        openFileDialog.Filter = filters[0];
                        if (openFileDialog.ShowDialog() == false) return;

                        saveFileDialog.Filter = filters[1];
                        if (saveFileDialog.ShowDialog() == false) return;
                        await FileConverter.ToWave(openFileDialog.FileName, saveFileDialog.FileName);
                        break;
                }
                endStatus("SUKCES", "FROM: " + openFileDialog.SafeFileName, "TO: " + saveFileDialog.SafeFileName, new SolidColorBrush(Colors.LimeGreen));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                endStatus("PORAŻKA!", "", "", new SolidColorBrush(Colors.Red));
            }
        }

        private void endStatus(string status, string streamIn, string streamOut, SolidColorBrush sCB)
        {
            StatusLabel.Content = status;
            StreamINLabel.Content = streamIn;
            StreamOUTLabel.Content = streamOut;
            StatusLabel.Foreground = sCB;
        }

        private void converterStream(string btn, bool checkConv, bool qualityBox)
        {
            if (StatusLabel.Content.ToString() != "")
            {
                endStatus("", "", "", null);
            }
            StreamBtn.Content = btn;
            CheckConverter = checkConv;
            QualityBox.IsEnabled = qualityBox;
        }
    }
}
