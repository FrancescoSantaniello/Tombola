using System.Net;
using System.Net.NetworkInformation;
namespace tombola;
class Game
{
    public enum Cartelle : int
    {
        SingolaCartella = 1,
        DueCartelle = 2,
        TreCartelle = 3,
        QuattroCartelle = 4,
        CinqueCartelle = 5,
        SeiCartelle = 6,
        SetteCartelle = 7,
        OttoCartelle = 8,
        NoveCartelle = 9,
        DieciCartelle = 10,
        UndiciCartelle = 11,
        DodiciCartelle = 12
    }

    public enum Modalita : int
    {
        AutomaticoUnSecondo = 1000,
        AutomaticoDueSecondo = 2000,
        AutomaticTreSecondo = 3000,
        Manuale = 0
    }

    private Cartelle numero_cartelle;
    private Modalita mod;
    private int Gen(List<int> ex, int min = 1, int max = 90)
    {
        Random randomClass = new Random();

        int gen = randomClass.Next(min,max);

        while (ex.Contains(gen) )
            gen = randomClass.Next(min, max);

        return gen;
    }
    private bool Ping()
    {
        try
        {
            new Ping().Send("8.8.8.8");
            return true;
        }
        catch (Exception) { return false; }
    }
    private List<string> Download()
    {
        const string URL = "https://raw.githubusercontent.com/nbattezzati/pytombola/main/smorfia_napoletana.txt";
        return new(new HttpClient().GetStringAsync(URL).Result.Split("\n"));
    }
    private string PrintLine(char c, int n)
    {
        string r = "";
        while (n-- > 0)
            r += c;
        return r;
    }
    private string SmorfiaDecode(List<string> list, int n)
    {
        string r = "";

        foreach(string s in list)
        {
            string[] subString = s.Split(";");
            if (subString.Length > 0)
            {
                int value;
                if (int.TryParse(subString[0], out value))
                {
                    if (n == value)
                    {
                        r = subString[2];
                        break;
                    }
                }

            }
        }

        return r;
    }
    private void PrintPunti(List<Cartella> cartelle)
    {
        const char HORIZONTAL_SPERARATOR = '─';
        const char VERTICAL_SEPARATOR = '│';
        const int BUFFER = 49;

        Console.WriteLine("Punteggio");

        Console.WriteLine(PrintLine(HORIZONTAL_SPERARATOR, BUFFER));

        Console.WriteLine($"{VERTICAL_SEPARATOR}\tCartella\t{VERTICAL_SEPARATOR}   Punteggio\t\t{VERTICAL_SEPARATOR}");

        Console.WriteLine(PrintLine(HORIZONTAL_SPERARATOR, BUFFER));

        foreach (Cartella c in cartelle)
        {
            Console.Write($"{VERTICAL_SEPARATOR}\t{c.Nome}\t{VERTICAL_SEPARATOR}   ");

            switch (c.Punto)
            {
                case CalcoloPunti.Punti.Niente:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case CalcoloPunti.Punti.Ambo:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case CalcoloPunti.Punti.Terna:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case CalcoloPunti.Punti.Quaterna:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case CalcoloPunti.Punti.Cinquina:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case CalcoloPunti.Punti.Tombola:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }

            Console.Write($"{c.Punto}");

            Console.ResetColor();
            Console.WriteLine($"\t\t{VERTICAL_SEPARATOR}");
        }

        Console.WriteLine(PrintLine(HORIZONTAL_SPERARATOR, BUFFER));
    }
    private void CountDown(int second = 3)
    {
        while(second-- > 0)
        {
            Console.Clear();

            Console.Write("[");
            Console.ForegroundColor = (second % 2 == 0) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write((second % 2 == 0) ? '\\' : '/');
            Console.ResetColor();

            Console.Write("] ");

            Console.WriteLine($"Il gioco parte tra {second + 1} secondi");

            Console.Write("Buona fortuna");

            Console.ForegroundColor = (second % 2 == 0) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(" ☺");
            Console.ResetColor();
            
            Thread.Sleep(999);
        }
    }
    public void Start()
    {
        Console.Clear();

        const int MAX_NUMBER = 90;

        List<Cartella> cartelle = new List<Cartella>();
        for (int i = 0; i < (int)numero_cartelle; i++)
            cartelle.Add(Cartella.GenCartella($"Cartella #{i + 1}"));

        List<int> numerisUsciti = new List<int>();
        
        List<string> smorfiaList = new List<string>();
        bool isConnected = Ping();
        if (isConnected)
        {
            Console.WriteLine("Download database smorfia in corso . . .");
            smorfiaList = Download();
        }
        
        CountDown();

        int sum = ((MAX_NUMBER * (MAX_NUMBER + 1)) / 2) - MAX_NUMBER;

        while (true)
        {
            Console.Clear();

            if (cartelle.Exists((c) => c.Punto.Equals(CalcoloPunti.Punti.Tombola)))
            {
                End(true);
                return;
            }
            else if (sum <= 0)
            {
                End(false);
                return;
            }

            int nuovoNumero = Gen(numerisUsciti);
            numerisUsciti.Add(nuovoNumero);
            sum -= nuovoNumero;

            foreach (Cartella c in cartelle)
            {
                c.Punto = CalcoloPunti.Calcolo(c, numerisUsciti);

                Console.Write(c.Nome);
                Console.WriteLine(PrintLine('─', c.Nome.Length + (Cartella.X * 4) + 8));
                c.PrintTable(numerisUsciti);
                Console.WriteLine();
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"Numero uscito: {nuovoNumero}");
        
            if (isConnected && smorfiaList.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Smorfia: {SmorfiaDecode(smorfiaList, nuovoNumero)}{Environment.NewLine}");
                Console.ResetColor();
            }

            PrintPunti(cartelle);

            if (mod == Modalita.Manuale)
                Console.ReadKey();
            else
                Thread.Sleep((int)mod);
        }
    }

    public void End(bool win, bool clear = true)
    {
        if (clear)
            Console.Clear();
        
        if (win)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nHai vinto :)");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nHai perso :(");
        }

        Console.ResetColor();
        int sc = -1;
        while(sc < 1 || sc > 2)
        {
            if (sc != -1)
                Console.Clear();

            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("1");
            Console.ResetColor();
            Console.WriteLine("] Nuova partita");

            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("2");
            Console.ResetColor();
            Console.WriteLine("] Esci");

            int.TryParse(Console.ReadLine(), out sc);
        }

        if (sc == 1)
            Program.Main();
        else
            Environment.Exit(0);
    }

    public Game(Cartelle cartelle, Modalita modalita)
    {
        numero_cartelle = cartelle;
        mod = modalita;
    }
}