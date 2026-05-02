using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace Mooforest.Features.Memo {
    public record Memo(int Id, string Title);
    public class MemoModel {
        private static string FilePath { get; } = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Mooforest",
            "Memo.db");
        private static readonly string DataSource = $"Data Source = {FilePath}";

        public static void Initialize() {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            CreateTable(con);
        }

        public static string GetContent(int memoId) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Select Content From Memos Where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", memoId);
            using var reader = cmd.ExecuteReader();
            reader.Read();
            return reader.GetString(0);
        }

        public static void GetMemos(ObservableCollection<Memo> memos) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            GetMemos(memos, con);
        }

        private static void GetMemos(ObservableCollection<Memo> memos, SqliteConnection con) {
            using var cmd = new SqliteCommand(@"Select Id, Title From Memos", con);
            using var reader = cmd.ExecuteReader();
            memos.Clear();
            while (reader.Read()) {
                var id = reader.GetInt32(0);
                var title = reader.GetString(1);
                memos.Add(new Memo(id, title));
            }
        }

        public static void Save(int id, string title, string content) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Update Memos Set Title=@Title, Content=@Content Where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Content", content);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public static int SaveNew(string title, string content) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Insert Into Memos (Title, Content) Values(@Title, @Content)", con);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Content", content);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT last_insert_rowid();";
            return (int)cmd.ExecuteScalar()!;
        }

        private static void CreateTable(SqliteConnection con) {
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
