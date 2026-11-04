#region svn header

/*
  $HeadURL: https://serena.icsfl.com:8443/svn/jde/Wireless-JDE/branches/5.3/Wireless/Adapters/Database/TableObjects/F4311.cs $:
  $Author: mhefti@ICSNT $:
  $Rev: 16972 $:
  $Date: 2016-10-06 17:15:20 -0600 (Thu, 06 Oct 2016) $:
*/

#endregion

using System;
using System.Text;
using ICS.Database.TableObjects.RFS;
using ICS.Database;
using System.Security.Policy;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F4311

    /// <summary>
    /// Creates Class for Enterprise 1 Purchase Order Detail Table - F4311
    /// </summary>
    public class F4311 : F4311_Base
    {
        #region Constructors

        /// <summary>
        /// Constructor for DataAccess to Table: F4311
        /// </summary>
        public F4311()
            : base()
        {
        } // end F4311 constructor

        /// <summary>
        /// Constructor for DataAccess to Table: F4311
        /// </summary>
        /// <param name="DBConnections">Data Access to Data and System Tables used by Table Object Class</param>
        /// <param name="TableId">Id used internally in dataset</param>
        public F4311(DAL DBConnections, string TableId)
            : base(DBConnections, TableId)
        {
        } // end F4311 constructor

        #endregion

        #region ShowOpenLines

        /// <summary>
        /// Given an array of allowed next status codes (defined in workflow options
        /// for workflow using this method), the document number, type, and line number (optional),
        /// return all open lines for this order
        /// </summary>
        /// <param name="aNXTR">Array of allowable next statuses (PDNXTR)</param>
        /// <param name="sDOCO">Document number (PDDOCO)</param>
        /// <param name="sDCTO">Document Type (PDDCTO</param>
        /// <param name="sLNID">Line Number (PDLNID), optional</param>
        /// <returns>True if any records are fetched that match the criteria specified for
        /// status codes, document number, document type, line number, and having open quantity;
        /// otherwise, false</returns>
        public virtual bool ShowOpenLines(string[] aNXTR, string sDOCO, string sDCTO, string sLNID)
        {
            return (ShowOpenLines(aNXTR, sDOCO, sDCTO, sLNID, ""));
        } // end ShowOpenLines

        /// <summary>
        /// Given an array of allowed next status codes (defined in workflow options
        /// for workflow using this method), the document number, type, line number (optional),
        /// and branch plant (optional), return all open lines for this order
        /// </summary>
        /// <param name="AllowedNextStatus">Array of allowable next statuses (PDNXTR)</param>
        /// <param name="DocumentNumber">Document number (PDDOCO)</param>
        /// <param name="DocumentType">Document Type (PDDCTO)</param>
        /// <param name="LineNumber">Line Number (PDLNID), optional</param>
        /// <param name="BranchPlant">Branch Plant (PDMCU), optional</param>
        /// <returns>True if any records are fetched that match the criteria specified for
        /// status codes, document number, document type, line number, branch plant, and
        /// having open quantity; otherwise, false</returns>
        public virtual bool ShowOpenLines(string[] AllowedNextStatus, string DocumentNumber, string DocumentType,
            string LineNumber, string BranchPlant)
        {
            ResetAll();

            // If the Branch is blank, do not add it to the search
            if (BranchPlant.Trim().Length > 0)
            {
                PDMCU = BranchPlant;
            } // end if

            PDDOCO = Convert.ToDouble(DocumentNumber);
            if (!string.IsNullOrEmpty(DocumentType))
            {
                PDDCTO = DocumentType;
            }

            // If the Line number is blank, do not add it to the search
            if (!string.IsNullOrEmpty(LineNumber))
            {
                PDLNID = Convert.ToDouble(LineNumber);
            } // end if

            Where("AND", Columns.PDUOPN, ICS.Database.Condition.GreaterThan, 0, "");

            AppendValidStatuses(AllowedNextStatus, "AND", "");

            return (Fetch());
        } // end ShowOpenLines

        #endregion

        #region ShowOpenItems

        /// <summary>
        /// Given an array of allowed next status codes (defined in workflow options for the
        /// workflow using this method), a Branch/Plant (optional) and short item number, return
        /// all open items.
        /// </summary>
        /// <param name="aNXTR">Array of allowable next statuses (PDNXTR)</param>
        /// <param name="sMCU">Branch Plant (PDMCU), optional</param>
        /// <param name="sITM">Short Item Number (PDITM)</param>
        /// <returns>True if any records are fetched that match the criteria specified for
        /// status codes, branch plant (optional), short item number, and having open quantity;
        /// otherwise, false</returns>
        public virtual bool ShowOpenItems(string[] aNXTR, string sMCU, string sITM)
        {
            ResetAll();

            if (sMCU.Length > 0)
            {
                PDMCU = sMCU;
            } // end if

            PDITM = Convert.ToDouble(sITM);

            AppendValidStatuses(aNXTR, "AND", "");

            Where("AND", Columns.PDUOPN, ICS.Database.Condition.NotEqual, 0, "");

            return (Fetch());
        } // end ShowOpenItems

        //AGCH 2025-04 Begin
        public virtual bool ShowOpenbyVendorDetail(string[] aNXTR, string sMCU, string sLITM, string sVendor)
        {
            ResetAll();
            

            if (sMCU.Length > 0)
            {
                PDMCU = sMCU;
            } // end if
            PDDCTO = "OP";
            PDLITM = sLITM;
            PDAN8 = Convert.ToDouble(sVendor);


            AppendValidStatuses(aNXTR, "AND", "");

            Where("AND", Columns.PDUOPN, ICS.Database.Condition.NotEqual, 0, "");
            
            return (Fetch());
        }

        public virtual bool ShowOpenbyVendor(string[] aNXTR, string sMCU, string sLITM, string  sVendor)
        {
            ResetAll();
            SQL = "SELECT pdmcu, PDAN8 , min(pdlnid) as pdlnid, pdlitm, pditm ,pduom,  min(pddoco) as pddoco ,sum(PDUOPN) PDUOPN,sum(pduorg) as pduorg,sum(pdurec) as pdurec ";
            SQL = "FROM " + GetFullTableName() + " ";


            if (sMCU.Length > 0)
            {
                PDMCU = sMCU;
            } // end if
            PDDCTO = "OP";
            PDLITM = sLITM;
            PDAN8 = Convert.ToDouble(sVendor); 


            AppendValidStatuses(aNXTR, "AND", "");

            Where("AND", Columns.PDUOPN, ICS.Database.Condition.NotEqual, 0, "");
            AppendToWhere("group by pdmcu, PDAN8,  pdlitm, pditm, pduom ");
            return (Fetch());
        }
        public virtual bool ShowOpenbyITEM(string[] aNXTR, string sMCU, string sITM )
        {
            ResetAll();
            SQL = "SELECT pdmcu, PDAN8 , min(pdlnid) as pdlnid, pdlitm, pditm ,pduom,  min(pddoco) as pddoco ,sum(PDUOPN) PDUOPN,sum(pduorg) as pduorg,sum(pdurec) as pdurec ";
            SQL = "FROM " + GetFullTableName() + " ";


            if (sMCU.Length > 0)
            {
                PDMCU = sMCU;
            } // end if

            PDLITM = sITM;
            PDDCTO = "OP";


            AppendValidStatuses(aNXTR, "AND", "");

            Where("AND", Columns.PDUOPN, ICS.Database.Condition.NotEqual, 0, "");
            AppendToWhere("group by pdmcu, PDAN8, pdlitm, pditm,pduom ");
            return (Fetch());
        }



        //AGCH 2025-04 end

















        /// <summary>
        /// Given an array of allowed next status codes (defined in workflow options for the
        /// workflow using this method) and short item number, return all open items.
        /// </summary>
        /// <param name="aNXTR">Array of allowable next statuses (PDNXTR)</param>
        /// <param name="sITM">Short Item Number (PDITM)</param>
        /// <returns>True if any records are fetched that match the criteria specified for
        /// status codes, short item number, and having open quantity; otherwise, false</returns>
        public virtual bool ShowOpenItems(string[] aNXTR, string sITM)
        {
            return (ShowOpenItems(aNXTR, "", sITM));
        } // end ShowOpenItems

        /// <summary>
        /// Given an array of allowed next status codes (defined in workflow options for the
        /// workflow using this method) and short item number, return all open items.
        /// </summary>
        /// <param name="aNXTR">Array of allowable next statuses (PDNXTR)</param>
        /// <param name="sMCU">Branch Plant (MCU)</param>
        /// <param name="sITM">Short Item Number (PDITM)</param>
        /// <param name="DocTypes">Array of Valid Doc TypesDocument Types (DCTO)</param>
        /// <returns>True if any records are fetched that match the criteria specified for
        /// status codes, short item number, and having open quantity; otherwise, false</returns>
        public virtual bool ShowOpenItems(string[] aNXTR, string sMCU, string sITM, string[] DocTypes)
        {
            ResetAll();

            if (sMCU.Length > 0)
            {
                PDMCU = sMCU;
            } // end if

            PDITM = Convert.ToDouble(sITM);

            AppendValidStatuses(aNXTR, "AND", "");
            AppendValidDocTypes(DocTypes, "AND", "");

            Where("AND", Columns.PDUOPN, ICS.Database.Condition.NotEqual, 0, "");

            return (Fetch());
        } // end ShowOpenItems

        #endregion

        #region AppendValidStatuses

        /// <summary>
        /// Uses the specified Beginning to append the set of valid status codes separated by an
        /// OR conditional.  For example, AND (PDNXTR = '30' OR PDNXTR = '40')
        /// </summary>
        /// <param name="Statuses">An array of valid status codes or *, which allows all.
        /// An empty array will also be treated as allow all.</param>
        /// <param name="Begin">Used to determine how the statuses will be appended.  For example,
        /// WHERE, AND, OR, etc.  NOTE:  The statuses will be listed within Parenthesis.</param>
        /// <param name="End">Used to determine how the statuses will be ended.  For example,
        /// AND, OR, etc.</param>
        public virtual void AppendValidStatuses(string[] Statuses, string Begin, string End)
        {
            try
            {
                // Setting the available statuses inside of an IN condition.  If the value is
                // * or empty, all status codes are allowed
                if (Statuses.Length > 0 && Statuses[0].Trim() != "*" && Statuses[0].Trim() != "")
                {
                    Where(Begin, Columns.PDNXTR, Condition.IN, String.Join(",", Statuses), End, false);
                } // end if
            } // end try
            catch
            {
            } // end catch
        } // end AppendValidStatuses

        #endregion

        #region AppendValidDocTypes

        /// <summary>
        /// Uses the specified Beginning to append the set of valid document types separated by an
        /// OR conditional.  For example, AND (PDDCTO = 'OP' OR PDDCTO = 'OQ')
        /// </summary>
        /// <param name="DocTypes">An array of valid document types or *, which allows all.
        /// An empty array will also be treated as allow all.</param>
        /// <param name="Begin">Used to determine how the document types will be appended.  For example,
        /// WHERE, AND, OR, etc.  NOTE:  The document types will be listed within Parenthesis.</param>
        /// <param name="End">Used to determine how the document types will be ended.  For example,
        /// AND, OR, etc.  NOTE:  The document types will be listed within Parenthesis.</param>
        /// <returns>The _Conditionals created </returns>
        public virtual void AppendValidDocTypes(string[] DocTypes, string Begin, string End)
        {
            try
            {
                // Setting the available document types inside of an IN condition.  If the value is
                // * or empty, all document types are allowed
                if (DocTypes.Length > 0 && DocTypes[0].Trim() != "*" && DocTypes[0].Trim() != "")
                {
                    Where(Begin, Columns.PDDCTO, Condition.IN, String.Join(",", DocTypes), End, false);
                } // end if
            } // end try
            catch
            {
            } // end catch
        } // end AppendValidDocTypes

        #endregion

        public virtual bool IsItemOnOrder(string branch, double item, double order)
        {
            ResetAll();
            PDDOCO = order;
            PDITM = item;
            PDMCU = branch;
            return Fetch();
        }

        #region GetStagingOrderLines

        public virtual bool GetStagingOrderLines(string sBranch, double dOrderNumber, string[] sAllowedOrderTypes, string[] sAllowedNextStatusCodes)
        {
            bool bRetVal = false;

            Log.Debug("Entering F4311.GetStagingOrderLines().");

            ResetAll();

            SQL = "SELECT " + ColumnsForSelect;
            SQL = "FROM " + FullName + " ";
            SQL = "INNER JOIN " + GetFullTableName("F59RFLPO") + " ON ((PDKCOO = ASRKCO) AND (PDDOCO = CAST(COALESCE(LTRIM(RTRIM(ASRORN)), N'0') AS FLOAT))) AND (PDDCTO = ASRCTO) AND (PDLNID = ASRLLN)) ";

            PDMCU = sBranch;
            PDDOCO = dOrderNumber;
            AppendValidDocTypes(sAllowedOrderTypes, "AND", "");
            AppendValidStatuses(sAllowedNextStatusCodes, "AND", "");

            bRetVal = Fetch();

            Log.Debug("Leaving F4311.GetStagingOrderLines() with return value = '" + bRetVal.ToString() + "'.");

            return bRetVal;
        }

        public virtual bool GetStagingOrderLines(string sBranch, double dOrderNumber, string sOrderType, string[] sAllowedNextStatusCodes)
        {
            bool bRetVal = false;

            Log.Debug("Entering F4311.GetStagingOrderLines().");

            ResetAll();

            SQL = "SELECT " + ColumnsForSelect;
            SQL = "FROM " + FullName + " ";
            SQL = "INNER JOIN " + GetFullTableName("F59RFLPO") + " ON ((PDKCOO = ASRKCO) AND (PDDOCO = CAST(COALESCE(LTRIM(RTRIM(ASRORN)), N'0') AS FLOAT))) AND (PDDCTO = ASRCTO) AND (PDLNID = ASRLLN)) ";

            PDMCU = sBranch;
            PDDOCO = dOrderNumber;
            PDDCTO = sOrderType;
            AppendValidStatuses(sAllowedNextStatusCodes, "AND", "");

            bRetVal = Fetch();

            Log.Debug("Leaving F4311.GetStagingOrderLines() with return value = '" + bRetVal.ToString() + "'.");

            return bRetVal;
        }

        public virtual bool GetStagingOrderLines(string sBranch, double dOrderNumber, string sOrderType, string[] sAllowedNextStatusCodes, string sLicensePlate)
        {
            bool bRetVal = false;

            Log.Debug("Entering F4311.GetStagingOrderLines().");

            ResetAll();

            SQL = "SELECT " + ColumnsForSelect;
            SQL = "FROM " + FullName + " ";
            SQL = "INNER JOIN " + GetFullTableName("F59RFLPO") + " ON ((PDKCOO = ASRKCO) AND (PDDOCO = CAST(COALESCE(LTRIM(RTRIM(ASRORN)), N'0') AS FLOAT))) AND (PDDCTO = ASRCTO) AND (PDLNID = ASRLLN)) ";

            PDMCU = sBranch;
            PDDOCO = dOrderNumber;
            PDDCTO = sOrderType;
            AppendValidStatuses(sAllowedNextStatusCodes, "AND", "");
            AppendToWhere("AND ASLPNU = '" + sLicensePlate + "' ");

            bRetVal = Fetch();

            Log.Debug("Leaving F4311.GetStagingOrderLines() with return value = '" + bRetVal.ToString() + "'.");

            return bRetVal;
        }

        #endregion

        #region TNT Receipt

        public virtual bool GetOrderLines(string sBranch, double dOrderNumber, string[] sAllowedOrderTypes, string[] sAllowedNextStatusCodes)
        {
            bool bRetVal = false;

            Log.Debug("Entering F4311.GetOrderLines().");

            ResetAll();

            PDMCU = sBranch;
            PDDOCO = dOrderNumber;
            AppendValidDocTypes(sAllowedOrderTypes, "AND", "");
            AppendValidStatuses(sAllowedNextStatusCodes, "AND", "");

            bRetVal = Fetch();

            Log.Debug("Leaving F4311.GetOrderLines() with return value = '" + bRetVal.ToString() + "'.");

            return bRetVal;
        }

        public virtual bool ValidOrderItem(string branch, double orderNumber, double shortItem, string[] allowedOrderTypes, string[] allowedNextStatusCodes)
        {
            bool bRetVal = false;

            Log.Debug("Entering F4311.GetOrderLines().");

            ResetAll();

            PDMCU = branch;
            PDDOCO = orderNumber;
            AppendValidDocTypes(allowedOrderTypes, "AND", "");
            AppendValidStatuses(allowedNextStatusCodes, "AND", "");

            bRetVal = Fetch();

            Log.Debug("Leaving F4311.GetOrderLines() with return value = '" + bRetVal.ToString() + "'.");

            return bRetVal;
        }

        #endregion

        #region Line Locking

        #region IsLocked

        /// <summary>
        /// Determine if order detail line is currently in use
        /// </summary>
        /// <param name="erplock">RFS_ERPRecordLocks system table object</param>
        /// <param name="sEnv">ERP Environment</param>
        /// <returns>True if the record is locked; otherwise, false</returns>
        public virtual bool IsLocked(RFS_ERPRecordLocks erplock, string sEnv)
        {
            erplock.ResetAll();
            erplock.EnvironmentID = sEnv;
            erplock.TableName = _TableName;
            erplock.Keys = BuildKey();

            return (erplock.Fetch());
        } // end IsLocked

        #endregion

        #region LockRecord

        /// <summary>
        /// Lock this order detail line.
        /// </summary>
        /// <param name="erplock">RFS_ERPRecordLocks system table object</param>
        /// <param name="sEnv">ERP Environment</param>
        /// <param name="sUser">User to lock the record</param>
        /// <param name="sWorkflow">Workflow in which the record is locked</param>
        /// <returns>True if the record was locked successfully; otherwise, false</returns>
        public virtual bool LockRecord(RFS_ERPRecordLocks erplock, string sEnv, string sUser, string sWorkflow)
        {
            erplock.ResetAll();
            erplock.EnvironmentID = sEnv;
            erplock.TableName = _TableName;
            erplock.Keys = BuildKey();
            erplock.LockDate = System.DateTime.Now;
            erplock.UserID = sUser;
            erplock.WorkflowID = sWorkflow;

            return (erplock.Insert() > 0);
        } // end LockRecord

        #endregion

        #region UnLockRecord

        /// <summary>
        /// Unlock this order detail line.
        /// </summary>
        /// <param name="erplock">RFS_ERPRecordLocks system table object</param>
        /// <param name="sEnv">ERP Environment</param>
        /// <returns>Returns the number of records unlocked (deleted from the RFS_ERPRecordLocks
        /// table</returns>
        public virtual int UnLockRecord(RFS_ERPRecordLocks erplock, string sEnv)
        {
            erplock.ResetAll();
            erplock.EnvironmentID = sEnv;
            erplock.TableName = _TableName;
            erplock.Keys = BuildKey();

            return (erplock.Fetch() ? erplock.Delete() : 0);
        } // end UnLockRecord

        #endregion

        #endregion

        #region BuildKey

        /// <summary>
        /// Build key used in locking ERP table
        ///      Key consists of Key Company, Document Number, Type,
        ///      Order Suffix, and Line Number, all separated by a
        ///      comma
        /// </summary>
        /// <returns>Returns a string of the keys for this table</returns>
        protected virtual string BuildKey()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PDKCOO);
            sb.Append(",");
            sb.Append(PDDOCO);
            sb.Append(",");
            sb.Append(PDDCTO);
            sb.Append(",");
            sb.Append(PDSFXO);
            sb.Append(",");
            sb.Append(PDLNID);

            return (sb.ToString());
        } // end BuildKey

        #endregion

        #region Show Purchase Order Line Details

        public bool ShowPOLineDetails(string BranchPlant, double OrderNo, double LineNo, string[] OrderType)
        {
            ResetAll();
            PDMCU = BranchPlant;
            PDDOCO = OrderNo;
            PDLNID = LineNo;

            #region WHERE IN for Column PDDCTO

            if (OrderType.Length != 0 && OrderType[0] != "" && OrderType[0] != "*")
            {
                AppendToWhere(string.Format(" AND PDDCTO IN ('{0}') ", string.Join("','", OrderType)));
            }

            #endregion

            return Fetch();
        }

        #endregion

        #region Distinct Items on Purchase Order

        public bool DistinctItemPO(string Company, double OrderNo, string OrderType, string[] StatusCodes)
        {
            ResetAll();
            SQL = "SELECT distinct PDLITM, PDDSC1, PDDOCO, PDDCTO, PDKCOO ";
            SQL = "FROM " + GetFullTableName() + " ";
            PDDOCO = OrderNo;
            PDDCTO = OrderType;
            PDKCOO = Company;
            AppendToWhere(string.Format(" AND PDNXTR IN ('{0}') ", string.Join("','", StatusCodes)));
            //PDNXTR = "400";
            Log.Debug($"(***) {OrderNo} {OrderType} {Company} {SQL}");
            return Fetch();
        }

        #endregion
    } // end F4311 Class

    #endregion

    #region F4311_Collection

    /// <summary>
    /// Provides Collection for F4311
    /// </summary>
    public class F4311_Collection : DataObjectBase_Collection
    {
        #region Properties

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F4311.
        /// </summary>
        /// <value>The F4311 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F4311 temp = new F4311();

                if (index <= -1)
                {
                    throw (new System.InvalidOperationException());
                } // end if

                temp.SetColumns = thisObj.SetColumns;
                temp.CurrentRow = thisObj.Rows[index];

                return (temp);
            } // end get
        } // end Current property

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Collection Access for Table: F4311
        /// </summary>
        /// <param name="clsBase">Base Class F4311_Base Object Used in Collection</param>
        public F4311_Collection(F4311_Base clsBase)
            : base(clsBase)
        {
        } // end F4311_Collection constructor

        #endregion
    } // end F4311_Collection Class

    #endregion
} // end ICS.Database.TableObjects.JDE Namespace