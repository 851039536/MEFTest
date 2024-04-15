using System;
using System.ComponentModel.Composition;
using MEFTest;

namespace EmailSender
{
    [Export(typeof(IMessageSender))]
    public class SmsSender : IMessageSender
    {
        public void Send(string message)
        {
            Console.WriteLine($@"SmsSender: {message}");
            // 这里可以添加实际的短信发送代码  
        }
    }
}