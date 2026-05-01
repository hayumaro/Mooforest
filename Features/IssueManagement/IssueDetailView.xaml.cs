using System.Collections.ObjectModel;
using System.Windows;

namespace Mooforest.Features.IssueManagement
{
    public partial class IssueDetailView : Window {
        public ObservableCollection<History> Histories { get; private set; } = [];
        public Issue OwnIssue { get; private set; }

        public IssueDetailView(Issue issue, ObservableCollection<History> histories) {
            InitializeComponent();
            Histories = histories;
            OwnIssue = issue;
            DataContext = this;
        }

        private void Reload() {
            OwnIssue = IssueManagementModel.GetIssue(OwnIssue.Id)!;
            Histories = IssueManagementModel.LoadHistories(OwnIssue.Id);
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            var view = new EditIssueView(OwnIssue) {
                Owner = this
            };
            view.ShowDialog();
            Reload();
        }

        private void AddHistory_Click(object sender, RoutedEventArgs e) {
            var view = new AddHistoryView(OwnIssue) {
                Owner = this
            };
            view.ShowDialog();
            Reload();
        }
    }
}
