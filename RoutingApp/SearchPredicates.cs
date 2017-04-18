using System;
using RoutingLib;

namespace RoutingApp
{
    public static class SearchPredicates
    {
        public static Func<Route, bool> GetDepthPredicate(int depth, string expression)
        {
            switch (expression)
            {
                case ">":
                    return s => s.Stops > depth;
                case ">=":
                    return s => s.Stops >= depth;
                case "<":
                    return s => s.Stops < depth;
                case "<=":
                    return s => s.Stops <= depth;
                case "=":
                    return s => s.Stops == depth;
                default:
                    return s => s.Stops <= depth;
            }
        }

        public static Func<Route, bool> GetCostPredicate(int cost, string expression)
        {
            switch (expression)
            {
                case ">":
                    return s => s.Cost > cost;
                case ">=":
                    return s => s.Cost >= cost;
                case "<":
                    return s => s.Cost < cost;
                case "<=":
                    return s => s.Cost <= cost;
                case "=":
                    return s => s.Cost == cost;
                default:
                    return s => s.Cost <= cost;
            }
        }
    }
}