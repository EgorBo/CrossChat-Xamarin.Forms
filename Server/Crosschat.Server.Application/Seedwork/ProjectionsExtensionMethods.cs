using System.Collections.Generic;
using Crosschat.Server.Domain.Seedwork;

namespace Crosschat.Server.Application.Seedwork
{
    public static class ProjectionsExtensionMethods
    {
        /// <summary>
        /// Project a type using a DTO
        /// </summary>
        /// <typeparam name="TProjection">The dto projection</typeparam>
        /// <param name="entity">The source entity to project</param>
        /// <returns>The projected type</returns>
        public static TProjection ProjectedAs<TProjection>(this Entity entity)
            where TProjection : class,new()
        {
            var adapter = AutomapperTypeAdapterFactory.Create();
            return adapter.Adapt<TProjection>(entity);
        }

        /// <summary>
        /// projected a enumerable collection of items
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <param name="items">the collection of entity items</param>
        /// <returns>Projected collection</returns>
        public static List<TProjection> ProjectedAsCollection<TProjection>(this IEnumerable<Entity> items)
            where TProjection : class,new()
        {
            var adapter = AutomapperTypeAdapterFactory.Create();
            return adapter.Adapt<List<TProjection>>(items);
        }
    }
}
