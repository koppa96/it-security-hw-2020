using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace parser_test_cs
{
	class Program
	{
		[DllImport(@"..\parser_core.dll")]
		private static extern ulong ParseAnimation(byte[] in_buffer, ulong in_len, byte[] out_buffer, ulong out_len);

		static void Main(string[] args)
		{
			byte[] in_buff = File.ReadAllBytes("..\\..\\..\\files\\1.caff");
			byte[] out_buff = new byte[in_buff.Length];
			int real_out_len = (int)ParseAnimation(in_buff, (ulong)in_buff.Length + 1, out_buff, (ulong)out_buff.Length + 1);
			string out_txt = Encoding.ASCII.GetString(out_buff, 0, real_out_len);
			Console.WriteLine(out_txt);
		}
	}
}
