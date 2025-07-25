using System.Collections.Generic;

namespace UToolkit.UI
{
    public class Intent
    {
        private Dictionary<string, object> _tables;

        public Controller Context { get; private set; }

        public Intent(Controller context)
        {
            Context = context;

            _tables = new Dictionary<string, object>();
        }

        public void PutString(string key, string value)
        {
            _tables.Add(key, value);
        }
    }
}