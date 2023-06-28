using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoGame
{
    public class GameOver
    {
        public void Over()
        {
            StartEnd startEnd = new StartEnd();

            Console.Clear();

            Console.SetCursorPosition(72, 10);
            Console.Write(" 게임 오버 ");

            Console.SetCursorPosition(58, 28);
            Console.Write(" 아무키나 누르면 메인 화면으로 이동합니다 ");

            Console.ReadLine();


            Console.Clear();
            startEnd.Start();
        }
    }
}
