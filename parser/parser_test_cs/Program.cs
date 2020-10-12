using System;
using System.Text;
using System.Runtime.InteropServices;

namespace parser_test_cs
{
	class Program
	{
		[DllImport(@"..\parser_core.dll")]
		private static extern void ParseAnimation(byte[] in_buffer, int in_len, byte[] out_buffer, int out_len);

		static void Main(string[] args)
		{
			string in_txt = "MyTestString";
			byte[] in_buff = Encoding.ASCII.GetBytes(in_txt);
			byte[] out_buff = new byte[in_buff.Length];
			ParseAnimation(in_buff, in_buff.Length + 1, out_buff, out_buff.Length + 1);
			string out_txt = Encoding.ASCII.GetString(out_buff);
			Console.WriteLine(out_txt);
		}
	}
}
