using System;
using System.Windows;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EML_Aggregator
{
    internal class DatabaseInterface
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=C:\\Sqlite\\EmailDB.sqlite; Version = 3;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return sqlite_conn;
        }
        public static void CreateTable(SQLiteConnection conn)
        {
            string sql = "CREATE TABLE IF NOT EXISTS Emails (EFrom TEXT, ETo TEXT, ESubject TEXT, EBody TEXT)";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }
        public static void InsertData(SQLiteConnection conn, IList<ParsedEmail> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                string sql = "INSERT INTO Emails (EFrom, ETo, ESubject, EBody) " +
                                         $"VALUES(\'{data[i].From}\', \'{data[i].To}\', \'{data[i].Subject}\', \'{data[i].Body}\');";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
            }
        }
        public static void ExtractData(SQLiteConnection conn, IList<ParsedEmail> extData)
        {
            string stm = "SELECT * FROM Emails";

            var cmd = new SQLiteCommand(stm, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ParsedEmail parsedEmail = new ParsedEmail
                {
                    To = rdr.GetString(0),
                    From = rdr.GetString(1),
                    Subject = rdr.GetString(2),
                    Body = rdr.GetString(3)
                };
                extData.Add(parsedEmail);
            }
        }
        public static void ClearDB(SQLiteConnection conn)
        {
            string sql = "DELETE FROM Emails";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();
        }
    }
}