namespace DelegationCSharp
{
    public interface IArithmeticOperation
    {
        bool Check(int number);
    }

    public class EvenCheck : IArithmeticOperation
    {
        public bool Check(int number) => number % 2 == 0;
    }

    public class OddCheck : IArithmeticOperation
    {
        public bool Check(int number) => number % 2 != 0;
    }

    public class PrimeCheck : IArithmeticOperation
    {
        public bool Check(int number)
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
    }

    public class FibonacciCheck : IArithmeticOperation
    {
        public bool Check(int number)
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

    public class ArithmeticChecker
    {
        private Dictionary<string, IArithmeticOperation> operations = new Dictionary<string, IArithmeticOperation>();

        public ArithmeticChecker()
        {
            RegisterOperation("Even", new EvenCheck());
            RegisterOperation("Odd", new OddCheck());
            RegisterOperation("Prime", new PrimeCheck());
            RegisterOperation("Fibonacci", new FibonacciCheck());
        }

        private void RegisterOperation(string operationName, IArithmeticOperation operation)
        {
            operations[operationName] = operation;
        }

        public bool PerformOperation(string operationName, int number)
        {
            if (!operations.ContainsKey(operationName))
                throw new ArgumentException("Invalid operation");

            return operations[operationName].Check(number);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ArithmeticChecker checker = new ArithmeticChecker();

            int number = 13;

            Console.WriteLine($"Is {number} even? {checker.PerformOperation("Even", number)}");
            Console.WriteLine($"Is {number} odd? {checker.PerformOperation("Odd", number)}");
            Console.WriteLine($"Is {number} prime? {checker.PerformOperation("Prime", number)}");
            Console.WriteLine($"Is {number} a Fibonacci number? {checker.PerformOperation("Fibonacci", number)}");
        }
    }
}
