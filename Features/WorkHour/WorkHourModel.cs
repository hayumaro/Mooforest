using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Mooforest.Features.WorkHour {
    public record Work(string Content, DateTime Begin, DateTime End, TimeSpan Hour, string Case);

    [Serializable()]
    public class WorkTimeIsZeroException : Exception {
        public WorkTimeIsZeroException()
            : base() {
        }
    }

    internal class WorkHourModel {
        public DateTime Begin;
        public DateTime End;

        public ObservableCollection<Work> Works { get; } = [];
        public ObservableCollection<string> WorkCases { get; } = [];
        public bool IsWorking { get; private set; } = false;
        public string WorkCase { get; private set; } = "";
        public string WorkContent { get; private set; } = "";
        
        public WorkHourModel() {
            ReadWorkCases();
        }

        public void InsertWorks() {
        }

        public void ReadWorkCases() {
            // kari code
            WorkCases.Add("打ち合わせ");
            WorkCases.Add("実装");
            WorkCases.Add("テスト");
        }

        // 作業を開始する。
        // 作業開始時刻を決定する。15分未満は切り捨てる。秒は不要なため取り除く。
        // ex: 8:44 => 8:30
        public void BeginWork(string workContent, string workCase) {
            WorkContent = workContent;
            WorkCase = workCase;
            var now = DateTime.Now;
            var minute = (now.Minute / 15) * 15;
            Begin = now.Date.AddHours(now.Hour).AddMinutes(minute);
            if (Works.Count > 0 && Begin < End) {
                Begin = End;
            }
            IsWorking = true;
        }

        // 作業を終了する。
        // 作業終了時刻を決定する。15分未満は切りあげる。秒は不要なため取り除く。
        // ex: 8:31 => 8:45
        public void EndWork() {
            var now = DateTime.Now;
            var rawEnd = now.AddMinutes(Math.Round(Math.Ceiling(now.Minute / 15.0)) * 15 - now.Minute);
            End = rawEnd.Date.AddHours(rawEnd.Hour).AddMinutes(rawEnd.Minute);
            IsWorking = false;
            if (Begin.Hour == End.Hour && Begin.Minute == End.Minute) {
                throw new WorkTimeIsZeroException();
            }
            Works.Add(new Work(WorkContent, Begin, End, WorkHour(), WorkCase));
        }

        private TimeSpan WorkHour() {
            return End - Begin;
        }
    }
}
