using System;

namespace GameOfLife {
    class Program {
        static void Main(string[] args) {
            try {
                new Game().Start();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }

            Console.WriteLine("\nDone. Press any key to exit.");
            Console.ReadLine();
        }
    }
}
