namespace OrionLib.CommonContracts
{
    public class EntityQueryDto
    {
        public string Type { get; private set; }
        public string IsPattern { get; private set; }
        public string Id { get; private set; }

        public static EntityQueryDto QueryAllEntitiesOfType(string type)
        {
            return new EntityQueryDto
            {
                // We're selecting only entities of given type.
                Type = type,

                // This allows Id property to use wildcards, thus select many entities.
                IsPattern = "true",

                // This pattern selects all entities
                Id = ".*"
            };
        }
    }
}