using Microsoft.Win32;
using System;
using System.Threading;


namespace MangoGame
{
    public class Map
    {
        Timer timer;
        int mapSecond = 20;
        public void MainMap()
        {
            BossMap bossMap = new BossMap();
            Player();

            // 20초 타이머
            TimerCallback callback = Second;
            timer = new Timer(callback, null, 0, 1000);
            // 20초 타이머
            while (true)
            {
                if (mapSecond == -1)
                {
                    timer.Dispose();
                }
            }
           
        }

        void Second(object state)
        {
            Console.SetCursorPosition(71, 1);
            Console.WriteLine("┌");
            Console.SetCursorPosition(80, 1);
            Console.WriteLine("┐");
            Console.SetCursorPosition(71, 3);
            Console.WriteLine("└");
            Console.SetCursorPosition(80, 3);
            Console.WriteLine("┘");

            Console.SetCursorPosition(75, 2);
            Console.Write("     ");
            Console.SetCursorPosition(75, 2);
            Console.WriteLine(mapSecond);
            mapSecond--;
        }

        void Player()
        {

            int playerX = 10;
            int playerY = 10;
            int sizeX = 40;
            int sizeY = 40;

            string[,] player = new string[sizeX, sizeY];

            Console.CursorVisible = false;
            
            for(int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    player[i, j] = "  ";
                }
            }


            while (true)
            {
                
                player[playerX, playerY] = "♡";

                for (int y = 0; y < sizeX; y++)
                {
                    for (int x = 0; x < sizeY; x++)
                    {
                        Console.SetCursorPosition(x, y);

                        Console.Write($"{player[x,y]}");
                    }
                }

                    //Console.WriteLine();
              
                ConsoleKeyInfo user = Console.ReadKey(true);
                player[playerX, playerY] = " ";
                switch (user.Key)
                {
                    case ConsoleKey.RightArrow:
                        playerX++;
                        break;
                    case ConsoleKey.LeftArrow:
                        playerX--;
                        break;
                    case ConsoleKey.UpArrow:
                        playerY--;
                        break;
                    case ConsoleKey.DownArrow:
                        playerY++;
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
