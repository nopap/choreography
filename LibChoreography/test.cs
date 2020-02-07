using System;
using System.Collections.Generic;
using System.Text;

namespace LibChoreography
{
    class test
    {

        static void trucs()
        {
            // generate base64 for [0,20,ear,dir,0,17,ear,stepsPos]
            CommandService m = new CommandService();
            Random random = new Random();

            string ret;

            ret = m.moveEar(CommandService.Ear.left, random.Next(-17, 17));
            Console.WriteLine(ret);
            ret = m.moveEar(CommandService.Ear.right, random.Next(-17, 17));
            Console.WriteLine(ret);
            ret = m.blinkLED(CommandService.Led.nose,
                            String.Format("#{0:X6}", random.Next(0x1000000)));
            Console.WriteLine(ret);
            ret = m.blinkLED(CommandService.Led.left,
                            String.Format("#{0:X6}", random.Next(0x1000000)));
            Console.WriteLine(ret);
            ret = m.blinkLED(CommandService.Led.center,
                            String.Format("#{0:X6}", random.Next(0x1000000)));
            Console.WriteLine(ret);
            ret = m.blinkLED(CommandService.Led.right,
                            String.Format("#{0:X6}", random.Next(0x1000000)));
            Console.WriteLine(ret);
            ret = m.blinkLED(CommandService.Led.bottom,
                            String.Format("#{0:X6}", random.Next(0x1000000)));
            Console.WriteLine(ret);
            ret = m.playAudio("nabsurprised/" + random.Next(1, 297) + ".mp3");
            Console.WriteLine(ret);

            ret = m.setLED(CommandService.Led.nose,
                String.Format("#{0:X6}", random.Next(0x1000000))
                );
            Console.WriteLine(ret);

            ret = m.cylonLED("red");
            Console.WriteLine(ret);
        }

    }
}
