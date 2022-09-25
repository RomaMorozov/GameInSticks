using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInSticks
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(20, Player.Computer);
            game.ComputerPlaying += Game_ComputerPlaying;
            game.ManTurnToMove += Game_ManTurnToMove;
            game.EndOfGame += Game_EndOfGame;
            game.Start();

            Console.ReadLine();
        }

        private static void Game_EndOfGame(Player player)
        {
            Console.WriteLine($"Победитель: {player}.");
        }

        private static void Game_ManTurnToMove(object sender, int e)
        {
            Console.WriteLine($"Осталось палок {e}.");
            Console.Write("Ваш ход, возьмите определенное кол-во палок: ");

            bool correctNumber = false;
            while (!correctNumber)
            {
                if (int.TryParse(Console.ReadLine(), out int takenSticks))
                {
                    var game = sender as Game;

                    try
                    {
                        game.ManTake(takenSticks);
                        correctNumber = true;
                    }
                    catch (ArgumentException ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private static void Game_ComputerPlaying(int obj)
        {
            Console.WriteLine($"Компьютер взял: {obj}.");
        }
    }
}
