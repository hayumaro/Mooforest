using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mooforest.Features.WorkHour {
    public partial class WorkHourView : UserControl {
        public ObservableCollection<Work> Works { get; set; } = [];
        public ObservableCollection<string> WorkCases { get; set; } = [];
        private readonly WorkHourModel Model;

        public WorkHourView() {
            InitializeComponent();
            Model = new WorkHourModel();
            DataContext = Model;
        }

        private void SaveButton_Click(object? sender, CancelEventArgs e) {
            try {
                Model.InsertWorks();
                SetStatusText($"レコードの登録に成功しました。");
            } catch (Exception ex) {
                SetErrorText($"Error: {ex.Message}");
            }
        }

        private void SetErrorText(string text) {
            StatusText.Foreground = (SolidColorBrush)Resources["EndButtonColor"];
            StatusText.Text = text;
        }

        private void SetStatusText(string text) {
            StatusText.Foreground = (SolidColorBrush)Resources["StatusTextColor"];
            StatusText.Text = text;
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e) {
            if (Model.IsWorking) {
                // End work
                try {
                    Model.EndWork();
                    SetStatusText($"{WorkContent.Text}を{Model.End:t}に終了しました");
            } catch (WorkTimeIsZeroException) {
                    SetErrorText("工数が0のため記録しません");
                } finally {
                    // Update UIs
                    CmbWorkCases.IsEnabled = true;
                    CmbWorkCases.SelectedIndex = -1;
                    RecordButton.Background = (SolidColorBrush)Resources["BeginButtonColor"];
                    RecordButton.Content = "▶";
                    WorkContent.IsReadOnly = false;
                    WorkContent.Clear();
                }
            } else {
                // Begin work
                if (WorkContent.Text == string.Empty) {
                    SetErrorText("作業内容を入力してください");
                    return;
                }
                if (CmbWorkCases.SelectedItem is not string workCase) {
                    SetErrorText("作業区分を入力してください");
                    return;
                }
                Model.BeginWork(WorkContent.Text, workCase);

                // Update UIs
                CmbWorkCases.IsEnabled = false;
                RecordButton.Background = (SolidColorBrush)Resources["EndButtonColor"];
                RecordButton.Content = "■";
                WorkContent.IsReadOnly = true;
                SetStatusText($"{WorkContent.Text}を{Model.Begin:t}に開始しました");
            }
        }

        private void ShortcutButtons_Click(object sender, RoutedEventArgs e) {
            if (sender is not Button button) {
                return;
            }
            if (Model.IsWorking) {
                SetErrorText("作業中のため開始できません");
                return;
            }
            WorkContent.Text = button.Content.ToString() switch {
                "事務" => "事務作業",
                "部会" => "部内会議",
                "PC" => "PCセットアップ",
                "点検" => "点検",
                "問い合わせ" => "問い合わせ対応",
            };
            var workCaseName = button.Content.ToString() switch {
                "事務" => "事務",
                "部会" => "打ち合わせ",
                "PC" => "ハード管理",
                "点検" => "定例",
                "問い合わせ" => "問い合わせ",
            };
            RecordButton_Click(sender, e);
        }
    }
}
