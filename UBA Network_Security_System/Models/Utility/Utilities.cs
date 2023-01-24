using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.IO;

namespace UBA_Network_Security_System.Models.Utility
{
    public class Utilities
    {
        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public class State
        {
            public string Name { get; set; }
        }

       public List<State> States()
        {
            List<State> _states = new List<State>()
            {
                new State(){ Name = "Abia"},new State(){ Name = "Adamawa"},new State(){ Name = "Akwa-Ibom"},new State(){ Name = "Anambra"},
                new State(){ Name = "bonyi"},new State(){ Name = "Edo"},new State(){ Name = "Ekiti"},new State(){ Name = "Enugu"},
                new State(){ Name = "FCT"},new State(){ Name = "Gombe"},new State(){ Name = "Imo"},new State(){ Name = "Jigawa"},
                new State(){ Name = "Kaduna"},new State(){ Name = "Kano"},new State(){ Name = "Kastina"},new State(){ Name = "Kebbi"},
                new State(){ Name = "Kogi"},new State(){ Name = "Kwara"},new State(){ Name = "Lagos"},new State(){ Name = "Nasarawa"},
                new State(){ Name = "Niger"},new State(){ Name = "Ogun"},new State(){ Name = "Ondo"},new State(){ Name = "Osun"},
                new State(){ Name = "Oyo"},new State(){ Name = "Plateau"},new State(){ Name = "Rivers"},new State(){ Name = "Sokoto"},
                new State(){ Name = "Taraba"},new State(){ Name = "Yobe"},new State(){ Name = "Zamfara"}
            };
            return _states;
        }


        public Image GetBinaryImage(Binary binaryData)
        {
            if (binaryData == null) return null;

            byte[] buffer = binaryData.ToArray();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(buffer, 0, buffer.Length);
            return Image.FromStream(memStream);
        }


    }
}