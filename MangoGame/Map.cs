using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;


namespace MangoGame
{
    public class Map
    {
        List<EnemySpot> enemySpots = new List<EnemySpot>();
        Random random = new Random();
        int moveCount = 0;
        const int wallX = 156;
        const int wallY = 35;

        int playerX = 71;
        int playerY = 15;

        char[,] wall = new char[wallY, wallX];
        Timer timer;
        int mapSecond = 20;
        public void MainMap()
        {
            Wall();
            Player();
            Enemy();

            // Cursor visible 처리
            Console.CursorVisible = false;

            DrawWalls();

            // 20초 타이머
            TimerCallback callback = Second;
            timer = new Timer(callback, null, 0, 1000);
            // 20초 타이머

            while (true)
            {
                ConsoleKeyInfo user = Console.ReadKey(true);

                switch (user.Key)
                {
                    case ConsoleKey.RightArrow:

                        if (wall[playerY, playerX + 1] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY, playerX + 1]);
                            playerX += 1;
                            moveCount += 1;
                        }
                        break;
                    case ConsoleKey.LeftArrow:

                        if (wall[playerY, playerX - 1] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY, playerX - 1]);
                            playerX -= 1;
                            moveCount += 1;
                        }

                        break;
                    case ConsoleKey.UpArrow:

                        if (wall[playerY - 1, playerX] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY - 1, playerX]);
                            playerY -= 1;
                            moveCount += 1;
                        }

                        break;
                    case ConsoleKey.DownArrow:

                        if (wall[playerY + 1, playerX] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY + 1, playerX]);
                            playerY += 1;
                            moveCount += 1;
                        }

                        break;
                    default:
                        break;
                }

                DrawWalls();
            }
        }

        void Second(object state)
        {
            // 시간이 흘러간다.
            mapSecond--;

            // Dispose
            if (mapSecond == 0)
            {
                timer.Dispose();
            }
        }       // Second()

        public void Wall()
        {
            for (int y = 0; y < wallY; y++)
            {
                for (int x = 0; x < wallX; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        wall[y, x] = '┌';
                    }

                    else if (x == 0 && y == wallY - 1)
                    {
                        wall[y, x] = '└';
                    }

                    else if (y == 0 && x == wallX - 1)
                    {
                        wall[y, x] = '┐';
                    }

                    else if (y == wallY - 1 && x == wallX - 1)
                    {
                        wall[y, x] = '┘';
                    }

                    else if (x == 0 || x == wallX - 1)
                    {
                        wall[y, x] = '│';
                    }

                    else if (y == 0 || y == wallY - 1)
                    {
                        wall[y, x] = '─';
                    }

                    else
                    {
                        wall[y, x] = ' ';
                    }
                }
            }
        }

        public void Player()
        {
            for (int y = 0; y < wallY; y++)
            {
                for (int x = 0; x < wallX; x++)
                {
                    if (x == playerX && y == playerY)
                    {
                        wall[y, x] = '♡';
                    }
                }
            }
        }

        //! 맵 그리는 로직
        private void DrawWalls()
        {
            for (int y = 0; y < wallY; y++)
            {
                Console.SetCursorPosition(0, y + 5);
                for (int x = 0; x < wallX; x++)
                {
                    Console.Write("{0}", wall[y, x]);
                }
            }


            // 시간을 Draw 한다.
            int topPos = 1;
            Console.SetCursorPosition(71, topPos);
            Console.Write("┌        ┐");

            Console.SetCursorPosition(71, topPos + 2);
            Console.Write("└        ┘");

            Console.SetCursorPosition(75, topPos + 1);
            Console.Write("     ");
            Console.SetCursorPosition(75, topPos + 1);
            Console.Write(mapSecond);
        }

        void Enemy()
        {
            if ( moveCount == 0 )
            {
                int count = 0;
               while ( count < 5 )
                {
                    int randX = random.Next(1, 154);
                    int randY = random.Next(1, 33);

                    if ( wall[randY, randX] != wall[playerY, playerX] )
                    {
                        wall[randY, randX] = 'δ';
                        
                        EnemySpot temp = new EnemySpot();
                        temp.x = randX;
                        temp.y = randY;

                        enemySpots.Add(temp);
                        count += 1;
                    }
                }
            }

            if ( moveCount != 0 && moveCount % 5 == 0 )
            {
                while ( true )
                {
                    int randX = random.Next(1, 154);
                    int randY = random.Next(1, 33);

                    if (wall[randY, randX] != wall[playerY, playerX])
                    {
                        wall[randY, randX] = 'δ';

                        EnemySpot temp = new EnemySpot();
                        temp.x = randX;
                        temp.y = randY;

                        enemySpots.Add(temp);
                        break;
                    }

                }
            }
        }



        void Swap(ref char a, ref char b)
        {
            char temp = a;
            a = b;
            b = temp;
        }
    }
}
