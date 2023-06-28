using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;


namespace MangoGame
{
    public class Map
    {
        List<EnemySpot> enemySpots = new List<EnemySpot>();
        Random random = new Random();
        const int wallX = 156;
        const int wallY = 35;

        int moveCount = 0;
        int heart = 3;

        int playerX = 71;
        int playerY = 15;

        char[,] wall = new char[wallY, wallX];
        Timer timer;
        int mapSecond = 20;
        public void MainMap()
        {
            Wall();
            Player();


            // Cursor visible 처리
            Console.CursorVisible = false;

            Enemy();
            DrawWalls();

            GameOver gameOver = new GameOver();

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
                            EnemyFollow();
                        }
                        else if (wall[playerY, playerX + 1] == 'δ')
                        {
                            heart -= 1;
                        }
                        break;
                    case ConsoleKey.LeftArrow:

                        if (wall[playerY, playerX - 1] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY, playerX - 1]);
                            playerX -= 1;
                            moveCount += 1;
                            EnemyFollow();
                        }
                        else if (wall[playerY, playerX - 1] == 'δ')
                        {
                            heart -= 1;
                        }

                        break;
                    case ConsoleKey.UpArrow:

                        if (wall[playerY - 1, playerX] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY - 1, playerX]);
                            playerY -= 1;
                            moveCount += 1;
                            EnemyFollow();
                        }
                        else if (wall[playerY - 1, playerX] == 'δ')
                        {
                            heart -= 1;
                        }

                        break;
                    case ConsoleKey.DownArrow:

                        if (wall[playerY + 1, playerX] == ' ')
                        {
                            Swap(ref wall[playerY, playerX], ref wall[playerY + 1, playerX]);
                            playerY += 1;
                            moveCount += 1;
                            EnemyFollow();
                        }
                        else if (wall[playerY + 1, playerX] == 'δ')
                        {
                            heart -= 1;
                        }

                        break;
                    default:
                        break;
                }

                if (heart <= 0)
                {
                    gameOver.Over();
                }

                DrawWalls();
                Enemy();

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

            Console.SetCursorPosition(75, topPos + 3);
            Console.WriteLine("{0}",moveCount);
        }

        void Enemy()
        {
            int count = 0;

            if (moveCount == 0)
            {

                while (count < 5)
                {
                    int randX = random.Next(1, 154);
                    int randY = random.Next(1, 33);

                    for ( int i = 0; i < enemySpots.Count; i++)
                    {
                        if (wall[enemySpots[i].y ,enemySpots[i].x ] == wall[randY, randX])
                        {
                            continue;
                        }
                    }

                    if (wall[randY, randX] != wall[playerY, playerX])
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

            if (moveCount != 0 && moveCount % 5 == 0)
            {
                while (true)
                {
                    int randX = random.Next(1, 154);
                    int randY = random.Next(1, 33);

                    for (int i = 0; i < enemySpots.Count; i++)
                    {
                        if (wall[enemySpots[i].y, enemySpots[i].x] == wall[randY, randX])
                        {
                            continue;
                        }
                    }

                    if (wall[randY, randX] != wall[playerY, playerX])
                    {
                        wall[randY, randX] = 'δ';

                        EnemySpot temp = new EnemySpot();
                        temp.x = randX;
                        temp.y = randY;

                        enemySpots.Add(temp);
                        break;
                    }
                    EnemyFollow();
                }
            }
        }

        void EnemyFollow()
        {
            for (int i = 0; i < enemySpots.Count; i++)
            {
                wall[enemySpots[i].y, enemySpots[i].x] = ' ';

                if (enemySpots[i].x > playerX)
                {
                    if (enemySpots[i].y > playerY) // 적Y > 내Y
                    {
                        if (wall[enemySpots[i].y, enemySpots[i].x - 1] == ' ')
                        {
                            enemySpots[i].x--; // 더 긴 쪽을 먼저 가겠다
                        }
                        if (wall[enemySpots[i].y - 1, enemySpots[i].x] == ' ')
                        {
                            enemySpots[i].y--; // 더 긴 쪽을 먼저 가겠다
                        }
                    }

                    else if (enemySpots[i].y < playerY) // 적Y < 내Y
                    {
                        if (enemySpots[i].x - playerX > playerY - enemySpots[i].y) // 적X - 내X > 내Y - 적Y
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x - 1] == ' ')
                            {
                                enemySpots[i].x--; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (enemySpots[i].x - playerX < playerY - enemySpots[i].y) // 적X - 내X < 내Y - 적Y
                        {
                            if (wall[enemySpots[i].y + 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                    }
                }

                else if (enemySpots[i].x < playerX) // 적X < 내X
                {

                    if (enemySpots[i].y > playerY) // 적Y > 내Y
                    {
                        if (playerY - enemySpots[i].y > enemySpots[i].y - playerY) // 내X - 적X > 적Y - 내Y
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x + 1] == ' ')
                            {
                                enemySpots[i].x++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (playerX - enemySpots[i].x < enemySpots[i].y - playerY) // 내X - 적X < 적Y - 내Y
                        {
                            if (wall[enemySpots[i].y - 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y--; // 더 긴 쪽을 먼저 가겠다
                            }
                        }

                    }
                    else if (enemySpots[i].y < playerY) // 적Y < 내Y
                    {
                        if (playerX - enemySpots[i].x > playerX - enemySpots[i].x) // 적X - 내X > 내Y - 적Y
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x + 1] == ' ')
                            {
                                enemySpots[i].x++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (playerX - enemySpots[i].x < playerX - enemySpots[i].x) // 적X - 내X < 내Y - 적Y
                        {
                            if (wall[enemySpots[i].y + 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                    }
                    else if (enemySpots[i].x == playerX)
                    {
                        if (enemySpots[i].y > playerY) // 적Y > 내Y
                        {
                            if (wall[enemySpots[i].y - 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y--; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (enemySpots[i].y < playerY) // 적Y < 내Y
                        {
                            if (wall[enemySpots[i].y + 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                    }
                    else if (enemySpots[i].x == playerY)
                    {
                        if (enemySpots[i].x > playerX)
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x - 1] == ' ')
                            {
                                enemySpots[i].x--; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (enemySpots[i].x < playerX)
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x + 1] == ' ')
                            {
                                enemySpots[i].x++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                    }
                   
                }
                wall[enemySpots[i].y, enemySpots[i].x] = 'δ';
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
