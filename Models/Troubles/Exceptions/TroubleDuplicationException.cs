using System;

namespace Models.Troubles.Exceptions
{
    public class TroubleDuplicationException : Exception
    {
        public TroubleDuplicationException(string troubleId)
            : base($"Trouble \"{troubleId}\" already exists.")
        {
            
        }
    }
}