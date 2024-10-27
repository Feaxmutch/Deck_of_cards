namespace DeckOfCards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new(0, 15);
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

        public Game(int minCardRank, int maxCardRank)
        {
            _dealer = new Dealer(minCardRank, maxCardRank);
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
                        GiveCardToPlayer();
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

        public void GiveCardToPlayer()
        {
            if (_dealer.TryGiveNextCard(out Card card))
            {
                _player.TakeCard(card);
            }
            else
            {
                Console.WriteLine("Карт не осталось");
                Console.ReadKey();
            }
        }
    }

    public class Dealer
    {
        private int _cardsDublicates = 3;
        private Deck _deck;

        public Dealer(int minCardRank, int maxCardRank)
        {
            CreateDeck(minCardRank, maxCardRank);
        }

        public bool TryGiveNextCard(out Card card)
        {
            return _deck.TryGetNextCard(out card);
        }

        public void TakeCards(List<Card> cards)
        {
            _deck.TakeCadrs(cards);
        }

        public void ShuffleCards()
        {
            _deck.ShuffleCards();
        }

        public void CreateDeck(int minCardRank, int maxCardRank)
        {
            _deck = new Deck(_cardsDublicates, minCardRank, maxCardRank);
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
        private int _minCardRank = 0;
        private int _maxCardRank = 15;

        private Stack<Card> _cards;

        public Deck(int cardDublicates, int minCardRank, int maxCardRank)
        {
            _cards = new(CreateCards(cardDublicates, minCardRank, maxCardRank));

        }

        public bool TryGetNextCard(out Card card)
        {
            return _cards.TryPop(out card);
        }

        public void TakeCadrs(List<Card> cards)
        {
            foreach (var card in cards)
            {
                _cards.Push(card);
            }
        }

        public void ShuffleCards()
        {
            List<Card> cards = _cards.ToList();
            UserUtilits.Shuffle(cards);
            _cards = new(cards);
        }

        private List<Card> CreateCards(int cardDublicates, int minCardRank, int maxCardRank)
        {
            List<Card> cards = new();

            for (int i = 0; i < cardDublicates; i++)
            {
                for (int rank = minCardRank; rank <= maxCardRank; rank++)
                {
                    cards.Add(new Card(rank));
                }
            }

            return cards;
        }
    }

    public class Card
    {
        public Card(int rank)
        {
            Rank = rank;
        }

        public int Rank { get; }
    }
}
