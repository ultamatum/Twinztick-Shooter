using System;

namespace TwinztickShooter
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TwinztickShooter())
                game.Run();
        }
    }
#endif
}
