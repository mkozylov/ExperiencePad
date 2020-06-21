using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ExperiencePad
{
    public static class InjectorExtensions
    {
        private static ConcurrentDictionary<Type, Action<IServiceProvider, object>> _serviceInitializerStore = new ConcurrentDictionary<Type, Action<IServiceProvider, object>>();
        private static MethodInfo _getServiceMI = typeof(IServiceProvider).GetMethod("GetService");

        public static void BindProperties(this IServiceProvider injector, object instance)
        {
            var bindAction = _serviceInitializerStore.GetOrAdd(instance.GetType(), instanceType =>
            {
                var blocks = new List<Expression>();

                var providerParamExpr = Expression.Parameter(typeof(IServiceProvider));

                var instanceParamExpr = Expression.Parameter(typeof(object));

                var instanceExpr = Expression.Variable(instanceType);

                blocks.Add(
                    Expression.Assign(instanceExpr, Expression.Convert(instanceParamExpr, instanceType))
                    );

                var properties = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(x => x.PropertyType != typeof(string)
                                                      && !x.PropertyType.IsValueType
                                                      && !x.PropertyType.Namespace.StartsWith(nameof(System))
                                                      && x.DeclaringType != typeof(System.Windows.Window)
                                                      && x.DeclaringType != typeof(System.Windows.Controls.UserControl))
                                             .ToArray();

                foreach (var prop in properties)
                {
                    blocks.Add(
                        Expression.Assign(
                            Expression.Property(instanceExpr, prop),
                            Expression.Coalesce(
                                Expression.Property(instanceExpr, prop),
                                Expression.Convert(
                                    Expression.Call(providerParamExpr, _getServiceMI, Expression.Constant(prop.PropertyType)),
                                    prop.PropertyType)
                            )
                         )
                     );
                }

                var body = Expression.Block(new[] { instanceExpr }, blocks);

                var lambda = Expression.Lambda<Action<IServiceProvider, object>>(body, providerParamExpr, instanceParamExpr);

                return lambda.Compile();
            });

            bindAction(injector, instance);
        }
    }
}
