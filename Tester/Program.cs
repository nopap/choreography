using Lib.MQTT;
using LibChoreography;
using System;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Rabbits!");

            CommandService m = new CommandService();
            Random random = new Random();

            MessageSender ms = new MessageSender();

            //string command = m.moveEar(CommandService.Ear.left, random.Next(-17, 17));
            //string command = "{\"type\":\"command\", \"sequence\":" + sequence + " }";
            string command = m.cylonLED("red");

            Console.WriteLine(command);
            ms.Send("nopaphome/nabaztag",
                command,
                false);

            Console.ReadLine();
        }

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

        }
    }
}
