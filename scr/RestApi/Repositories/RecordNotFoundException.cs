using System;

namespace RefactorThis.Repositories
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        {
            
        }

        public RecordNotFoundException(Guid id, string name):base($"{name} not found for the Id {id}")
        {
            
        }
    }
}