using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace VersatileFileComparerHost
{
    /// <summary>
    /// Responsible to discover, load and initialize plugins for comparison
    /// </summary>
    public class PluginLoader
    {

        private IIO _io;

        private List< Assembly > _assemblies = new List< Assembly >();

        public PluginLoader( IIO io )
        {
            _io = io;
        }

        /// <summary>
        /// Utility function that provides the assemblies from disk
        /// </summary>
        /// <param name="directory"></param>
        public void LoadFromPluginDirectory(string directory)
        {
            var files = _io.GetFiles(directory, "*.dll", true);

            foreach ( var file in files )
            {
                using ( var fs = _io.ReadFile( file ) )
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    _assemblies.Add(Assembly.Load(data));
                }
            }

        }

        /// <summary>
        /// Adds assemblies to the internal reflection list
        /// </summary>
        /// <param name="assemblies"></param>
        public void AddAssembly(params Assembly[] assemblies)
        {
            foreach ( var assembly in assemblies )
            {
                _assemblies.Add(assembly);
            }
        }

        /// <summary>
        /// Reflects upon assemblies and locates candidate plugin instances. Constructs and inits them.
        /// </summary>
        /// <returns></returns>
        public List< IVFComparer > DiscoverAndInitializePlugins()
        {
            var types = new List< Type >();
            foreach ( var assembly in _assemblies )
            {
                types.AddRange(_discoverComparerTypes(assembly));
            }

            var list = new List< IVFComparer >();
            // Now initialize them all
            foreach ( var type in types )
            {
                try
                {
                    var comparerInterface = (IVFComparer) ( Activator.CreateInstance(type) );

                    comparerInterface.Init(_io);
                    list.Add(comparerInterface);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Tried to load type '{type.FullName}' but it failed", ex);
                }
            }

            return list;
        }

        private IEnumerable<Type> _discoverComparerTypes(Assembly assembly)
        {
            var baseType = typeof( IVFComparer );
            foreach ( var type in assembly.GetTypes() )
            {
                if (type.IsInterface == false && baseType.IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

    }
}
