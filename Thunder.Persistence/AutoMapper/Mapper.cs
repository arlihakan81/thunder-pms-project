


namespace Thunder.Persistence.AutoMapper
{
	public class Mapper : IMapper
	{
		public List<TDestination> Map<TSource, TDestination>(List<TSource> source)
		{
			if (source == null)
				return [];

			var destination = new List<TDestination>(source.Count);

			foreach (var item in source)
			{
				destination.Add(Map<TSource, TDestination>(item));
			}

			return destination;
		}

		public TDestination Map<TSource, TDestination>(TSource source)
		{
			if (source == null)
				return default!;

			var destination = Activator.CreateInstance<TDestination>();

			foreach (var sourceProperty in typeof(TSource).GetProperties())
			{
				var destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name);

				if (destinationProperty != null && destinationProperty.CanWrite)
				{
					var value = sourceProperty.GetValue(source);
					destinationProperty.SetValue(destination, value);
				}
			}

			return destination;
		}

	}
}
