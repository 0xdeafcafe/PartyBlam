using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyBlam;

namespace QuickTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.Games currentGame;

            Console.WriteLine("#########################################");
            Console.WriteLine("BlamCon Quick Testing Unit");
            Console.WriteLine("Coded and Researched by: Xerax (Alex Reed)");
            Console.WriteLine("Copyright, Valhalla Studios, 2012");
            Console.WriteLine("#########################################");
            Console.WriteLine();
            Console.WriteLine();
            bool cont1 = false;
            while(!cont1)
            {
                Console.WriteLine("What would you like to test? (For a list, type help)");
                string input = Console.ReadLine();

                if (input.ToLower() == "help")
                {
                    Console.WriteLine();
                    Console.WriteLine("Halo Games Supported;");
                    foreach (var game in Enum.GetValues(typeof(Core.Games)))
                        Console.WriteLine("{0} - {1}",
                           (int)game, ((Core.Games)game));
                    Console.WriteLine();
                }
                else
                {
                    Core.Games game = (Core.Games)int.Parse(input);
                    currentGame = game;
                    cont1 = true;
                    switch (game)
                    {
                        case Core.Games.Halo3:
                            break;
                        case Core.Games.Halo3ODST:
                            break;
                        case Core.Games.HaloAnniversary:
                            break;
                        case Core.Games.HaloReach:
                            break;
                        default:
                            cont1 = false;

                            Console.WriteLine();
                            Console.WriteLine("Not a valid game, or it isn't supported by BlamCon yet.");
                            Console.WriteLine();
                            break;
                    }
                }
            }
        }
    }
}
