using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInSticks
{
    class Game
    {
        private readonly Random random;
        public int QuantitySticks { get; set; }
        public Player Turn { get; set; }
        public StatusGame StatusGame {get;set; }
        public int RemaningSticks { get; set; }
        public event Action<Player> EndOfGame;
        public event Action<int> ComputerPlaying;
        public event EventHandler<int> ManTurnToMove;

        public Game(int quantitSticks, Player whoFirstMove)
        {
            if (quantitSticks <= 10)
                throw new ArgumentException("Количество палок должно быть не менее 10 штук.");
            random = new Random();
            StatusGame = StatusGame.NotStarted;
            QuantitySticks = quantitSticks;
            RemaningSticks = QuantitySticks;
            Turn = whoFirstMove;
        }
        public void ManTake(int sticks)
        {
            if (sticks<1||sticks>3)
            {
                throw new ArgumentException("Вы можете взять не более 3 палок за один ход.");
            }
            if (sticks>RemaningSticks)
            {
                throw new ArgumentException($"Невозможно взять больше чем осталось. Палок осталось {RemaningSticks}");
            }
            TakeOfSticks(sticks);
            
        }
        public void Start()
        {
            if (StatusGame == StatusGame.GameOver)
            {
                RemaningSticks = QuantitySticks;
            }
            if (StatusGame==StatusGame.InProcess)
            {
                throw new InvalidOperationException("Игра в процессе.");
            }
            StatusGame = StatusGame.InProcess;

            while (StatusGame==StatusGame.InProcess)
            {
                if (Turn == Player.Computer)
                {
                    ComputerMove();
                }
                else
                {
                    ManMove();
                }
                StatusOfGame();
                Turn = Turn == Player.Computer ? Player.Man : Player.Computer;
            }
        }

        private void StatusOfGame()
        {
            if (RemaningSticks ==0)
            {
                StatusGame = StatusGame.GameOver;
                if (EndOfGame != null)
                    EndOfGame(Turn == Player.Computer ? Player.Man : Player.Computer);
            }
        }

        private void ComputerMove()
        {
            int numberOfSticks = RemaningSticks >= 3 ? 3 : RemaningSticks;
            int sticks = random.Next(1, numberOfSticks);

            TakeOfSticks(sticks);
            if (ComputerPlaying != null)
            {
                ComputerPlaying(sticks);
            }
        }

        private void TakeOfSticks(int sticks)
        {
            RemaningSticks -= sticks;

        }

        private void ManMove()
        {
            if (ManTurnToMove != null)
            {
                ManTurnToMove(this, RemaningSticks);
            };
        }
    }
}
