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
        List<Boss> bosss = new List<Boss>();

        Random random = new Random();

        const int wallX = 156;
        const int wallY = 35;

        int topPos = 1;

        int moveCount = 0;
        int heart = 300;

        int bossHp = 100;
        int bossTimer = 0;
        int bossTimeCheck = 0;

        bool spaceDown = false;

        int playerX = 71;
        int playerY = 15;

        char[,] wall = new char[wallY, wallX];
        char coin = 'ⓒ';

        int coinPoint = 0;

        Timer timer;
        Timer timer1;
        Timer timer2;

        int mapSecond = 10;

        int type = 0;
        int bossType = 0;

        int enemyCount = 0;

        int speed = 100; // 몬스터 스피드
        int bossSpeed = 300; // 보스 스피드
        public void MainMap()
        {
            bosss.Add(new Boss(66, 6)); // 가운데
            bosss.Add(new Boss(67, 6)); // 오른쪽
            bosss.Add(new Boss(67, 5)); // 오른쪽 대각선 위
            bosss.Add(new Boss(67, 7)); // 오른쪽 대각선 아래
            bosss.Add(new Boss(65, 6)); // 왼쪽
            bosss.Add(new Boss(65, 5)); // 왼쪽 대각선 위
            bosss.Add(new Boss(65, 7)); // 왼쪽 대각선 아래
            bosss.Add(new Boss(66, 5)); // 위
            bosss.Add(new Boss(66, 7)); // 아래

            GameOver gameOver = new GameOver();

            Wall();
            Player();

            bool isCoinMaked = false;
            // Cursor visible 처리
            Console.CursorVisible = false;

            Enemy();
            DrawWalls();

            // 20초 타이머 설정
            TimerCallback callback = Second;
            timer = new Timer(callback, null, 0, 1000);
            // 20초 타이머

            TimerCallback enemytimes = EnemyTime;
            timer1 = new Timer(enemytimes, null, 0, speed);

            TimerCallback bossTimes = BossTime;
            timer2 = new Timer(bossTimes, null, 0, bossSpeed); // // // //

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

            // 공격 
            while (true)
            {
                int playerLive = 0;

                for (int y = 0; y < wallY; y++)
                {
                    for (int x = 0; x < wallX; x++)
                    {
                        if (wall[y, x] == '♡')
                        {
                            playerLive = 1;
                        }
                    }
                }

                if (playerLive == 0)
                {
                    playerX = 5;
                    playerY = 10;
                    heart -= 100;
                    wall[playerY, playerX] = '♡';
                }

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

                            if (wall[playerY, playerX + 1] == ' ' && wall[playerY, playerX + 1] != '□')
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY, playerX + 1]);
                                playerX += 1;
                                moveCount += 1;
                            }

                            else if (wall[playerY, playerX + 1] == coin)
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY, playerX + 1]);
                                wall[playerY, playerX] = ' ';
                                playerX += 1;
                                coinPoint += 10;
                            }

                            break;
                        case ConsoleKey.LeftArrow:

                            if (wall[playerY, playerX - 1] == ' ' && wall[playerY, playerX - 1] != '□')
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY, playerX - 1]);
                                playerX -= 1;
                                moveCount += 1;
                            }

                            else if (wall[playerY, playerX - 1] == coin)
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY, playerX - 1]);
                                wall[playerY, playerX] = ' ';
                                playerX -= 1;
                                coinPoint += 10;
                            }

                            break;
                        case ConsoleKey.UpArrow:

                            if (wall[playerY - 1, playerX] == ' ' && wall[playerY - 1, playerX] != '□')
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY - 1, playerX]);
                                playerY -= 1;
                                moveCount += 1;
                            }

                            else if (wall[playerY - 1, playerX] == coin)
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY - 1, playerX]);
                                wall[playerY, playerX] = ' ';
                                playerY -= 1;
                                coinPoint += 10;
                            }

                            break;
                        case ConsoleKey.DownArrow:

                            if (wall[playerY + 1, playerX] == ' ' && wall[playerY + 1, playerX] != '□')
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY + 1, playerX]);
                                playerY += 1;
                                moveCount += 1;
                            }

                            else if (wall[playerY + 1, playerX] == coin)
                            {
                                Swap(ref wall[playerY, playerX], ref wall[playerY + 1, playerX]);
                                wall[playerY, playerX] = ' ';
                                playerY += 1;
                                coinPoint += 10;
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

                if (wall[playerY, playerX + 1] == 'δ' || wall[playerY, playerX - 1] == 'δ'
                    || wall[playerY + 1, playerX] == 'δ' || wall[playerY - 1, playerX] == 'δ')
                {
                    heart -= 10;
                }

                Console.SetCursorPosition(10, 3);
                Console.Write(" 보유 코인 : {0}", coinPoint);

                if (heart > 200 && heart <= 300)
                {
                    HeartClear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(10, 1);
                    Console.Write(" ♥  ♥  ♥ ");
                    Console.ResetColor();
                }

                else if (heart > 100 && heart <= 200)
                {
                    HeartClear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(10, 1);
                    Console.Write(" ♥  ♥  ");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("♥");
                    Console.ResetColor();
                }

                else if (heart > 0 && heart <= 100)
                {
                    HeartClear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetCursorPosition(10, 1);
                    Console.Write(" ♥  ");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("♥  ♥");
                    Console.ResetColor();
                }

                else
                {
                    HeartClear();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.SetCursorPosition(10, 1);
                    Console.Write(" ♥  ♥  ♥ ");
                    Console.ResetColor();
                    gameOver.Over();
                }

                if (bossType == 1)
                {
                    //ClearTimer();

                    if (bossTimer == 0)
                    {
                        bossTimer = System.Environment.TickCount; // 시간을 담음
                    }

                    bossTimeCheck = System.Environment.TickCount; // 현재 시간 갱신

                    if (bossTimeCheck - bossTimer <= 500) // 갱신된 시간 - 타이머에 담은 시간이 {0} 미만 일 때
                                                          // 숫자를 줄여서 더 빠르게 가능
                    {

                    }
                    else
                    {
                        bossTimer = 0;
                        Boss();
                    }

                    BossATK();
                }

                DrawWalls();

                if (bossHp == 0 && isCoinMaked == false)
                {
                    bosss.Clear();
                    timer = new Timer(callback, null, 0, 1000);
                    Coin();
                    mapSecond = 10;
                    isCoinMaked = true;
                }

                if ( coinPoint > 10 && mapSecond <= 0 )
                {
                    Store();
                }


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
                    if (bosss.Count != 0)
                    {
                        if (wall[y, x] == '♡')
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }

                        else if (wall[y, x] == 'ⓒ')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }

                        else if (wall[y,x] == 'δ')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }

                        else if (x == bosss[0].X && y == bosss[0].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[1].X && y == bosss[1].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[2].X && y == bosss[2].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[3].X && y == bosss[3].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[4].X && y == bosss[4].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[5].X && y == bosss[5].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[6].X && y == bosss[6].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[7].X && y == bosss[7].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else if (x == bosss[8].X && y == bosss[8].Y)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write("{0}",wall[y, x]); // 앨 넣으면 초라한 보스

                        }
                    }

                    else
                    {
                        if (wall[y, x] == 'ⓒ')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(wall[y, x]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write("{0}", wall[y, x]);
                        }
                    }


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
            if (mapSecond == 0)
            {
                Console.Write(bossHp);
            }
            else
            {
                Console.Write(mapSecond);
            }

            // 확인용
            Console.SetCursorPosition(75, topPos + 3);
            Console.WriteLine("{0}", moveCount);

            Console.SetCursorPosition(75, topPos + 2);
            Console.WriteLine("{0} {1}", heart, bossHp);
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

                else if (wall[atk.Y - 1, atk.X] == '□')
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
            if (bosss.Count == 0)
            {
                return;
            }

            for (int y = 0; y < wallY; y++)
            {
                for (int x = 0; x < wallX; x++)
                {
                    if (bosss[0].X == x && bosss[0].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[1].X == x && bosss[1].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[2].X == x && bosss[2].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[3].X == x && bosss[3].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[4].X == x && bosss[4].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[5].X == x && bosss[5].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[6].X == x && bosss[6].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[7].X == x && bosss[7].Y == y)
                    {
                        wall[y, x] = '□';
                    }

                    else if (bosss[8].X == x && bosss[8].Y == y)
                    {
                        wall[y, x] = '□';
                    }
                }
            }
            BossFollow();
        }

        void BossFollow()
        {
            for (int i = 0; i < bosss.Count; i++)
            {
                wall[bosss[i].Y, bosss[i].X] = ' ';
            }

            bossTimer = 0;
            if (bosss[0].X > playerX) //적X > 내X
            {
                if (bosss[0].Y > playerY) // 적Y > 내Y
                {
                    if (wall[bosss[0].Y, bosss[0].X - 1] == ' ')
                    {
                        bosss[0].X--;
                        bosss[1].X--;
                        bosss[2].X--;
                        bosss[3].X--;
                        bosss[4].X--;
                        bosss[5].X--;
                        bosss[6].X--;
                        bosss[7].X--;
                        bosss[8].X--;
                    }
                    if (wall[bosss[0].Y - 1, bosss[0].X] == ' ')
                    {
                        bosss[0].Y--;
                        bosss[1].Y--;
                        bosss[2].Y--;
                        bosss[3].Y--;
                        bosss[4].Y--;
                        bosss[5].Y--;
                        bosss[6].Y--;
                        bosss[7].Y--;
                        bosss[8].Y--;
                    }
                }

                else if (bosss[0].Y < playerY) // 적Y < 내Y
                {
                    if (bosss[0].X - playerX > playerY - bosss[0].Y)
                    {
                        if (wall[bosss[0].Y, bosss[0].X - 1] == ' ')
                        {
                            bosss[0].X--;
                            bosss[1].X--;
                            bosss[2].X--;
                            bosss[3].X--;
                            bosss[4].X--;
                            bosss[5].X--;
                            bosss[6].X--;
                            bosss[7].X--;
                            bosss[8].X--;
                        }
                    }
                    else if (bosss[0].X - playerX <= playerY - bosss[0].Y)
                    {
                        if (wall[bosss[0].Y + 1, bosss[0].X] == ' ')
                        {
                            bosss[0].Y++;
                            bosss[1].Y++;
                            bosss[2].Y++;
                            bosss[3].Y++;
                            bosss[4].Y++;
                            bosss[5].Y++;
                            bosss[6].Y++;
                            bosss[7].Y++;
                            bosss[8].Y++;
                        }
                    }
                }
            }

            else if (bosss[0].X < playerX) // 적X < 내X
            {
                if (bosss[0].Y > playerY) // 적Y > 내Y
                {
                    if (playerY - bosss[0].Y <= bosss[0].Y - playerY)
                    {
                        if (wall[bosss[0].Y, bosss[0].X + 1] == ' ')
                        {
                            bosss[0].X++;
                            bosss[1].X++;
                            bosss[2].X++;
                            bosss[3].X++;
                            bosss[4].X++;
                            bosss[5].X++;
                            bosss[6].X++;
                            bosss[7].X++;
                            bosss[8].X++;
                        }
                    }
                    else if (playerX - bosss[0].X > bosss[0].Y - playerY)
                    {
                        if (wall[bosss[0].Y - 1, bosss[0].X] == ' ')
                        {
                            bosss[0].Y--;
                            bosss[1].Y--;
                            bosss[2].Y--;
                            bosss[3].Y--;
                            bosss[4].Y--;
                            bosss[5].Y--;
                            bosss[6].Y--;
                            bosss[7].Y--;
                            bosss[8].Y--;
                        }
                    }

                }

                else if (bosss[0].Y < playerY) // 적Y < 내Y
                {
                    if (playerX - bosss[0].X > playerX - bosss[0].Y)
                    {
                        if (wall[bosss[0].Y, bosss[0].X + 1] == ' ')
                        {
                            bosss[0].X++;
                            bosss[1].X++;
                            bosss[2].X++;
                            bosss[3].X++;
                            bosss[4].X++;
                            bosss[5].X++;
                            bosss[6].X++;
                            bosss[7].X++;
                            bosss[8].X++;
                        }
                    }
                    else if (playerX - bosss[0].X < playerX - bosss[0].Y)
                    {
                        if (wall[bosss[0].Y + 1, bosss[0].X] == ' ')
                        {
                            bosss[0].Y++;
                            bosss[1].Y++;
                            bosss[2].Y++;
                            bosss[3].Y++;
                            bosss[4].Y++;
                            bosss[5].Y++;
                            bosss[6].Y++;
                            bosss[7].Y++;
                            bosss[8].Y++;

                        }
                    }

                }
            }

            if (bosss[0].X == playerX) // 적X == 내X
            {
                if (bosss[0].Y > playerY) // 적Y > 내Y
                {
                    if (wall[bosss[0].Y - 1, bosss[0].X] == ' ')
                    {
                        bosss[0].Y--;
                        bosss[1].Y--;
                        bosss[2].Y--;
                        bosss[3].Y--;
                        bosss[4].Y--;
                        bosss[5].Y--;
                        bosss[6].Y--;
                        bosss[7].Y--;
                        bosss[8].Y--;
                    }
                }
                else if (bosss[0].Y < playerY) // 적Y < 내Y
                {
                    if (wall[bosss[0].Y + 1, bosss[0].X] == ' ')
                    {
                        bosss[0].Y++;
                        bosss[1].Y++;
                        bosss[2].Y++;
                        bosss[3].Y++;
                        bosss[4].Y++;
                        bosss[5].Y++;
                        bosss[6].Y++;
                        bosss[7].Y++;
                        bosss[8].Y++;
                    }

                }

            }

            if (bosss[0].Y == playerY) // 적Y == 내Y
            {
                if (bosss[0].X > playerX) // 적X > 내X
                {
                    if (wall[bosss[0].Y, bosss[0].X - 1] == ' ')
                    {
                        bosss[0].X--;
                        bosss[1].X--;
                        bosss[2].X--;
                        bosss[3].X--;
                        bosss[4].X--;
                        bosss[5].X--;
                        bosss[6].X--;
                        bosss[7].X--;
                        bosss[8].X--;
                    }
                }
                else if (bosss[0].X < playerX) // 적X < 내X
                {
                    if (wall[bosss[0].Y, bosss[0].X + 1] == ' ')
                    {
                        bosss[0].X++;
                        bosss[1].X++;
                        bosss[2].X++;
                        bosss[3].X++;
                        bosss[4].X++;
                        bosss[5].X++;
                        bosss[6].X++;
                        bosss[7].X++;
                        bosss[8].X++;
                    }
                }
            }

            if (bosss.Count > 0)
            {
                wall[bosss[0].Y, bosss[0].X] = '□';
                wall[bosss[1].Y, bosss[1].X] = '□';
                wall[bosss[2].Y, bosss[2].X] = '□';
                wall[bosss[3].Y, bosss[3].X] = '□';
                wall[bosss[4].Y, bosss[4].X] = '□';
                wall[bosss[5].Y, bosss[5].X] = '□';
                wall[bosss[6].Y, bosss[6].X] = '□';
                wall[bosss[7].Y, bosss[7].X] = '□';
                wall[bosss[8].Y, bosss[8].X] = '□';
            }
            else
            {
                wall[bosss[0].Y, bosss[0].X] = ' ';
                wall[bosss[1].Y, bosss[1].X] = ' ';
                wall[bosss[2].Y, bosss[2].X] = ' ';
                wall[bosss[3].Y, bosss[3].X] = ' ';
                wall[bosss[4].Y, bosss[4].X] = ' ';
                wall[bosss[5].Y, bosss[5].X] = ' ';
                wall[bosss[6].Y, bosss[6].X] = ' ';
                wall[bosss[7].Y, bosss[7].X] = ' ';
                wall[bosss[8].Y, bosss[8].X] = ' ';
            }
        }

        // 적 생성 및 따라오게 하기 {
        void Enemy()
        {

            if (enemyCount > 1)
            {
                int randX = random.Next(1, 154);
                int randY = random.Next(1, 33);

                if (mapSecond % 3 == 0) // 몬스터가 나오는 시간
                {

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

                EnemyFollow();

                enemyCount = 0;
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
            }   // for
        }
        // } 적 생성 및 따라오게 하기 


        void Coin()
        {
            int coinCount = 0;

            while (true)
            {
                int randX = random.Next(1, 154);
                int randY = random.Next(5, 33);

                Console.SetCursorPosition(randX, randY);

                if (wall[randY, randX] == ' ')
                {
                    wall[randY, randX] = coin;
                    coinCount++;
                    Console.Write(wall[randY, randX]); // 코인의 좌표
                }

                if (coinCount == 40)
                {
                    break;
                }
            }
            Player();
        }

        void Store()
        {
            Console.Clear();

            Console.SetCursorPosition(75, topPos + 3);
            Console.WriteLine(" 상 점 ");

            for (int y = 0; y < wallY; y++)
            {
                for (int x = 0; x < wallX; x++)
                {

                    if (x == 0 && y == 0 || x == 4 && y == 2 || x == 84 && y == 2)
                    {
                        wall[y, x] = '┌';
                    }

                    else if (x == 0 && y == wallY - 1 || x == 4 && y == wallY - 3 || x == 84 && y == wallY - 13)
                    {
                        wall[y, x] = '└';
                    }

                    else if (y == 0 && x == wallX - 1 || y == 2 && x == wallX - 5 || y == 2 && x == 80)
                    {
                        wall[y, x] = '┐';
                    }

                    else if (y == wallY - 1 && x == wallX - 1 || y == wallY - 13 && x == wallX - 5 || y == wallY - 3 && x == wallX - 75 )
                    {
                        wall[y, x] = '┘';
                    }

                    else if (x == 0 || x == wallX - 1 )
                    {
                        wall[y, x] = '│';
                    }

                    else if (y == 0 || y == wallY - 1 )
                    {
                        wall[y, x] = '─';
                    }

                    else
                    {
                        wall[y, x] = ' ';
                    }
                }
            }
            DrawWalls();
            Console.ReadKey();
            while (true)
            {

            }


        }

        void Second(object state)
        {
            // 시간이 흘러간다.
            mapSecond--;

            // Dispose
            if (mapSecond == 0)
            {
                type = 1;
                timer.Dispose();
            }
        }       // Second()

        void EnemyTime(object state)
        {
            // 시간이 흘러간다.
            if (mapSecond != 0)
            {
                if (type == 0)
                {
                    enemyCount++;
                    Enemy();
                }
            }
        }
        void BossTime(object state)
        {
            // 시간이 흘러간다.
            if (mapSecond == 0)
            {
                bossType = 1;
            }
        }
        void Swap(ref char a, ref char b)
        {
            char temp = a;
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

        void HeartClear()
        {
            for (int i = 0; i < 1; i++)
            {
                Console.SetCursorPosition(10, 1 + i);
                Console.Write("              ");
            }
        }

        public void Clearbuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }
    }
}
