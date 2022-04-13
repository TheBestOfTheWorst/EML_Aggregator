using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace EML_Aggregator
{
    internal class EMLParser
    {
        public ObservableCollection<ParsedEmail> Emails { get; private set; } = new ObservableCollection<ParsedEmail>();

        public ObservableCollection<ParsedEmail> LoadFiles(String dir)
        {
            OpenFileDialog fileDlg = new OpenFileDialog()
            {
                FileName = "",
                InitialDirectory = dir,
                Filter = "E-mail files (*.eml)|*.eml",
                Title = "Choose e-mail files",
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true,
                ReadOnlyChecked = true
            };
            //если нажали открыть в диалоге
            if (fileDlg.ShowDialog().Value)
            {
                foreach (string file in fileDlg.FileNames)
                {
                    FileInfo fi = new FileInfo(file);

                    var loadedEmail = LoadEmlFromFile(fi.FullName);

                    ParsedEmail parsedEmail = new ParsedEmail
                    {
                        From = loadedEmail.From.Replace("\'", "").Replace("\"", ""),
                        To = loadedEmail.To.Replace("\'", "").Replace("\"", ""),
                        Subject = loadedEmail.Subject.Replace("\'", "").Replace("\"", ""),
                        Body = loadedEmail.TextBody.Replace("\'", "").Replace("\"", "")
                    };
                    Emails.Add(parsedEmail);
                }
            }
            return Emails;
        }
        private CDO.Message LoadEmlFromFile(String emlFileName)
        {
            CDO.Message msg = new CDO.MessageClass();
            ADODB.Stream stream = new ADODB.StreamClass();

            try
            {
                stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, String.Empty, String.Empty);
                stream.LoadFromFile(emlFileName);
                stream.Flush();
                msg.DataSource.OpenObject(stream, "_Stream");
                msg.DataSource.Save();

                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error {ex.Message} parsing email at " + emlFileName, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return msg;
        }
    }
}
