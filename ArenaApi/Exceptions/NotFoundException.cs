using System;

namespace ArenaApi.Exceptions
{
    public class NotFoundException : Exception
    {
        private readonly string _message;

        public string GetMessage(){
            return _message;
        }

        public NotFoundException(string message){
            _message = message;
        }
    }
}