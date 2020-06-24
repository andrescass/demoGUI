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
using System.Windows.Threading;

namespace demoGUI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private bool launching;
        private List<Led> leds;
        private List<ComboBox> timeList;
        private List<ComboBox> colorList;
        private List<ComboBox> patternList;

        // Save and load variables
        String destFileText;
        String fileText;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            launching = false;

            leds = new List<Led>();
            for(int i = 0; i < 10; i++)
            {
                leds.Add(new Led());
            }

            initLists();
        }

        private void initLists()
        {
            timeList = new List<ComboBox>();
            colorList = new List<ComboBox>();
            patternList = new List<ComboBox>();

            timeList.Add(timeBox0);
            timeList.Add(timeBox1);
            timeList.Add(timeBox2);
            timeList.Add(timeBox3);
            timeList.Add(timeBox4);
            timeList.Add(timeBox5);
            timeList.Add(timeBox6);
            timeList.Add(timeBox7);
            timeList.Add(timeBox8);
            timeList.Add(timeBox9);

            colorList.Add(ColorBox0);
            colorList.Add(ColorBox1);
            colorList.Add(ColorBox2);
            colorList.Add(ColorBox3);
            colorList.Add(ColorBox4);
            colorList.Add(ColorBox5);
            colorList.Add(ColorBox6);
            colorList.Add(ColorBox7);
            colorList.Add(ColorBox8);
            colorList.Add(ColorBox9);

            patternList.Add(patternBox0);
            patternList.Add(patternBox1);
            patternList.Add(patternBox2);
            patternList.Add(patternBox3);
            patternList.Add(patternBox4);
            patternList.Add(patternBox5);
            patternList.Add(patternBox6);
            patternList.Add(patternBox7);
            patternList.Add(patternBox8);
            patternList.Add(patternBox9);

            for(int i = 0; i<10; i++)
            {
                foreach(String it in Led.colors)
                {
                    colorList[i].Items.Add(it);
                }

                foreach (String it in Led.times)
                {
                    timeList[i].Items.Add(it);
                }

                foreach (String it in Led.patterns)
                {
                    patternList[i].Items.Add(it);
                }

                colorList[i].SelectedItem = colorList[i].Items.GetItemAt(0);
                timeList[i].SelectedItem = timeList[i].Items.GetItemAt(0);
                patternList[i].SelectedItem = patternList[i].Items.GetItemAt(0);
            }
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(launching)
            {
                uploadBut.Background = Brushes.Green;
                launching = false;
            }
            else
            {
                uploadBut.Background = Brushes.LightGray;
                timer.Stop();
            }
            
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void uploadBut_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            launching = true;
        }

        private void PatternBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /* Save Configuration */
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Led config file (*.led)|*.led";
            saveDialog.FilterIndex = 1;

            bool? userClickOk = saveDialog.ShowDialog();

            if (userClickOk == true)
            {
                destFileText = saveDialog.FileName;
                SaveConfig();
            }
        }

        void SaveConfig()
        {
            System.IO.StreamWriter outFile;
            string path = destFileText;

            try
            {
                outFile = File.CreateText(path);
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Can't access to the selected folder", "Autentication error");
                return;
            }
            
            //Header
            outFile.WriteLine("* Led configuration file");
            outFile.WriteLine("* You can comment lines by starting it with an *");
            outFile.WriteLine("* Each line represents a led");
            outFile.WriteLine("* Each line is in the form of time_color_pattern");
            outFile.WriteLine("* Only the first ten valid lines are used");
            outFile.WriteLine(" ");

            // Leds: time_color_pattern
            for (int i = 0; i < colorList.Count(); i++)
            {
                string ledColor = colorList[i].SelectedItem.ToString();
                string ledTime = timeList[i].SelectedItem.ToString();
                string ledPat = patternList[i].SelectedItem.ToString();

                outFile.WriteLine(ledTime + "_" + ledColor + "_" + ledPat);
            }

            outFile.Close();

            MessageBox.Show("Config file created", "Succes");

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Led config file (*.led)|*.led";
            openDialog.FilterIndex = 1;

            openDialog.Multiselect = false;

            bool? userClickOk = openDialog.ShowDialog();

            if (userClickOk == true)
            {
                fileText = openDialog.FileName;
                LoadConfig();
            }
        }

        private void LoadConfig()
        {
            StreamReader inTestFile;
            inTestFile = File.OpenText(fileText);
            List <Led> loadLed = new List<Led>();
            int ledIdx = 0;
            string ledData;
            string[] splitLed;
            
            for (int i = 0; i < 10; i++)
            {
                loadLed.Add(new Led());
            }

            string lineRead = inTestFile.ReadLine();
            while (lineRead != null && ledIdx < 10)
            {
                while ((lineRead[0] == '*') || String.IsNullOrWhiteSpace(lineRead)) // Not comment or white line
                {
                    lineRead = inTestFile.ReadLine();
                }

                // data line
                ledData = lineRead;
                splitLed = ledData.Split('_');
                if(splitLed.Count() != 3)
                {
                    MessageBox.Show("Config file read error. Process terminated.", "Error");
                    inTestFile.Close();
                    return;
                }

                timeList[ledIdx].SelectedItem = splitLed[0];
                colorList[ledIdx].SelectedItem = splitLed[1];
                patternList[ledIdx].SelectedItem = splitLed[2];
                ledIdx++;
                lineRead = inTestFile.ReadLine();

            }

            MessageBox.Show("Configuration ended.", "Success");
            inTestFile.Close();
            return;
        }
    }
}
