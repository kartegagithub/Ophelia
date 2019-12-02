using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
namespace Ophelia.Data.EntityFramework
{
    public static class ObjectSetExtensions
    {
        public static TEntity FindOrAttach<TElement, TEntity>(this ObjectSet<TElement> objectSet, TEntity entity)
            where TEntity : class, TElement
            where TElement : class
        {
            Guard.ArgumentNullException(objectSet, "objectSet");
            if (entity != null)
            {
                string qualifiedEntitySetName = objectSet.EntitySet.EntityContainer.Name + "." + objectSet.EntitySet.Name;
                EntityKey entityKey = objectSet.Context.CreateEntityKey(qualifiedEntitySetName, entity);
                ObjectStateEntry existingStateEntry;
                if (objectSet.Context.ObjectStateManager.TryGetObjectStateEntry(entityKey, out existingStateEntry) && existingStateEntry.Entity != null)
                {
                    try
                    {
                        return (TEntity)existingStateEntry.Entity;
                    }
                    catch (InvalidCastException)
                    {
                        throw new InvalidOperationException("Attached Entity Has Wrong Type");
                    }
                }
                else
                {
                    objectSet.Attach(entity);
                    return entity;
                }
            }
            return null;
        }

        public static IEnumerable<TElement> GetTrackedEntities<TElement>(this ObjectSet<TElement> objectSet)
            where TElement : class
        {
            return GetTrackedEntities<TElement>(objectSet, EntityState.Detached);
        }

        public static IEnumerable<TElement> GetTrackedEntities<TElement>(this ObjectSet<TElement> objectSet, EntityState state)
            where TElement : class
        {
            Guard.ArgumentNullException(objectSet, "objectSet");

            return objectSet.Context.ObjectStateManager.GetObjectStateEntries(state)
                .Where(entry => IsMemberOfObjectSet(objectSet, entry)).Select(e => e.Entity).Cast<TElement>();
        }

        private static bool IsMemberOfObjectSet<TElement>(ObjectSet<TElement> objectSet, ObjectStateEntry entry)
            where TElement : class
        {
            return !entry.IsRelationship
                && entry.Entity != null
                && entry.EntitySet == objectSet.EntitySet;
        }
    }
}
