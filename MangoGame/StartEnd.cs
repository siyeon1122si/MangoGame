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
            Console.WriteLine("▶");

            Console.SetCursorPosition(72, 24);
            Console.WriteLine(" 게임 시작 ");

            Console.SetCursorPosition(72, 28);
            Console.WriteLine(" 게임 종료 ");

            while (true)
            {
                ConsoleKeyInfo startEndChoice = Console.ReadKey();
                switch (startEndChoice.Key)
                {
                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(70, 28);
                        Console.WriteLine("  ");
                        Console.SetCursorPosition(70, 24);
                        Console.WriteLine("▶");
                        break;
                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(70, 24);
                        Console.WriteLine("  ");
                        Console.SetCursorPosition(70, 28);
                        Console.WriteLine("▶");
                        break;
                    case ConsoleKey.Enter:
                        if ( Console.CursorTop == 24)
                        {
                            characterChoice.Character();
                        }
                        else if ( Console.CursorTop == 28)
                        {
                            End();
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }

        }


        public void End()
        {
            Environment.Exit(0);
        }
    }
}
