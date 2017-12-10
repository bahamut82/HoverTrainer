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

namespace HoverTrainer
{
    /// <summary>
    /// Interaction logic for FeedbackItem.xaml
    /// </summary>
    public partial class FeedbackItem : UserControl
    {
        private int value;
        public int Value {
            get { return value; }
            set {
                this.value = value;

                switch (value) {
                    case 1: rect.Fill = Brushes.Green; break;
                    case 2: rect.Fill = Brushes.Yellow; break;
                    case 3: rect.Fill = Brushes.Orange; break;
                    default: rect.Fill = Brushes.Red; break;
                }

                if (value >= 0 && value < 10)
                    label.Content = value;
                else
                    label.Content = "X";
            }
        }

        public FeedbackItem(long x)
        {
            InitializeComponent();
            Value = (int)x;
        }
    }
}
