using System.Collections.ObjectModel;
using System.Windows;

namespace Mooforest.Features.IssueManagement
{
    public partial class IssueDetailView : Window {
        public Issue OwnIssue { get; private set; }

        public IssueDetailView(Issue issue, ObservableCollection<History> histories) {
            InitializeComponent();
            OwnIssue = issue;
            UpdateUI();
            DataContext = this;
        }

        private void UpdateUI() {
            Title = OwnIssue.Title;
            TextTitle.Text = OwnIssue.Title;
            TextCategory.Text = OwnIssue.Category;
            TextStatus.Text = OwnIssue.Status;
            TextDescription.Text = OwnIssue.Description;
            TextCreatedAt.Text = $"{OwnIssue.CreatedAt:yyyy-MM-dd}";
            TextUpdatedAt.Text = $"{OwnIssue.UpdatedAt:yyyy-MM-dd}";
            TextToDo.Text = OwnIssue.ToDo;
            DataGridHistories.ItemsSource = IssueManagementModel.LoadHistories(OwnIssue.Id);
        }

        private void Reload() {
            OwnIssue = IssueManagementModel.GetIssue(OwnIssue.Id)!;
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ImInstance?.Reload();
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            var view = new EditIssueView(OwnIssue) {
                Owner = this
            };
            view.ShowDialog();
            Reload();
            UpdateUI();
        }

        private void AddHistory_Click(object sender, RoutedEventArgs e) {
            var view = new AddHistoryView(OwnIssue) {
                Owner = this
            };
            view.ShowDialog();
            Reload();
            UpdateUI();
        }
    }
}
