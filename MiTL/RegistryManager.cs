using Microsoft.Win32;

namespace MiTL
{
    internal class RegistryManager
    {
        private RegistryKey regKey;
        private RegistryValueKind valueKind;

        private string RetVal { get; set; }

        public string RegKeyRead(string subKey, string key)
        {
            //open the subkey, if it exists retrieve the stored values
            regKey = Registry.CurrentUser.OpenSubKey(subKey);
            if (key != null)
            {
                RetVal = regKey.GetValue(key).ToString();
                regKey.Close();
            }
            return RetVal;
        }

        public void RegKeyWrite(string subKey, string key, string value, RegistryValueKind kind)
        {
            //open the subkey, if it exists, retrieve the value kind
            regKey = Registry.CurrentUser.OpenSubKey(subKey);
            if (key != null)
            {
                valueKind = regKey.GetValueKind(key);
                regKey.Close();
            }
            else
            {
                valueKind = kind;
            }

            //write subkey
            regKey = Registry.CurrentUser.CreateSubKey(subKey);
            regKey.SetValue(key, value, valueKind);
            regKey.Close();
        }
    }
}