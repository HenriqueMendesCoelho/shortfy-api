namespace suavesabor_api.Application.Attributes
{
    public class IncludeRelatedEntitiesAttribute(params string[] entities) : Attribute
    {
        public string[] EntitiesToInclude { get; } = entities;
    }
}
