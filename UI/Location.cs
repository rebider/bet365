using System;
using System.Collections.Generic;
using System.Text;

namespace Bet365.UI
{
    public class Location
    {
        #region 变量属性
        private int _x;
        private int _y;

        /// <summary>
        /// 横坐标，或表格单元的列索引
        /// </summary>
        public int X
        {
            get { return _x; }
            set
            {
                this._x = value;
            }
        }

        /// <summary>
        /// 纵坐标，或表格单元的行索引
        /// </summary>
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                this._y = value;
            }
        }
        #endregion
    }
}
