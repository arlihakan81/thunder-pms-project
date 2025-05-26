namespace Thunder.Persistence.AutoMapper
{
	public interface IMapper
	{
		List<TDestination> Map<TSource, TDestination>(List<TSource> source);
		TDestination Map<TSource, TDestination>(TSource source);

	}
}
