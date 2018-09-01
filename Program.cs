using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CurtainFireMaker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8))
            {
                Console.SetOut(sw);

                try
                {
                    using (var game = new Game())
                    {
                        game.Run(30.0);
                    }
                }
                catch(Exception e)
                {
                    using (StreamWriter error_sw = new StreamWriter("error.log", false, Encoding.UTF8))
                    {
                        error_sw.WriteLine(e.ToString());
                    }
                }
            }
        }
    }
}
