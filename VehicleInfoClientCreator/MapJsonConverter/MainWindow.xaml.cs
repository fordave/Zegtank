using Com.Zegtank.MapFileOperation.Model;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MapJsonConverter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
        {
            InitialDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName,
            Filter = "xml|*.xml"
        };

        private readonly IMapConverter converter = new XmlToJsonMapConverter();

        private IList<MapCoordinateModel> mapCoordinateModels;

        private string _exportPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == load)
            {
                xmlpath.Clear();
                var result = openFileDialog.ShowDialog();
                mapxml.Clear();
                if (result.Value)
                {
                    mapxml.Text = converter.MapReaderToString(openFileDialog.FileName);
                    mapCoordinateModels=converter.MapReader(openFileDialog.FileName);
                    xmlpath.Text = openFileDialog.FileName;
                }
            }
            else if (e.OriginalSource == convert)
            {
                mapjson.Clear();
                if (mapCoordinateModels==null)
                {
                    return;
                }
                _exportPath = Path.ChangeExtension(openFileDialog.FileName, "json");
                converter.MapWriter(mapCoordinateModels, _exportPath);
                mapjson.Text= converter.MapReaderToString(_exportPath);
            }
        }
    }
}
