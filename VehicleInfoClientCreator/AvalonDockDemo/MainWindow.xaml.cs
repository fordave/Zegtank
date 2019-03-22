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
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace AvalonDockDemo
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var writer = new StreamWriter("AvalonDockSavedFile.txt"))
            {
                var layoutSerializer = new XmlLayoutSerializer(_dockingManager);
                layoutSerializer.Serialize(writer);
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            using (var reader = new StreamReader("AvalonDockSavedFile.txt"))
            {
                var layoutSerializer = new XmlLayoutSerializer(_dockingManager);
                layoutSerializer.Deserialize(reader);
            }
        }
    }
}
