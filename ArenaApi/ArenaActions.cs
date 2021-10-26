using System;

namespace ArenaApi
{
    public class ArenaAction
    {
        public string Message { get; }
        public DateTime Time { get; }

        public ArenaAction(DateTime time, string message){
            Time = time;
            Message = message;
        }

        public override string ToString(){
            return $"Time: {Time.ToLongTimeString()}, {Message}";
        }
    }
}