namespace tombola;
class Cartella
{
    public const char EMPTY_CELL = '□';
    public const int X = 9;
    public const int Y = 3;
    public const int EMPTY_SPACE = 4;
    public const char SPACE_FROM_CELL = '\t';
    public const char REPLACE_CHAR = '■';
    public List<List<string>> ListCartella { get; } = new();
    private CalcoloPunti.Punti _punto;
    private string? _nome;
    public CalcoloPunti.Punti Punto
    {
        get => _punto;
        set => _punto = value;
    }
    public string? Nome
    {
        get => _nome;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Nome non valido");
            _nome = value;
        }
    }
    public void PrintTable(List<int> numeriUsciti)
    {
        if (ListCartella.Count <= 0)
            return;

        foreach (List<string> l2 in ListCartella)
        {
            foreach (string c in l2)
            {
                int value;
                if (c != EMPTY_CELL.ToString() && int.TryParse(c, out value))
                {
                    if (numeriUsciti.Contains(value))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(REPLACE_CHAR);
                    }
                    else
                        Console.Write(c);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(c);
                }

                Console.ResetColor();
                Console.Write(SPACE_FROM_CELL);
            }
            Console.WriteLine();
        }
    }
    private int Gen(List<int> ex, int min = 1, int max = 90)
    {
        Random randomClass = new Random();

        int gen = randomClass.Next(min,max);
        
        while (ex.Contains(gen))
            gen = randomClass.Next(min,max);
        
        return gen;
    }
    private List<int> GenPosEmptySpace()
    {
        List<int> listaPosizioneVuote = new List<int>();
        List<int> listaPosizioniUscite = new List<int>();
        
        for (int i = 0; i < EMPTY_SPACE; i++)
        {
            int gen = Gen(listaPosizioniUscite,0,X);
            listaPosizioneVuote.Add(gen);
            listaPosizioniUscite.Add(gen);
        }
        
        return listaPosizioneVuote;
    }
    private void GenTable()
    {
		ListCartella.Clear();
        List<int> numeriUsciti = new List<int>();
        for(int y = 0; y < Y; y++)
        {
            List<string> rows = new List<string>();
            List<int> emptySpacePosition = GenPosEmptySpace();

            for (int x = 0; x < X; x++)
            {
                int min = x*10;
                int max = (x+1)*10;

                if (!emptySpacePosition.Contains(x))
                {
                    int number = Gen(numeriUsciti, (min <= 0) ? ++min : min, max);
                    numeriUsciti.Add(number);
                    rows.Add(number.ToString());
                }
                else
                    rows.Add(EMPTY_CELL.ToString());
            }

			ListCartella.Add(rows);
        }
    }
    public static Cartella GenCartella(string nome)
    {
        Cartella cartella = new();
        cartella.Nome = nome;
        cartella.GenTable();
        return cartella;
    }
}