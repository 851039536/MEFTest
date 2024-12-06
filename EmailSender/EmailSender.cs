using System;
using System.ComponentModel.Composition;

namespace EmailSender
{
    [Export(typeof(IMessageSender))] // 导出为 IMessageSender  
    [Export(typeof(IMessageSender2))] // 导出为 IMessageSender2  
    public class EmailSender : IMessageSender, IMessageSender2
    {
        public void Send(string message)
        {
            Console.WriteLine($@"EmailSender : {message}");
        }
        
        public void Send2(string message)
        {
            Console.WriteLine($@"EmailSender Send2: {message}");
        }
    }
}