namespace MEFTest  
{  
    /// <summary>  
    /// 定义了消息发送者的契约。  
    /// 实现此接口的类将能够提供发送消息的功能。  
    /// 这将作为扩展点，供插件实现具体的消息发送逻辑。  
    /// </summary>  
    public interface IMessageSender2  
    {  
        /// <summary>  
        /// 发送一条消息。  
        /// </summary>  
        /// <param name="message">要发送的消息内容。</param>  
        void Send2(string message);  
    }  
}