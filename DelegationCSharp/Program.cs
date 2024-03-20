namespace DelegationCSharp
{
    public enum CreditCardEvent
    {
        AccountReplenished,
        MoneySpent,
        CreditStarted,
        MoneyLimitReached,
        PinChanged
    }

    public class CreditCardEventArgs : EventArgs
    {
        public CreditCardEvent Event { get; private set; }
        public double Amount { get; private set; }

        public CreditCardEventArgs(CreditCardEvent @event, double amount = 0)
        {
            Event = @event;
            Amount = amount;
        }
    }

    public interface IObserver
    {
        void Update(object sender, CreditCardEventArgs eventArgs);
    }

    public class CreditCard
    {
        public string CardNumber { get; private set; }
        public string CardHolder { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public int Pin { get; private set; }
        public double CreditLimit { get; private set; }
        public double Balance { get; private set; }

        public event EventHandler<CreditCardEventArgs> AccountReplenished;
        public event EventHandler<CreditCardEventArgs> MoneySpent;
        public event EventHandler<CreditCardEventArgs> CreditStarted;
        public event EventHandler<CreditCardEventArgs> MoneyLimitReached;
        public event EventHandler<CreditCardEventArgs> PinChanged;

        public CreditCard(string cardNumber, string cardHolder, DateTime expiryDate, int pin, double creditLimit)
        {
            CardNumber = cardNumber;
            CardHolder = cardHolder;
            ExpiryDate = expiryDate;
            Pin = pin;
            CreditLimit = creditLimit;
            Balance = 0;
        }

        public void ReplenishAccount(double amount)
        {
            Balance += amount;
            AccountReplenished?.Invoke(this, new CreditCardEventArgs(CreditCardEvent.AccountReplenished, amount));
        }

        public void SpendMoney(double amount)
        {
            if (Balance - amount < 0)
            {
                Console.WriteLine("Insufficient funds.");
                return;
            }

            Balance -= amount;
            MoneySpent?.Invoke(this, new CreditCardEventArgs(CreditCardEvent.MoneySpent, amount));
        }

        public void StartCredit()
        {
            CreditStarted?.Invoke(this, new CreditCardEventArgs(CreditCardEvent.CreditStarted));
        }

        public void ChangePin(int newPin)
        {
            Pin = newPin;
            PinChanged?.Invoke(this, new CreditCardEventArgs(CreditCardEvent.PinChanged));
        }
    }

    public class CreditCardObserver : IObserver
    {
        public string Name { get; private set; }

        public CreditCardObserver(string name)
        {
            Name = name;
        }

        public void Update(object sender, CreditCardEventArgs eventArgs)
        {
            Console.WriteLine($"Observer '{Name}' received event: {eventArgs.Event}");

            if (eventArgs.Event == CreditCardEvent.AccountReplenished)
            {
                Console.WriteLine($"Account replenished with amount: {eventArgs.Amount}");
            }
            else if (eventArgs.Event == CreditCardEvent.MoneySpent)
            {
                Console.WriteLine($"Money spent with amount: {eventArgs.Amount}");
            }
            else if (eventArgs.Event == CreditCardEvent.CreditStarted)
            {
                Console.WriteLine("Credit started.");
            }
            else if (eventArgs.Event == CreditCardEvent.MoneyLimitReached)
            {
                Console.WriteLine("Money limit reached.");
            }
            else if (eventArgs.Event == CreditCardEvent.PinChanged)
            {
                Console.WriteLine("PIN changed.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string cardNumber;
            string cardHolder;
            DateTime expiryDate;
            int pin;
            double creditLimit;
            double initialBalance;

            Console.WriteLine("Enter card number:");
            cardNumber = Console.ReadLine();

            Console.WriteLine("Enter card holder name:");
            cardHolder = Console.ReadLine();

            Console.WriteLine("Enter card expiry date (MM/YYYY):");
            while (!DateTime.TryParseExact(Console.ReadLine(), "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out expiryDate))
            {
                Console.WriteLine("Invalid date format. Please enter the date in MM/YYYY format:");
            }

            Console.WriteLine("Enter PIN:");
            while (!int.TryParse(Console.ReadLine(), out pin) || pin < 1000 || pin > 9999)
            {
                Console.WriteLine("Invalid PIN. Please enter a 4-digit PIN:");
            }

            Console.WriteLine("Enter credit limit:");
            while (!double.TryParse(Console.ReadLine(), out creditLimit) || creditLimit < 0)
            {
                Console.WriteLine("Invalid credit limit. Please enter a non-negative value:");
            }

            Console.WriteLine("Enter initial balance:");
            while (!double.TryParse(Console.ReadLine(), out initialBalance) || initialBalance < 0)
            {
                Console.WriteLine("Invalid initial balance. Please enter a non-negative value:");
            }

            CreditCard creditCard = new CreditCard(cardNumber, cardHolder, expiryDate, pin, creditLimit);

            CreditCardObserver observer1 = new CreditCardObserver("Observer 1");
            CreditCardObserver observer2 = new CreditCardObserver("Observer 2");

            creditCard.AccountReplenished += observer1.Update;
            creditCard.AccountReplenished += observer2.Update;
            creditCard.MoneySpent += observer1.Update;
            creditCard.MoneySpent += observer2.Update;

            ThreadPool.QueueUserWorkItem(o =>
            {
                creditCard.ReplenishAccount(initialBalance);
                Thread.Sleep(1000);
                creditCard.SpendMoney(300);
            });

            creditCard.AccountReplenished -= observer2.Update;
            creditCard.MoneySpent -= observer1.Update;

            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.Sleep(2000);
                creditCard.SpendMoney(800);
                creditCard.ChangePin(5678);
            });

            Console.ReadLine();
        }
    }
}