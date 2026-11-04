using System;
using System.Text;
using ICS.Database.TableObjects.RFS;
using ICS.Database.TableObjects.JDE;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    /// <summary>
    /// Creates Class for Enterprise 1 Receipt Routing Master File - F43092
    /// </summary>
    public class F43092 : F43092_Base
    {
        protected F0005 UserDefinedCodes;

        protected RFS_TableOwnerOverride TableOwnerOverride;

        protected F41001 _WarehouseConstants;

        /// <summary>
        /// Constructor for DataAccess to Table: F43092
        /// </summary>
        public F43092()
            :
            base()
        {
        }

        #region ShowOpenRouteRecordsByOrder

        /// <summary>
        /// Given a Branch/Plant, return distinct order number, all open route records
        /// where the active operation (PXACTO) is 'Y'es
        /// </summary>
        /// <param name="_BranchPlant">Branch</param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowOpenRouteRecordsByOrder(string _BranchPlant)
        {
            ResetAll();
            StringBuilder sb = new StringBuilder();
            string owner = _TableOwner.ToString();
            sb.Append("SELECT distinct(PXDOCO), PXDCTO, PXNLIN, F41.PRDOCO, F1.ABALPH FROM ");
            sb.Append(owner);
            sb.Append(".F43092 F4 inner join ");
            sb.Append(owner);
            sb.Append(".F43121 F41 ON (F4.PXDOCO = F41.PRDOCO ) left join ");
            sb.Append(owner);
            sb.Append(".F0101 F1 ON (F41.PRAN8 = F1.ABAN8) WHERE ");
            sb.Append("PXMCU = '" + _BranchPlant + "' AND PXACTO = 'Y'  ORDER BY PXDOCO");
            SQL = sb.ToString();
            return (Fetch());
        }

        #endregion

        #region ShowOrders

        /// <summary>
        /// Given a Branch/Plant, return distinct order number, all open route records
        /// where the active operation (PXACTO) = 'Y'es
        /// </summary>
        /// <param name="BranchPlant">Branch</param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowOrders(string BranchPlant)
        {
            ResetAll();
            string owner = _TableOwner.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct(PXDOCO), PXDCTO,  F41.PRDOCO, F1.ABALPH FROM ");
            sb.Append(owner);
            sb.Append(".F43092 F4 inner join ");
            sb.Append(owner);
            sb.Append(".F43121 F41 ON (F4.PXDOCO = F41.PRDOCO ) left join ");
            sb.Append(owner);
            sb.Append(".F0101 F1 ON (F41.PRAN8 = F1.ABAN8) WHERE ");
            sb.Append("PXMCU = '" + BranchPlant + "' AND PXACTO = 'Y'");
            sb.Append(" GROUP BY PXDOCO, PXDCTO, F41.PRDOCO, F1.ABALPH");
            SQL = sb.ToString();
            return (Fetch());
        }

        #endregion

        #region ShowOrders

        /// <summary>
        /// Given a Branch/Plant, and an array of allowed document types,
        /// return all open route records where the active operation (PXACTO) = 'Y'es
        /// </summary>
        /// <param name="BranchPlant">Branch/Plant being processed</param>
        /// <param name="AllowedDocumentTypes">From Workflow options - AllowedDocType </param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowOrders(string BranchPlant, string[] AllowedDocumentTypes)
        {
            string DocTypes = "";
            if (AllowedDocumentTypes[0].Trim().Length > 0)
            {
                for (int i = 0; i < AllowedDocumentTypes.Length; i++)
                {
                    if (i == 0)
                        DocTypes = "'" + AllowedDocumentTypes[i].ToString().ToUpper() + "'";
                    else
                        DocTypes = DocTypes + ", '" + AllowedDocumentTypes[i] + "'";
                }
            }

            ResetAll();
            string owner = _TableOwner.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct(PXDOCO), PXDCTO,  F41.PRDOCO, F1.ABALPH FROM ");
            sb.Append(owner);
            sb.Append(".F43092 F4 inner join ");
            sb.Append(owner);
            sb.Append(".F43121 F41 ON (F4.PXDOCO = F41.PRDOCO ) left join ");
            sb.Append(owner);
            sb.Append(".F0101 F1 ON (F41.PRAN8 = F1.ABAN8) WHERE ");
            sb.Append("PXMCU = '" + BranchPlant + "' AND PXACTO = 'Y'");
            if (DocTypes.Trim().Length > 0)
            {
                sb.Append(" AND PXDCTO IN (");
                sb.Append(DocTypes);
                sb.Append(")");
            }

            sb.Append(" GROUP BY PXDOCO, PXDCTO, F41.PRDOCO, F1.ABALPH");
            SQL = sb.ToString();
            return (Fetch());
        }

        #endregion

        #region LocationFormatExpression

        protected virtual void LocationFormatExpression(string BranchPlant)
        {
            try
            {
                // add a column to the F41021 table called "FormattedLocation"
                // initially, default it to the same value as the location field
                StringBuilder sb = new StringBuilder();
                sb = _WarehouseConstants.FormattedLocationExpression(BranchPlant, "PRLOCN");
                ThisDataTable.Columns["FormattedLocation"].Expression = sb.ToString();
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        #endregion

        #region ShowRoutingSteps

        /// <summary>
        /// Given a Branch/Plant, Document (order) Number, Type, and environment,
        /// show all routing steps for this order
        /// </summary>
        /// <param name="BranchPlant">Branch</param>
        /// <param name="Document">Document Number</param>
        /// <param name="DocumentType">Document Type</param>
        /// <param name="Environment">Enterprise 1 Environment</param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowRoutingSteps(string BranchPlant, string Document, string DocumentType, string Environment)
        {
            ResetAll();
            string owner = _DataAccess.Data.TableOwner;
            string ctlowner = "";
            TableOwnerOverride.ResetAll();
            TableOwnerOverride.TableName = "F0005";
            TableOwnerOverride.EnvironmentID = Environment;
            bool bFnd = TableOwnerOverride.Fetch();
            if (bFnd)
            {
                ctlowner = TableOwnerOverride.OwnerName.ToString();
            }
            else
            {
                ctlowner = owner;
            }

            string sSQL = "SELECT DISTINCT(PXOPRC), PXDOCO, PXDCTO, DRDL01, PXOPRS, PXQTYO, PXUOM FROM ";
            sSQL = sSQL + owner + ".F43092 t1, ";
            sSQL = sSQL + ctlowner + ".F0005 t2 WHERE ";
            sSQL = sSQL + "PXMCU = '" + BranchPlant + "' AND PXDOCO = " + Document + " and PXDCTO = '" + DocumentType + "' ";
            sSQL = sSQL + " and PXOPRC = ltrim(DRKY) and DRRT = 'OC' ";
            sSQL = sSQL + " ORDER BY PXOPRS";
            SQL = sSQL;
            return (Fetch());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BranchPlant">Branch</param>
        /// <param name="Document">Document Number</param>
        /// <param name="DocumentType">Document Type</param>
        /// <param name="Environment">Enterprise 1 Environment</param>
        /// <param name="LineNumber">Line Number</param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowRoutingSteps(string BranchPlant, string Document, string DocumentType, string Environment, double LineNumber)
        {
            ResetAll();
            string owner = _DataAccess.Data.TableOwner;
            string ctlowner = "";
            TableOwnerOverride.ResetAll();
            TableOwnerOverride.TableName = "F0005";
            TableOwnerOverride.EnvironmentID = Environment;
            bool bFnd = TableOwnerOverride.Fetch();
            if (bFnd)
            {
                ctlowner = TableOwnerOverride.OwnerName.ToString();
            }
            else
            {
                ctlowner = owner;
            }

            string sSQL = "SELECT DISTINCT(PXOPRC), DRDL01 FROM ";
            sSQL = sSQL + owner + ".F43092 t1, ";
            sSQL = sSQL + ctlowner + ".F0005 t2 WHERE ";
            sSQL = sSQL + "PXMCU = '" + BranchPlant + "' AND PXDOCO = " + Document + " and PXDCTO = '" + DocumentType + "' ";
            sSQL = sSQL + " and PXOPRC = ltrim(DRKY) and DRRT = 'OC' ";
            sSQL = sSQL + " and PXLNID = " + LineNumber.ToString();
            sSQL = sSQL + " ORDER BY PXOPRC";
            SQL = sSQL;
            return (Fetch());
        }

        #endregion

        #region ShowOpenLines

        /// <summary>
        /// Given a Branch/Plant, Document (order) Number and Type, 
        /// return all open lines
        /// </summary>
        /// <param name="sMCU">Branch</param>
        /// <param name="sDOCO">Document Number</param>
        /// <param name="sDCTO">Document Type</param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowOpenLines(string sMCU, string sDOCO, string sDCTO)
        {
            ResetAll();
            string owner = _TableOwner.ToString();
            string sSQL = "SELECT distinct PXNROU, PXDOCO, PXDCTO, PXSFXO, PXKCOO, PXLNID, PXNLIN, F43.PRLITM, PXOPRS, PXOPRC, PXQTYO, PXUOM, F41.IMDSC1, F41.IMITM, F41.IMUOM1, F43.PRLOCN, F43.PRLOTN FROM ";
            sSQL = sSQL + owner + ".F43092 F4 inner join ";
            sSQL = sSQL + owner + ".F43121 F43 ON (pxdoco = prdoco and pxlnid = prlnid and  pxkcoo = prkcoo and pxsfxo = prsfxo and pxnlin = prnlin and pxdcto = prdcto) inner join ";
            sSQL = sSQL + owner + ".F4101 F41 ON (F43.PRLITM = F41.IMLITM) WHERE ";
            sSQL = sSQL + "PXMCU = '" + sMCU + "' AND PXACTO = 'Y' AND F43.PRDOCO = ";
            sSQL = sSQL + sDOCO + " AND F43.PRDCTO = '" + sDCTO + "'";
            sSQL = sSQL + " AND F4.PXQTYO <> 0";
            sSQL = sSQL + " AND PRMATC = '1' ";
            sSQL = sSQL + " ORDER BY F4.PXDOCO, F43.PRLITM";
            SQL = sSQL;
            bool Retval = Fetch();
            if (Retval)
                LocationFormatExpression(sMCU);
            return (Retval);
        }

        /// <summary>
        /// Given a Branch/Plant, Document (order) Number and Type, 
        /// return all open lines
        /// </summary>
        /// <param name="sMCU">Branch</param>
        /// <param name="sDOCO">Document Number</param>
        /// <param name="sDCTO">Document Type</param>
        /// <param name="ShortItemNumber">Short Item Number</param>
        /// <returns>True if records found.</returns>
        public virtual bool ShowOpenLines(string sMCU, string sDOCO, string sDCTO, double ShortItemNumber)
        {
            ResetAll();
            string owner = _TableOwner.ToString();
            string sSQL = "SELECT distinct PXNROU, PXDOCO, PXDCTO, PXSFXO, PXKCOO, PXLNID, PXNLIN, F43.PRLITM, PXOPRS, PXOPRC, PXQTYO, PXUOM, F41.IMDSC1, F41.IMITM, F41.IMUOM1, F43.PRLOCN, F43.PRLOTN FROM ";
            sSQL = sSQL + owner + ".F43092 F4 inner join ";
            sSQL = sSQL + owner + ".F43121 F43 ON (pxdoco = prdoco and pxlnid = prlnid and  pxkcoo = prkcoo and pxsfxo = prsfxo and pxnlin = prnlin and pxdcto = prdcto) inner join ";
            sSQL = sSQL + owner + ".F4101 F41 ON (F43.PRLITM = F41.IMLITM) WHERE ";
            sSQL = sSQL + "PXMCU = '" + sMCU + "' AND PXACTO = 'Y' AND F43.PRDOCO = ";
            sSQL = sSQL + sDOCO + " AND F43.PRDCTO = '" + sDCTO + "'";
            sSQL = sSQL + " AND F4.PXQTYO <> 0 and F43.PRITM = " + ShortItemNumber;
            sSQL = sSQL + " AND PRMATC = '1' ";
            sSQL = sSQL + " ORDER BY F4.PXDOCO, F43.PRLITM";
            SQL = sSQL;
            bool Retval = Fetch();
            if (Retval)
                LocationFormatExpression(sMCU);
            return (Retval);
        }

        #endregion

        #region Fetch Routing Steps

        public bool FetchPurchaseOrderRoutingSteps(string BranchPlant, double OrderNumber, string[] OrderType)
        {
            ResetAll();

            PXMCU = BranchPlant;
            PXDOCO = OrderNumber;
            PXACTO = "Y";

            #region WHERE IN for Column PXDCTO

            if (OrderType.Length != 0 && OrderType[0] != "" && OrderType[0] != "*")
            {
                AppendToWhere(string.Format(" AND PXDCTO IN ('{0}') ", string.Join("','", OrderType)));
            }

            #endregion

            return Fetch();
        }

        #endregion

        /// <summary>
        /// Constructor for DataAcces to Table: F43092
        /// </summary>
        /// <param name="DBConnections">Data Access to Data and System Tables used by Table Object Class</param>
        /// <param name="TableId">Id used internally in dataset</param>
        public F43092(DAL DBConnections, string TableId)
            :
            base(DBConnections, TableId)
        {
            UserDefinedCodes = new F0005(DataAccess, "");
            TableOwnerOverride = new RFS_TableOwnerOverride(DataAccess, "");
            _WarehouseConstants = new F41001(this.DataAccess, "");
            try
            {
                if (!ThisDataTable.Columns.Contains("FormattedLocation"))
                {
                    ThisDataTable.Columns.Add("FormattedLocation", typeof(System.String));
                } // end if
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }
    }

    /// <summary>
    /// Provides Collection for F43092
    /// </summary>
    public class F43092_Collection : DataObjectBase_Collection
    {
        /// <summary>
        /// Constructor for Collection Access for Table: F43092
        /// </summary>
        /// <param name="clsBase">Base Class F43092_Base Object Used in Collection</param>
        public F43092_Collection(F43092_Base clsBase)
            :
            base(clsBase)
        {
        }

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F43092.
        /// </summary>
        /// <value>The F43092 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F43092 temp = new F43092();
                if (index <= -1)
                {
                    throw new System.InvalidOperationException();
                }

                temp.SetColumns = thisObj.SetColumns;
                temp.CurrentRow = thisObj.Rows[index];
                return temp;
            }
        }
    }
}