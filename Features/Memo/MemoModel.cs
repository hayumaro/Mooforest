using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace Mooforest.Features.Memo {
    public record Memo(int Id, string Title);
    public class MemoModel {
        public static ObservableCollection<Memo> Memos { get; private set; } = [];
        public static int CurrentId { get; set; } = -1;

        private static string FilePath { get; } = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Mooforest",
            "Memo.db");
        private static readonly string DataSource = $"Data Source = {FilePath}";

        public static void Initialize() {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            CreateTable(con);
            LoadMemos(con);
        }

        public static string LoadContent(int memoId) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Select Content From Memos Where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", memoId);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            return reader.GetString(0);
        }

        public static void Save(string title, string content) {
            if (CurrentId == -1) {
                SaveNew(title, content);
            }
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Update Memos Set Title=@Title, Content=@Content Where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Content", content);
            cmd.Parameters.AddWithValue("@Id", CurrentId);
            cmd.ExecuteNonQuery();
            LoadMemos(con);
        }

        public static void SaveNew(string title, string content) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Insert Into Memos (Title, Content) Values(@Title, @Content)", con);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Content", content);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT last_insert_rowid();";
            CurrentId = (int)cmd.ExecuteScalar()!;
            LoadMemos(con);
        }

        private static void LoadMemos(SqliteConnection con) {
            Memos.Clear();
            using var cmd = new SqliteCommand(@"Select Id, Title From Memos", con);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                var id = reader.GetInt32(0);
                var title = reader.GetString(1);
                Memos.Add(new Memo(id, title));
            }
        }

        private static void CreateTable(SqliteConnection con) {
            // Memos
            {
                using var cmd = new SqliteCommand(@"Create Table If Not Exists
                ""Memos"" (
	                ""Id""	INTEGER,
	                ""Title""	TEXT NOT NULL,
	                ""Content""	TEXT NOT NULL,
	                PRIMARY KEY(""Id"")
                );", con);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
