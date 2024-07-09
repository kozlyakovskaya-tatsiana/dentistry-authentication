using Mapster;

namespace Application.Mapster;

public abstract class BaseDto<TEntity, TDto> : IRegister
    where TDto : class
    where TEntity : class
{
    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public static TDto FromEntity(TEntity entity)
    {
        return entity.Adapt<TDto>();
    }

    private TypeAdapterConfig Config { get; set; }

    public virtual void AddCustomMappings() { }

    protected TypeAdapterSetter<TEntity, TDto> SetCustomMappings() => Config.ForType<TEntity, TDto>();

    protected TypeAdapterSetter<TDto, TEntity> SetCustomMappingsInverse() => Config.ForType<TDto, TEntity>();

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        AddCustomMappings();
    }
}