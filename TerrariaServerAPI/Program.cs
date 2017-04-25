﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Terraria.ID;
using TerrariaApi.Server;

namespace OTAPI.Shims.TShock
{
	class Program
	{
		/// <summary>
		/// Initialises any internal values before any server initialisation begins
		/// </summary>
		public static void InitialiseInternals()
		{
			ItemID.Sets.Explosives = ItemID.Sets.Factory.CreateBoolSet(new int[]
			{
				ItemID.Bomb,
				ItemID.Dynamite,
				ItemID.StickyBomb,
				ItemID.Explosives,
				ItemID.GrenadeLauncher,
				ItemID.RocketLauncher,
				ItemID.RocketII,
				ItemID.RocketIV,
				ItemID.SnowmanCannon,
				ItemID.StickyDynamite,
				ItemID.BouncyBomb,
				ItemID.BombFish
			});
		}

		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledException;
			try
			{
				InitialiseInternals();
				ServerApi.Hooks.AttachOTAPIHooks(args);

				// avoid any Terraria.Main calls here or the heaptile hook will not work.
				// this is because the hook is executed on the Terraria.Main static constructor,
				// and simply referencing it in this method will trigger the constructor.
				StartServer(args);

				ServerApi.DeInitialize();
			}
			catch (Exception ex)
			{
				ServerApi.LogWriter.ServerWriteLine("Server crashed due to an unhandled exception:\n" + ex, TraceLevel.Error);
			}
		}

		static void StartServer(string[] args)
		{
			var gitInfo = Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;

			if (gitInfo != null )
			{
				Console.Write(" Mintaka v");
				Console.WriteLine(gitInfo?.InformationalVersion);
			} 

			if (args.Any(x => x == "-skipassemblyload"))
			{
				Terraria.Main.SkipAssemblyLoad = true;
			}

			Terraria.WindowsLaunch.Main(args);
		}

		/// <summary>
		/// TShock sets up its own unhandled exception handler; this one is just to catch possible
		/// startup exceptions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine($"Unhandled exception\n{e}");
		}


	}
}
