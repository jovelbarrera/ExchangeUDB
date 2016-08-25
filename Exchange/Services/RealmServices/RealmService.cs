using System;
using Realms;
using Kadevjo.Core.Dependencies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using Exchange.Interfaces;

namespace Exchange.RealmServices
{
    // To implement Framework ICache it should be modified for beign an standard interface first
    // So I proposed a new ICache interface
    public class RealmService<T> : ICache<T>
        where T : RealmObject, IModel, new()
    {
        private Realm _realm;

        private static RealmService<T> _instance;
        public static RealmService<T> Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RealmService<T>();
                return _instance;
            }
        }

        public string DBName
        {
            get { return "DBName"; }
        }

        public RealmService()
        {
            _realm = Realm.GetInstance();
        }

        #region ICache implementation

        public async Task<T> GetObject(string objectId)
        {
            if (!ObjectAlreadyExist(objectId))
                throw new Exception("Object with ObjectId '" + objectId + "' was not found");

            List<T> results = Read(i => i.ObjectId == objectId);
            return results.FirstOrDefault();
        }

        public async Task<List<T>> GetObjects()
        {
            List<T> results = Read();
            return results;
        }

        public async Task<List<T>> GetObjects(Func<T, bool> whereQuery)
        {
            List<T> results = Read(whereQuery);
            return results;
        }

        public async Task InsertObject(T obj)
        {
            if (ObjectAlreadyExist(obj.ObjectId))
                throw new Exception("Object with ObjectId '" + obj.ObjectId + "' already exist");
            Create(obj);
        }

        public async Task InsertObjects(List<T> objects)
        {
            foreach (var obj in objects)
                if (ObjectAlreadyExist(obj.ObjectId))
                    throw new Exception("Object with ObjectId '" + obj.ObjectId + "' already exist. No objects were inserted");

            foreach (var obj in objects)
                Create(obj);
        }

        public async Task UpdateObject(T obj)
        {
            if (!ObjectAlreadyExist(obj.ObjectId))
                throw new Exception("Object with ObjectId '" + obj.ObjectId + "' was not found");
            T oldObj = Read(i => i.ObjectId == obj.ObjectId).FirstOrDefault();
            Update(oldObj, obj);
        }

        // Custom Update Transaction for Realm
        public async Task UpdateObjectTransaction(Action transaction)
        {
            using (var trans = _realm.BeginWrite())
            {
                transaction();
                trans.Commit();
            }
        }

        public async Task RemoveObject(string objectId)
        {
            if (!ObjectAlreadyExist(objectId))
                throw new Exception("Object with ObjectId '" + objectId + "' was not found");

            List<T> results = Read(i => i.ObjectId == objectId);
            Delete(results.FirstOrDefault());
        }

        public async Task RemoveObjects()
        {
            List<T> objects = Read();
            foreach (var obj in objects)
                Delete(obj);
        }

        #endregion

        #region CRUD
        private List<T> Read(Func<T, bool> whereQuery = null)
        {
            IEnumerable<T> results;
            if (whereQuery == null)
                results = _realm.All<T>();
            else
                results = _realm.All<T>().Where(whereQuery);
            return results.ToList();
        }

        private T Create(T entity)
        {
            string[] properties = GetModelProperties();
            T realmEntity = null;
            _realm.Write(() =>
            {
                realmEntity = _realm.CreateObject<T>();
                foreach (var property in properties)
                {
                    object val = entity.GetType().GetRuntimeProperty(property).GetValue(entity);
                    realmEntity.GetType().GetRuntimeProperty(property).SetValue(realmEntity, val);
                }
            });
            return realmEntity;
        }

        private void Update(T realmEntity, T newEntity)
        {
            string[] properties = GetModelProperties();
            using (var trans = _realm.BeginWrite())
            {
                foreach (var property in properties)
                {
                    object val = newEntity.GetType().GetRuntimeProperty(property).GetValue(newEntity);
                    realmEntity.GetType().GetRuntimeProperty(property).SetValue(realmEntity, val);
                }
                trans.Commit();
            }
        }

        private void Delete(T realmEntity)
        {
            using (var trans = _realm.BeginWrite())
            {
                _realm.Remove(realmEntity);
                trans.Commit();
            }
        }
        #endregion

        #region Helpers
        private bool ObjectAlreadyExist(string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
                throw new Exception("ObjectId is null or empty");

            List<T> results = Read(i => i.ObjectId == objectId);
            return results != null && results.Count > 0;
        }

        private string[] GetModelProperties()
        {
            PropertyInfo[] modelProperties = typeof(T).GetRuntimeProperties().ToArray();
            PropertyInfo[] realmProperties = typeof(RealmObject).GetRuntimeProperties().ToArray();

            List<string> allModelPropertiesNames = new List<string>();
            List<string> realmPropertiesNames = new List<string>();

            foreach (var property in modelProperties)
                allModelPropertiesNames.Add(property.Name);
            foreach (var property in realmProperties)
                realmPropertiesNames.Add(property.Name);

            allModelPropertiesNames = allModelPropertiesNames.Except(realmPropertiesNames).ToList();

            List<string> ignoredProperties = new List<string>();
            foreach (var property in modelProperties)
            {
                Attribute[] attrs = property.GetCustomAttributes().ToArray();
                foreach (var attr in attrs)
                {
                    if (attr is IgnoredAttribute)
                        ignoredProperties.Add(property.Name);
                }
            }

            allModelPropertiesNames = allModelPropertiesNames.Except(ignoredProperties).ToList();

            return allModelPropertiesNames.ToArray();
        }
        #endregion
    }
}

