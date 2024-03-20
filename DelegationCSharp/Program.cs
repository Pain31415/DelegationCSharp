namespace DelegationCSharp
{
    public interface IOperation
    {
        double PerformOperation(double x, double y);
        string GetSymbol();
    }

    public class AddOperation : IOperation
    {
        public double PerformOperation(double x, double y) => x + y;
        public string GetSymbol() => "+";
    }

    public class SubtractOperation : IOperation
    {
        public double PerformOperation(double x, double y) => x - y;
        public string GetSymbol() => "-";
    }

    public class MultiplyOperation : IOperation
    {
        public double PerformOperation(double x, double y) => x * y;
        public string GetSymbol() => "*";
    }

    public class OperationFactory
    {
        public static IOperation CreateOperation(string operationName)
        {
            return operationName switch
            {
                "Addition" => new AddOperation(),
                "Subtraction" => new SubtractOperation(),
                "Multiplication" => new MultiplyOperation(),
                _ => throw new ArgumentException("Invalid operation name"),
            };
        }
    }

    public class Calculator
    {
        private Dictionary<string, IOperation> operations = new Dictionary<string, IOperation>();

        public Calculator()
        {
            RegisterOperation("Addition");
            RegisterOperation("Subtraction");
            RegisterOperation("Multiplication");
        }

        private void RegisterOperation(string operationName)
        {
            operations[operationName] = OperationFactory.CreateOperation(operationName);
        }

        public double PerformOperation(string operationName, double x, double y)
        {
            if (!operations.ContainsKey(operationName))
                throw new ArgumentException("Invalid operation");

            return operations[operationName].PerformOperation(x, y);
        }

        public string GetSymbol(string operationName)
        {
            if (!operations.ContainsKey(operationName))
                throw new ArgumentException("Invalid operation");

            return operations[operationName].GetSymbol();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();

            double x = 13;
            double y = 7;

            foreach (var operationName in new string[] { "Addition", "Subtraction", "Multiplication" })
            {
                double result = calculator.PerformOperation(operationName, x, y);
                Console.WriteLine($"{operationName}: {x} {calculator.GetSymbol(operationName)} {y} = {result}");
            }
        }
    }
}