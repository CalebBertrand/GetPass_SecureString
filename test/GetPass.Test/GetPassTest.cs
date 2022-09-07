using Xunit;

namespace GetPass.Test
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using Console = GetPass.Console;

    public partial class GetPassTest
    {
        [Theory]
        [InlineData(new char[0], "", "")]
        [InlineData(new[] { 'a' }, "*", "a")]
        [InlineData(new[] { 'a', 'b' }, "**", "ab")]
        [InlineData(new[] { 'a', '\b', 'b' }, "*\b \b*", "b")]
        [InlineData(new[] { 'a', '\b', '\b', 'b' }, "*\b \b*", "b")]
        [InlineData(new[] { '\0' }, "", "")]
        [InlineData(new[] { ';' }, "*", ";")]
        [InlineData(new[] { '€' }, "*", "€")]
        public void ConsoleTest(char[] characters, string expectedOutput, string expectedPassword)
        {
            // Arrange
            var console = new FakeConsole(characters);
            Console.FakeConsole = console;

            // Act
            var password = ConsolePasswordReader.Read();

            var expectedSecureString = new SecureString();
            foreach (var c in expectedPassword.ToCharArray())
                expectedSecureString.AppendChar(c);

                // Assert
            Assert.True(SecureStringEqual(expectedSecureString, password));
            Assert.Equal($"Password: {expectedOutput}\n", console.Output);
        }

        static bool SecureStringEqual(SecureString s1, SecureString s2)  
        {  
            if (s1 == null)  
            {  
                throw new ArgumentNullException("s1");  
            }  
            if (s2 == null)  
            {  
                throw new ArgumentNullException("s2");  
            }  

            if (s1.Length != s2.Length)  
            {  
                return false;  
            }  

            IntPtr bstr1 = IntPtr.Zero;  
            IntPtr bstr2 = IntPtr.Zero;  

            RuntimeHelpers.PrepareConstrainedRegions();

            try 
            {  
                bstr1 = Marshal.SecureStringToBSTR(s1);  
                bstr2 = Marshal.SecureStringToBSTR(s2);  

                unsafe 
                {  
                    for (Char* ptr1 = (Char*)bstr1.ToPointer(), ptr2 = (Char*)bstr2.ToPointer();  
                         *ptr1 != 0 && *ptr2 != 0;  
                         ++ptr1, ++ptr2)  
                    {  
                        if (*ptr1 != *ptr2)  
                        {  
                            return false;  
                        }  
                    }  
                }  

                return true;  
            }  
            finally 
            {  
                if (bstr1 != IntPtr.Zero)  
                {  
                    Marshal.ZeroFreeBSTR(bstr1);  
                }  

                if (bstr2 != IntPtr.Zero)  
                {  
                    Marshal.ZeroFreeBSTR(bstr2);  
                }  
            }  
        }
    }
}
