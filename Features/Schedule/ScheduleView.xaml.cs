using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mooforest.Features.Schedule {
    public record ScheduleItem(string Title, DateTime Begin, DateTime End);

    public partial class ScheduleView : UserControl {
        public static List<string> TimeSlots { get; } = [
            "7:00",
            "8:00",
            "9:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00",
        ];

        const int HalfHourHeight = 20;
        const int HourHeight = HalfHourHeight * 2;
        const int DayBeginHour = 7;
        readonly int TotalTimelineHeight = TimeSlots.Count * HourHeight;
        public List<ScheduleItem> Schedules = [];

        public ScheduleView() {
            InitializeComponent();
            DataContext = this;
            TimeLineContainer.Height = TotalTimelineHeight;
            ScheduleDateLabel.Content = DateTime.Now.ToShortDateString();
            Schedules.Add(new("会議", DateTime.Today.AddHours(9), DateTime.Today.AddHours(10.5)));
            Schedules.Add(new("開発作業", DateTime.Today.AddHours(11), DateTime.Today.AddHours(13)));
            GridCanvas.Loaded += (s, e) => {
                DrawGridLines();
                RenderSchedules();
            };
        }

        void RenderSchedules() {
            ScheduleItemCanvas.Children.Clear();
            foreach (var item in Schedules) {
                var button = new Button {
                    Width = ScheduleItemCanvas.ActualWidth,
                    Height = (int)((item.End - item.Begin).TotalMinutes * (2.0 / 3.0)),
                    Style = (Style)FindResource("ScheduleItemButton"),
                    Tag = item,
                    Content = item.Title,
                };
                Canvas.SetTop(button, (item.Begin.Hour - DayBeginHour) * HourHeight + item.Begin.Minute * HalfHourHeight);
                Canvas.SetLeft(button, 0);
                ScheduleItemCanvas.Children.Add(button);
            }
        }

        void ScheduleItemOnClick(object sender, RoutedEventArgs e) {
        }

        void DrawGridLines() {
            var totalHeight = TimeSlots.Count * HourHeight;
            for (int y = 0; y <= totalHeight; y += HalfHourHeight) {
                var line = new Line {
                    X1 = 0,
                    X2 = GridCanvas.ActualWidth,
                    Y1 = y,
                    Y2 = y,
                };
                if (y % HourHeight == 0) {
                    line.Stroke = (SolidColorBrush)FindResource("BrushGray200");
                    line.StrokeThickness = 1.5;
                } else {
                    line.Stroke = (SolidColorBrush)FindResource("BrushGray100");
                    line.StrokeThickness = 1;
                }
                GridCanvas.Children.Add(line);
            }
        }
    }
}
