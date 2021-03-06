﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BRServerFunctionsGen
{
	class Program
	{
		static void Main(string[] args)
		{
			string serverRoot = "";
			string outputFile = "";
			string functionsPrefix = "";
			string dirRoot = "";
			string usageMessage = "Usage: BRServerFunctionsGen.exe <server_files_root> <output file> <function prefix>\n" +
									"\n\tExample: BRServerFunctionsGen.exe \"E:\\Code\\BR\\BattleRoyale\\Source\\Server\\BattleRoyaleUpdates\\@BattleRoyale\\addons\\br_server\" Functions.hpp BR_Server\n";
			if (args.Length < 3)
			{
				Console.WriteLine("Error. Invalid argument count.\n");
				Console.WriteLine(usageMessage);
				return;
			}

			bool isSuccess = false;
			try
			{
				serverRoot = args[0].Trim();
				outputFile = args[1].Trim();
				functionsPrefix = args[2].Trim();
				dirRoot = Path.GetFullPath(serverRoot);
				Console.WriteLine(string.Format("INPUT ServerRoot={0}\nOutput={1}\nPrefix={2}", dirRoot, outputFile, functionsPrefix));
				isSuccess = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Invalid arguments! Check your paths." + e.Message);
				Console.WriteLine(usageMessage);
			}
			if (!isSuccess) { return; }

			// Parse folders
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("/* AUTOGENERATED FILE */");
			sb.AppendLine((functionsPrefix + "_" + "PREDEFINED_FUNCTIONS = [").ToUpper());
			IEnumerable<String> sqfFiles = Directory.EnumerateFiles(dirRoot, "*.sqf", SearchOption.AllDirectories);

			int count = 0;
			int total = sqfFiles.Count();
			foreach (string f in sqfFiles)
			{
				count += 1;
				string sqfFilePathFromRoot = f.Replace(dirRoot + "\\", "");
				string sqfFunctionName = sqfFilePathFromRoot.Replace(".sqf", "");
				sqfFunctionName = sqfFunctionName.Replace("\\", "_");
				sqfFunctionName = functionsPrefix + "_" + sqfFunctionName;
				string sqfFunctionFilePath = functionsPrefix + "_" + sqfFilePathFromRoot.Replace("\\", "_");
				string output = string.Format("\t[\"{0}\",\"{1}\"]{2}", sqfFunctionName, functionsPrefix.ToLower() + "\\" + sqfFilePathFromRoot, count == total ? "" : ",");
				sb.AppendLine(output);
			}
			sb.AppendLine("];");

			try
			{
				File.WriteAllText(outputFile, sb.ToString());
			} catch(Exception e)
			{
				Console.WriteLine("Error writing to output file. " + e.Message);
			}
			return;
		}
	}
}
