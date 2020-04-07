using System;
using System.Threading;

namespace dodo {

	internal class Program {

		private static void Main(string[] args) {
			var browser = new Browser();
			var join = DateTime.Parse(args[0]);
			var to = join.Subtract(DateTime.Now);
			Console.WriteLine("La session sera lancé à " + join + " (dans " + to.TotalMinutes + " minutes)");

			Thread.Sleep((int) to.TotalMilliseconds);

			browser.startBrowser(args[1], args[2]);
		}
	}

}