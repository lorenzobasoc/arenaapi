using System;
using System.Collections.Generic;

namespace ArenaApi
{
    public class Action
    {
        private string Message;
        private DateTime Time;

        public Action(DateTime time, string message){
            Time = time;
            Message = message;
        }

        public override string ToString()
        {
            return $"Time: {Time.ToLongTimeString()}, {Message}";
        }
    }
}