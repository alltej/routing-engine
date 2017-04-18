using System;
using System.Linq;
using RoutingLib;

namespace RoutingApp
{
    public class CommandOptions
    {
        private string[] _cmdParams;

        private readonly string[] _validExpressions = { "<=", "<", "=" };

        public CommandOptions(string cmdArg)
        {
            if (Commands.ValidCommands.Contains(cmdArg, StringComparer.OrdinalIgnoreCase))
            {
                IsValid = true;
                IsExitCommand = cmdArg.Equals("exit", StringComparison.OrdinalIgnoreCase);
                int commandType;
                var isNumCommand = int.TryParse(cmdArg, out commandType);
                if (isNumCommand)
                {
                    CommandType = commandType;
                }
            }
            
        }

        public bool IsValid { get; }

        public int? CommandType { get; }
        public int? MaxDepth { get; set; }
        public int? MaxCost { get; set; }

        public string StartArg { get; set; }

        public string EndArg { get; set; }

        public void SetCommandParams(string[] cmdParams)
        {
            _cmdParams = cmdParams;

            //TODO
            if (CommandType == 1)
            {

            }
            else
            {
                if (_cmdParams.Length < 2)
                {
                    throw new ArgumentException("Missing required parameters");
                }

                StartArg = _cmdParams[0];
                EndArg = _cmdParams[1];

            }

        }

        public string[] GetNodeArgs()
        {
            return _cmdParams;
        }

        public Func<Route, bool> SetMaxDepth(string arg)
        {
            string depthParam;
            string expArg;

            if (!TryGetArgExpression(arg, out depthParam, out expArg)) return null;

            var maxStops = GetParamValue(depthParam);
            MaxDepth = maxStops;
            return maxStops <= 0 ? null : SearchPredicates.GetDepthPredicate(maxStops, expArg);
        }

        private static int GetParamValue(string depthParam)
        {
            int paramDepth;
            var maxDepth = 0;
            if (int.TryParse(depthParam, out paramDepth))
            {
                maxDepth = paramDepth;
            }
            return maxDepth;
        }

        private bool TryGetArgExpression(string arg, out string depthParam, out string expParam)
        {
            depthParam = null;
            expParam = null;
            if (arg.Length <= 0) return true;
            depthParam = arg.Trim().Replace(" ", string.Empty);

            foreach (var s in _validExpressions)
            {
                if (depthParam.IndexOf(s, StringComparison.Ordinal) < 0) continue;
                expParam = s;
                depthParam = depthParam.Substring(expParam.Length);
                return true;
            }
            return false;
        }

        public Func<Route, bool> SetMaxCost(string arg)
        {
            string depthParam;
            string expArg;

            if (!TryGetArgExpression(arg, out depthParam, out expArg)) return null;

            var maxCost = GetParamValue(depthParam);

            MaxCost = maxCost;

            return maxCost <= 0 ? null : SearchPredicates.GetCostPredicate(maxCost, expArg);
        }

        public bool IsExitCommand { get; }
    }
}