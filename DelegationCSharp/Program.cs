namespace DelegationCSharp
{
    public enum Strategy
    {
        Even,
        Odd,
        Prime,
        Fibonacci
    }

    public class ArrayOperations
    {
        private Dictionary<Strategy, Func<int, bool>> strategies = new Dictionary<Strategy, Func<int, bool>>();

        public ArrayOperations()
        {
            strategies.Add(Strategy.Even, IsEven);
            strategies.Add(Strategy.Odd, IsOdd);
            strategies.Add(Strategy.Prime, IsPrime);
            strategies.Add(Strategy.Fibonacci, IsFibonacci);
        }

        public List<int> GetNumbersByCondition(int[] array, Strategy strategy)
        {
            if (!strategies.ContainsKey(strategy))
                throw new ArgumentException("Invalid strategy");

            List<int> result = new List<int>();
            foreach (int num in array)
            {
                if (strategies[strategy](num))
                {
                    result.Add(num);
                }
            }
            return result;
        }

        private bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        private bool IsOdd(int number)
        {
            return number % 2 != 0;
        }

        private bool IsPrime(int number)
        {
            if (number <= 1)
                return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }

        private bool IsFibonacci(int number)
        {
            if (number <= 1)
                return true;

            int a = 0;
            int b = 1;
            while (b < number)
            {
                int temp = a + b;
                a = b;
                b = temp;
            }
            return b == number;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ArrayOperations arrayOperations = new ArrayOperations();
            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Strategy[] strategies = { Strategy.Even, Strategy.Odd, Strategy.Prime, Strategy.Fibonacci };
            foreach (Strategy strategy in strategies)
            {
                Console.WriteLine($"{strategy} numbers:");
                List<int> numbers = arrayOperations.GetNumbersByCondition(array, strategy);
                foreach (int num in numbers)
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
