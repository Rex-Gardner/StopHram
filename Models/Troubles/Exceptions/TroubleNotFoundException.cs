using System;

namespace Models.Troubles.Exceptions
{
    public class TroubleNotFoundException : Exception
    {
        public TroubleNotFoundException(string troubleId)
            : base($"Trouble \"{troubleId}\" not found.")
        {
            
        }
    }
}