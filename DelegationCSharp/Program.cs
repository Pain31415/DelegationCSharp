namespace DelegationCSharp
{
    public enum OperationType
    {
        Addition,
        Subtraction,
        Multiplication
    }

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

    public class Calculator
    {
        private Dictionary<OperationType, Func<double, double, double>> operations = new Dictionary<OperationType, Func<double, double, double>>();

        public Calculator()
        {
            RegisterOperation(OperationType.Addition, new AddOperation().PerformOperation);
            RegisterOperation(OperationType.Subtraction, new SubtractOperation().PerformOperation);
            RegisterOperation(OperationType.Multiplication, new MultiplyOperation().PerformOperation);
        }

        public void RegisterOperation(OperationType operationType, Func<double, double, double> operation)
        {
            operations[operationType] = operation;
        }

        public double PerformOperation(OperationType operationType, double x, double y)
        {
            if (!operations.ContainsKey(operationType))
                throw new ArgumentException("Invalid operation");

            return operations[operationType].Invoke(x, y);
        }

        public string GetSymbol(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Addition:
                    return new AddOperation().GetSymbol();
                case OperationType.Subtraction:
                    return new SubtractOperation().GetSymbol();
                case OperationType.Multiplication:
                    return new MultiplyOperation().GetSymbol();
                default:
                    throw new ArgumentException("Invalid operation");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();

            double x = 13;
            double y = 7;

            foreach (OperationType operationType in Enum.GetValues(typeof(OperationType)))
            {
                double result = calculator.PerformOperation(operationType, x, y);
                Console.WriteLine($"{operationType}: {x} {calculator.GetSymbol(operationType)} {y} = {result}");
            }
        }
    }
}
