using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;


namespace MangoGame
{
    public class Map
    {
        List<EnemySpot> enemySpots = new List<EnemySpot>();
        List<Attack> attacks = new List<Attack>();

        Random random = new Random();

        const int wallX = 156;
        const int wallY = 35;

        int topPos = 1;

        int moveCount = 0;
        int heart = 300;

        int bossHp = 100;

        bool spaceDown = false;

        int playerX = 71;
        int playerY = 15;

        char[,] wall = new char[wallY, wallX];

        Timer timer;
        int mapSecond = 20;

        public void MainMap()
        {
            GameOver gameOver = new GameOver();

            Wall();
            Player();

            // Cursor visible 처리
            Console.CursorVisible = false;

            Enemy();
            DrawWalls();


            // 20초 타이머 설정
            TimerCallback callback = Second;
            timer = new Timer(callback, null, 0, 1000);
            // 20초 타이머 설정


            // 공격 
            while (true)
            {
                foreach (var atk in attacks)
                {
                    wall[atk.Y, atk.X] = '◎';
                }
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo user = Console.ReadKey();
                    Clearbuffer();
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

                            break;
                        case ConsoleKey.LeftArrow:

                            if (wall[playerY, playerX - 1] == ' ')
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY, playerX - 1]);
                                playerX -= 1;
                                moveCount += 1;
                                EnemyFollow();
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

                            break;
                        case ConsoleKey.DownArrow:

                            if (wall[playerY + 1, playerX] == ' ')
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY + 1, playerX]);
                                playerY += 1;
                                moveCount += 1;
                                EnemyFollow();
                            }

                            break;

                        case ConsoleKey.Spacebar:

                            if (spaceDown != true)
                            {
                                attacks.Add(new Attack(playerX, playerY - 1));
                            }
                            spaceDown = true;

                            break;
                        default:
                            break;
                    }
                }


                if (mapSecond == 0)
                {
                    BossATK();
                    Boss();
                }

                DrawWalls();
                if (wall[playerY, playerX + 1] == 'δ' || wall[playerY, playerX - 1] == 'δ'
                    || wall[playerY + 1, playerX] == 'δ' || wall[playerY - 1, playerX] == 'δ')
                {
                    heart -= 1;
                }

                if (heart <= 0)
                {
                    gameOver.Over();
                }


                Enemy();
            }
        }



        // 유저 }
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
        // } 유저

        public void Clearbuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }


        // 맵 {
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
            Console.SetCursorPosition(71, topPos);
            Console.Write("┌        ┐");

            Console.SetCursorPosition(71, topPos + 2);
            Console.Write("└        ┘");

            Console.SetCursorPosition(75, topPos + 1);
            Console.Write("     ");
            Console.SetCursorPosition(75, topPos + 1);
            Console.Write(mapSecond);

            Console.SetCursorPosition(75, topPos + 3);
            Console.WriteLine("{0}", moveCount);

            Console.SetCursorPosition(75, topPos + 2);
            Console.WriteLine("{0} {1}", heart , bossHp);
        }
        // } 맵

        void BossATK()
        {
            for (int i = 0; i < enemySpots.Count; i++)
            {
                wall[enemySpots[i].y, enemySpots[i].x] = ' ';
                enemySpots.Remove(enemySpots[i]);
            }

            // 공격 {
            for (int i = attacks.Count - 1; i >= 0; i--)
            {
                var atk = attacks[i];

                if (atk.Y == 1)
                {
                    attacks.RemoveAt(i);
                    wall[atk.Y, atk.X] = ' ';
                    spaceDown = false;
                }
                else if (wall[atk.Y - 1, atk.X] == ' ')
                {
                    Swap(ref wall[atk.Y, atk.X], ref wall[atk.Y - 1, atk.X]);
                    atk.Y -= 1;
                }
                else if (wall[atk.Y - 1, atk.X] == '★')
                {
                    bossHp -= 10;
                    attacks.RemoveAt(i);
                    wall[atk.Y, atk.X] = ' ';
                    spaceDown = false;
                }

               

            }


            // } 공격  
        }

        void Boss()
        {
            for (int y = 0; y < wallY; y++)
            {
                for (int x = 0; x < wallX; x++)
                {
                    if (x == 35 && y == 5)
                    {
                        wall[y, x] = '★';
                    }
                }
            }
        }


        // 적 생성 및 따라오게 하기 {
        void Enemy()
        {

            if (moveCount == 0)
            {

                while (enemySpots.Count < 5)
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
                    }
                }
            }
            EnemyFollow();
            if (moveCount != 0 && moveCount % 20 == 0 )
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

                if (wall[randY, randX] != wall[playerY, playerX] && moveCount % 20 == 0 && enemySpots.Count % 5 <= 5 )
                {
                    wall[randY, randX] = 'δ';

                    EnemySpot temp = new EnemySpot();
                    temp.x = randX;
                    temp.y = randY;

                    enemySpots.Add(temp);

                }
                EnemyFollow();

            }
        }
        void EnemyFollow()
        {
            

            for (int i = 0; i < enemySpots.Count; i++)
            {


                wall[enemySpots[i].y, enemySpots[i].x] = ' ';

                if (enemySpots[i].x > playerX) //적X > 내X
                {
                    if (enemySpots[i].y > playerY) // 적Y > 내Y
                    {
                        if (wall[enemySpots[i].y, enemySpots[i].x - 1] == ' ')
                        {
                            enemySpots[i].x--;
                        }
                        if (wall[enemySpots[i].y - 1, enemySpots[i].x] == ' ')
                        {
                            enemySpots[i].y--;
                        }
                    }

                    else if (enemySpots[i].y < playerY) // 적Y < 내Y
                    {
                        if (enemySpots[i].x - playerX > playerY - enemySpots[i].y)
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x - 1] == ' ')
                            {
                                enemySpots[i].x--;
                            }
                        }
                        else if (enemySpots[i].x - playerX <= playerY - enemySpots[i].y)
                        {
                            if (wall[enemySpots[i].y + 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y++;
                            }
                        }


                    }
                }

                else if (enemySpots[i].x < playerX) // 적X < 내X
                {
                    if (enemySpots[i].y > playerY) // 적Y > 내Y
                    {
                        if (playerY - enemySpots[i].y <= enemySpots[i].y - playerY) // 내X - 적X > 적Y - 내Y
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x + 1] == ' ')
                            {
                                enemySpots[i].x++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (playerX - enemySpots[i].x > enemySpots[i].y - playerY) // 내X - 적X < 적Y - 내Y
                        {
                            if (wall[enemySpots[i].y - 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y--; // 더 긴 쪽을 먼저 가겠다
                            }
                        }

                    }

                    else if (enemySpots[i].y < playerY) // 적Y < 내Y
                    {
                        if (playerX - enemySpots[i].x > playerX - enemySpots[i].y) // 적X - 내X > 내Y - 적Y
                        {
                            if (wall[enemySpots[i].y, enemySpots[i].x + 1] == ' ')
                            {
                                enemySpots[i].x++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                        else if (playerX - enemySpots[i].x < playerX - enemySpots[i].y) // 적X - 내X < 내Y - 적Y
                        {
                            if (wall[enemySpots[i].y + 1, enemySpots[i].x] == ' ')
                            {
                                enemySpots[i].y++; // 더 긴 쪽을 먼저 가겠다
                            }
                        }
                    }
                }

                if (enemySpots[i].x == playerX) // 적X == 내X
                {
                    if (enemySpots[i].y > playerY) // 적Y > 내Y
                    {
                        if (wall[enemySpots[i].y - 1, enemySpots[i].x] == ' ')
                        {
                            enemySpots[i].y--;
                        }
                    }
                    else if (enemySpots[i].y < playerY) // 적Y < 내Y
                    {
                        if (wall[enemySpots[i].y + 1, enemySpots[i].x] == ' ')
                        {
                            enemySpots[i].y++;
                        }
                    }
                }

                if (enemySpots[i].y == playerY) // 적Y == 내Y
                {
                    if (enemySpots[i].x > playerX) // 적X > 내X
                    {
                        if (wall[enemySpots[i].y, enemySpots[i].x - 1] == ' ')
                        {
                            enemySpots[i].x--;
                        }
                    }
                    else if (enemySpots[i].x < playerX) // 적X < 내X
                    {
                        if (wall[enemySpots[i].y, enemySpots[i].x + 1] == ' ')
                        {
                            enemySpots[i].x++;
                        }
                    }
                }
                wall[enemySpots[i].y, enemySpots[i].x] = 'δ';
            }
        }
        // } 적 생성 및 따라오게 하기 


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
        void Swap(ref char a, ref char b)
        {
            char temp = a;
            a = b;
            b = temp;
        }

        void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public void ClearTimer()
        {
            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(71, 1 + i);
                Console.Write("              ");
            }
        }
    }
}
