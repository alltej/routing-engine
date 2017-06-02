namespace RoutingLib.Engine
{
    public class TargetDepthCalculator
    {
        public static int CalculateTargetDepth(int? maxDepth, int targetCost, int lowestCost, int maxDepthInShortestRoutes)
        {
            var depthMultiplier = targetCost/lowestCost;

            var targetDepth = depthMultiplier*(maxDepthInShortestRoutes);

            if (maxDepth.HasValue && maxDepth <= targetDepth)
            {
                targetDepth = maxDepth.Value;
            }
            return targetDepth;
        }
    }
}