using System.Collections.Generic;
using System.IO;

using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Tools
{
    class MasterKeys
    {
        #region Local Variables

        static readonly MasterKeys _instance = new MasterKeys();

        HashSet<string> _keys;

        #endregion

        #region Constructor

        private MasterKeys()
        {
            _keys = new HashSet<string>();

            try
            {
                //Load master keys from file
                string keyFile = Path.Combine(GlobalSettings.Items["EwsSiteMapLocation"], "Keys.txt");

                foreach (string key in File.ReadAllLines(keyFile))
                {
                    if (!string.IsNullOrEmpty(key.Trim()))
                    {
                        if (!_keys.Contains(key))
                        {
                            _keys.Add(key);
                        }
                    }
                }
            }
            catch (SettingNotFoundException)
            {
                // do nothing
            }           
        }

        #endregion

        #region Public Methods

        public static MasterKeys Instance()
        {
            return _instance;
        }

        public bool IsExists(string key)
        {
            return _keys.Contains(key);
        }

        public HashSet<string> GetKeys()
        {
            return _keys;
        }

        public void SetKeys(HashSet<string> keys)
        {
            _keys = keys;

            string keyFile = Path.Combine(GlobalSettings.Items["EwsSiteMapLocation"], "Keys.txt");
            StreamWriter sw = File.CreateText(keyFile);

            foreach (string key in keys)
            {
                sw.WriteLine(key);
            }

            sw.Close();
        }

        #endregion
        
    }
}
