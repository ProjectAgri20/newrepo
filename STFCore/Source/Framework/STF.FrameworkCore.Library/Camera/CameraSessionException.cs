﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Camera
{
    /// <summary>
    /// Exception generated by having a failed execution of a command on the server.
    /// </summary>
    public class CameraSessionException : Exception
    {
        /// <summary>
        /// New Exception without a message.
        /// </summary>
        public CameraSessionException()
        {

        }
        /// <summary>
        /// New Exception with a message
        /// </summary>
        /// <param name="message">The message for the exception</param>
        public CameraSessionException(string message)
             : base(message)
        {

        }
        /// <summary>
        /// New Exception with a message and an Exception
        /// </summary>
        /// <param name="message">The message for the exception</param>
        /// <param name="inner">The exception</param>
        public CameraSessionException(string message, Exception inner)
            : base(message, inner)
        {

        }


    }
}
