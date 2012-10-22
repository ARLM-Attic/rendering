using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders
{
    public interface IBinder
    {
        IEnumerable<ShaderMethodBase> Source {get;}

        bool CanConvert(ShaderType fromType, ShaderType toType);

        IEnumerable<ShaderMethod> Match(ShaderType declaringType, string methodName, params ShaderType[] arguments);
        
        IEnumerable<ShaderMethod> Match(Operators op, params ShaderType[] arguments);

        IEnumerable<ShaderConstructor> Match(ShaderType type, params ShaderType[] arguments);

        ShaderMethodBase[] BestOverloads(IEnumerable<ShaderMethodBase> selection, params ShaderType[] arguments);

        bool IsOverload(ShaderType[] methodArguments, ShaderType[] passingArguments, out int distance);
    }

    public class DefaultBinder : IBinder
    {
        public IEnumerable<ShaderMethodBase> Source { get; private set; }

        public DefaultBinder(IEnumerable<ShaderMethodBase> source)
        {
            this.Source = source;
        }

        #region static methods

        Dictionary<ShaderType, List<ShaderType>> implicitConversions = new Dictionary<ShaderType, List<ShaderType>>();
        Dictionary<ShaderType, List<ShaderType>> explicitConversions = new Dictionary<ShaderType, List<ShaderType>>();

        public void UpdateConversions()
        {
            foreach (var m in Source.OfType<ShaderMethod>().Where(method => (method.Operator == Operators.Implicit || method.Operator == Operators.Explicit) && method.Parameters.Count() == 1))
            {
                ShaderType from = m.Parameters.First().ParameterType;
                ShaderType to = m.ReturnType;

                bool isImplicit = m.Operator == Operators.Implicit;

                if (isImplicit)
                {
                    if (!implicitConversions.ContainsKey(from))
                        implicitConversions.Add(from, new List<ShaderType>());

                    implicitConversions[from].Add(to);
                }
                else
                {
                    if (!explicitConversions.ContainsKey(from))
                        explicitConversions.Add(from, new List<ShaderType>());

                    explicitConversions[from].Add(to);
                }
            }
        }

        Dictionary<ShaderType, Dictionary<ShaderType, int>> c_distances = new Dictionary<ShaderType, Dictionary<ShaderType, int>>();

        private bool SCanConvert(ShaderType fromType, ShaderType toType, out int distance)
        {
            Action<ShaderType, ShaderType, int> addToCache = (f, t, d) =>
            {
                if (!c_distances.ContainsKey(f))
                    c_distances.Add(f, new Dictionary<ShaderType, int>());
                if (!c_distances[f].ContainsKey(t))
                    c_distances[f].Add(t, int.MaxValue);
                if (c_distances[f][t] > d)
                    c_distances[f][t] = d;
            };

            if (c_distances.ContainsKey(fromType) && c_distances[fromType].ContainsKey(toType))
            {
                distance = c_distances[fromType][toType];
                return distance >= 0;
            }

            if (fromType.Equals(toType))
            {
                //addToCache(fromType, toType, 0);
                distance = 0;
                return true;
            }

            if (fromType.InternalIsSubClass(toType, out distance))
            {
                //addToCache(fromType, toType, distance);
                return true;
            }

            if (c_distances.ContainsKey(fromType))
            {
                distance = int.MaxValue;
                return false;
            }

            HashSet<ShaderType> marks = new HashSet<ShaderType>();
            Queue<ShaderType> q = new Queue<ShaderType>();
            Queue<int> dist = new Queue<int>();

            marks.Add(fromType);
            q.Enqueue(fromType);
            dist.Enqueue(0);

            while (q.Count > 0)
            {
                ShaderType a = q.Dequeue();
                int d = dist.Dequeue();

                if (implicitConversions.ContainsKey(a))
                    foreach (var to in implicitConversions[a])
                        if (!marks.Contains(to))
                        {
                            marks.Add(to);
                            q.Enqueue(to);
                            dist.Enqueue(d + 1);
                            addToCache(fromType, to, d + 1);
                        }

                if (explicitConversions.ContainsKey(a))
                    foreach (var to in explicitConversions[a])
                        if (!marks.Contains(to))
                        {
                            marks.Add(to);
                            q.Enqueue(to);
                            dist.Enqueue(d + 1000);
                            addToCache(fromType, to, d + 1000);
                        }
            }

            if (c_distances.ContainsKey(fromType) && c_distances[fromType].ContainsKey(toType))
            {
                distance = c_distances[fromType][toType];
                return distance >= 0;
            }

            distance = -1;
            addToCache(fromType, toType, distance);
            return false;
        }

        IEnumerable<ShaderMethod> SMatch(IEnumerable<ShaderMethodBase> source, Operators Operator, params ShaderType[] argumentTypes)
        {
            var operators = source.OfType<ShaderMethod>().Where(m => m.Operator == Operator).Cast<ShaderMethodBase>();
            return SMatch(operators, argumentTypes).Cast<ShaderMethod>().ToList();
        }

        IEnumerable<ShaderMethodBase> SMatch(IEnumerable<ShaderMethodBase> selection, params ShaderType[] arguments)
        {
            foreach (var m in selection)
            {
                int d;
                if (SIsOverload(m.Parameters.Select(p => p.ParameterType).ToArray(), arguments, out d))
                    yield return m;
            }
        }

        private bool SIsOverload(ShaderType[] methodArguments, ShaderType[] passingArguments, out int distance)
        {
            if (methodArguments.Length != passingArguments.Length)
            {
                distance = 0;
                return false;
            }

            int totalDistance = 0;

            for (int i = 0; i < methodArguments.Length; i++)
            {
                int d;
                if (!SCanConvert(passingArguments[i], methodArguments[i], out d))
                {
                    distance = 0;
                    return false;
                }
                totalDistance += d;
            }

            distance = totalDistance;
            return true;
        }
        
        #endregion

        public bool IsOverload(ShaderType[] methodArguments, ShaderType[] passingArguments, out int distance)
        {
            return SIsOverload(methodArguments, passingArguments, out distance);
        }

        public bool CanConvert(ShaderType fromType, ShaderType toType)
        {
            int dist;
            return SCanConvert(fromType, toType, out dist);
        }

        public IEnumerable<ShaderMethod> Match(ShaderType declaringType, string methodName, params ShaderType[] arguments)
        {
            return SMatch(Source.OfType<ShaderMethod>().Where(m => m.DeclaringType == declaringType && m.Name == methodName).Cast<ShaderMethodBase>(), arguments).Cast<ShaderMethod>();
        }

        public IEnumerable<ShaderMethod> Match(Operators op, params ShaderType[] arguments)
        {
            return SMatch(Source,op, arguments).Cast <ShaderMethod>();
        }

        public IEnumerable<ShaderConstructor> Match(ShaderType type, params ShaderType[] arguments)
        {
            return SMatch(Source.OfType<ShaderConstructor>().Where(c => c.DeclaringType == type).Cast<ShaderMethodBase>(), arguments).Cast<ShaderConstructor>();
        }

        public ShaderMethodBase[] BestOverloads(IEnumerable<ShaderMethodBase> selection, params ShaderType[] arguments)
        {
            int distanceOfBestOverload = int.MaxValue;

            List<ShaderMethodBase> bestOverloads = new List<ShaderMethodBase>();

            foreach (var m in selection)
            {
                int thisOverloadDistance;
                if (IsOverload(arguments, m.Parameters.Select(p => p.ParameterType).ToArray(), out thisOverloadDistance))
                {
                    if (thisOverloadDistance < distanceOfBestOverload)
                    {
                        distanceOfBestOverload = thisOverloadDistance;
                        bestOverloads.Clear();
                    }

                    if (thisOverloadDistance == distanceOfBestOverload)
                        bestOverloads.Add(m);
                }
            }

            return bestOverloads.ToArray();
        }
    }
}
