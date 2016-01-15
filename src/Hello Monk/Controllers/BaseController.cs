using System;
using System.Web.Configuration;
using System.Web.Mvc;
using Monk.Core.Kernel;
using Monk.Domain.Managers.Interface;
using Monk.Log;
using Ninject;

namespace Monk.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Managers

        [Inject]
        public IMessageManager MessageManager { get; set; }

        [Inject]
        public ILog<BaseController> Log { get; set; } 

        #endregion

        #region Smtp data

        protected readonly string Host;
        protected readonly int Port;
        protected readonly string MailAddress;
        protected readonly string Password;
        protected readonly bool SslEnabled;

        #endregion

        public BaseController()
        {
            ObjectFactory.ResolveDependencies(this);

            //Read Smtp Configuration settings from web.config
            int portValue;
            bool ssl;
            Host = WebConfigurationManager.AppSettings["Smtp.Host"] ?? String.Empty;
            Port = int.TryParse(WebConfigurationManager.AppSettings["Smtp.Port"], out portValue) ? portValue : 0;
            SslEnabled = bool.TryParse(WebConfigurationManager.AppSettings["Smtp.SSL"], out ssl) && ssl;
            MailAddress = WebConfigurationManager.AppSettings["Smtp.Network.MailAddress"] ?? String.Empty;
            Password = WebConfigurationManager.AppSettings["Smtp.Network.MailAddress.Password"] ?? String.Empty;

            LogSmtpSettings();
        }

        #region Privates

        /// <summary>
        /// When controller is initialized and read the defined smtp setting
        /// this function logs the current settings for info
        /// </summary>
        private void LogSmtpSettings()
        {
            Log.Debug(string.Format("Smtp host is resolved as {0}", Host));
            Log.Debug(string.Format("Smtp port is resolved as {0}", Port));
            Log.Debug(string.Format("Smtp mail address is resolved as {0}", MailAddress));
            Log.Debug(string.Format("Smtp password is resolved as {0}", Password));
            Log.Debug(string.Format("Smtp ssl connection enabled is resolved as {0}", SslEnabled));
        }

        #endregion
    }
}