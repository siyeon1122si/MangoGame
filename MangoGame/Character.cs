using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoGame
{
    public class CharacterChoice
    {
        public void Character()
        {
            Map map = new Map();

            // Cursor visible 처리
            Console.CursorVisible = false;

            Console.SetCursorPosition(23, 3);
            Console.Write("  애긔 망고 ");

            Console.SetCursorPosition(72, 3);
            Console.Write("  똥  망고 ");

            Console.SetCursorPosition(120, 3);
            Console.Write("  가시 망고 ");

            for (int y = 0; y < 15; y++) // Y
            {
                for (int x = 0; x < 30; x++) // X
                {
                    Console.SetCursorPosition(x + 15, y + 5);

                    if (x == 0 && y == 0)
                    {
                        Console.Write("┌");
                    }
                    else if (x == 29 && y == 0)
                    {
                        Console.Write("┐");
                    }
                    else if (x == 0 && y == 14)
                    {
                        Console.Write("└");
                    }
                    else if (x == 29 && y == 14)
                    {
                        Console.Write("┘");
                    }
                    else if (y == 0 || y == 14)
                    {
                        Console.Write("─");
                    }
                    else if (x == 0 || x == 29)
                    {
                        Console.Write("┃");
                    }
                    else
                    {
                        /*empty*/
                    }
                }
            }

            int pos = 0;
            bool isChooseCharactor = false;
            while (true)
            {


                ConsoleKeyInfo user = Console.ReadKey();


                switch (user.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (pos == 2)
                        {
                            break;
                        }
                        else
                        {
                            for (int y = 0; y < 15; y++) // Y
                            {
                                for (int x = 0; x < 30; x++) // X   
                                {
                                    Console.SetCursorPosition(x + 15 + 48 * pos, y + 5);

                                    if (x == 0 && y == 0)
                                    {
                                        Console.Write(" ");
                                    }
                                    else if (x == 29 && y == 0)
                                    {
                                        Console.Write(" ");
                                    }
                                    else if (x == 0 && y == 14)
                                    {
                                        Console.Write(" ");
                                    }
                                    else if (x == 29 && y == 14)
                                    {
                                        Console.Write(" ");
                                    }
                                    else if (y == 0 || y == 14)
                                    {
                                        Console.Write(" ");
                                    }
                                    else if (x == 0 || x == 29)
                                    {
                                        Console.Write(" ");
                                    }
                                    else
                                    {
                                        /*empty*/
                                    }
                                }
                            }
                            pos += 1;

                            for (int y = 0; y < 15; y++) // Y
                            {
                                for (int x = 0; x < 30; x++) // X
                                {
                                    Console.SetCursorPosition(x + 15 + 48 * pos, y + 5);

                                    if (x == 0 && y == 0)
                                    {
                                        Console.Write("┌");
                                    }
                                    else if (x == 29 && y == 0)
                                    {
                                        Console.Write("┐");
                                    }
                                    else if (x == 0 && y == 14)
                                    {
                                        Console.Write("└");
                                    }
                                    else if (x == 29 && y == 14)
                                    {
                                        Console.Write("┘");
                                    }
                                    else if (y == 0 || y == 14)
                                    {
                                        Console.Write("─");
                                    }
                                    else if (x == 0 || x == 29)
                                    {
                                        Console.Write("┃");
                                    }
                                    else
                                    {
                                        /*empty*/
                                    }
                                }
                            }
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            if (pos == 0)
                            {
                                break;
                            }

                            else
                            {

                                for (int y = 0; y < 15; y++) // Y
                                {
                                    for (int x = 0; x < 30; x++) // X   
                                    {
                                        Console.SetCursorPosition(x + 15 + 48 * pos, y + 5);

                                        if (x == 0 && y == 0)
                                        {
                                            Console.Write(" ");
                                        }
                                        else if (x == 29 && y == 0)
                                        {
                                            Console.Write(" ");
                                        }
                                        else if (x == 0 && y == 14)
                                        {
                                            Console.Write(" ");
                                        }
                                        else if (x == 29 && y == 14)
                                        {
                                            Console.Write(" ");
                                        }
                                        else if (y == 0 || y == 14)
                                        {
                                            Console.Write(" ");
                                        }
                                        else if (x == 0 || x == 29)
                                        {
                                            Console.Write(" ");
                                        }
                                        else
                                        {
                                            /*empty*/
                                        }
                                    }
                                }
                                pos -= 1;

                                for (int y = 0; y < 15; y++) // Y
                                {
                                    for (int x = 0; x < 30; x++) // X
                                    {
                                        Console.SetCursorPosition(x + 15 + 48 * pos, y + 5);

                                        if (x == 0 && y == 0)
                                        {
                                            Console.Write("┌");
                                        }
                                        else if (x == 29 && y == 0)
                                        {
                                            Console.Write("┐");
                                        }
                                        else if (x == 0 && y == 14)
                                        {
                                            Console.Write("└");
                                        }
                                        else if (x == 29 && y == 14)
                                        {
                                            Console.Write("┘");
                                        }
                                        else if (y == 0 || y == 14)
                                        {
                                            Console.Write("─");
                                        }
                                        else if (x == 0 || x == 29)
                                        {
                                            Console.Write("┃");
                                        }
                                        else
                                        {
                                            /*empty*/
                                        }
                                    }

                                }
                                break;

                            }
                        }
                    case ConsoleKey.Enter:
                        isChooseCharactor = true;
                        break;
                }

                if(isChooseCharactor) { break; }
            } // while

            // 여기에서 맵으로 넘어간다.
            if (pos == 0)
            {
                ClearCharacter();
                map.MainMap();
            }
            else if (pos == 1)
            {
                ClearCharacter();
                map.MainMap();
            }
            else if (pos == 2)
            {
                ClearCharacter();
                map.MainMap();
            }
        }

        public void ClearCharacter()
        {
            for (int i = 0; i < 30; i++)
            {
                Console.SetCursorPosition(15, 3 + i);
                Console.Write("                                                                   " +
                    "                                                                             ");
            }
        }
    }
}
