using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Workaround.ALM.RemoteContainer
{
	class Program
	{
		private const string processName = "Microsoft.Alm.Shared.Remoting.RemoteContainer.dll";

		static void Main(string[] args)
		{
			if (Environment.ProcessorCount == 1)
			{
				Console.WriteLine("Setting affinities has no use, since there is only one logical core");
			}
			else
			{
				bool success;
				try
				{
					success = RestrictAffinity(processName);
				}
				catch
				{
					success = false;
				}

				if (success)
				{
					Console.WriteLine("Successfully set affinities for " + processName);
				}
				else
				{
					Console.WriteLine("Failed to set affinities for " + processName);
				}
			}
			Console.ReadLine();
		}

		/// <summary> Assigns one logical core to each process with the specified name. </summary>
		/// <returns> whether the restriction was applied. </returns>
		private static bool RestrictAffinity(string processName)
		{
			var processes = Process.GetProcessesByName(processName);

			if (processes.Length == 0)
			{
				return false;
			}

			for (int processIndex = 0; processIndex < Math.Min(processes.Length, Environment.ProcessorCount); processIndex++)
			{
				SetAffinity(processes[processIndex], Environment.ProcessorCount - processIndex - 1);
			}
			return true;
		}
		/// <summary> Assigns each thread in the specified process to the specified logical core. </summary>
		private static void SetAffinity(Process process, int logicalCoreIndex)
		{
			process.ProcessorAffinity = (IntPtr)(1 << logicalCoreIndex);
		}
	}
}
