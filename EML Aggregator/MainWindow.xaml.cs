using System.Windows;

namespace EML_Aggregator
{
    public partial class MainWindow : Window
    {
        private readonly EMLParser emlParser = new EMLParser();

        public MainWindow()
        {
            InitializeComponent();          
            DataContext = emlParser;
        }

        private void OpenDialog_Click(object sender, RoutedEventArgs e)
        {   
            emlParser.LoadFiles("C:\\Users\\Andrew\\source\\repos\\EML Aggregator\\EML samples");
            
            var connect = DatabaseInterface.CreateConnection();
            DatabaseInterface.CreateTable(connect);
            DatabaseInterface.InsertData(connect, emlParser.Emails);
        }

        private void DisplayDatabase_Click(object sender, RoutedEventArgs e)
        {
            var connect = DatabaseInterface.CreateConnection();
            emlParser.Emails.Clear();
            DatabaseInterface.ExtractData(connect, emlParser.Emails);
        }

        private void ClearDB_Click(object sender, RoutedEventArgs e)
        {
            var connect = DatabaseInterface.CreateConnection();
            emlParser.Emails.Clear();
            DatabaseInterface.ClearDB(connect);
        }
    }
}
