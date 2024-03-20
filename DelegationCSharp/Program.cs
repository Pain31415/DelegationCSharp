namespace DelegationCSharp
{
    public enum Operation
    {
        CurrentTime,
        CurrentDate,
        CurrentDayOfWeek,
        TriangleArea,
        RectangleArea
    }

    public interface IOperation
    {
        string Execute();
        void SetParameters(double[] parameters);
        double[] GetParameters();
    }

    public class TimeOperation : IOperation
    {
        public string Execute() => DateTime.Now.ToLongTimeString();

        public void SetParameters(double[] parameters) { }

        public double[] GetParameters() => new double[0];
    }

    public class DateOperation : IOperation
    {
        public string Execute() => DateTime.Today.ToShortDateString();

        public void SetParameters(double[] parameters) { }

        public double[] GetParameters() => new double[0];
    }

    public class DayOfWeekOperation : IOperation
    {
        public string Execute() => DateTime.Today.DayOfWeek.ToString();

        public void SetParameters(double[] parameters) { }

        public double[] GetParameters() => new double[0];
    }

    public class TriangleAreaOperation : IOperation
    {
        private double[] parameters;

        public string Execute()
        {
            if (parameters == null || parameters.Length != 3)
                throw new InvalidOperationException("TriangleArea operation parameters are not set correctly");

            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];

            double p = (a + b + c) / 2;
            double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

            return $"Triangle area with sides {a}, {b}, {c} is {area:F2}";
        }

        public void SetParameters(double[] parameters)
        {
            if (parameters == null || parameters.Length != 3)
                throw new ArgumentException("TriangleArea operation requires 3 parameters");

            this.parameters = parameters;
        }

        public double[] GetParameters() => parameters;
    }

    public class RectangleAreaOperation : IOperation
    {
        private double[] parameters;

        public string Execute()
        {
            if (parameters == null || parameters.Length != 2)
                throw new InvalidOperationException("RectangleArea operation parameters are not set correctly");

            double length = parameters[0];
            double width = parameters[1];

            double area = length * width;

            return $"Rectangle area with length {length} and width {width} is {area:F2}";
        }

        public void SetParameters(double[] parameters)
        {
            if (parameters == null || parameters.Length != 2)
                throw new ArgumentException("RectangleArea operation requires 2 parameters");

            this.parameters = parameters;
        }

        public double[] GetParameters() => parameters;
    }

    public class Calculator
    {
        private IOperation[] operations = new IOperation[Enum.GetValues(typeof(Operation)).Length];

        public Calculator()
        {
            operations[(int)Operation.CurrentTime] = new TimeOperation();
            operations[(int)Operation.CurrentDate] = new DateOperation();
            operations[(int)Operation.CurrentDayOfWeek] = new DayOfWeekOperation();
            operations[(int)Operation.TriangleArea] = new TriangleAreaOperation();
            operations[(int)Operation.RectangleArea] = new RectangleAreaOperation();
        }

        public string PerformOperation(Operation operation, params double[] parameters)
        {
            if ((int)operation >= operations.Length || operations[(int)operation] == null)
                throw new ArgumentException("Invalid operation");

            IOperation op = operations[(int)operation];
            op.SetParameters(parameters);

            return op.Execute();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();

            Console.WriteLine("Current time: " + calculator.PerformOperation(Operation.CurrentTime));
            Console.WriteLine("Current date: " + calculator.PerformOperation(Operation.CurrentDate));
            Console.WriteLine("Current day of week: " + calculator.PerformOperation(Operation.CurrentDayOfWeek));
            Console.WriteLine(calculator.PerformOperation(Operation.TriangleArea, 3, 4, 5));
            Console.WriteLine(calculator.PerformOperation(Operation.RectangleArea, 6, 8));
        }
    }
}