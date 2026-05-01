using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Mooforest.Features.IssueManagement {
	public record Status(int Id, string Name, int SortOrder, bool IsClosed);
    public record Issue(int Id, string Title, string Description, int StatusId, string Status, DateTime CreatedAt, DateTime UpdatedAt, int? ParentId, string? ToDo);
	public record History(int Id, int IssueId, DateTime CreatedAt, int StatusId, string Status, string Description);

	public class IssueManagementModel {
		// 状態一覧のリスト。マスタのキャッシュ。
        public static ObservableCollection<Status> Statuses { get; private set; } = [];

		// 現在画面に表示しているIssueのリスト。画面を更新するたびに読み込みなおす。
		public static ObservableCollection<Issue> Issues { get; private set; } = [];

        public static void Initialize() {
			using var con = new SqliteConnection(DataSource);
			con.Open();
			CreateTable(con);
			LoadStatuses(con);
        }

		// Select * from Issues
        public static void LoadIssues() {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            LoadIssues(con);
        }

        public static ObservableCollection<History> LoadHistories(int issueId) {
            var histories = new ObservableCollection<History>();
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Select Id, CreatedAt, StatusId, Description From Histories Where IssueId=@IssueId", con);
            cmd.Parameters.AddWithValue("@IssueId", issueId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                var id = reader.GetInt32(0);
                var createdAt = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var statusId = reader.GetInt32(2);
                var status = Statuses.FirstOrDefault(x => x.Id == statusId)!.Name;
                var desc = reader.GetString(3);
                histories.Add(new History(id, issueId, createdAt, statusId, status, desc));
            }
            return histories;
        }

        public static void InsertIssue(string title, string description, string toDo) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Insert Into Issues (Title, Description, StatusId, CreatedAt, UpdatedAt, ToDo)
				Values(@Title, @Description, 1, @CreatedAt, @UpdatedAt, @ToDo)", con);
			cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@ToDo", toDo);
            cmd.ExecuteNonQuery();
            LoadIssues(con);
        }

        public static void InsertHistory(int issueId, int statusId, string description) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Insert Into Histories (IssueId, CreatedAt, Description, StatusId)
				Values(@IssueId, @CreatedAt, @Description, @StatusId)", con);
			cmd.Parameters.AddWithValue("@IssueId", issueId);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@StatusId", statusId);
            cmd.ExecuteNonQuery();
        }

		public static void UpdateToDo(int issueId, string toDo) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
            using var cmd = new SqliteCommand(@"Update Issues Set ToDo=@ToDo, UpdatedAt=@UpdatedAt Where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", issueId);
            cmd.Parameters.AddWithValue("@ToDo", toDo);
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
            LoadIssues(con);
        }

		public static void UpdateIssue(int issueId, string title, string description, int statusId, string toDo) {
            using var con = new SqliteConnection(DataSource);
            con.Open();
			using var cmd = new SqliteCommand(@"Update Issues
				Set Title=@Title, Description=@Description, StatusId=@StatusId, UpdatedAt=@UpdatedAt, ToDo=@ToDo
				Where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@StatusId", statusId);
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@ToDo", toDo);
            cmd.Parameters.AddWithValue("@Id", issueId);
            cmd.ExecuteNonQuery();
            LoadIssues(con);
        }

        private static string FilePath { get; } = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Mooforest",
            "IssueManagement.db");
        private static readonly string DataSource = $"Data Source = {FilePath}";

        private static void CreateTable(SqliteConnection con) {
			// Statuses
			{
				using var cmd = new SqliteCommand(@"Create Table If Not Exists
                ""Statuses"" (
	                ""Id""	INTEGER,
	                ""Name""	TEXT NOT NULL UNIQUE,
	                ""SortOrder""	INTEGER NOT NULL UNIQUE,
	                ""IsClosed""	INTEGER NOT NULL,
	                PRIMARY KEY(""Id"")
                );", con);
				cmd.ExecuteNonQuery();
			}

			// Issues
			{
				using var cmd = new SqliteCommand(@"Create Table If Not Exists
                ""Issues"" (
	                ""Id""	INTEGER,
	                ""Title""	TEXT NOT NULL,
	                ""Description""	TEXT NOT NULL,
	                ""StatusId""	INTEGER NOT NULL,
	                ""CreatedAt""	TEXT NOT NULL CHECK(""CreatedAt"" GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
	                ""UpdatedAt""	TEXT NOT NULL CHECK(""UpdatedAt"" GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
	                ""ParentId""	INTEGER,
	                ""ToDo""	TEXT,
	                PRIMARY KEY(""Id""),
	                FOREIGN KEY(""ParentId"") REFERENCES ""Issues""(""Id""),
	                FOREIGN KEY(""StatusId"") REFERENCES ""Statuses""(""Id"")
                );", con);
				cmd.ExecuteNonQuery();
			}

			// Histories
			{
				using var cmd = new SqliteCommand(@"Create Table If Not Exists
				""Histories"" (
					""Id""	INTEGER,
					""IssueId""	INTEGER NOT NULL,
					""CreatedAt""	TEXT NOT NULL CHECK(""CreatedAt"" GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]'),
					""Description""	TEXT NOT NULL,
					""StatusId""	INTEGER NOT NULL,
					PRIMARY KEY(""Id""),
					FOREIGN KEY(""IssueId"") REFERENCES ""Issues""(""Id""),
					FOREIGN KEY(""StatusId"") REFERENCES ""Statuses""(""Id"")
				);", con);
				cmd.ExecuteNonQuery();
			}
		}

        private static void LoadStatuses(SqliteConnection con) {
			Statuses.Clear();
			using var cmd = new SqliteCommand(@"Select Id, Name, SortOrder, IsClosed From Statuses", con);
			using var reader = cmd.ExecuteReader();
			while (reader.Read()) {
				var id = reader.GetInt32(0);
				var name = reader.GetString(1);
				var sortOrder = reader.GetInt32(2);
				var isClosed = reader.GetInt32(3) == 1;
                Statuses.Add(new Status(id, name, sortOrder, isClosed));
			}
		}

        private static void LoadIssues(SqliteConnection con) {
			Issues.Clear();
			using var cmd = new SqliteCommand(@"Select Id, Title, Description, StatusId, CreatedAt, UpdatedAt, ParentId, Todo From Issues", con);
			using var reader = cmd.ExecuteReader();
			while (reader.Read()) {
                var id = reader.GetInt32(0);
                var title = reader.GetString(1);
				var desc = reader.GetString(2);
				var statusId = reader.GetInt32(3);
				var status = Statuses.FirstOrDefault(x => x.Id == statusId)!.Name;
				var createdAt = DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var updatedAt = DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture);
				int? parentId = reader.IsDBNull(6) ? null : reader.GetInt32(6);
				string? toDo = reader.IsDBNull(7) ? null : reader.GetString(7);
                Issues.Add(new Issue(id, title, desc, statusId, status, createdAt, updatedAt, parentId, toDo));
            }
        }
    }
}
