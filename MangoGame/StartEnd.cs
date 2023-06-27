using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoGame
{
    public class StartEnd
    {
        public void Start()
        {
            CharacterChoice characterChoice = new CharacterChoice();

            Console.SetCursorPosition(70, 24);
            Console.Write("▶");

            Console.SetCursorPosition(72, 24);
            Console.Write(" 게임 시작 ");

            Console.SetCursorPosition(72, 28);
            Console.Write(" 게임 종료 ");

            bool isChooseGameState = false;
            while (true)
            {
                ConsoleKeyInfo startEndChoice = Console.ReadKey();
                switch (startEndChoice.Key)
                {
                    case ConsoleKey.UpArrow: // 게임 시작
                        Console.SetCursorPosition(70, 28);
                        Console.Write("  ");

                        Console.SetCursorPosition(70, 24);
                        Console.Write("▶");
                        break;
                    case ConsoleKey.DownArrow: // 게임 종료
                        Console.SetCursorPosition(70, 24);
                        Console.Write("  ");

                        Console.SetCursorPosition(70, 28);
                        Console.Write("▶");
                        break;
                    case ConsoleKey.Enter:
                        isChooseGameState = true;
                        break;
                        
                    default:
                        break;

                } // switch

                if(isChooseGameState) { break; }
            } // while 

            // 게임을 실행할지 종료할지 결정한다.
            if (Console.CursorTop == 24) // 게임 시작 시 캐릭터 선택화면으로 이동
            {
                ClearStart();
                characterChoice.Character();
            }
            else if (Console.CursorTop == 28) // 게임 종료 시 콘솔 창 종료
            {
                ClearStart();
                End();
                return;
            }

        } // void Start()
        public void End() // 게임 종료를 누를 시
        {
            Environment.Exit(0);
        }

        public void ClearStart()
        {
            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(70, 22 + i * 2);
                Console.Write("              ");
            }
        }
    }
}
