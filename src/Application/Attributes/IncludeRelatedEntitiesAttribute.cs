namespace suavesabor_api.src.Application.Attributes
{
    public class IncludeRelatedEntitiesAttribute(params string[] entities) : Attribute
    {
        public string[] EntitiesToInclude { get; } = entities;
    }
}
