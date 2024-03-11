﻿using System;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    [Serializable]
    internal class PluginNotFoundException : Exception
    {
        public PluginNotFoundException()
        {
        }

        public PluginNotFoundException(string message) : base(message)
        {
        }

        public PluginNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PluginNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}