using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Log;
using SF.Framework.File;
using SF.Framework.EmailSender;
using SF.Framework.SysDateTime;
using SF.Framework.StringZipper;
using SF.Framework.Encrypt;
using SF.Framework.Cache;
using StructureMap;

namespace SF.Framework.Runtime
{
    public class Components
    {
        public ILog Log
        {
            get
            {
                return ObjectFactory.GetInstance<ILog>();
            }
        }
        public IFile File
        {
            get
            {
                return ObjectFactory.GetInstance<IFile>();
            }
        }
        public IEmailSender EmailSender
        {
            get
            {
                return ObjectFactory.GetInstance<IEmailSender>();
            }
        }
        public ISystemDateTime SystemDateTime
        {
            get
            {
                return ObjectFactory.GetInstance<ISystemDateTime>();
            }
        }
        public IStringZipper StringZipper
        {
            get
            {
                return ObjectFactory.GetInstance<IStringZipper>();
            }
        }
        public IEncrypt Encrypt
        {
            get
            {
                return ObjectFactory.GetInstance<IEncrypt>();
            }
        }

        private Dictionary<Type, ICache> caches = new Dictionary<Type, ICache>();
        public ICache Cache(Type type)
        {
            if (caches.Keys.Contains(type))
                return caches[type];

            ICache cache = ObjectFactory.GetInstance<ICache>();
            cache.CachePrefix = type.ToString();
            caches.Add(type, cache);
            return cache;
        }

    }
}