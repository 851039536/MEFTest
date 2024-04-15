using System;
using EmailSender;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            while (true)
            {
                var pluginLoader = new PluginLoader();  
                pluginLoader.LoadPlugins(@"D:\sw\Console\MEFTest\EmailSender\bin\Debug"); // 替换为你的插件路径  
                Console.WriteLine("回车执行dll");
                Console.ReadKey();
                pluginLoader.Run();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine( ex.Message);
        }
    }


  

  

  

  
    

    
}