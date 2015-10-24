namespace HelloWorld
{
    using System;

    public interface IWriter
    {
        void Write(string message);
    }

    public class Writer : IWriter
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
