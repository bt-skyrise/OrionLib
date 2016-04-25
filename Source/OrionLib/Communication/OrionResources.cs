namespace OrionLib.Communication
{
    public class OrionResources
    {
        public static string GetUpdateContextResource()
        {
            return "v1/updateContext";
        }

        public static string GetEntityResource(string type, string id)
        {
            return $"v1/contextEntities/type/{type}/id/{id}";
        }

        public static string GetAttributeResource(string type, string id, string attributeName)
        {
            return $"v1/contextEntities/type/{type}/id/{id}/attributes/{attributeName}";
        }
    }
}