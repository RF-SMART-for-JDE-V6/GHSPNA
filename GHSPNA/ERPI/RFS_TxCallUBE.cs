using ICS.ERPI;

namespace ICS.Stoneridge.Wireless.JDE.ERPI
{
    /// <summary>
    /// ERP Class for: RFS_TxCallUBE
    /// </summary>
    public class RFS_TxCallUBE : ErpDataStructure
    {
        #region Constructor

        /// <summary>
        /// Constructor for ERPI Transaction: RFS_TxCallUBE
        /// </summary>
        public RFS_TxCallUBE()
        {
            TxType = "RFS_TxCallUBE";
            _Fields.AddRange(new string[]
            {
                "UBEName",
                "UBEVersion",
                "DataSelection1",
                "DataSelection2",
                "DataSelection3",
                "DataSelection4",
                "DataSelection5",
                "DataSelection6",
                "DataSelection7",
                "ErrorCode",
                "ErrorDesc"
            });
        }

        #endregion

        #region Propeties

        #region UBEName

        /// <summary>
        /// Field szUBEName
        /// </summary>
        public string UBEName
        {
            get { return GetField("UBEName"); }
            set { SetField("UBEName", value); }
        }

        #endregion

        #region UBEVersion

        /// <summary>
        /// Field szUBEVersion
        /// </summary>
        public string UBEVersion
        {
            get { return GetField("UBEVersion"); }
            set { SetField("UBEVersion", value); }
        }

        #endregion

        #region DataSelection1

        /// <summary>
        /// Field szDataSelection1
        /// </summary>
        public string DataSelection1
        {
            get { return GetField("DataSelection1"); }
            set { SetField("DataSelection1", value); }
        }

        #endregion

        #region DataSelection2

        /// <summary>
        /// Field szDataSelection2
        /// </summary>
        public string DataSelection2
        {
            get { return GetField("DataSelection2"); }
            set { SetField("DataSelection2", value); }
        }

        #endregion

        #region DataSelection3

        /// <summary>
        /// Field szDataSelection3
        /// </summary>
        public string DataSelection3
        {
            get { return GetField("DataSelection3"); }
            set { SetField("DataSelection3", value); }
        }

        #endregion

        #region DataSelection4

        /// <summary>
        /// Field szDataSelection4
        /// </summary>
        public string DataSelection4
        {
            get { return GetField("DataSelection4"); }
            set { SetField("DataSelection4", value); }
        }

        #endregion

        #region DataSelection5

        /// <summary>
        /// Field szDataSelection5
        /// </summary>
        public string DataSelection5
        {
            get { return GetField("DataSelection5"); }
            set { SetField("DataSelection5", value); }
        }

        #endregion

        #region DataSelection6

        /// <summary>
        /// Field szDataSelection6
        /// </summary>
        public string DataSelection6
        {
            get { return GetField("DataSelection6"); }
            set { SetField("DataSelection6", value); }
        }

        #endregion

        #region DataSelection7

        /// <summary>
        /// Field szDataSelection7
        /// </summary>
        public string DataSelection7
        {
            get { return GetField("DataSelection7"); }
            set { SetField("DataSelection7", value); }
        }

        #endregion

        #region ErrorCode

        /// <summary>
        /// Field cErrorCode
        /// </summary>
        public string ErrorCode
        {
            get { return GetField("ErrorCode"); }
            set { SetField("ErrorCode", value); }
        }

        #endregion

        #region ErrorDesc

        /// <summary>
        /// Field szErrorDesc
        /// </summary>
        public string ErrorDesc
        {
            get { return GetField("ErrorDesc"); }
            set { SetField("ErrorDesc", value); }
        }

        #endregion

        #endregion
    }
}