using System;
using System.Collections.Generic;
using System.Text;

namespace GwhrSettings.Core
{
    public class GwhrSetting
    {
        #region Properties

        public string Key { get; set; }
        public object Value { get; set; }
        public State State { get; set; }

        #endregion
    }

    public enum State
    {
        Added,
        //Deleted,
        Modified,
        Unchanged
    }
}
