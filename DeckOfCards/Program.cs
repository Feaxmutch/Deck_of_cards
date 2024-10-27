namespace DeckOfCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new();
            game.Start();
        }
    }

    public static class UserUtilits
    {
        public static void Shuffle<T>(List<T> array)
        {
            Random random = new Random();

            for (int i = 0; i < array.Count; i++)
            {
                int randomIndex = random.Next(0, array.Count);
                SwapValues(array, i, randomIndex);
            }
        }

        private static void SwapValues<T>(List<T> array, int firstIndex, int secondIndex)
        {
            T numberBuffer = array[firstIndex];
            array[firstIndex] = array[secondIndex];
            array[secondIndex] = numberBuffer;
        }
    }

    public class Game
    {
        private const string CommandEnd = "end";
        private const string CommandRestart = "restart";

        private Dealer _dealer;
        private Player _player;
        private bool _isRunning = false;

        public Game()
        {
            _dealer = new Dealer();
            _player = new Player();
        }

        public void Start()
        {
            _isRunning = true;
            _dealer.ShuffleCards();
            RunGameloop();
        }

        public void End()
        {
            _dealer.TakeCards(_player.ReturnCards());
            _isRunning = false;
        }

        public void Restart()
        {
            End();
            Start();
        }

        public void RunGameloop()
        {
            while (_isRunning)
            {
                Console.Clear();
                _player.ShowCards();
                Console.WriteLine("Введите end, restart или нажмите ввод чтобы взять карту");
                string userCommand = Console.ReadLine();

                switch (userCommand)
                {
                    default:
                        _player.TakeCard(_dealer.GiveNextCard());
                        break;

                    case CommandEnd:
                        End();
                        break;

                    case CommandRestart:
                        Restart();
                        break;
                }
            }
        }
    }

    public class Dealer
    {
        private int _cardsDublicates = 3;
        private Deck _deck;

        public Dealer()
        {
            CreateDeck();
        }

        public Card GiveNextCard()
        {
            return _deck.GetNextCard();
        }

        public void TakeCards(List<Card> cards)
        {
            _deck.TakeCadrs(cards);
        }

        public void ShuffleCards()
        {
            _deck.ShuffleCards();
        }

        public void CreateDeck()
        {
            _deck = new Deck(_cardsDublicates);
        }
    }

    public class Player
    {
        private List<Card> _cards;

        public Player()
        {
            _cards = new();
        }

        public void ShowCards()
        {
            Console.WriteLine("Карты в руках:");

            foreach (var card in _cards)
            {
                Console.WriteLine($"ранг карты {card.Rank}");
            }
        }

        public void TakeCard(Card card)
        {
            _cards.Add(card);
        }

        public List<Card> ReturnCards()
        {
            List<Card> cardsForReturn = new(_cards);
            _cards.Clear();
            return cardsForReturn;
        }

        public void ClearCards()
        {
            _cards.Clear();
        }
    }

    public class Deck
    {
        private Stack<Card> _cards;

        public Deck(int cardDublicates)
        {
            _cards = new(CreateDeck(cardDublicates));

        }

        public Card GetNextCard()
        {
            return _cards.Pop();
        }

        public void TakeCadrs(List<Card> cards)
        {
            foreach (var card in cards)
            {
                _cards.Push(card);
            }
        }

        private List<Card> CreateDeck(int cardDublicates)
        {
            List<Card> cards = new();

            for (int i = 0; i < cardDublicates; i++)
            {
                for (int rank = Card.MinRank; rank <= Card.MaxRank; rank++)
                {
                    cards.Add(new Card(rank));
                }
            }

            return cards;
        }

        public void ShuffleCards()
        {
            List<Card> cards = _cards.ToList();
            UserUtilits.Shuffle(cards);
            _cards = new(cards);
        }
    }

    public class Card
    {
        private static int _minRank = 0;  
        private static int _maxRank = 15;

        public Card(int rank)
        {
            Rank = rank;
        }

        public static int MinRank { get => _minRank; }

        public static int MaxRank { get => _maxRank; }

        public int Rank { get; }
    }
}
