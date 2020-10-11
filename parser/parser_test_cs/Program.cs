using System;
using System.IO;
using System.Runtime.InteropServices;

namespace parser_test_cs
{
	class Program
	{
		[DllImport(@"..\parser_core.dll")]
		private static extern void helloWorld(byte[] out_buffer);

		static void Main(string[] args)
		{
			byte[] buff = new byte[16];
			helloWorld(buff);
			string txt = System.Text.Encoding.ASCII.GetString(buff);
			Console.WriteLine(txt);
		}
	}
}
