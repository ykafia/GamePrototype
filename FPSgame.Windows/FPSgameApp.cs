using Stride.Engine;

namespace FPSgame.Windows
{
    class FPSgameApp
    {
        static void Main(string[] _)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
