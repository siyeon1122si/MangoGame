using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MangoGame
{
    public class BossMap
    {
        Timer timer;
        int mapSecond = 10;
        public void SubMap()
        {
            // 10초 타이머
            TimerCallback callback = Second;
            timer = new Timer(callback, null, 0, 1000);
            // 10초 타이머

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

    }
}
