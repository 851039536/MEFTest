using System;
using System.ComponentModel.Composition;

namespace EmailSender
{
    [Export(typeof(IMessageSender))]
    public class SmsSender : IMessageSender
    {
        public void Send(string message)
        {
            Console.WriteLine($@"SmsSender: {message}");
        }
    }
}