namespace Clustering
{
	interface IDistanceMetric<T>
	{
		double DistanceSquared(T deck1, T deck2);
	}
}
