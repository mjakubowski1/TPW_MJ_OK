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
    public class Logger : IDisposable
    {

        private Task loggerTask;
        private StreamWriter streamWriter;
        //BlockingCollection<BallDataToSerialize> _queue;
        BlockingCollection<LogData> _queue;
        private string path = Directory.GetCurrentDirectory();

        public Logger()
        {
            this.loggerTask = Task.Run(WriteToFile);
            _queue = new BlockingCollection<LogData>();
        }


        private void WriteToFile()
        {

            string currentTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"balls_{currentTime}.json";
            string filePath = Path.Combine(path, fileName);

            while (true)
            {
                using (streamWriter = new StreamWriter(filePath, append: false))
                {


                    foreach (LogData b in _queue.GetConsumingEnumerable())
                    {
                        string ballLog = b.Serialize();
                        streamWriter.Write("\n" + ballLog);
                        streamWriter.Write("\n");
                    }
                    streamWriter.Write("\n");
                    streamWriter.Flush();

                }
                Thread.Sleep(1000);
            }

        }

        //public void AddBallToQueue(BallInterface ball)
        //{
        //    if (ball == null)
        //    {
        //        return;
        //    }

        //    BallDataToSerialize ballToAdd = new BallDataToSerialize(ball.Position, ball.Mass, ball.Velocity, ball.Radius, ball.Id);
        //    if (!_queue.IsAddingCompleted)
        //    {
        //        _queue.Add(ballToAdd);
        //    }

        //  }

        public void AddLogDataToQueue(LogData logData)
        {
            if (!_queue.IsAddingCompleted)
            {
                _queue.Add(logData);
            }
        }

        public void Dispose()
        {
            _queue.CompleteAdding();
            loggerTask.Wait();
            loggerTask.Dispose();
        }

    }

        public abstract class LogData
        {
            public string Time { get; private set; }

            protected LogData()
            {
                Time = GetPolishLocalTime();
            }

            private string GetPolishLocalTime()
            {
                TimeZoneInfo polandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, polandTimeZone).ToString("yyyy-MM-dd HH:mm:ss");
            }

           
            public virtual string Serialize()
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                return JsonSerializer.Serialize(this, options);
            }
        }

        internal class BallDataToSerialize : LogData
        {
            public BallDataToSerialize(Vector2 Position, int mass, Vector2 Velocity, int radius, int iD)
                : base()
            {
                X = Position.X;
                Y = Position.Y;
                Mass = mass;
                VelX = Velocity.X;
                VelY = Velocity.Y;
                Radius = radius;
                ID = iD;
            }

            public float X { get; set; }
            public float Y { get; set; }
            public int Mass { get; set; }
            public float VelX { get; set; }
            public float VelY { get; set; }
            public int Radius { get; set; }
            public int ID { get; set; }

        public override string Serialize()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            return JsonSerializer.Serialize(this, options);
        }
    }

        public class CollisionDataToSerialize : LogData
        {
            public CollisionDataToSerialize(int firstBallID, int secondBallID)
                : base()
            {
                FirstBallID = firstBallID;
                SecondBallID = secondBallID;
            }

            public int FirstBallID { get; set; }
            public int SecondBallID { get; set; }

        public override string Serialize()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            return JsonSerializer.Serialize(this, options);
        }

    }



    }

