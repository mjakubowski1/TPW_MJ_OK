using System.Numerics;

namespace Data
{
    public interface BallInterface : IDisposable
    {
        int Id { get; }
        int Radius { get; }


        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }

        int Mass { get; }

        event EventHandler? BallChanged;
    }
}
