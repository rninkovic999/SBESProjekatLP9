using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public enum AuditEventTypes
    {
        AuthenticationSuccess=0,
        AuthorizationSuccess=1,
        AuthorizationFailure=2,
        ReadFromFileSuccess=3,
        ReadFromFileFailed=4,
        WriteInFileSuccess=5,
        WriteInFileFailed=6,
        AddSuccess=7,
        ChangeSuccess=8,
        PaySuccess=9,
        MethodCallFailed=10
    }
    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock(resourceLock)
                {
                    if(resourceManager==null)
                    {
                        //resourceManager = new ResourceManager (typeof(AuditEventFile).ToString(), Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthenticationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
            }
        }

        public static string AuthorizationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
        }

        public static string AuthorizationFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailure.ToString());
            }
        }

        public static string ReadFromFileSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReadFromFileSuccess.ToString());
            }
        }

        public static string ReadFromFileFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReadFromFileFailed.ToString());
            }
        }

        public static string WriteInFileSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WriteInFileSuccess.ToString());
            }
        }

        public static string WriteInFileFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WriteInFileFailed.ToString());
            }
        }

        public static string AddSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddSuccess.ToString());
            }
        }

        public static string ChangeSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ChangeSuccess.ToString());
            }
        }

        public static string PaySuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.PaySuccess.ToString());
            }
        }
        
        

        public static string MethodCallFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.MethodCallFailed.ToString());
            }
        }
    }
}
