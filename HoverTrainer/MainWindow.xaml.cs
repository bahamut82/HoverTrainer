using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HoverTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int STACKSIZE = 20;

        private Timer t;
        private Controller c;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            c = new Controller();

            t = new Timer
            {
                Interval = (int)1000 / 60
            };
            t.Elapsed += Tick;
            t.Start();
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            var inp = c.GetCurrentInput();
            inputLabel.Dispatcher.Invoke(new Action(() => inputLabel.Content = inp));

            long frames = c.FramesDashReleased();
            if (frames == 0)
                return;
            stack.Dispatcher.Invoke(
                new Action(() => {
                    if (stack.Children.Count > STACKSIZE) {
                        stack.Children.RemoveAt(0);
                    }
                    stack.Children.Add(new FeedbackItem(frames));
                }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t.Stop();
            t.Close();
        }
    }
}
