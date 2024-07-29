using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using Spectre.Console;

class Program
{
    static int difficultyLevel = 0;
    static int range = 0;
    static int[] secretCode; // Codice segreto da indovinare
    static int[] guessCode = new int [difficultyLevel]; // Codice tentativo
    static int attempts = 10; // Tentativi effettuati dall'utente
    static int rounds = 1;
    static bool youWon = true;

    static void Main(string[] args)
    {
        while (true)
        {
        Console.WriteLine("Benvenuto a Mastermind! Prova a indovinare il codice di numeri.");

        DifficultyMenu();
        GenerateSecretCode();
        while (youWon)
        {
            GenerateGuessCode();
        }    
        }
    }

    static void GenerateSecretCode()
    {
        Random random = new Random(); // Generare un codice segreto casuale di difficultyLevel numeri
        secretCode = new int[difficultyLevel];
        for (int i = 0; i < difficultyLevel; i++)
        {
            secretCode[i] = random.Next(1, range);  // Numeri da 1 a 6
        }
        // commentare se vuoi barare
        Console.WriteLine($"{string.Join("-", secretCode)}");
    }

    static bool Hints(int[] guessCode)
    {
        int blackDot = 0, whiteDot = 0; // counter black dot - white dot settato a zero
        bool[] visited = new bool[difficultyLevel]; // traccia dei numeri già controllati nel codice segreto
        bool[] guessVisited = new bool[difficultyLevel]; // traccia dei numeri già controllati nel tentativo dell'utente (per evitare di controllare lo stesso numero più volte)

        for (int i = 0; i < difficultyLevel; i++)   // counter numeri corretti posizione corretta
        {
            if (guessCode[i] == secretCode[i]) 
            {
                blackDot++;
                visited[i] = true;
                guessVisited[i] = true;
            }
        }
        for (int i = 0; i < difficultyLevel; i++)   //counter numeri corretti posizione sbagliata
        {
            if (!guessVisited[i])
            {
                for (int j = 0; j < difficultyLevel; j++)
                {
                    if (!visited[j] && guessCode[i] == secretCode[j]) // Se il numero è corretto ma non nella posizione corretta incrementare il contatore e segnare il numero come visitato nel codice segreto
                    {
                        whiteDot++;
                        visited[j] = true;
                        break;
                    }
                }
            }
        }
        Console.WriteLine($"Tentativo {rounds}: {string.Join("-", guessCode)} - Neri: {blackDot} - Bianchi: {whiteDot}");
        return blackDot == difficultyLevel; // Restituire true se tutti i numeri sono corretti e nella posizione corretta altrimenti false
    }

    static void DifficultyMenu()
    {
        Console.WriteLine("Scegli la difficoltà: di quante cifre è composto il codice segreto?");
        difficultyLevel = int.Parse(Console.ReadLine ()!);

        Console.WriteLine("Scegli la difficoltà: tra quanti numeri vuoi scegliere? (1-9)");
        range = int.Parse(Console.ReadLine ()!);

        Console.WriteLine("Scegli la difficoltà: in quanti turni pensi di indovinare?");
        attempts = int.Parse(Console.ReadLine ()!);
    }
    
    static void GenerateGuessCode()
    {
            Console.Write("Inserisci il tuo tentativo: ");
            string input = Console.ReadLine()!;
            guessCode = Array.ConvertAll(input.Split('-'), int.Parse);

            string guesscode = string.Join("-", guessCode);
            Console.WriteLine(guesscode);

            youWon = Hints(guessCode); // Valutare il tentativo dell'utente e controllare se è corretto
        
        attempts--;
        rounds++;

        if (youWon)
            {
                Console.WriteLine($"Complimenti! Hai indovinato il codice segreto in {rounds} tentativi!");
                Console.WriteLine("Cosa vuoi fare?");
                Console.WriteLine("1. Ricomincia la partita\n2. Esci dal gioco");
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 1)
                {
                    rounds = 0;
                    youWon = false;
                }        
                if (choice == 2)
                {
                    
                    Console.WriteLine("Ciao ciao");
                }
            }
        else if (attempts == 0)
        {
            Console.WriteLine("\nMi dispiace, ma hai perso!");
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
                    youWon = false;
                break;
                case 3:
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
}
