using System;
using System.Collections.Generic;
using System.Linq;

class Card
{
    public string Suit { get; private set; }
    public string Rank { get; private set; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int GetValue()
    {
        if (int.TryParse(Rank, out int value))
            return value;
        if (Rank == "Ace")
            return 11; // Aces are high in this simplified version
        return 10; // Face cards are worth 10
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

class Deck
{
    private List<Card> cards;
    private Random random = new Random();

    public Deck()
    {
        cards = new List<Card>();
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public Card Deal()
    {
        if (cards.Count == 0)
            throw new InvalidOperationException("No cards left in the deck.");
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}

class Hand
{
    public List<Card> Cards { get; private set; }

    public Hand()
    {
        Cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
    }

    public int CalculateScore()
    {
        return Cards.Sum(card => card.GetValue());
    }

    public override string ToString()
    {
        return string.Join(", ", Cards.Select(c => c.ToString()));
    }
}

class Game
{
    private Deck deck;
    private Hand playerHand;
    private Hand dealerHand;
    private Random random;

    public Game()
    {
        deck = new Deck();
        playerHand = new Hand();
        dealerHand = new Hand();
        random = new Random();
        deck.Shuffle();
    }

    public void Start()
    {
        playerHand.AddCard(deck.Deal());
        playerHand.AddCard(deck.Deal());
        dealerHand.AddCard(deck.Deal());
        dealerHand.AddCard(deck.Deal());

        Console.WriteLine("Your hand: " + playerHand);
        Console.WriteLine("Dealer shows: " + dealerHand.Cards.First());

        while (playerHand.CalculateScore() < 21 && PromptForHit())
        {
            playerHand.AddCard(deck.Deal());
            Console.WriteLine("Your hand: " + playerHand);
        }

        if (playerHand.CalculateScore() > 21)
        {
            Console.WriteLine("Bust! Dealer wins.");
            return;
        }

        while (dealerHand.CalculateScore() < 17)
        {
            dealerHand.AddCard(deck.Deal());
        }

        Console.WriteLine("Dealer's hand: " + dealerHand);
        if (dealerHand.CalculateScore() > 21 || playerHand.CalculateScore() > dealerHand.CalculateScore())
        {
            Console.WriteLine("You win!");
        }
        else
        {
            Console.WriteLine("Dealer wins.");
        }
    }

    private bool PromptForHit()
    {
        Console.WriteLine("Would you like to hit? (y/n)");
        return Console.ReadLine().ToLower() == "y";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        string playAgain = "y";
        while (playAgain == "y")
        {
            game.Start();
            Console.WriteLine("Would you like to play again? (y/n)");
            playAgain = Console.ReadLine().ToLower();
        }
    }
}
