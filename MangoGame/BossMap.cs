using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Threading.Timer;

namespace MangoGame
{
    public class BossMap
    {

        Timer timer;
        int bossMapSecond = 10;
        public void Boss()
        {
            Map map = new Map();

            map.Wall();

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
            Console.WriteLine(bossMapSecond);
            bossMapSecond--;
        }

        void BossDead()
        {
            Map map = new Map();

            map.Wall();

            // 20초 타이머
            TimerCallback bosscallback = Second;
            timer = new Timer(bosscallback, null, 0, 1000);
            // 20초 타이머

            while (true)
            {
                if (bossMapSecond == -1)
                {
                    timer.Dispose();
                }
            }
        }


    }
}
