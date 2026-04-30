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

        private void Close_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
