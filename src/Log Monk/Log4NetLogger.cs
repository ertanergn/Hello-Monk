using System;
using log4net;
using Monk.Log.Helpers;

namespace Monk.Log
{
    public class Log4NetLogger<TType> : ILog<TType>
    {
        public Log4NetLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Debug(params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Debug(LogFormatter.BuildMessageFromParams(message));         
        }

        public void Debug(Exception exception, params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Debug(LogFormatter.BuildMessageFromParams(message), exception);
        }

        public void Info(params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Info(LogFormatter.BuildMessageFromParams(message));
        }

        public void Info(Exception exception, params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Info(LogFormatter.BuildMessageFromParams(message), exception);
        }

        public void Warn(params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Warn(LogFormatter.BuildMessageFromParams(message));
        }

        public void Warn(Exception exception, params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Warn(LogFormatter.BuildMessageFromParams(message), exception);
        }

        public void Error(params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Error(LogFormatter.BuildMessageFromParams(message));
        }

        public void Error(Exception exception, params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Error(LogFormatter.BuildMessageFromParams(message), exception);
        }

        public void Fatal(params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Fatal(LogFormatter.BuildMessageFromParams(message));
        }

        public void Fatal(Exception exception, params string[] message)
        {
            LogManager.GetLogger(typeof(TType)).Fatal(LogFormatter.BuildMessageFromParams(message), exception);
        }
    }
}
