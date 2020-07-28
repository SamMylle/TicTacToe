using System;

namespace TicTacToe {
    static class Program {
        static void Main() {
            while (Game.GameLoop())
                Console.Clear();
        }
    }
}