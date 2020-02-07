using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace LibChoreography
{
    public class CommandService
    {
        public enum Led { nose, right, center, left, bottom };
        public enum Ear { right, left }

        private enum MTL_OPCODE_HANDLDERS
        {
            nop = 0,
            frame_duration = 1,
            // set_color = 6,  # 'set_color', but commented
            set_led_color = 7,
            set_motor = 8,
            set_leds_color = 9, // # v16
            set_led_off = 10, // # v17
            set_led_palette = 14,
            //set_palette = 15,  # 'set_palette', but commented
            randmidi = 16,
            avance = 17,
            ifne = 18,  //# only used for taichi
            attend = 19,
            setmotordir = 20,  //# v16
        }

        public string sendChoreography(byte[] choreography)
        {
            string base64 = Convert.ToBase64String(choreography);
            string sequence = "[{ \"choreography\":\"data:application/x-nabaztag-mtl-choreography;base64," + base64 + "\" }]";

            string command = "{\"type\":\"command\", \"sequence\":" + sequence + " }";

            return command;
        }

        public string playAudio(string path)
        {
            string sequence = "[{ \"audio\":\"" + path + "\" }]";
            string command = "{\"type\":\"command\", \"sequence\":" + sequence + " }";
            return command;
        }

        public string moveEar(Ear ear, int steps) => sendChoreography(_moveEar(ear, steps));
        private byte[] _moveEar(Ear ear, int steps)
        {
            if (steps < -17 || steps > 17)
                throw new Exception("Invalid steps provided");
            //if (!steps) throw new Error('No steps provided');
            int stepsPos = steps;
            int dir = 0;
            if (steps < 0)
            {
                dir = 1;
                stepsPos = -steps;
            }

            var choreography = new byte[] {
                0, (byte)MTL_OPCODE_HANDLDERS.setmotordir,  (byte)ear, (byte)dir,
                0, (byte)MTL_OPCODE_HANDLDERS.avance,       (byte)ear, (byte)stepsPos };

            return choreography;
        }

        /// <summary>
        /// </summary>
        /// <param name="led">LEd value, apparently 0 to 3 or 4 (unclear)</param>
        /// <param name="color">Color in html format, #xxxxxx or name</param>
        /// <param name="duration">Duration in ms</param>
        /// <returns></returns>
        public string setLED(Led led, string color)
        {
            if (string.IsNullOrEmpty(color))
                throw new Exception("No color provided");
            Color rgba = ColorTranslator.FromHtml(color);

            var choreography = new byte[] {
                0, (byte)MTL_OPCODE_HANDLDERS.set_led_color, (byte)led, rgba.R, rgba.G, rgba.B, 0, 0 };

            return this.sendChoreography(choreography);
        }

        /// <summary>
        /// </summary>
        /// <param name="led">LEd value, apparently 0 to 3 or 4 (unclear)</param>
        /// <param name="color">Color in html format, #xxxxxx or name</param>
        /// <returns></returns>
        public string blinkLED(Led led, string color)
        {
            if (string.IsNullOrEmpty(color))
                throw new Exception("No color provided");
            Color rgba = ColorTranslator.FromHtml(color);
            var choreography = new byte[] {
                0,  (byte)MTL_OPCODE_HANDLDERS.frame_duration,  100,
                0,  (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)led, rgba.R, rgba.G, rgba.B, 0, 0,
                15, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)led, 0, 0, 0, 0, 0,
                15, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)led, rgba.R, rgba.G, rgba.B, 0, 0,
                15, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)led, 0, 0, 0, 0, 0,
                15, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)led, rgba.R, rgba.G, rgba.B, 0, 0,
                15, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)led, 0, 0, 0, 0, 0
            };

            return this.sendChoreography(choreography);
        }

        public string cylonLED(string color)
        {
            if (string.IsNullOrEmpty(color))
                throw new Exception("No color provided");
            Color rgba = ColorTranslator.FromHtml(color);

            byte[] frame = new byte[] { 0,  (byte)MTL_OPCODE_HANDLDERS.frame_duration,  12, };
            byte[] pingpong = new byte[] {
                0,  (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.left, rgba.R, rgba.G, rgba.B, 0, 0,
                10, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.left, 0, 0, 0, 0, 0,
                0, (byte)MTL_OPCODE_HANDLDERS.set_led_color,    (byte)Led.center, rgba.R, rgba.G, rgba.B, 0, 0,
                10, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.center, 0, 0, 0, 0, 0,
                0, (byte)MTL_OPCODE_HANDLDERS.set_led_color,    (byte)Led.right, rgba.R, rgba.G, rgba.B, 0, 0,
                20, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.right, 0, 0, 0, 0, 0,
                0, (byte)MTL_OPCODE_HANDLDERS.set_led_color,    (byte)Led.center, rgba.R, rgba.G, rgba.B, 0, 0,
                10, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.center, 0, 0, 0, 0, 0,
                0,  (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.left, rgba.R, rgba.G, rgba.B, 0, 0,
                20, (byte)MTL_OPCODE_HANDLDERS.set_led_color,   (byte)Led.left, 0, 0, 0, 0, 0,
            };

            var choreography = frame.Concat(_moveEar(Ear.left, 10));
            choreography = choreography.Concat(_moveEar(Ear.right, 10));
            for (int i=0; i < 20; i++)
                choreography = choreography.Concat(pingpong);

            return this.sendChoreography(choreography.ToArray());
        }

    }
}
