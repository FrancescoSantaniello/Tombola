namespace tombola;
internal abstract class CalcoloPunti
{
    public enum Punti : int
    {
        Ambo = 2,
        Terna = 3, 
        Quaterna = 4,
        Cinquina = 5, 
        Tombola = (Cartella.X * Cartella.Y) - (Cartella.EMPTY_SPACE * Cartella.Y), 
        Niente = 0
    }

    public static Punti Calcolo(Cartella c, List<int> numeriUsciti)
    {
        if (c is null || numeriUsciti is null) 
            throw new ArgumentNullException("Cartella non valida");

        Punti punto = Punti.Niente;

        List<int> points = new();
        foreach (List<string> row in c.ListCartella)
        {
            int caselleUscite = 0;
            foreach(string cell in row)
            {
                int cellInt;
                if (int.TryParse(cell, out cellInt))
                {
                    if (numeriUsciti.Contains(cellInt))
                        caselleUscite++;
                }
            }

            points.Add(caselleUscite);
        }

        if (points.Sum() == (int)Punti.Tombola)
            return Punti.Tombola;

        points.Sort();

        switch (points[points.Count - 1])
        {
            case 0 or 1:
                punto = Punti.Niente;
                break;
            case 2:
                punto = Punti.Ambo;
                break;
            case 3:
                punto = Punti.Terna;
                break;
            case 4:
                punto = Punti.Quaterna;
                break;
            case 5:
                punto = Punti.Cinquina;
                break;
        }

        return punto;
    }
}