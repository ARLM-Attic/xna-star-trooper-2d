using System;

namespace TestProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (StarTrooperGame game = new StarTrooperGame())
            {
                game.Run();
            }
        }
    }
}

