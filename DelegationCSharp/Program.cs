namespace DelegationCSharp
{
    public delegate void DisplayMessageDelegate(string message);

    public class MessageDisplayer
    {
        public static void DisplayMessageInConsole(string message)
        {
            Console.WriteLine("Programming is PAIN (Console): " + message);
        }

        public static void DisplayMessageInWindow(string message)
        {
            Console.WriteLine("Programming is PAIN (Window): " + message);
        }

        public static void DisplayMessageInLinux(string message)
        {
            Console.WriteLine("Programming is PAIN (Linux): " + message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DisplayMessageDelegate consoleDelegate = MessageDisplayer.DisplayMessageInConsole;
            DisplayMessageDelegate windowDelegate = MessageDisplayer.DisplayMessageInWindow;
            DisplayMessageDelegate linuxDelegate = MessageDisplayer.DisplayMessageInLinux;

            consoleDelegate("Hello, this is displayed in the console.");
            windowDelegate("Hello, this is displayed in the window.");
            linuxDelegate("Hello, this is displayed in the Linux environment.");
        }
    }
}
