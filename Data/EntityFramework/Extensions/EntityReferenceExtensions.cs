//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Metadata.Edm;
//using System.Data.Objects.DataClasses;
//using System.Linq;

//namespace Ophelia.Data.EntityFramework
//{
//    public static class EntityReferenceExtensions
//    {
//        public static object GetKey<T>(this EntityReference<T> entityReference)
//            where T : class, IEntityWithRelationships
//        {
//            Guard.ArgumentNullException(entityReference, "entityReference");
//            EntityKey entityKey = entityReference.EntityKey;
//            if (null == entityKey)
//            {
//                if (entityReference.GetTargetEntitySet().ElementType.KeyMembers.Count != 1)
//                {
//                    throw new InvalidOperationException(Messages.SimpleKeyOnly);
//                }
//                return null;
//            }
//            var entityKeyValues = entityKey.EntityKeyValues;
//            if (entityKeyValues.Length != 1)
//            {
//                throw new InvalidOperationException(Messages.SimpleKeyOnly);
//            }
//            return entityKeyValues[0].Value;
//        }

//        public static object GetKey<T>(this EntityReference<T> entityReference, int keyOrdinal)
//            where T : class, IEntityWithRelationships
//        {
//            Guard.ArgumentNullException(entityReference, "entityReference");
//            if (keyOrdinal < 0)
//            {
//                throw new ArgumentOutOfRangeException("keyOrdinal");
//            }
//            EntityKey entityKey = entityReference.EntityKey;
//            if (null == entityKey)
//            {
//                if (entityReference.GetTargetEntitySet().ElementType.KeyMembers.Count <= keyOrdinal)
//                {
//                    throw new ArgumentOutOfRangeException("keyOrdinal");
//                }
//                return null;
//            }
//            if (entityKey.EntityKeyValues.Length <= keyOrdinal)
//            {
//                throw new ArgumentOutOfRangeException("keyOrdinal");
//            }
//            return entityKey.EntityKeyValues[keyOrdinal].Value;
//        }

//        public static void SetKey<T>(this EntityReference<T> entityReference, params object[] keyValues)
//            where T : class, IEntityWithRelationships
//        {
//            Guard.ArgumentNullException(entityReference, "entityReference");

//            if (null == keyValues)
//                entityReference.EntityKey = null;

//            IEnumerable<string> keyComponentNames;
//            int expectedKeyComponentCount;
//            string entitySetName;

//            if (null == entityReference.EntityKey)
//            {
//                EntitySet targetEntitySet = entityReference.GetTargetEntitySet();
//                keyComponentNames = targetEntitySet.ElementType.KeyMembers.Select(m => m.Name);
//                expectedKeyComponentCount = targetEntitySet.ElementType.KeyMembers.Count;
//                entitySetName = targetEntitySet.EntityContainer.Name + "." + targetEntitySet.Name;
//            }
//            else
//            {
//                EntityKey existingKey = entityReference.EntityKey;
//                keyComponentNames = existingKey.EntityKeyValues.Select(v => v.Key);
//                expectedKeyComponentCount = existingKey.EntityKeyValues.Length;
//                entitySetName = existingKey.EntityContainerName + "." + existingKey.EntitySetName;
//            }

//            if (keyValues != null && expectedKeyComponentCount != keyValues.Length)
//                throw new ArgumentException(Messages.UnexpectedKeyCount, "keyValues");

//            if (keyValues == null || keyValues.Any(v => null == v))
//                entityReference.EntityKey = null;
//            else
//            {
//                EntityKey entityKey = new EntityKey(entitySetName, keyComponentNames.Zip(keyValues,
//                    (name, value) => new EntityKeyMember(name, value)));
//                entityReference.EntityKey = entityKey;
//            }
//        }

//        public static EntitySet GetTargetEntitySet(this RelatedEnd relatedEnd)
//        {
//            Guard.ArgumentNullException(relatedEnd, "relatedEnd");

//            AssociationSet associationSet = (AssociationSet)relatedEnd.RelationshipSet;

//            if (null == associationSet)
//                throw new InvalidOperationException(Messages.CannotDetermineMetadataForRelatedEnd);

//            return associationSet.AssociationSetEnds[relatedEnd.TargetRoleName].EntitySet;
//        }
//    }
//}
