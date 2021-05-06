using System.Collections.Generic;
using System.Linq;

namespace Clustering
{
	class ClusterItem<T>
	{
		public ClusterItem(T item)
		{
			_item = item;
		}

		public const int NOISE = -1;
		public const int UNCLASSIFIED = 0;
		public readonly T _item;
		public int _clusterId;
	}

	class ClusterFinder<T>
	{
		readonly IDistanceMetric<T> distanceProvider;
		readonly List<ClusterItem<T>> points;
		readonly double eps;
		readonly int minPts;

		public ClusterFinder(IDistanceMetric<T> d, IEnumerable<T> p, double e, int min)
		{
			distanceProvider = d;
			points = p.Select(x => new ClusterItem<T>(x)).ToList();
			eps = e * e; // square eps
			minPts = min;
		}

		public IEnumerable<List<T>> GetClusters()
		{
			return GetItems().Select(x => x.Select(y => y._item).ToList());
		}

		public IEnumerable<List<ClusterItem<T>>> GetItems()
		{
			if (points == null) return null;
			List<List<ClusterItem<T>>> clusters = new List<List<ClusterItem<T>>>();
			int clusterId = 1;
			for (int i = 0; i < points.Count; i++)
			{
				ClusterItem<T> p = points[i];
				if (p._clusterId == ClusterItem<object>.UNCLASSIFIED)
				{
					if (ExpandCluster(p, clusterId)) clusterId++;
				}
			}
			// sort out points into their clusters, if any
			int maxClusterId = points.OrderBy(p => p._clusterId).Last()._clusterId;
			if (maxClusterId < 1) return clusters; // no clusters, so list is empty
			for (int i = 0; i < maxClusterId; i++) clusters.Add(new List<ClusterItem<T>>());
			foreach (ClusterItem<T> p in points)
			{
				if (p._clusterId > 0) clusters[p._clusterId - 1].Add(p);
			}
			return clusters;
		}
		List<ClusterItem<T>> GetRegion(ClusterItem<T> p)
		{
			List<ClusterItem<T>> region = new List<ClusterItem<T>>();
			for (int i = 0; i < points.Count; i++)
			{
				double distSquared = distanceProvider.DistanceSquared(p._item, points[i]._item);
				if (distSquared <= eps) region.Add(points[i]);
			}
			return region;
		}

		bool ExpandCluster(ClusterItem<T> p, int clusterId)
		{
			List<ClusterItem<T>> seeds = GetRegion(p);
			if (seeds.Count < minPts) // no core point
			{
				p._clusterId = ClusterItem<object>.NOISE;
				return false;
			}
			else // all points in seeds are density reachable from point 'p'
			{
				for (int i = 0; i < seeds.Count; i++) seeds[i]._clusterId = clusterId;
				seeds.Remove(p);
				while (seeds.Count > 0)
				{
					ClusterItem<T> currentP = seeds[0];
					List<ClusterItem<T>> result = GetRegion(currentP);
					if (result.Count >= minPts)
					{
						for (int i = 0; i < result.Count; i++)
						{
							ClusterItem<T> resultP = result[i];
							if (resultP._clusterId == ClusterItem<object>.UNCLASSIFIED || resultP._clusterId == ClusterItem<object>.NOISE)
							{
								if (resultP._clusterId == ClusterItem<object>.UNCLASSIFIED) seeds.Add(resultP);
								resultP._clusterId = clusterId;
							}
						}
					}
					seeds.Remove(currentP);
				}
				return true;
			}
		}
	}
}
