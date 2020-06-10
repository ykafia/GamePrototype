using Stride.Engine;

namespace FPSgame.Windows
{
    class FPSgameApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
