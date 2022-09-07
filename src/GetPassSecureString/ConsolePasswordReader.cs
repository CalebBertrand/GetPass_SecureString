namespace GetPassSecureString
{
    using System.Security;

    public class ConsolePasswordReader
    {
        public static SecureString Read(string prompt = "Password: ")
        {
            var password = new SecureString();
            
            Console.Write(prompt);
            do
            {
                var key = Console.ReadKey(true).KeyChar;

                if (key == '\b')
                {
                    if (password.Length > 0)
                    {
                        password.RemoveAt(password.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (key == '\r')
                {
                    Console.WriteLine();
                    break;
                }
                else if (!char.IsControl(key))
                {
                    password.AppendChar(key);
                    Console.Write('*');
                }
            } while (true);
            
            return password;
        }
    }
}
