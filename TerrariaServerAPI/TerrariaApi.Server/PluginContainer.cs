using System;

namespace TerrariaApi.Server
{
	public class PluginContainer : IDisposable
	{
		public TerrariaPlugin Plugin
		{
			get;
			protected set;
		}
		public bool Initialized
		{
			get;
			protected set;
		}
		public bool Dll
		{
			get;
			set;
		}
		public string Path
		{
			get;
			set;
		}

		public PluginContainer(TerrariaPlugin plugin,string path) : this(plugin, true, path)
		{
		}

		public PluginContainer(TerrariaPlugin plugin, bool dll,string path)
		{
			this.Plugin = plugin;
			this.Initialized = false;
			this.Dll = dll;
			this.Path = path;
		}

		public void Initialize()
		{
			this.Plugin.Initialize();
			this.Initialized = true;
		}

		public void DeInitialize()
		{
			this.Initialized = false;
		}

		public void Dispose()
		{
			this.Plugin.Dispose();
		}
	}
}
