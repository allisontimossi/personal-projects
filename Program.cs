﻿using Microsoft.VisualBasic;
using Spectre.Console;
class Program
{
    static int cursor = 0;
    static int range = 0;
    static int attempts = 1;
//------------------PALETTE----------------------------------------------------------------------------------------------------------
    static string giallo = "" + Emoji.Known.YellowCircle;
    static string blu = "" + Emoji.Known.BlueCircle;
    static string rosso = "" + Emoji.Known.RedCircle;
    static string verde = "" + Emoji.Known.GreenCircle;
    static string viola = "" + Emoji.Known.PurpleCircle;
    static string arancione = "" + Emoji.Known.OrangeCircle;
    static string marrone = "" + Emoji.Known.BrownCircle;
    static List <string> palette = new List<string> { giallo, viola, blu, verde, rosso, arancione, marrone };
    static List <string> chosenPalette = new List<string> {};
//-----------------------------------------------------------------------------------------------------------------------------------------
    static string[] secretCode; // Codice segreto da indovinare
    static string[] guessCode;  // Codice tentativo
    static string[] hints = new string [50];
    static string[] dots = new string [50];


    static int rounds = 0;
    static bool playAgain = true;
    static string path = @"punteggi.csv";
    static string name = "";
    static double score = 0;
    static string currentDate = DateTime.Now.ToString("dd");
    static string currentMonth = DateTime.Now.ToString("MM");
    static string currentHour = DateTime.Now.ToString("HH");
    static string currentMinute = DateTime.Now.ToString("mm");
        
    static void Main(string[] args)
    {
        Console.Clear();
        CreateScoresFile();

        while (playAgain)
        {
        Console.WriteLine("Benvenuto a Mastermind! Prova a indovinare il codice di numeri.");
        DifficultyMenu();
        GenerateSecretCode();
        while (attempts > 0)
        {
            GenerateGuessCode();
            rounds++;
            EndOfTheGame();
            Table();
        }
        }
    }
    static void CreateScoresFile()
    {
        if (!File.Exists(path)) 
        {
            File.Create(path).Close();
        }
    }
    static void DifficultyMenu()
        {
            name = AnsiConsole.Prompt(new TextPrompt<string>("Contro chi sto giocando?"));
            cursor = AnsiConsole.Prompt(new TextPrompt<int>("Quanti cursori ha il codice segreto?"));
            range = AnsiConsole.Prompt(new TextPrompt<int>("Quanti colori posso scegliere"));
            chosenPalette = palette.GetRange(0, range);
            attempts = AnsiConsole.Prompt(new TextPrompt<int>("In quanti turni pensi di battermi?"));
                
        }
    static void GenerateSecretCode()
    {
        Random random = new Random(); 
        secretCode = new string [cursor];
        for (int i = 0; i < cursor; i++)
        {
            secretCode[i] = chosenPalette[random.Next(1, range)];  
        }
    }

    static bool Hints(string[] guessCode)
    {
        
        int blackDot = 0; int whiteDot = 0;  // counter black dot - white dot settato a zero
        bool[] visited = new bool[cursor];          // traccia dei numeri già controllati nel codice segreto
        bool[] guessVisited = new bool[cursor];     // traccia dei numeri già controllati nel tentativo dell'utente (per evitare di controllare lo stesso numero più volte)

        for (int i = 0; i < cursor; i++)            // counter numeri corretti posizione corretta
        {
            if (guessCode[i] == secretCode[i]) 
            {
                blackDot++;
                visited[i] = true;
                guessVisited[i] = true;
            }
        }
        for (int i = 0; i < cursor; i++)            //counter numeri corretti posizione sbagliata
        {
            if (!guessVisited[i])
            {
                for (int j = 0; j < cursor; j++)
                {
                    if (!visited[j] && guessCode[i] == secretCode[j]) // numero corretto posizione sbagliata: incrementa il contatore e segna il numero come visitato nel codice segreto
                    {
                        whiteDot++;
                        visited[j] = true;
                        break;
                    }
                }
            }
        }
        string hint = $"{Emoji.Known.BlackCircle}: {blackDot} - {Emoji.Known.WhiteCircle}: {whiteDot}";
        Console.WriteLine(hint);  
        hints [rounds-1] = hint;
        return blackDot == cursor; // restituisce vero se tutti i numeri sono corretti e nella posizione corretta, altrimenti false  
    }

    static void GenerateGuessCode()
    {
        guessCode = new string [cursor];

            for (int i = 0; i < guessCode.Length; i++)
            {
                AnsiConsole.WriteLine("\n\nScegli il tuo codice: ");
                guessCode[i] = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .PageSize(chosenPalette.Count)
                    .AddChoices(chosenPalette));
                Console.Clear();
            Console.WriteLine($"{string.Join(" ", guessCode)}");
            // decommentare se vuoi barare
            Console.WriteLine($"{string.Join(" ", secretCode)}");
            }
            string guesscode = $"{string.Join(" ", guessCode)}";
            dots [rounds] = guesscode;
            attempts--;

    }
    static void EndOfTheGame()
    {
        //calcolo del punteggio
        double value = 2;
        double power = (1-(attempts-rounds))*(cursor*range/81);
        score = Math.Pow(value, power);

        bool youWon = Hints(guessCode);
        if (youWon)
        {
            Table();
            attempts = 0;
            Console.WriteLine($"Complimenti! Hai indovinato il codice segreto in {rounds} tentativi!");
            File.AppendAllText(path, $"\n{score} - {name} - {currentDate}/{currentMonth}-{currentHour}:{currentMinute}");
            //Menu di scelta fine partita
            Console.WriteLine("Cosa vuoi fare?");
            Console.WriteLine("1. Ricomincia la partita\n2. Esci dal gioco");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
            case 1:
                rounds = 0;
                break;     
            case 2:
                Console.WriteLine("Ciao ciao");
                playAgain = false;
            break;
            }
        }
        else if (attempts == 0)
        {
            Console.WriteLine("\nMi dispiace, ma hai perso!");
            Table();
            //Menu di scelta fine partita
            Console.WriteLine("Cosa vuoi fare?");
            Console.WriteLine("1. Continua la partita \n2. Comincia una nuova partita\n3. Esci dal gioco");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Questa volta in quanti tentativi vuoi riprovare?");
                    attempts = int.Parse(Console.ReadLine ()!);
                break;
                case 2:
                    Console.WriteLine ($"Il codice era {string.Join("-", secretCode)}");
                    File.AppendAllText(path, $"\n{0} - {name} - {currentDate}/{currentMonth}-{currentHour}:{currentMinute}");
                    rounds = 0;
                    break;
                case 3:
                    playAgain = false;
                break;
            }
        }
        else if (attempts == 1)
        {
            Console.WriteLine("\nTic-tac, hai ancora un tentativo.");
        }
        else
        {
            Console.WriteLine($"\nRitenta, hai ancora {attempts} tentativi.");
        } 
        
    }
    
    static void Table()
    {
        var table = new Table();
            table.AddColumn("Round");
            table.AddColumn("Pedine");
            table.AddColumn("Suggerimenti");
            for (int i = 0; i < rounds; i++)
            {
                table.AddRow((i+1).ToString(), dots[i], hints[i]);
            }
            
            Console.Clear();
            AnsiConsole.Write(table);
    }
}

