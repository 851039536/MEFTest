using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MEFTest;

namespace EmailSender
{
    /// <summary>
    /// 负责插件的加载和卸载 
    /// </summary>
    public class PluginLoader
    {
        // 用于加载插件的独立AppDomain  
        private AppDomain _pluginDomain;

        // 远程插件加载器，它在_pluginDomain中执行  
        private RemotePluginLoader _remoteLoader;

        /// <summary>
        /// 初始化_pluginDomain和_remoteLoader  
        /// </summary>
        public PluginLoader()
        {
            // 创建一个新的AppDomain来加载插件  
            _pluginDomain = AppDomain.CreateDomain("PluginDomain");
            // 在新的AppDomain中创建RemotePluginLoader的实例  
            _remoteLoader = (RemotePluginLoader)_pluginDomain.CreateInstanceAndUnwrap(
                typeof(RemotePluginLoader).Assembly.FullName,
                typeof(RemotePluginLoader).FullName);
        }

        /// <summary>
        /// 加载指定路径下的插件  
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugins(string path)
        {
            _remoteLoader.LoadPlugins(path);
        }

        /// <summary>
        /// 卸载已加载的插件，通过卸载并重新创建AppDomain来实现  
        /// </summary>
        public void UnloadPlugins()
        {
            // 通过卸载AppDomain来卸载插件  
            AppDomain.Unload(_pluginDomain);
            _pluginDomain = null;
            // 重新创建一个新的AppDomain以备后续加载插件  
            _pluginDomain = AppDomain.CreateDomain("PluginDomain");
            var fullName = typeof(RemotePluginLoader).FullName;
            if (fullName != null)
                // 重新创建一个新的AppDomain和远程加载器  
                _remoteLoader = (RemotePluginLoader)_pluginDomain.CreateInstanceAndUnwrap(
                    typeof(RemotePluginLoader).Assembly.FullName,
                    fullName);
        }

        /// <summary>
        /// 运行已加载的插件  
        /// </summary>
        public void Run()
        {
            _remoteLoader.Run();
            // UnloadPlugins();
            // 注意：这里注释掉了UnloadPlugins()方法，可能是为了在某些场景下保持插件的加载状态  
            // 如果需要在运行后立即卸载插件，可以取消此行的注释  
        }
    }

    /// <summary>
    /// RemotePluginLoader类在独立的AppDomain中执行MEF的加载和组合操作
    /// MarshalByRefObject允许该类的实例跨AppDomain边界进行通信  
    /// </summary>
    public class RemotePluginLoader : MarshalByRefObject
    {
        // MEF的组合容器 
        private CompositionContainer _container;

        // MEF的聚合目录，用于收集多个目录或程序集  
        private AggregateCatalog _catalog;

        public RemotePluginLoader()
        {
            _catalog = new AggregateCatalog();
            _container = new CompositionContainer(_catalog);
        }

        /// <summary>
        /// 加载指定路径下的插件  
        /// </summary>
        /// <param name="path"></param>
        public void LoadPlugins(string path)
        {
            // 从指定路径创建目录目录  
            var directoryCatalog = new DirectoryCatalog(path);
            // 将目录目录添加到聚合目录中 
            _catalog.Catalogs.Add(directoryCatalog);
            // 组合当前实例，这将填充下面的MessageSenders2属性
            _container.ComposeParts(this);
        }

        /// <summary>
        /// 使用MEF的ImportMany属性导入所有IMessageSender2接口的实例  
        /// </summary>
        [ImportMany(typeof(IMessageSender2))] public IEnumerable<Lazy<IMessageSender2>> MessageSenders2 { get; set; }

        /// <summary>
        /// 运行已加载的插件，即调用每个IMessageSender2实例的Send2方法
        /// </summary>
        public void Run()
        {
            const string messageToSend = "测试!";
            foreach (var sender in MessageSenders2)
            {
                sender.Value.Send2(messageToSend);
            }
        }
    }
}