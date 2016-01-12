using System;

namespace StarterKit.DataAccess
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int entityKey)
        {
            this.EntityName = entityName;
            this.EntityKey = entityKey;
            _message = String.Format("Entity van type '{0}' en key {1} niet gevonden in de huidige context.", entityName, EntityKey);
        }

        public string EntityName { get; set; }

        public int EntityKey { get; set; }

        private readonly string _message = null;
        public override string Message
        {
            get { return _message; }
        }
    }
}
