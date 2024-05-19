/*
    Gioco della Tombola
    Autore: Francesco Santaniello
 */

using System.Text;
namespace tombola;
class Program
{
    private static void Setting()
    {
        Console.Title = "Tombola";
        Console.OutputEncoding = Encoding.UTF8;
        Console.ResetColor();
        Console.Clear();
    }

    private static Game.Cartelle SceltaCartelle()
    {
        Game.Cartelle[] mods = (Game.Cartelle[])Enum.GetValues(typeof(Game.Cartelle));
        int sc = -1;

        while(sc <= 0 || sc > mods.Length)
        {
            Console.Clear();
            int i = 1;
            foreach (Game.Cartelle Cartelle in mods)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(i);
                Console.ResetColor();
                Console.WriteLine($"] {Cartelle}");
                i++;
            }
            Console.WriteLine();
            Console.Write("Scegli con che Cartelle giocare: ");
            int.TryParse(Console.ReadLine(), out sc);
        }
        return mods[--sc];
    }

    private static Game.Modalita SceltaModalita()
    {
        Game.Modalita[] mods = (Game.Modalita[])Enum.GetValues(typeof(Game.Modalita));
        int sc = -1;

        while (sc <= 0 || sc > mods.Length)
        {
            Console.Clear();
            int i = 1;
            foreach (Game.Modalita Cartelle in mods)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(i);
                Console.ResetColor();
                Console.WriteLine($"] {Cartelle}");
                i++;
            }
            Console.WriteLine();
            Console.Write("Scegli con che Cartelle giocare: ");
            int.TryParse(Console.ReadLine(), out sc);
        }
        return mods[--sc];
    }

    public static void Main()
    {
        Setting();
        Game game = new Game(SceltaCartelle(), SceltaModalita());
        game.Start();
    }
}