using Mapster;

namespace Application.Mapster
{
    public class MapsterCodeGenerationConfig : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            /*config.AdaptTo("[name]Dto")
                .ForAllTypesInNamespace(typeof(BaseEntity).Assembly, "Domain.Entities")
                .ExcludeTypes(typeof(BaseEntity));*/
        }
    }
}
