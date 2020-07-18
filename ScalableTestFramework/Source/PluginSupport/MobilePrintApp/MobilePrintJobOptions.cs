using System.Collections.Generic;

namespace HP.ScalableTest.PluginSupport.MobilePrintApp
{
    /// <summary>
    /// Mobile Print Job Options
    /// </summary>
    public abstract class MobilePrintJobOptions
    {
        private Dictionary<string, string> _optionDic;

        /// <summary>
        /// Initialize Class
        /// </summary>
        public MobilePrintJobOptions()
        {
            _optionDic = new Dictionary<string, string>();
        }
        /// <summary>
        /// Set job option
        /// </summary>
        /// <param name="key">Option name (i.e. numberOfCopies)</param>
        /// <param name="value">Option value(i.e. 2)</param>
        public virtual void SetOption(string key, string value)
        {
            _optionDic[key] = value;
        }

        /// <summary>
        /// Get saved option
        /// </summary>
        /// <param name="key">key to get option</param>
        /// <returns>value of option</returns>
        public virtual string GetOption(string key)
        {
            if(!_optionDic.ContainsKey(key))
            {
                return null;
            }
            return _optionDic[key];
        }
    }
}
