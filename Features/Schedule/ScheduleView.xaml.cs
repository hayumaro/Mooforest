using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
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

        private const int HalfHourHeight = 20;
        private const int HourHeight = HalfHourHeight * 2;
        private const int DayBeginHour = 7;
        private readonly int TotalTimelineHeight = TimeSlots.Count * HourHeight;

        public ScheduleView() {
            InitializeComponent();
            DataContext = this;
            TimeLineContainer.Height = TotalTimelineHeight;
            ScheduleDateLabel.Content = DateTime.Now.ToShortDateString();
            var schedules = new List<ScheduleItem> {
                new("会議", DateTime.Today.AddHours(9), DateTime.Today.AddHours(10.5)),
                new("開発作業", DateTime.Today.AddHours(11), DateTime.Today.AddHours(13)),
            };
            GridCanvas.Loaded += (s, e) => {
                DrawGridLines();
                RenderSchedules(schedules);
            };
        }

        void RenderSchedules(List<ScheduleItem> schedules) {
            ScheduleItemCanvas.Children.Clear();

            foreach (var item in schedules) {
                var top = (item.Begin.Hour - DayBeginHour) * HourHeight + item.Begin.Minute * HalfHourHeight;
                var height = (item.End - item.Begin).TotalMinutes * (2.0 / 3.0);
                var border = new Border {
                    Width = ScheduleItemCanvas.ActualWidth,
                    Height = height,
                    Background = (SolidColorBrush)FindResource("BrushGreen50"),
                    BorderThickness = new Thickness(1),
                    BorderBrush = (SolidColorBrush)FindResource("BrushGreen300"),
                    Opacity = 0.6,
                    Child = new TextBlock {
                        Text = item.Title,
                        Margin = new Thickness(5),
                        Foreground = Brushes.Black
                    }
                };
                Canvas.SetTop(border, top);
                Canvas.SetLeft(border, 0);
                ScheduleItemCanvas.Children.Add(border);
            }
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
