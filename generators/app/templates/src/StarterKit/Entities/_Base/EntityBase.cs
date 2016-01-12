using System.ComponentModel.DataAnnotations;

namespace StarterKit.Entities
{
    public class EntityBase
    {
        // Dit is de base class waarvan je alle entities overerft. 
        // Als er algemene logica nodig is in alle entities, kan je deze hier toevoegen.
        // De DataAccess repositories hebben deze base class ook als constraint voor de entities die naar de database gaan.

        [Key]
        public int Id { get; set; }
    }
}
