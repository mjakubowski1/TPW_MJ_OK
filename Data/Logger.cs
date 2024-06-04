using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data
{
    internal class Logger : IDisposable
    {

        private Task loggerTask;
        private StreamWriter streamWriter;
        BlockingCollection<BallDataToSerialize> _queue;
        private string path = Directory.GetCurrentDirectory();

        public Logger()
        {
            this.loggerTask = Task.Run(WriteToFile);
            _queue = new BlockingCollection<BallDataToSerialize>();
        }


        private void WriteToFile()
        {

            using (streamWriter = new StreamWriter(path + "/balls.json", append: false))
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                foreach (BallDataToSerialize b in _queue.GetConsumingEnumerable())
                {
                    string ballLog = JsonSerializer.Serialize(b, options);
                    streamWriter.Write("\n" + ballLog);
                    streamWriter.Write("\n");
                }
                streamWriter.Write("\n");
                streamWriter.Flush();

            }

        }

        public void AddBallToQueue(BallInterface ball)
        {
            if (ball == null)
            {
                return;
            }

            BallDataToSerialize ballToAdd = new BallDataToSerialize(ball.Position, ball.Mass, ball.Velocity, ball.Radius, ball.Id);
            if (!_queue.IsAddingCompleted)
            {
                _queue.Add(ballToAdd);
            }

        }


        public void Dispose()
        {
            _queue.CompleteAdding();
            loggerTask.Wait();
            loggerTask.Dispose();
        }

        internal class BallDataToSerialize
        {
            public BallDataToSerialize(Vector2 Position, int mass, Vector2 Velocity, int radius, int iD)
            {
                X = Position.X;
                Y = Position.Y;
                Mass = mass;
                VelX = Velocity.X;
                VelY = Velocity.Y;
                Radius = radius;
                ID = iD;
                Time = GetPolishLocalTime();
            }

            public float X { get; set; }
            public float Y { get; set; }
            public int Mass { get; set; }
            public float VelX { get; set; }
            public float VelY { get; set; }
            public int Radius { get; set; }
            public int ID { get; set; }
            public string Time { get; set; }
            private string GetPolishLocalTime()
            {
                TimeZoneInfo polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, polandTimeZone).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


    }
}
