#region svn header

/*
  $HeadURL: https://serena.icsfl.com:8443/svn/jde/Wireless-JDE/branches/5.3/Wireless/Adapters/Database/TableObjects/F41021.cs $:
  $Author: joshea@ICSNT $:
  $Rev: 20127 $:
  $Date: 2017-03-06 12:15:56 -0700 (Mon, 06 Mar 2017) $:
*/

#endregion

#region Usings

using System;
using System.Data;
using System.Globalization;
using System.Text;
using ICS.Utilities;
using ICS.Database.TableObjects.RFS;
using ICS.Database.TableObjects.JDE;
using ICS.Database;

#endregion

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    #region F41021

    /// <summary>
    /// Creates Class for Enterprise 1 Item Location Table - F41021
    /// </summary>
    public class F41021 : F41021_Base
    {
        #region Fields

        protected string _OmitHeldLots;

        protected F41001 _WarehouseConstants;

        protected string _TodaysDateInJulian;

        private static bool PickManagerTagTableExists;

        protected RFS_DataManipulator _DataManipulatorTable;

        #endregion

        #region Properties

        #region OmitHeldLots

        /// <summary>
        /// OmitHeldLots
        /// </summary>
        public virtual string OmitHeldLots
        {
            get { return (_OmitHeldLots); } // end get
            set { _OmitHeldLots = value; } // end set
        } // end OmitHeldLots property

        #endregion

        #region Availability

        /// <summary>
        /// Gets the availability value with the correct data manipulation if it has been added.  If
        /// the column does not exist or has no value, zero is returned.
        /// </summary>
        public virtual double Availability
        {
            get
            {
                if (ThisDataTable.Columns.Contains("Availability") && (CurrentRow["Availability"] != DBNull.Value))
                {
                    if (ThisDataTable.Columns.Contains("rfsDM_Availability") && (CurrentRow["rfsDM_Availability"] != DBNull.Value))
                    {
                        return (Convert.ToDouble(CurrentRow["rfsDM_Availability"], CultureInfo.InvariantCulture));
                    } // end if
                    else
                    {
                        return (Convert.ToDouble(DataManip.FormatDataForDisplay(CurrentRow["Availability"],
                                ThisDataTable.Columns["Availability"],
                                (DataRow) ThisDataTable.Columns["Availability"].ExtendedProperties["DManip"]),
                            CultureInfo.InvariantCulture));
                    } // end else
                } // end if
                else
                {
                    return (0);
                } // end else
            } // end get
            set
            {
                if (ThisDataTable.Columns.Contains("Availability"))
                {
                    object avail = value;
                    if (ThisDataTable.Columns.Contains("rfsDM_Availability"))
                    {
                        DataManip.FormatForWhere(ref avail,
                            ThisDataTable.Columns["Availability"],
                            (DataRow) ThisDataTable.Columns["Availability"].ExtendedProperties["DManip"]);
                    }

                    CurrentRow["Availability"] = avail.ToString();
                }
            }
        } // end Availability

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for DataAcces to Table: F41021
        /// </summary>
        public F41021()
            : base()
        {
        } // end F41021 constructor

        /// <summary>
        /// Constructor for DataAcces to Table: F41021.  Includes the instantiating of a F41001
        /// TableObject object with a blank table name prefix.
        /// </summary>
        /// <param name="dataAccessLayer">Data Access to Data and System Tables used by Table Object
        /// Class</param>
        /// <param name="sTableId">Id used internally in dataset</param>
        public F41021(DAL dataAccessLayer, string sTableId)
            : base(dataAccessLayer, sTableId)
        {
            _DataManipulatorTable = new RFS_DataManipulator(this.DataAccess, "");
            _WarehouseConstants = new F41001(this.DataAccess, "");
            // Convert today's date to a JDE julian for checking against the
            // F4108 lot master to return a count of expired lots
            DateTime dt = DateTime.Now;
            _TodaysDateInJulian = DateTimeUtils.ConvertToJulian(dt.ToString("yyyyMMdd"));
            AddAvailabilityColumn();
            //AvailabilityExpression(LIMCU);

            if (!ThisDataTable.Columns.Contains("FormattedLocation"))
            {
                ThisDataTable.Columns.Add("FormattedLocation", typeof(System.String));
            } // end if

            Log.Debug("*** Checking if Pick Manager Tag Table (F59RFPMT) Exists *** ");
            PickManagerTagTableExists = (DataAccess.Data.GetSchemaInfo(DataAccess.GetDataTableOwnerAndSeparator("F59RFPMT")) != null);
            Log.Debug("Pick Manager Tag Table Exists: " + PickManagerTagTableExists);
        } // end F41021 constructor

        #endregion

        #region Methods

        #region Fetch

        /// <summary>
        /// Calls the DataObjectBase class's Fetch method.  If the result is true, it also sets the
        /// Availability Expression.
        /// </summary>
        /// <returns>Returns true records are Fetched; otherwise, false.</returns>
        protected virtual new bool Fetch()
        {
            if (base.Fetch())
            {
                LocationFormatExpression(LIMCU);
                //AvailabilityExpression(LIMCU);

                return (true);
            } // end if

            return (false);
        } // end Fetch

        /// <summary>
        /// copy of F41021.Fetch without quering warehouse constants table. 
        /// relies on the fact that warehouse constants table is already loaded in dataset
        /// </summary>
        /// <returns></returns>
        protected virtual bool FetchEfficient()
        {
            if (base.Fetch())
            {
                LocationFormatExpression();
                //AvailabilityExpression(LIMCU);

                return (true);
            } // end if

            return (false);
        } // end Fetch

        #endregion

        #region LocationFormatExpression

        /// <summary>
        /// Sets the FormattedLocation column using the value of the F41001 TableObject object's
        /// FormattedLocationExpression method based on the Branch plant specified.
        /// </summary>
        /// <param name="sBranchPlant">Branch Plant for which the sLocation is to be formatted</param>
        protected virtual void LocationFormatExpression(string sBranchPlant)
        {
            // add a column to the F41021 table called "FormattedLocation"
            // initially, default it to the same value as the location field
            try
            {
                ThisDataTable.Columns["FormattedLocation"].Expression =
                    _WarehouseConstants.FormattedLocationExpression(sBranchPlant, "LILOCN").ToString();
            } // end try
            catch (Exception ex)
            {
                Log.LogException(ex);
            } // end catch
        } // end LocationFormatExpression

        /// <summary>
        /// Sets the FormattedLocation column using the value of the F41001 TableObject object's
        /// FormattedLocationExpression method based on the Branch plant constants already loaded in dataset.
        /// </summary>
        protected virtual void LocationFormatExpression()
        {
            // add a column to the F41021 table called "FormattedLocation"
            // initially, default it to the same value as the location field
            try
            {
                ThisDataTable.Columns["FormattedLocation"].Expression =
                    _WarehouseConstants.FormattedLocationExpression("LILOCN").ToString();
            } // end try
            catch (Exception ex)
            {
                Log.LogException(ex);
            } // end catch
        } // end LocationFormatExpression

        #endregion

        #region AvailabilityExpression

        /// <summary>
        /// Given a branch, return column containing availability expression.
        /// Availability is determined by retrieved the warehouse constants
        /// record from Enterprise 1 and applying the availability rules defined.
        /// </summary>
        /// <param name="sBranch">Branch/Plant</param>
        public virtual string AvailabilityExpression(string sBranch)
        {
            StringBuilder sAvailability = new StringBuilder();

            Log.Debug("Entering AvailabilityExpression");

            _WarehouseConstants.ResetAll();
            _WarehouseConstants.CIMCU = sBranch;
            _WarehouseConstants.Fetch();

            #region Constructing the Availability Expression

            sAvailability.Append("LIPQOH");

            if (_WarehouseConstants.CIDA01 == "-") // soft commits
            {
                sAvailability.Append(" - LIPCOM");
            } // end if

            if (_WarehouseConstants.CIDA02 == "-") // hard commits
            {
                sAvailability.Append(" - LIHCOM");
            } // end if

            if (_WarehouseConstants.CIDA03 == "-") // future commits
            {
                sAvailability.Append(" - LIFCOM");
            } // end if

            if (_WarehouseConstants.CIDA04 == "-") // hard commit to work order
            {
                sAvailability.Append(" - LIQOWO");
            } // end if

            if (_WarehouseConstants.CIDA05 == "-") // other quantity 1
            {
                sAvailability.Append(" - LIOT1P");
            } // end if

            if (_WarehouseConstants.CIDA06 == "-") // other quantity 2
            {
                sAvailability.Append(" - LIOT2P");
            } // end if

            // The following is no longer needed since it is filtered out
            // and accounted for in the SQL

            /*if (_WarehouseConstants.CIDA07 == "-")  // quantity on hold
            {
               sAvailability.Append(" - IIF (LILOTS <> ' ', LIPQOH , 0)");
            } // end if
             */
            if (_WarehouseConstants.CIDA08 == "+") // on receipt
            {
                sAvailability.Append(" + LIPREQ");
            } // end if

            if (_WarehouseConstants.CIDA09 == "+") // on work order receipt
            {
                sAvailability.Append(" + LIQWBO");
            } // end if

            if (_WarehouseConstants.CIDA10 == "+") // on P/O other 1
            {
                sAvailability.Append(" + LIOT1A");
            } // end if

            if (_WarehouseConstants.CIDA12 == "+") // inbound
            {
                sAvailability.Append(" + LIQTIN");
            } // end if

            if (_WarehouseConstants.CIDA13 == "+") // in transit
            {
                sAvailability.Append(" + LIQTTR");
            } // end if

            if (_WarehouseConstants.CIDA14 == "+") // qty in operation 1
            {
                sAvailability.Append(" + LIQTO1");
            } // end if

            if (_WarehouseConstants.CIDA15 == "+") // qty in operation 2
            {
                sAvailability.Append(" + LIQTO2");
            } // end if

            if (_WarehouseConstants.CIDA16 == "+") // qty on loan to Mfg.
            {
                sAvailability.Append(" + LIQONL");
            } // end if

            #endregion

            try
            {
                AddAvailabilityColumn();
                // ThisDataTable.Columns["Availability"].Expression = sAvailability.ToString();
            } // end try
            catch (Exception ex)
            {
                Log.LogException(ex);
            } // end catch

            return (sAvailability.ToString());
            Log.Debug("Leaving AvailabilityExpression");
        } // end AvailabilityExpression

        public virtual string AvailabilityExpression()
        {
            StringBuilder sAvailability = new StringBuilder();

            Log.Debug("Entering AvailabilityExpression");

            #region Constructing the Availability Expression

            sAvailability.Append("LIPQOH");

            if (_WarehouseConstants.CIDA01 == "-") // soft commits
            {
                sAvailability.Append(" - LIPCOM");
            } // end if

            if (_WarehouseConstants.CIDA02 == "-") // hard commits
            {
                sAvailability.Append(" - LIHCOM");
            } // end if

            if (_WarehouseConstants.CIDA03 == "-") // future commits
            {
                sAvailability.Append(" - LIFCOM");
            } // end if

            if (_WarehouseConstants.CIDA04 == "-") // hard commit to work order
            {
                sAvailability.Append(" - LIQOWO");
            } // end if

            if (_WarehouseConstants.CIDA05 == "-") // other quantity 1
            {
                sAvailability.Append(" - LIOT1P");
            } // end if

            if (_WarehouseConstants.CIDA06 == "-") // other quantity 2
            {
                sAvailability.Append(" - LIOT2P");
            } // end if

            // The following is no longer needed since it is filtered out
            // and accounted for in the SQL

            /*if (_WarehouseConstants.CIDA07 == "-")  // quantity on hold
            {
               sAvailability.Append(" - IIF (LILOTS <> ' ', LIPQOH , 0)");
            } // end if
             */
            if (_WarehouseConstants.CIDA08 == "+") // on receipt
            {
                sAvailability.Append(" + LIPREQ");
            } // end if

            if (_WarehouseConstants.CIDA09 == "+") // on work order receipt
            {
                sAvailability.Append(" + LIQWBO");
            } // end if

            if (_WarehouseConstants.CIDA10 == "+") // on P/O other 1
            {
                sAvailability.Append(" + LIOT1A");
            } // end if

            if (_WarehouseConstants.CIDA12 == "+") // inbound
            {
                sAvailability.Append(" + LIQTIN");
            } // end if

            if (_WarehouseConstants.CIDA13 == "+") // in transit
            {
                sAvailability.Append(" + LIQTTR");
            } // end if

            if (_WarehouseConstants.CIDA14 == "+") // qty in operation 1
            {
                sAvailability.Append(" + LIQTO1");
            } // end if

            if (_WarehouseConstants.CIDA15 == "+") // qty in operation 2
            {
                sAvailability.Append(" + LIQTO2");
            } // end if

            if (_WarehouseConstants.CIDA16 == "+") // qty on loan to Mfg.
            {
                sAvailability.Append(" + LIQONL");
            } // end if

            #endregion

            try
            {
                AddAvailabilityColumn();
                // ThisDataTable.Columns["Availability"].Expression = sAvailability.ToString();
            } // end try
            catch (Exception ex)
            {
                Log.LogException(ex);
            } // end catch

            return (sAvailability.ToString());
            Log.Debug("Leaving AvailabilityExpression");
        } // end AvailabilityExpression

        #endregion

        #region AddAvailabilityColumn

        /// <summary>
        /// Adds the availability column to the table and includes the data manipulation if it exists
        /// to the columns extended properties.
        /// </summary>
        protected virtual void AddAvailabilityColumn()
        {
            DataRow rowManipulatorData;
            DataColumn colAvailability;
            // Checking to see if the Availability Column exists.  If it does not, add it.
            if (!ThisDataTable.Columns.Contains("Availability"))
            {
                ThisDataTable.Columns.Add("Availability", typeof(System.Double));
                rowManipulatorData = DataAccess.DataManipulator.Select(_TableName, "Availability");
                if (rowManipulatorData != null)
                {
                    ThisDataTable.Columns["Availability"].ExtendedProperties.Add("DManip",
                        rowManipulatorData);
                    //BD025541 DLOMNECK - make the availability column show data manipulation
                    colAvailability = DataAccess.Data.GetDataManipColumnForValueType(ThisDataTable.Columns["Availability"]);
                    if (colAvailability != null)
                    {
                        ThisDataTable.Columns.Add(colAvailability);
                    } //end if
                } // end if
            } // end if
        } // end AddAvailabilityColumn

        #endregion

        #region GetOnHandQtyNotOnLP

        /// <summary>
        /// Fetches the on-hand quantity not on a LP for an item and branch.</summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).</param>
        /// <returns>Returns sum of F41021.LIPQOH On-Hand qty - sum of F46L11.IDPQOR qty.</returns>
        public virtual double GetOnHandQtyNotOnLP(string sBranch, double dShortItemNumber)
        {
            double dReturnQty = 0;

            Log.Debug("Entering GetOnHandQtyNotOnLP");

            SQL = "SELECT SUM (LIPQOH - CASE WHEN IDPQOR IS NULL THEN 0 ELSE IDPQOR END) AS LIPQOH FROM ";

            SQL = "(SELECT LIMCU, LIITM, LILOTN, LILOCN, SUM(LIPQOH) AS LIPQOH FROM ";
            SQL = this.FullName;
            SQL = " WHERE LIMCU = '";
            SQL = sBranch;
            SQL = "' AND LIITM = ";
            SQL = dShortItemNumber.ToString(CultureInfo.InvariantCulture);
            SQL = " GROUP BY LIMCU, LIITM, LILOTN, LILOCN) ItemLocationQuery ";

            SQL = " LEFT OUTER JOIN ";

            SQL = "(SELECT IDMCU, IDITM, IDLOTN, IDLOCN, SUM(IDPQOR) AS IDPQOR FROM ";
            SQL = this.GetFullTableName("F46L11");
            SQL = " WHERE IDMCU = '";
            SQL = sBranch;
            SQL = "' AND IDITM = ";
            SQL = dShortItemNumber.ToString(CultureInfo.InvariantCulture);
            SQL = " GROUP BY IDMCU, IDITM, IDLOTN, IDLOCN) LPDetailQuery ";

            SQL = " ON ((LIMCU = IDMCU) AND (LIITM = IDITM) AND (LILOTN = IDLOTN) AND (LILOCN = IDLOCN)) ";

            if (this.Fetch())
            {
                dReturnQty = this.LIPQOH;
            }

            Log.Debug("Leaving GetOnHandQtyNotOnLP with RetValue: " + dReturnQty.ToString());

            return (dReturnQty);
        }

        /// <summary>
        /// Fetches the on-hand quantity not on a LP for a branch, item, location, and lot.</summary>
        /// <param name="sBranch">Table-formatted Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).</param>
        /// <param name="sLocation">Table-formatted Location (LOCN)</param>
        /// <param name="sLotNumber">Table-formatted Lot Number (LOTN)</param>
        /// <returns>Returns sum of F41021.LIPQOH On-Hand qty - sum of F46L11.IDPQOR qty.</returns>
        public virtual double GetOnHandQtyNotOnLP(string sBranch, double dShortItemNumber, string sLocation, string sLotNumber)
        {
            double dReturnQty = 0;
            StringBuilder sb = new StringBuilder();

            Log.Debug("Entering GetOnHandQtyNotOnLP().");

            sb.Append("SELECT (SELECT SUM(LIPQOH) AS LIPQOH FROM ");
            sb.Append(this.GetFullTableName("F41021"));
            sb.Append(" WHERE LIMCU = '");
            sb.Append(sBranch);
            sb.Append("' AND LIITM = ");
            sb.Append(dShortItemNumber.ToString(CultureInfo.InvariantCulture));
            sb.Append(" AND LILOCN = '");
            sb.Append(sLocation);
            sb.Append("' AND LILOTN = '");
            sb.Append(sLotNumber);
            sb.Append("') - (SELECT CASE WHEN SUM(IDPQOR) IS NOT NULL THEN SUM(IDPQOR) ELSE 0 END AS IDPQOR FROM ");
            sb.Append(this.GetFullTableName("F46L11"));
            sb.Append(" WHERE IDMCU = '");
            sb.Append(sBranch);
            sb.Append("' AND IDITM = ");
            sb.Append(dShortItemNumber.ToString(CultureInfo.InvariantCulture));
            sb.Append(" AND IDLOCN = '");
            sb.Append(sLocation);
            sb.Append("' AND IDLOTN = '");
            sb.Append(sLotNumber);
            sb.Append("')  AS LIPQOH");

            using (F41021 tblItemLocation = new F41021(DataAccess, "GetOnHandQtyNotOnLP_"))
            {
                tblItemLocation.ResetAll();
                tblItemLocation.SQL = sb.ToString();
                if (tblItemLocation.Fetch())
                {
                    dReturnQty = tblItemLocation.LIPQOH;
                }
            }

            Log.Debug("Leaving GetOnHandQtyNotOnLP() with RetValue: " + dReturnQty.ToString());

            return (dReturnQty);
        }

        #endregion

        #region RetrieveSpecificRecord

        /// <summary>
        /// Fetches a specific record for the specified Branch, Short Item Number, and
        /// Location combination.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItem">Short Item Number (LIITM)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <returns>Returns true if a record is fetched based on the Branch, Short Item Number,
        /// and Location; otherwise, false.</returns>
        public virtual bool RetrieveSpecificRecord(string sBranch, double dShortItem, string sLocation)
        {
            Log.Debug("Entering RetrieveSpecificRecord");

            return (RetrieveSpecificRecord(sBranch, dShortItem, sLocation, null));
        } // end RetrieveSpecificRecord

        /// <summary>
        /// Fetches a specific record for the specified Branch, Short Item Number, Lot, and
        /// Location combination.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItem">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <returns>Returns true if a record is fetched based on the Branch, Short Item Number,
        /// Lot, and Location; otherwise, false.</returns>
        public virtual bool RetrieveSpecificRecord(string sBranch, double dShortItem, string sLocation, string sLot)
        {
            Log.Debug("Entering RetrieveSpecificRecord");
            ResetAll();
            string availabilityexpression = AvailabilityExpression(sBranch);

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS,";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, ";
            SQL = "  case ";
            SQL = " when lilots = ' ' then " + availabilityexpression;
            SQL = " when lilots = '' then " + availabilityexpression;
            SQL = " when lilots is null then " + availabilityexpression;
            SQL = " else  " + availabilityexpression + " - LIPQOH";
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ, IOLOTS ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            if (sBranch != null)
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItem != -1)
            {
                LIITM = dShortItem;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            Log.Debug("Leaving RetrieveSpecificRecord");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end RetrieveSpecificRecord

        #endregion

        #region RetrieveSpecificRecordWithQuantity

        /// <summary>
        /// Fetches a specific record for the specified Branch, Short Item Number, and
        /// Location combination.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItem">Short Item Number (LIITM)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <returns>Returns true if a record is fetched based on the Branch, Short Item Number,
        /// and Location; otherwise, false.</returns>
        public virtual bool RetrieveSpecificRecordWithQuantity(string sBranch, double dShortItem, string sLocation)
        {
            Log.Debug("Entering RetrieveSpecificRecord");

            return (RetrieveSpecificRecord(sBranch, dShortItem, sLocation, null));
        } // end RetrieveSpecificRecordWithQuantity

        /// <summary>
        /// Fetches a specific record for the specified Branch, Short Item Number, Lot, and
        /// Location combination.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItem">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <returns>Returns true if a record is fetched based on the Branch, Short Item Number,
        /// Lot, and Location; otherwise, false.</returns>
        public virtual bool RetrieveSpecificRecordWithQuantity(string sBranch, double dShortItem, string sLocation, string sLot)
        {
            Log.Debug("Entering RetrieveSpecificRecordWithQuantity");

            ResetAll();

            if (sBranch != null)
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItem != -1)
            {
                LIITM = dShortItem;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            Where(Columns.LIPQOH, Condition.GreaterThan, 0);

            Log.Debug("Leaving RetrieveSpecificRecordWithQuantity");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end RetrieveSpecificRecordWithQuantity

        #endregion

        #region ValidateLotLocn

        /// <summary>
        /// Fetches records based on the current query and sets the Availability Expression upon
        /// successful fetch.
        /// </summary>
        /// <returns>Returns true if any records are Fetched based on the current query; otherwise,
        /// false.</returns>
        public virtual bool ValidateLotLocn()
        {
            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end ValidateLotLocn

        /// <summary>
        /// Validates that there exists a record for the specified Branch, Short Item Number, Lot,
        /// and Location combination.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sLot">Lot Number (LILOTN)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <returns>Returns true if a record is fetched based on the Branch, Short Item Number,
        /// Lot, and Location; otherwise, false.</returns>
        public virtual bool ValidateLotLocn(string sBranch, double dShortItemNumber, string sLot, string sLocation)
        {
            Log.Debug("Entering ValidateLotLocn");

            return (RetrieveSpecificRecord(sBranch, dShortItemNumber,
                (sLocation == null) ? " " : sLocation,
                (sLot == null) ? " " : sLot));
        } // end ValidateLotLocn

        #endregion

        #region ValidateItemLot

        /// <summary>
        /// Fetches records based on the current query and sets the Availability Expression upon
        /// successful fetch.
        /// </summary>
        /// <returns>Returns true if any records are Fetched based on the current query; otherwise,
        /// false.</returns>
        public virtual bool ValidateItemLot()
        {
            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end ValidateItemLot

        /// <summary>
        /// Validates that there exists a record for the specified Branch, Short Item Number,
        /// and Lot combination.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sLot">Lot Number (LILOTN)</param>
        /// <returns>Returns true if a record is fetched based on the Branch, Short Item Number,
        /// and Lot; otherwise, false.</returns>
        public virtual bool ValidateItemLot(string sBranch, double dShortItemNumber, string sLot)
        {
            Log.Debug("Entering ValidateItemLot");

            return (RetrieveSpecificRecord(sBranch, dShortItemNumber, null, sLot));
        } // end ValidateItemLot

        #endregion

        #region CalcQty

        /// <summary>
        /// System supports a rudimentary calculator function.  There is no "order precedence
        /// of operators", it is simply a left to right calculation using "X" for multiplication,
        /// "/" for division, "-" for subtraction, and "+" for addition.
        /// </summary>
        /// <param name="sQuantityExpression">Standard Arithmetic Expression</param>
        /// <returns>Returns a string containing the calculated quantity</returns>
        public virtual string CalcQty(string sQuantityExpression)
        {
            double dTotalQuantity = 0;
            char[] cDelimiters = {'X', '/', '-', '+'};
            string[] sParsedQuantityExpression;

            if (sQuantityExpression != null)
            {
                sQuantityExpression = sQuantityExpression.Replace("x", "X");
                sParsedQuantityExpression = sQuantityExpression.Split(cDelimiters);

                Log.Debug("Entering CalcQty");

                if (sParsedQuantityExpression.Length <= 1)
                {
                    try
                    {
                        Convert.ToDecimal(sQuantityExpression, CultureInfo.InvariantCulture);
                    } // end try
                    catch
                    {
                        return (null);
                    } // end catch
                } // end if

                if (IsNumeric(sParsedQuantityExpression[0]))
                {
                    dTotalQuantity = Convert.ToDouble(sParsedQuantityExpression[0],
                        CultureInfo.InvariantCulture);
                } // end if

                for (int i = 0, j = 1; i < sQuantityExpression.Length; i++)
                {
                    switch (sQuantityExpression[i])
                    {
                        case 'X':
                            if (j <= sParsedQuantityExpression.Length)
                            {
                                if (IsNumeric(sParsedQuantityExpression[j]))
                                {
                                    dTotalQuantity *= Convert.ToDouble(sParsedQuantityExpression[j],
                                        CultureInfo.InvariantCulture);
                                } // end if

                                j++;
                            } // end if

                            break;
                        case '/':
                            if (j <= sParsedQuantityExpression.Length)
                            {
                                if (IsNumeric(sParsedQuantityExpression[j]))
                                {
                                    dTotalQuantity /= Convert.ToDouble(sParsedQuantityExpression[j],
                                        CultureInfo.InvariantCulture);
                                } // end if

                                j++;
                            } // end if

                            break;
                        case '-':
                            if (j <= sParsedQuantityExpression.Length)
                            {
                                if (IsNumeric(sParsedQuantityExpression[j]))
                                {
                                    dTotalQuantity -= Convert.ToDouble(sParsedQuantityExpression[j],
                                        CultureInfo.InvariantCulture);
                                } // end if

                                j++;
                            } // end if

                            break;
                        case '+':
                            if (j <= sParsedQuantityExpression.Length)
                            {
                                if (IsNumeric(sParsedQuantityExpression[j]))
                                {
                                    dTotalQuantity += Convert.ToDouble(sParsedQuantityExpression[j],
                                        CultureInfo.InvariantCulture);
                                } // end if

                                j++;
                            } // end if

                            break;
                    } // end switch
                } // end for
            } // end if

            Log.Debug("Leaving CalcQty with RetValue: " +
                      dTotalQuantity.ToString(CultureInfo.InvariantCulture));

            return (dTotalQuantity.ToString(CultureInfo.InvariantCulture));
        } // end CalcQty

        #endregion

        #region IsNumeric

        /// <summary>
        /// Determine whether value passed is a true numeric
        /// </summary>
        /// <param name="oValue">Object value</param>
        /// <returns>Returns true if the value can be parsed to a Double; otherwise, false.</returns>
        public static bool IsNumeric(object oValue)
        {
            try
            {
                if (oValue == null)
                {
                    return (false);
                } // end if
                else
                {
                    System.Double.Parse(oValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture);

                    return (true);
                } // end else
            } // end try
            catch (FormatException)
            {
                return (false);
            } // end catch
        } // end IsNumeric

        #endregion

        #region RetrievePrimaryLocation

        /// <summary>
        /// Retrieves Enterprise 1 Primary Location for a given Branch/Item combination.
        /// </summary>
        /// <example>
        /// <code>
        ///    _PrimaryLocation = RetrievePrimaryLocation(_BranchPlant, dShortItemNumber);
        ///    _UserSession.ScreenHandler.ScreenMap.Add("PRIMARYLOCATION", PrimaryLocation);
        /// </code>
        /// </example>
        /// <returns>Returns the Primary Location if found; otherwise, an empty string.</returns>
        public virtual string RetrievePrimaryLocation(string sBranch, double dShortItemNumber)
        {
            Log.Debug("Entering RetrievePrimaryLocation");

            ResetAll();
            LIMCU = sBranch;
            LIITM = dShortItemNumber;
            LIPBIN = "P";

            Log.Debug("Leaving RetrievePrimaryLocation with RetValue: ");

            return (base.Fetch() ? LILOCN : "");
        } // end RetrievePrimaryLocation

        #endregion

        #region FetchInventoryRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location, and Lot
        /// number where the Short Item Number is also in the F4101 (Item Master).  The fields included
        /// in the data set are the following:  LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM,
        /// IMUOM1, IMDSC1, IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,
        /// LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, and LIQONL.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sOrderByCols">Colums by which the fetched results should be sorted,
        /// delimited by commas.  For example, "LILOTN, LILOCN".  DO NOT INCLUDE THE ORDER BY
        /// CLAUSE WITHIN THE STRING JUST THE COLUMNS.  Default = "", which excludes the ORDER BY
        /// clause from the search.</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location, and Lot number specified; otherwise, false.</returns>
        public virtual bool FetchInventoryRecords(string sBranch, double dShortItemNumber, string sLocation, string sLot, string sShowZeroOnHand, string sOrderByCols)
        {
            Log.Debug("Entering FetchInventoryRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, sLocation, sLot, sShowZeroOnHand,
                sOrderByCols, new string[0]));
        } // end FetchInventoryRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location,
        /// Lot number, and allowed Lot Hold Code where the Short Item Number is also in the
        /// F4101 (Item Master).  The fields included in the data set are the following:  LIMCU,
        /// LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, IMDSC2, LIPCOM, LIHCOM,
        /// LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1,
        /// LIQTO2, and LIQONL.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sOrderByCols">Colums by which the fetched results should be sorted,
        /// delimited by commas.  For example, "LILOTN, LILOCN".  DO NOT INCLUDE THE ORDER BY
        /// CLAUSE WITHIN THE STRING JUST THE COLUMNS.  Default = "", which excludes the ORDER BY
        /// clause from the search.</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes (LILOTS).</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location, Lot number, and allowed Lot Hold Codes specified; otherwise, false.</returns>
        public virtual bool FetchInventoryRecords(string sBranch, double dShortItemNumber, string sLocation,
            string sLot, string sShowZeroOnHand, string sOrderByCols,
            string[] sAllowedHoldCodes)
        {
            Log.Debug("Entering FetchInventoryRecords");
            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, IMITM, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = "  case ";
            SQL = " when lilots = ' ' then " + availabilityexpression;
            SQL = " when lilots = '' then " + availabilityexpression;
            SQL = " when lilots is null then " + availabilityexpression;
            SQL = " else  " + availabilityexpression + " - LIPQOH";
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            #endregion

            if ((sAllowedHoldCodes != null) && (sAllowedHoldCodes.Length > 0))
            {
                Where("AND", Columns.LILOTS, GenerateAllowedHoldCodes(sAllowedHoldCodes), "");
            } // end if

            // Checking to see if the Quantity On Hand should be checked.
            if (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0)
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            } // end if

            if ((sOrderByCols != null) && (sOrderByCols.Trim().Length > 0))
            {
                _Conditionals.Append("ORDER BY " + sOrderByCols);
            } // end if

            Log.Debug("Leaving FetchInventoryRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        #endregion

        #region FetchInventoryCount

        /// <summary>
        /// Fetches count of all records that contain the specified Branch, Short Item Number,
        /// and Lot number where the Short Item Number is also in the F4101 (Item Master).
        /// If the count is zero, then there is no reason to attempt to retrieve data
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location specified; otherwise, false.</returns>
        public virtual Int32 FetchInventoryCount(string sBranch, double dShortItemNumber, string sLocation)
        {
            Log.Debug("Entering FetchInventoryCount");

            ResetAll();

            SQL = "SELECT count(*) AS COUNT ";
            SQL = "FROM " + FullName + " INNER JOIN ";
            SQL = GetFullTableName("F4101") + " ON (LIITM = IMITM) ";

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if (sBranch != null)
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            #endregion

            if (base.Fetch())
            {
                Log.Debug("Leaving FetchInventoryCount with " + Rows[0]["Count"].ToString() + " records.");

                return (Convert.ToInt32(Rows[0]["Count"], CultureInfo.InvariantCulture));
            }
            else
            {
                Log.Debug("Leaving FetchInventoryCount with 0 records.");

                return (0);
            }
        }

        #endregion

        #region RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, null, null, sShowZeroOnHand,
                "LILOTN, LILOCN"));
        } // end RetrieveItemLocationRecords

        #endregion

        #region RetrieveBranchItemRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch where the Short Item Number
        /// is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch specified;
        /// otherwise, false.</returns>
        public virtual bool RetrieveBranchItemRecords(string sBranch, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveBranchItemRecords");

            return (FetchInventoryRecords(sBranch, -1, null, null, sShowZeroOnHand,
                "IMDSC1, LILOTN, LILOCN"));
        } // end RetrieveBranchItemRecords

        #endregion

        #region RetrieveItemLotRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, and Lot
        /// where the Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sLotNumber">Lot Number (LILOTN)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item
        /// Number, and Lot specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLotRecords(string sBranch, double dShortItemNumber, string sLotNumber, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveItemLotRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, null, sLotNumber, sShowZeroOnHand,
                "LILOTN, LILOCN"));
        } // end RetrieveItemLotRecords

        #endregion

        #region RetrieveItemSerialRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, and Location
        /// where the Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item
        /// Number, and Location specified; otherwise, false.</returns>
        public virtual bool RetrieveItemSerialRecords(string sBranch, double dShortItemNumber, string sLocation, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveItemSerialRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, sLocation, null, sShowZeroOnHand,
                "LILOCN"));
        } // end RetrieveItemSerialRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location, and
        /// allowed Lot Hold Codes where the Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes (LILOTS).</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item
        /// Number, Location, and allowed Lot Hold Codes specified; otherwise, false.</returns>
        public virtual bool RetrieveItemSerialRecords(string sBranch, double dShortItemNumber, string sLocation, string sShowZeroOnHand, string[] sAllowedHoldCodes)
        {
            Log.Debug("Entering RetrieveItemSerialRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, sLocation, null, sShowZeroOnHand,
                "LILOCN", sAllowedHoldCodes));
        } // end RetrieveItemSerialRecords

        #endregion

        #region RetrieveItemsInLocation

        /// <summary>
        /// Fetches all records that contain the specified Branch and Location where the Short
        /// Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Location
        /// specified; otherwise, false.</returns>
        public virtual bool RetrieveItemsInLocation(string sBranch, string sLocation, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveItemsInLocation");

            return (FetchInventoryRecords(sBranch, -1, sLocation, null, sShowZeroOnHand, "LILOCN"));
        } // end RetrieveItemsInLocation

        #endregion

        #region RetrieveLotSerialNumberRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, and
        /// Lot Number where the Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sLotNumber">Lot Number (LILOTN)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// and Lot Number specified; otherwise, false.</returns>
        public virtual bool RetrieveLotSerialNumberRecords(string sBranch, double dShortItemNumber, string sLotNumber, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveLotSerialNumberRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, null, sLotNumber, sShowZeroOnHand,
                "LILOCN"));
        } // end RetrieveLotSerialNumberRecords

        #endregion

        #region VerifyContainerIntegrity

        /// <summary>
        /// Verifies that the container specified is in the F41021 at most once with quantity
        /// on-hand.
        /// </summary>
        /// <param name="sContainer">Container (Lot Number / LILOTN)</param>
        /// <returns>Returns true if the Container specified exists at most once with quantity
        /// on-hand; otherwise, false.</returns>
        public virtual bool VerifyContainerIntegrity(string sContainer)
        {
            Log.Debug("Entering VerifyContainerIntegrity");

            bool ReturnCode;

            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL ";

            SQL = "FROM " + FullName + " INNER JOIN ";
            SQL = GetFullTableName("F4101") + " ON (LIITM = IMITM) ";

            LILOTN = sContainer;
            Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");

            if (Fetch())
            {
                if (Rows.Count == 1)
                {
                    /// Only one record is returned, therefore integretiy is met.
                    /// Retrieve by specific record to ensure all calculated columns are correctly accounted for
                    ReturnCode = RetrieveSpecificRecord(LIMCU, LIITM, LILOCN, LILOTN);
                }
                else
                {
                    /// More than one row, container integrity is comprimised
                    ReturnCode = false;
                }
            }
            else
            {
                /// No records returned, therefore container integrity is met
                ReturnCode = true;
            }

            return ReturnCode;
        } // end VerifyContainerIntegrity

        /// <summary>
        /// Verifies that the container specified is in the F41021 at most once with quantity
        /// on-hand, and its Branch matches that specified.
        /// </summary>
        /// <param name="sBranch">Optional.  Branch Plant (LIMCU) to which the container must
        /// exist with quantity on-hand as long as the Container exists with quantity on-hand.
        /// If omitted, the Branch criteria is excluded.</param>
        /// <param name="sContainer">Container (Lot Number / LILOTN)</param>
        /// <returns>Returns true if the Container specified exists at most once with quantity
        /// on-hand, and the Branch specified matches; otherwise, false.</returns>
        public virtual bool VerifyContainerIntegrity(string sBranch, string sContainer)
        {
            return (VerifyContainerIntegrity(sBranch, sContainer, -1));
        } // end VerifyContainerIntegrity

        /// <summary>
        /// Verifies that the container specified is in the F41021 at most once with quantity
        /// on-hand, and its Branch and short item matches that specified.
        /// </summary>
        /// <param name="sBranch">Optional.  Branch Plant (LIMCU) to which the container must
        /// exist with quantity on-hand as long as the Container exists with quantity on-hand.
        /// If omitted, the Branch criteria is excluded.</param>
        /// <param name="sContainer">Container (Lot Number / LILOTN)</param>
        /// <param name="dItemNumber">Optional.  Short Item Number (LIITM) contained on the
        /// container, which must exist with quantity on-hand.  If omitted, a value of negative
        /// one is used, which will ensure that the short item criteria is excluded.</param>
        /// <returns>Returns true if the Container specified exists at most once with quantity
        /// on-hand, and the Branch and short item specified matches; otherwise, false.</returns>
        public virtual bool VerifyContainerIntegrity(string sBranch, string sContainer, double dItemNumber)
        {
            // Verifying that the Container exists in the F41021 at most once with quantity on-hand.
            if (VerifyContainerIntegrity(sContainer))
            {
                if (ThisDataTable.Rows.Count == 0)
                {
                    return (true);
                } // end if

                // The trim should not be necessary, but due to the problems with the DataManipulation,
                // it is.
                else if (((sBranch == null) || (sBranch.Trim() == LIMCU.Trim())) && // Branch must match
                         ((dItemNumber == -1) || (LIITM == dItemNumber))) // Item must match if given
                {
                    return (true);
                } // end else if
                else
                {
                    return (false);
                } // end else
            } // end if
            else
            {
                return (false);
            } // end else
        } // end VerifyContainerIntegrity

        #endregion

        #region RetrieveContainersInBranch

        /// <summary>
        /// Fetches all records that contain the specified Branch where the Short Item Number
        /// is also in the F4101 (Item Master) but excluding those records with the Lot Number
        /// equal to " ".
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch specified but
        /// excluding those records with the Lot Number equal to " "; otherwise, false.</returns>
        public virtual bool RetrieveContainersInBranch(string sBranch, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveContainersInBranch");

            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = "  case ";
            SQL = " when lilots = ' ' then " + availabilityexpression;
            SQL = " when lilots = '' then " + availabilityexpression;
            SQL = " when lilots is null then " + availabilityexpression;
            SQL = " else  " + availabilityexpression + " - LIPQOH";
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            }

            if ((sShowZeroOnHand != null) &&
                (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0))
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            } // end if

            Where("AND", Columns.LILOTN, Condition.NotEqual, " ", "");

            AppendToWhere("ORDER BY LILOTN, LILOCN");

            Log.Debug("Leaving RetrieveContainersInBranch with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end RetrieveContainersInBranch

        #endregion

        #region RetrieveContainersForItem

        /// <summary>
        /// Fetches all records that contain the specified Branch where the Short Item Number
        /// is also in the F4101 (Item Master) but excluding those records with the Lot Number
        /// equal to " ".
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItem">Short ItemNumber (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch specified but
        /// excluding those records with the Lot Number equal to " "; otherwise, false.</returns>
        public virtual bool RetrieveContainersForItem(string sBranch, double dShortItem, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveContainersForItem");
            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = "  case ";
            SQL = " when lilots = ' ' then " + availabilityexpression;
            SQL = " when lilots = '' then " + availabilityexpression;
            SQL = " when lilots is null then " + availabilityexpression;
            SQL = " else  " + availabilityexpression + " - LIPQOH";
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItem != 0)
            {
                LIITM = dShortItem;
            } // end if

            Where("AND", Columns.LILOTN, Condition.NotEqual, " ", "");

            // Checking to see if the Quantity On Hand should be checked.
            if (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0)
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            } // end if

            AppendToWhere("ORDER BY LILOTN, LILOCN");

            Log.Debug("Leaving RetrieveContainersForItem with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end RetrieveContainersInBranch

        #endregion

        #region RetrieveSpecificContainer

        /// <summary>
        /// Fetches all records that contain the specified Branch and Container Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sContainer">Container Number (LILOTN)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Container Number
        /// specified; otherwise, false.</returns>
        public virtual bool RetrieveSpecificContainer(string sBranch, string sContainer, string sShowZeroOnHand)
        {
            Log.Debug("Entering RetrieveSpecificContainer");

            return (FetchInventoryRecords(sBranch, -1, null, sContainer, sShowZeroOnHand, "LILOTN, LILOCN"));
        } // end RetrieveSpecificContainer

        #endregion

        #region ShowSummarizedItemLocationRecords

        /// <summary>
        /// Fetches Item Balance records from F41021/F4101 for a given Branch Plant and Short
        /// Item Number
        /// Does not appear to be used in Base RFS
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItem">Short Item Number (LIITM)</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item Number
        /// specified; otherwise, false.</returns>
        public virtual bool ShowSummarizedItemLocationRecords(string sBranch, double dShortItem)
        {
            Log.Debug("Entering ShowSummarizedItemLocationRecords");

            ResetAll();

            SQL = "SELECT LIITM, LILOCN, SUM(LIPQOH) AS LIPQOH, LILOTS, IMLITM, IMUOM1, ";
            SQL = "SUM(LIPCOM) AS LIPCOM, SUM(LIHCOM) AS LIHCOM, SUM(LIFCOM) AS LIFCOM, ";
            SQL = "SUM(LIQOWO) AS LIQOWO, SUM(LIOT1P) AS LIOT1P, SUM(LIOT2P) AS LIOT2P, ";
            SQL = "SUM(LIPREQ) AS LIPREQ, SUM(LIQWBO) AS LIQWBO, SUM(LIOT1A) AS LIOT1A, ";
            SQL = "SUM(LIQTIN) AS LIQTIN, SUM(LIQTTR) AS LIQTTR, SUM(LIQTO1) AS LIQTO1, ";
            SQL = "SUM(LIQTO2) AS LIQT02, SUM(LIQONL) AS LIQONL ";
            SQL = "FROM " + FullName + " INNER JOIN ";
            SQL = GetFullTableName("F4101") + " ON (LIITM = IMITM) ";

            LIMCU = sBranch;
            LIITM = dShortItem;

            _Conditionals.Append("GROUP BY LIITM, IMLITM, IMUOM1, LILOCN, LILOTS");

            Log.Debug("Leaving ShowSummarizedItemLocationRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end ShowSummarizedItemLocationRecords

        #endregion

        #region ValidateItemLocationRecord

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, and
        /// Location where the Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranchPlant">Branch Plant (LIMCU)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <param name="dShortItem">Short Item Number (LIITM)</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// and Location specified; otherwise, false.</returns>
        public virtual bool ValidateItemLocationRecord(string sBranchPlant, double dShortItem, string sLocation)
        {
            Log.Debug("Entering ValidateItemLocationRecord");

            return (FetchInventoryRecords(sBranchPlant, dShortItem, sLocation, null, "Y", ""));
        } // end ValidateItemLocationRecord

        #endregion

        #region ShowOtherLocationsForItem

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Quantity OnHand is greater than zero and the Short Item Number is also in the F4101
        /// (Item Master), but excluding records that contain the OmitLocation.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sOmitLocation">Location (LILOCN) to be omitted from the results</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item Number
        /// specified where the Quantity OnHand is greater than zero and the Short Item Number is
        /// also in the F4101 (Item Master), but excluding records that contain the OmitLocation;
        /// otherwise, false.</returns>
        public virtual bool ShowOtherLocationsForItem(string sBranch, double dShortItemNumber, string sOmitLocation)
        {
            Log.Debug("Entering ShowOtherLocationsForItem");

            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = "  case ";
            SQL = " when lilots = ' ' then " + availabilityexpression;
            SQL = " when lilots = '' then " + availabilityexpression;
            SQL = " when lilots is null then " + availabilityexpression;
            SQL = " else  " + availabilityexpression + " - LIPQOH";
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";
            LIMCU = sBranch;
            LIITM = dShortItemNumber;

            Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            Where("AND", Columns.LILOCN, Condition.NotEqual, sOmitLocation, "");

            _Conditionals.Append("ORDER BY LILOTN, LILOCN");

            Log.Debug("Leaving ShowOtherLocationsForItem with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end ShowOtherLocationsForItem

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Quantity OnHand is greater than zero and the Short Item Number is also in the F4101
        /// (Item Master), but excluding records that contain the OmitLocation.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sOmitLocation">Location (LILOCN) to be omitted from the results</param>
        /// <param name="sOmitLot">Lot to exclude from search</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item Number
        /// specified where the Quantity OnHand is greater than zero and the Short Item Number is
        /// also in the F4101 (Item Master), but excluding records that contain the OmitLocation;
        /// otherwise, false.</returns>
        public virtual bool ShowOtherLocationsForItem(string sBranch, double dShortItemNumber, string sOmitLocation, string sOmitLot)
        {
            Log.Debug("Entering ShowOtherLocationsForItem: ");

            ResetAll();

            SQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMUOM1, IMDSC1, LIPCOM, LIHCOM, ";
            SQL = "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            SQL = "LIQTTR, LIQTO1, LIQTO2, LIQONL ";
            SQL = "FROM " + FullName + " INNER JOIN ";
            SQL = GetFullTableName("F4101") + " ON (LIITM = IMITM) ";

            LIMCU = sBranch;
            LIITM = dShortItemNumber;

            Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            Where("AND ((", Columns.LILOCN, Condition.EqualTo, sOmitLocation, "");
            Where("AND", Columns.LILOTN, Condition.NotEqual, sOmitLot, ")");
            Where("OR", Columns.LILOCN, Condition.NotEqual, sOmitLocation, ")");

            _Conditionals.Append("ORDER BY LILOTN, LILOCN");

            Log.Debug("Leaving ShowOtherLocationsForItem with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end ShowOtherLocationsForItem

        #endregion

        #region RetrieveContainer

        /// <summary>
        /// Fetches all records that contain the specified Branch and Container where the
        /// Quantity OnHand is greater than zero and the Short Item Number is also in the F4101
        /// (Item Master), but excluding records that contain the OmitLocation.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="sContainer">Container Number (LILOTN)</param>
        /// <returns>Returns true if records are fetched based on the Branch and Container
        /// specified where the Quantity OnHand is greater than zero and the Short Item Number is
        /// also in the F4101 (Item Master); otherwise, false.</returns>
        public virtual bool RetrieveContainer(string sBranch, string sContainer)
        {
            Log.Debug("Entering RetrieveContainer");

            return (FetchInventoryRecords(((sBranch == null) || // Determine Branch
                                           (sBranch.Trim().Length == 0))
                    ? null
                    : sBranch,
                -1, null, sContainer, "N", "LILOTN, LILOCN", new string[0]));
        } // end RetrieveContainer

        #endregion

        #region IsContainerOnHold

        /// <summary>
        /// Checks to see if the specified Container (Lot) is on hold based on the specified Branch Plant, Short
        /// Item Number, Location, and allowed Hold Codes.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dItem">Short Item Number (LIITM)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <param name="sLot">Lot Number (LILOTN)</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes</param>
        /// <returns>Returns true if a record is found for the specified Branch, Item, Location, and
        /// Lot, and the status is not an allowed hold code; otherwise, false.</returns>
        public virtual bool IsContainerOnHold(string sBranch, double dItem, string sLocation, string sLot, string[] sAllowedHoldCodes)
        {
            bool bReturnVal = true;

            if ((sAllowedHoldCodes == null) || (sAllowedHoldCodes.Length == 0) ||
                (sAllowedHoldCodes[0].Trim() == "*"))
            {
                bReturnVal = false; // Allow all hold codes.
            } // end if

            // If a record is found, which should be at most one since the keys are the LIMCU, LIITM,
            // LILOCN, and LILOTN, check to see if it is on hold; otherwise, the Lot is considered to
            // not be on hold within the F41021; however, the next step would be to check the F4108 to
            // see if the Lot is on hold, but at this time, none of the other workflows do this so this
            // might be an enhancement.
            else if (RetrieveSpecificRecordWithQuantity(sBranch, dItem, sLocation, sLot))
            {
                if (LILOTS.Trim().Length == 0)
                {
                    bReturnVal = false; // Because the Lot status is equal to blank, the Lot is not on hold
                } // end if
                else if ((sAllowedHoldCodes.Length > 0) && (sAllowedHoldCodes[0].Length != 0))
                {
                    for (int i = 0; i < sAllowedHoldCodes.Length; i++)
                    {
                        if (LILOTS.Trim() == sAllowedHoldCodes[i])
                        {
                            bReturnVal = false; // Hold code matches an allowed hold code in the array.
                            break;
                        } // end if
                    } // end for
                } // end else if
            } // end else if
            else
            {
                bReturnVal = false; // Since a F41021 record was not found, the Lot is not on hold
            } // end else

            return (bReturnVal);
        } // end IsLotOnHold

        #endregion

        #region IsLotOnHold

        /// <summary>
        /// Checks to see if the specified Lot is on hold based on the specified Branch Plant, Short
        /// Item Number, Location, and allowed Hold Codes.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dItem">Short Item Number (LIITM)</param>
        /// <param name="sLocation">Location (LILOCN)</param>
        /// <param name="sLot">Lot Number (LILOTN)</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes</param>
        /// <returns>Returns true if a record is found for the specified Branch, Item, Location, and
        /// Lot, and the status is not an allowed hold code; otherwise, false.</returns>
        public virtual bool IsLotOnHold(string sBranch, double dItem, string sLocation, string sLot, string[] sAllowedHoldCodes)
        {
            bool bReturnVal = true;

            if ((sAllowedHoldCodes == null) || (sAllowedHoldCodes.Length == 0) ||
                (sAllowedHoldCodes[0].Trim() == "*"))
            {
                bReturnVal = false; // Allow all hold codes.
            } // end if

            // If a record is found, which should be at most one since the keys are the LIMCU, LIITM,
            // LILOCN, and LILOTN, check to see if it is on hold; otherwise, the Lot is considered to
            // not be on hold within the F41021; however, the next step would be to check the F4108 to
            // see if the Lot is on hold, but at this time, none of the other workflows do this so this
            // might be an enhancement.
            else if (RetrieveSpecificRecord(sBranch, dItem, sLocation, sLot))
            {
                if (LILOTS.Trim().Length == 0)
                {
                    bReturnVal = false; // Because the Lot status is equal to blank, the Lot is not on hold
                } // end if
                else if ((sAllowedHoldCodes.Length > 0) && (sAllowedHoldCodes[0].Length != 0))
                {
                    for (int i = 0; i < sAllowedHoldCodes.Length; i++)
                    {
                        if (LILOTS.Trim() == sAllowedHoldCodes[i])
                        {
                            bReturnVal = false; // Hold code matches an allowed hold code in the array.
                            break;
                        } // end if
                    } // end for
                } // end else
            } // end else if
            else
            {
                bReturnVal = false; // Since a F41021 record was not found, the Lot is not on hold
            } // end else

            return (bReturnVal);
        } // end IsLotOnHold

        #endregion

        #region GenerateAllowedHoldCodes

        /// <summary>
        /// Generates an array of allowed hold codes, which appends the blank hold code to a
        /// specified array.  If the specified array of allowed hold codes starts with the empty
        /// string or has no elements, then the array generated only contains the blank hold code.
        /// </summary>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot hold codes (LILOTS).</param>
        /// <returns>Returns an array containing the hold code in the specified array plus the blank
        /// hold code</returns>
        protected static string[] GenerateAllowedHoldCodes(string[] sAllowedHoldCodes)
        {
            int iNumHoldCodes;
            string[] sAllowedHoldCodesPlusBlank;

            // If there allowed hold codes array is empty or begins with an empty string, only
            // blank Lot statuses are valid; otherwise, all hold codes in the array are valid plus
            // the blank Lot status.
            if ((sAllowedHoldCodes.Length == 0) || (sAllowedHoldCodes[0].Length == 0))
            {
                iNumHoldCodes = 0;
            } // end if
            else
            {
                iNumHoldCodes = sAllowedHoldCodes.Length;
            } // end else

            sAllowedHoldCodesPlusBlank = new string[iNumHoldCodes + 1];

            // Setting the array of Lot hold codes to include a Lot status of blank " " too.
            // This will ensure that all hold codes in the array plus the blank will be fetched.
            // If for some reason the user does not have anything in the array or it is just "*",
            // the Where method will handle that and actually exclude this check, which is correct
            // because nothing or "*" means to allow all.
            for (int i = 0; i < iNumHoldCodes; i++)
            {
                sAllowedHoldCodesPlusBlank[i] = sAllowedHoldCodes[i];
            } // end for

            sAllowedHoldCodesPlusBlank[iNumHoldCodes] = " ";

            return (sAllowedHoldCodesPlusBlank);
        } // end GenerateAllowedHoldCodes

        #endregion

        #region SetAllowedHoldCodes

        protected virtual void SetAllowedHoldCodes(string[] allowedHoldCodes)
        {
            if ((allowedHoldCodes != null) && (allowedHoldCodes.Length > 0))
            {
                string prefix = ((DBEmptyString.Trim().StartsWith("N", StringComparison.InvariantCultureIgnoreCase)) ? "N" : "");

                SQL = String.Format(" AND COALESCE(LILOTS, {0}) IN ", DBEmptyString);
                SQL = String.Format("({0}'{1}') ", prefix, String.Join(String.Format("',{0}'", prefix), GenerateAllowedHoldCodes(allowedHoldCodes)));

                SQL = String.Format(" AND COALESCE(IOLOTS, {0}) IN ", DBEmptyString);
                SQL = String.Format("({0}'{1}') ", prefix, String.Join(String.Format("',{0}'", prefix), GenerateAllowedHoldCodes(allowedHoldCodes)));
            }
        }

        #endregion

        #region FetchInventoryRecordsForWOPick

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location,
        /// Lot number, and allowed Lot Hold Code where the Short Item Number is also in the
        /// F4101 (Item Master).  The fields included in the data set are the following:  LIMCU,
        /// LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, IMDSC2, LIPCOM, LIHCOM,
        /// LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1,
        /// LIQTO2, and LIQONL.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sOrderByCols">Colums by which the fetched results should be sorted,
        /// delimited by commas.  For example, "LILOTN, LILOCN".  DO NOT INCLUDE THE ORDER BY
        /// CLAUSE WITHIN THE STRING JUST THE COLUMNS.  Default = "", which excludes the ORDER BY
        /// clause from the search.</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes (LILOTS).</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location, Lot number, and allowed Lot Hold Codes specified; otherwise, false.</returns>
        public virtual bool FetchInventoryRecordsForWOPick(string sBranch, double dShortItemNumber, string sLocation,
            string sLot, string sShowZeroOnHand, string sOrderByCols,
            string[] sAllowedHoldCodes)
        {
            Log.Debug("Entering FetchInventoryRecords");
            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, IMITM, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej <=" + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = availabilityexpression + " as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LILOCN = LDLOCN AND LIMCU = LDMCU ";

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            #endregion

            if ((sAllowedHoldCodes != null) && (sAllowedHoldCodes.Length > 0))
            {
                Where("AND", Columns.LILOTS, GenerateAllowedHoldCodes(sAllowedHoldCodes), "");
            } // end if

            // Checking to see if the Quantity On Hand should be checked.
            if (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0)
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            } // end if

            //LD59RFLD03 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD03 = 1 OR LD59RFLD03 IS NULL) ");
            //LD59RFLD05 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD05 = 0 OR LD59RFLD05 IS NULL) ");

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if
            else if (sLot != null)
            {
                _Conditionals.AppendFormat(" AND LILOTN = {0} ", DBEmptyString);
            }

            if ((sOrderByCols != null) && (sOrderByCols.Trim().Length > 0))
            {
                _Conditionals.Append("ORDER BY " + sOrderByCols);
            } // end if

            Log.Debug("Leaving FetchInventoryRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        #endregion

        #region RetrieveSpecificRecordForWOPick

        public virtual bool RetrieveSpecificRecordForWOPick(string sBranch, double dShortItem, string sLocation, string sLot)
        {
            Log.Debug("Entering RetrieveSpecificRecord");
            string availabilityexpression = AvailabilityExpression(sBranch);

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS,";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej <=" + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, ";

            SQL = availabilityexpression + " as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ, IOLOTS ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            if (sBranch != null)
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItem != -1)
            {
                LIITM = dShortItem;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            Log.Debug("Leaving RetrieveSpecificRecord");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end RetrieveSpecificRecord

        #endregion

        #region GetSupplyForWOPicking

        public virtual bool GetSupplyForWOPicking(string branch, double WorkOrder, string orderType,
            string operSeq, string[] IssueTypesAllowed, string[] MatStatusExclude,
            string RFSmaterialStatus, string PRPcolumn, string[] PRPvalues,
            string[] AllowedHoldCodes, string StageLocation)
        {
            ResetAll();

            string availabilityExpression = AvailabilityExpression(branch);

            bool bIssueTypesAllowed = (IssueTypesAllowed != null && IssueTypesAllowed.Length > 0 && IssueTypesAllowed[0] != String.Empty);
            bool bPRPvalues = (PRPvalues != null && PRPvalues.Length > 0 && PRPvalues[0] != String.Empty);
            bool bMatStatusExclude = (MatStatusExclude != null && MatStatusExclude.Length > 0 && MatStatusExclude[0] != String.Empty);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, LMWSQQ, COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = availabilityExpression;
            SQL = " AS Availability, ";
            if (!string.IsNullOrEmpty(StageLocation))
            {
                SQL = string.Format(" case when LILOCN = '{0}' then 1 else 0 end as IsPrdLocn ", StageLocation);
            }
            else
            {
                SQL = "0 as IsPrdLocn ";
            }

            SQL = "FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F3111") + " ON LIITM = WMCPIT AND LIMCU = WMCMCU AND LIPQOH > 0 AND ";
            SQL = availabilityExpression + " > 0 ";
            SQL = "AND WMQNTA <> 999999999999999 AND (( ABS(WMUORG) + ABS(WMTRQT)) <> 0) ";
            SQL = " INNER JOIN " + GetFullTableName("F4102") + " ON LIITM = IBITM AND LIMCU = IBMCU ";
            SQL = "INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = "LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = "WHERE WMDOCO = " + WorkOrder + " AND WMDCTO = '" + orderType + "' ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";
            if (operSeq != null)
                SQL = "AND WMOPSQ = " + operSeq + " ";

            if (bIssueTypesAllowed)
                SQL = String.Format("AND WMITC IN ('{0}') ", String.Join("','", IssueTypesAllowed));

            if (bPRPvalues)
                SQL = String.Format("AND IBPRP {0} NOT IN ('{1}') ", PRPcolumn, String.Join("','", PRPvalues));

            if (bMatStatusExclude)
                SQL = String.Format("AND WMMTST NOT IN ('{0}') ", String.Join("','", MatStatusExclude));

            //LD59RFLD03 Pick Supply 1 = yes 0 = no
            SQL = " AND ((LD59RFLD03 = 1 OR LD59RFLD03 IS NULL) ";
            //LD59RFLD05 Pick Stage 1 = yes 0 = no
            SQL = " AND (LD59RFLD05 = 0 OR LD59RFLD05 IS NULL) ";
            if (!string.IsNullOrEmpty(StageLocation))
            {
                SQL = string.Format(" OR (LILOCN = '{0}')) ", StageLocation);
            }
            else
            {
                SQL = ") ";
            }

            SQL = "AND WMMTST <> '" + RFSmaterialStatus + "' ";
            SQL = "AND LIMCU = '" + _DataManipulatorTable.ManipulateDataForDisplay("F41021", "LIMCU", branch) + "' ";
            SetAllowedHoldCodes(AllowedHoldCodes);

            return FetchEfficient();
        }

        #endregion

        #region GetSupplyListForWOPicking

        public virtual bool GetSupplyListForWOPicking(string sBranch, double dShortItemNumber, string sLocation,
            string sOrderByCols, string[] AllowedHoldCodes,
            string PRPcolumn, string[] PRPvalues, string StageLocation)
        {
            Log.Debug("Entering FetchInventoryRecords");
            string availabilityexpression = AvailabilityExpression();
            StringBuilder sb = new StringBuilder();
            bool bPRPvalues = (PRPvalues != null && PRPvalues.Length > 0 && PRPvalues[0] != String.Empty);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";
            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = availabilityexpression + " as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";
            SQL = " INNER JOIN " + GetFullTableName("F4102") + " ON LIITM = IBITM AND LIMCU = IBMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LILOCN = LDLOCN AND LIMCU = LDMCU ";

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            #endregion

            Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");

            //LD59RFLD03 Pick Supply 1 = yes 0 = no
            SQL = " AND ((LD59RFLD03 = 1 OR LD59RFLD03 IS NULL) ";
            //LD59RFLD05 Pick Stage 1 = yes 0 = no
            SQL = " AND (LD59RFLD05 = 0 OR LD59RFLD05 IS NULL) ";
            if (!string.IsNullOrEmpty(StageLocation))
            {
                SQL = string.Format(" OR (LILOCN = '{0}')) ", StageLocation);
            }
            else
            {
                SQL = ") ";
            }

            if (bPRPvalues)
                _Conditionals.AppendFormat("AND IBPRP {0} NOT IN ('{1}') ", PRPcolumn, String.Join("','", PRPvalues));

            SQL = "AND LIMCU = '" + _DataManipulatorTable.ManipulateDataForDisplay("F41021", "LIMCU", sBranch) + "' ";
            SetAllowedHoldCodes(AllowedHoldCodes);

            if ((sOrderByCols != null) && (sOrderByCols.Trim().Length > 0))
            {
                sb.Append("ORDER BY " + sOrderByCols);
            } // end if

            AppendToWhere(sb.ToString());
            Log.Debug("Leaving FetchInventoryRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        #endregion

        #region GetSupplyForWOPicking

        public virtual bool GetSupplyForConsolidatedWOPicking(string branch, string startDate, string toDate, string[] issueTypesAllowed, string[] matStatusExclude, string pickedMatStatus, string prpColumn, string[] prpValuesExclude, string[] allowedLotHoldCodes, string stageLocation, string[] allowedWorkOrderStatusCodes, string componentItemFilter, string finishedGoodItemFilter)
        {
            ResetAll();

            string availabilityExpression = AvailabilityExpression(branch);

            bool bIssueTypesAllowed = (issueTypesAllowed != null && issueTypesAllowed.Length > 0 && issueTypesAllowed[0] != String.Empty);
            bool bPRPvalues = (prpValuesExclude != null && prpValuesExclude.Length > 0 && prpValuesExclude[0] != String.Empty);
            bool bMatStatusExclude = (matStatusExclude != null && matStatusExclude.Length > 0 && matStatusExclude[0] != String.Empty);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, LMWSQQ, COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = availabilityExpression;
            SQL = " AS Availability, ";
            if (!string.IsNullOrEmpty(stageLocation))
            {
                SQL = string.Format(" case when LILOCN = '{0}' then 1 else 0 end as IsPrdLocn ", stageLocation);
            }
            else
            {
                SQL = "0 as IsPrdLocn ";
            }

            SQL = "FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F3111") + " ON LIITM = WMCPIT AND LIMCU = WMCMCU AND LIPQOH > 0 AND ";
            SQL = availabilityExpression + " > 0 ";
            SQL = "AND WMQNTA <> 999999999999999 AND (( ABS(WMUORG) + ABS(WMTRQT)) <> 0) ";
            SQL = " INNER JOIN " + GetFullTableName("F4102") + " ON LIITM = IBITM AND LIMCU = IBMCU ";
            SQL = "INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = "LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = "WHERE COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";

            if (bIssueTypesAllowed)
                SQL = String.Format("AND WMITC IN ('{0}') ", String.Join("','", issueTypesAllowed));

            if (bPRPvalues)
                SQL = String.Format("AND IBPRP {0} NOT IN ('{1}') ", prpColumn, String.Join("','", prpValuesExclude));

            if (bMatStatusExclude)
                SQL = String.Format("AND WMMTST NOT IN ('{0}') ", String.Join("','", matStatusExclude));

            //LD59RFLD03 Pick Supply 1 = yes 0 = no
            SQL = " AND ((LD59RFLD03 = 1 OR LD59RFLD03 IS NULL) ";
            //LD59RFLD05 Pick Stage 1 = yes 0 = no
            SQL = " AND (LD59RFLD05 = 0 OR LD59RFLD05 IS NULL) ";
            if (!string.IsNullOrEmpty(stageLocation))
            {
                SQL = string.Format(" OR (LILOCN = '{0}')) ", stageLocation);
            }
            else
            {
                SQL = ") ";
            }

            SQL = "AND WMMTST <> '" + pickedMatStatus + "' ";
            SQL = "AND LIMCU = '" + _DataManipulatorTable.ManipulateDataForDisplay("F41021", "LIMCU", branch) + "' ";

            SetAllowedHoldCodes(allowedLotHoldCodes);

            return FetchEfficient();
        }

        #endregion

        #region Available

        /// <summary>
        /// Returns either the available or on hand quantity depending on the workflow option set
        /// </summary>
        /// <param name="Branch"></param>
        /// <param name="ShortItem"></param>
        /// <param name="Location"></param>
        /// <param name="Lot"></param>
        /// <param name="AvailOption"></param>
        /// <param name="TransQuantity"></param>
        /// <returns></returns>
        public virtual double Available(string Branch, double ShortItem, string Location, string Lot, string AvailOption, double TransQuantity)
        {
            this.ResetAll();
            double OnHandQuantity = 0;
            double QuantityAvailable = 0;

            if (this.RetrieveSpecificRecord(Branch, ShortItem, Location, Lot))
                OnHandQuantity = this.LIPQOH;
            else
                OnHandQuantity = 0;
            switch (AvailOption.Trim())
            {
                case "A": // available quantity
                    QuantityAvailable = this.Availability;
                    break;
                case "O": // on hand quantity
                    QuantityAvailable = OnHandQuantity;
                    break;
                case "I":
                    // SCR - BD015093 - Changed from _TransactionQuantity to _QuantityPicked because
                    // UOM Conversion may throw things off causing improper errors of "Insufficient
                    // Qty" to be displayed
                    QuantityAvailable = Convert.ToDouble(TransQuantity);
                    break;
            }

            return (QuantityAvailable);
        }

        #endregion

        #region FetchInventoryRecordsForFulfillment

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location,
        /// Lot number, and allowed Lot Hold Code where the Short Item Number is also in the
        /// F4101 (Item Master).  The fields included in the data set are the following:  LIMCU,
        /// LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, IMDSC2, LIPCOM, LIHCOM,
        /// LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1,
        /// LIQTO2, and LIQONL.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sOrderByCols">Colums by which the fetched results should be sorted,
        /// delimited by commas.  For example, "LILOTN, LILOCN".  DO NOT INCLUDE THE ORDER BY
        /// CLAUSE WITHIN THE STRING JUST THE COLUMNS.  Default = "", which excludes the ORDER BY
        /// clause from the search.</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes (LILOTS).</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location, Lot number, and allowed Lot Hold Codes specified; otherwise, false.</returns>
        public virtual bool FetchInventoryRecordsForFulfillment(string sBranch, double dShortItemNumber, string sLocation,
            string sLot, string sShowZeroOnHand, string sOrderByCols,
            string[] sAllowedHoldCodes, bool bAllowExpired)
        {
            Log.Debug("Entering FetchInventoryRecords");
            string availabilityexpression = AvailabilityExpression(sBranch);
            StringBuilder sb = new StringBuilder();
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, IMITM, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            if (!bAllowExpired)
            {
                SQL = " case when iommej = 0 then Available ";
                SQL = " when iommej <=" + this._TodaysDateInJulian + " then 0";
                SQL = " else Available end ";
            }
            else
            {
                SQL = " Available ";
            }

            SQL = " as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = availabilityexpression + " as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 ";
            SQL = " LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";
            SQL = " INNER JOIN ";
            SQL = GetFullTableName("F4100");
            SQL = " ON LIMCU = LMMCU AND LILOCN = LMLOCN ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            #endregion

            if ((sAllowedHoldCodes != null) && (sAllowedHoldCodes.Length > 0))
            {
                Where("AND", Columns.LILOTS, GenerateAllowedHoldCodes(sAllowedHoldCodes), "");
            } // end if

            // Checking to see if the Quantity On Hand should be checked.
            if (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0)
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            } // end if

            //Exclude All InterimLocation and Include Supply Locations
            //LD59RFLD01 AMF Supply 1 = yes 0 = no
            sb.Append(" AND (LD59RFLD01 = 1 OR LD59RFLD01 IS NULL) ");
            //LD59RFLD09 EndCap 1 = yes 0 = no
            sb.Append(" AND (LD59RFLD09 = 0 OR LD59RFLD09 IS NULL)  ");
            //LD59RFLD10  ForkLift 1 = yes 0 = no
            sb.Append(" AND (LD59RFLD10 = 0 OR LD59RFLD10 IS NULL)   ");
            if ((sOrderByCols != null) && (sOrderByCols.Trim().Length > 0))
            {
                sb.Append("ORDER BY " + sOrderByCols);
            } // end if

            AppendToWhere(sb.ToString());

            Log.Debug("Leaving FetchInventoryRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        #endregion

        #region Fetch Serial Number

        public bool FetchSerialNumber(string SerialNumber)
        {
            ResetAll();

            LILOTN = SerialNumber;
            AppendToWhere("AND LIPQOH > 0");

            return Fetch();
        }

        #endregion

        #region New Methods for JDESalesOrderPicking

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location,
        /// Lot number, and allowed Lot Hold Code where the Short Item Number is also in the
        /// F4101 (Item Master).  The fields included in the data set are the following:  LIMCU,
        /// LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, IMDSC2, LIPCOM, LIHCOM,
        /// LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1,
        /// LIQTO2, and LIQONL.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sOrderByCols">Colums by which the fetched results should be sorted,
        /// delimited by commas.  For example, "LILOTN, LILOCN".  DO NOT INCLUDE THE ORDER BY
        /// CLAUSE WITHIN THE STRING JUST THE COLUMNS.  Default = "", which excludes the ORDER BY
        /// clause from the search.</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes (LILOTS).</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location, Lot number, and allowed Lot Hold Codes specified; otherwise, false.</returns>
        public virtual bool FetchInventoryRecords(string sBranch, double dShortItemNumber, string sLocation,
            string sLot, string sShowZeroOnHand, string sOrderByCols,
            string[] sAllowedHoldCodes, bool pickingOnly)
        {
            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            F4108 lotTable = new F4108(DataAccess, "F41021Lot");
            Log.Debug("Entering FetchInventoryRecords");
            StringBuilder sb = new StringBuilder();
            _WarehouseConstants.ResetAll();
            _WarehouseConstants.CIMCU = sBranch;
            _WarehouseConstants.Fetch();

            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, ";
            SQL = "IMDSC2, IMITM, LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, ";
            SQL = "LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOMMEJ, IOLOTS, ";

            SQL = " case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = "  case ";
            SQL = " when lilots = ' ' then " + availabilityexpression;
            SQL = " when lilots = '' then " + availabilityexpression;
            SQL = " when lilots is null then " + availabilityexpression;
            SQL = " else  " + availabilityexpression + " - LIPQOH";
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            if (_WarehouseConstants.CILCTL.Trim() == "Y")
            {
                SQL = " INNER JOIN " + GetFullTableName("F4100");
                SQL = " ON LMMCU = LIMCU AND LMLOCN = LILOCN ";
                SQL = " LEFT JOIN " + GetFullTableName("F59RFLD");
                SQL = " ON LMMCU = LDMCU AND LMLOCN = LDLOCN ";
            }

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            #endregion

            SetAllowedHoldCodes(sAllowedHoldCodes);

            // Checking to see if the Quantity On Hand should be checked.
            if (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0)
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            } // end if

            if ((_WarehouseConstants.CILCTL.Trim() == "Y") && pickingOnly)
            {
                //LD59RFLD02 Pick Supply 1 = yes 0 = no
                sb.Append("  AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL)  ");
                //LD59RFLD02 Pick Stage 1 = yes 0 = no
                sb.Append("  AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            }

            if ((sOrderByCols != null) && (sOrderByCols.Trim().Length > 0))
            {
                sb.Append("ORDER BY " + sOrderByCols);
            } // end if

            AppendToWhere(sb.ToString());
            Log.Debug("Leaving FetchInventoryRecords with RetValue: ");

            locationTable.Dispose();
            lotTable.Dispose();
            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        #endregion

        #region New methods for AdvancedMaterialRequest & Fulfillment

        public virtual bool GetWarehouseAvailability(string sBranch, double dShortItemNumber, string sLocation,
            string sLot, string[] sAllowedHoldCodes, string toLocation)
        {
            Log.Warn("Entering GetWarehouseAvailability");
            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            SQL = "SELECT SUM(LIPQOH) as LIPQOH, ";
            SQL = " SUM(case when iommej = 0 then Available ";
            SQL = " when iommej < " + this._TodaysDateInJulian + " then 0";
            SQL = " else Available ";
            SQL = " end) as Availability ";
            SQL = " FROM " + GetFullTableName("F4101") + " Inner Join ";

            SQL = " (SELECT    LIMCU, LIITM, LILOCN, LIPQOH, LILOTN,LIPCOM, LIHCOM, LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ,LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1, LIQTO2, LIQONL, IOLOTS, ";
            SQL = "  " + availabilityexpression;
            SQL = " as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = " FROM " + GetFullTableName("F41021") + " F41021_1 LEFT OUTER JOIN ";
            SQL = GetFullTableName("F4108");
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";
            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LILOCN = LMLOCN AND LIMCU = LMMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            if ((sBranch != null) && (sBranch.Trim().Length > 0))
            {
                LIMCU = sBranch;
            } // end if

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            if (sLocation != null)
            {
                LILOCN = sLocation;
            } // end if

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            #endregion

            if ((sAllowedHoldCodes != null) && (sAllowedHoldCodes.Length > 0))
            {
                Where("AND", Columns.LILOTS, GenerateAllowedHoldCodes(sAllowedHoldCodes), "");
            } // end if

            // Checking to see if the Quantity On Hand should be checked.
            //Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");

            string locationFilter = "";
            if (toLocation != null && toLocation.Trim().Length > 0)
            {
                locationFilter += string.Format(" AND LILOCN <>  '{0}' ", toLocation);
            }

            //Exclude All InterimLocation and Include Supply Locations
            //LD59RFLD01 AMF Supply 1 = yes 0 = no
            locationFilter += " AND (LD59RFLD01 = 1 OR LD59RFLD01 IS NULL)   ";
            //LD59RFLD09 EndCap 1 = yes 0 = no
            locationFilter += " AND (LD59RFLD09 = 0 OR LD59RFLD09 IS NULL) ";
            //LD59RFLD10  ForkLift 1 = yes 0 = no
            locationFilter += " AND (LD59RFLD10 = 0 OR LD59RFLD10 IS NULL)  ";
            AppendToWhere(locationFilter);

            Log.Debug("Leaving GetWarehouseAvailability with RetValue: ");

            return (FetchEfficient()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        #endregion

        #region RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand, bool pickingOnly)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, null, null, sShowZeroOnHand,
                "LILOTN, LILOCN", new string[0], pickingOnly));
        } // end RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand, bool pickingOnly, string location)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, location, null, sShowZeroOnHand,
                "LILOTN, LILOCN", new string[0], pickingOnly));
        } // end RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand, bool pickingOnly, string[] allowedHoldCodes)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, null, null, sShowZeroOnHand,
                "LILOTN, LILOCN", allowedHoldCodes, pickingOnly));
        } // end RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand, bool pickingOnly, string location, string[] allowedHoldCodes)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, location, null, sShowZeroOnHand,
                "LILOTN, LILOCN", allowedHoldCodes, pickingOnly));
        } // end RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand,
            bool pickingOnly, string location, string[] allowedHoldCodes, string lot)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, location, lot, sShowZeroOnHand,
                "LILOTN, LILOCN", allowedHoldCodes, pickingOnly));
        } // end RetrieveItemLocationRecords

        /// <summary>
        /// Fetches all records that contain the specified Branch and Short Item Number where the
        /// Short Item Number is also in the F4101 (Item Master).
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU)</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM)</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <returns>Returns true if records are fetched based on the Branch and Short Item
        /// Number specified; otherwise, false.</returns>
        public virtual bool RetrieveItemLocationRecords(string sBranch, double dShortItemNumber, string sShowZeroOnHand, bool pickingOnly, string location, string lot)
        {
            Log.Debug("Entering RetrieveItemLocationRecords");

            return (FetchInventoryRecords(sBranch, dShortItemNumber, location, lot, sShowZeroOnHand,
                "LILOTN, LILOCN", new string[0], pickingOnly));
        } // end RetrieveItemLocationRecords

        #endregion

        #region GetSupplyForSOPicking

        public virtual bool GetSupplyForSOPickingByRoute(string branch, string route, string[] allowedStatus, string[] allowedHoldCodes, string userId, string pickManagerMode)
        {
            ResetAll();

            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            string availabilityExpression = AvailabilityExpression(branch);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, " + locationTable.ColumnsForSelect + " , COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = " COALESCE(LD59RFLD02, N'1') as Supply, COALESCE(LD59RFLD04, N'0') as Stage, ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end AS Availability ";
            SQL = " FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F4211") + " ON LIITM = SDITM AND LIMCU = SDMCU AND LIPQOH > 0 AND ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end > 0 ";
            if (pickManagerMode.Trim() != "0")
            {
                if (PickManagerTagTableExists)
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPMT");
                    SQL = " ON SDKCOO = PTKCOO AND SDDOCO = PTDOCO AND SDDCTO = PTDCTO AND SDLNID = PTLNID ";
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON PTUKID = PMUKID ";
                }
                else
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON SDURAB = PMUKID ";
                }
            }

            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = " WHERE SDMCU = '" + branch + "' AND SDROUT = '" + route + "' ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";

            SetAllowedSOStatusAndHoldCodes(allowedStatus, allowedHoldCodes);

            using (F4211 temp = new F4211(DataAccess, "F4211PickManager_"))
            {
                SQL = temp.GetPickManagerFilter(userId, pickManagerMode);
            }

            //LD59RFLD02 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL) ");
            //LD59RFLD02 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            locationTable.Dispose();
            return FetchEfficient();
        }

        public virtual bool GetSupplyForSOPickingByShipTo(string branch, double shipTo, string[] allowedStatus, string[] allowedHoldCodes, string userId, string pickManagerMode)
        {
            ResetAll();

            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            string availabilityExpression = AvailabilityExpression(branch);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, " + locationTable.ColumnsForSelect + " , COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = " COALESCE(LD59RFLD02, N'1') as Supply, COALESCE(LD59RFLD04, N'0') as Stage, ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end AS Availability ";
            SQL = " FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F4211") + " ON LIITM = SDITM AND LIMCU = SDMCU AND LIPQOH > 0 AND ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end > 0 ";
            if (pickManagerMode.Trim() != "0")
            {
                if (PickManagerTagTableExists)
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPMT");
                    SQL = " ON SDKCOO = PTKCOO AND SDDOCO = PTDOCO AND SDDCTO = PTDCTO AND SDLNID = PTLNID ";
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON PTUKID = PMUKID ";
                }
                else
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON SDURAB = PMUKID ";
                }
            }

            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = " WHERE SDMCU = '" + branch + "' AND SDSHAN = '" + shipTo.ToString() + "' ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";

            SetAllowedSOStatusAndHoldCodes(allowedStatus, allowedHoldCodes);

            using (F4211 temp = new F4211(DataAccess, "F4211PickManager_"))
            {
                SQL = temp.GetPickManagerFilter(userId, pickManagerMode);
            }

            //LD59RFLD02 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL) ");
            //LD59RFLD02 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            locationTable.Dispose();
            return FetchEfficient();
        }

        public virtual bool GetSupplyForSOPickingByOrder(string branch, double salesOrder, string orderType,
            string[] allowedStatus, string[] allowedHoldCodes, string userId, string pickManagerMode)
        {
            ResetAll();

            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            string availabilityExpression = AvailabilityExpression(branch);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, " + locationTable.ColumnsForSelect + " , COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = " COALESCE(LD59RFLD02, N'1') as Supply, COALESCE(LD59RFLD04, N'0') as Stage, ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end AS Availability ";
            SQL = " FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F4211") + " ON LIITM = SDITM AND LIMCU = SDMCU AND LIPQOH > 0 AND ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end > 0 ";
            if (pickManagerMode.Trim() != "0")
            {
                if (PickManagerTagTableExists)
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPMT");
                    SQL = " ON SDKCOO = PTKCOO AND SDDOCO = PTDOCO AND SDDCTO = PTDCTO AND SDLNID = PTLNID ";
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON PTUKID = PMUKID ";
                }
                else
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON SDURAB = PMUKID ";
                }
            }

            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = " WHERE SDMCU = '" + branch + "' AND SDDOCO = " + salesOrder + " AND SDDCTO = '" + orderType + "' ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";

            SetAllowedSOStatusAndHoldCodes(allowedStatus, allowedHoldCodes);

            using (F4211 temp = new F4211(DataAccess, "F4211PickManager_"))
            {
                SQL = temp.GetPickManagerFilter(userId, pickManagerMode);
            }

            //LD59RFLD02 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL) ");
            //LD59RFLD02 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            locationTable.Dispose();
            return FetchEfficient();
        }

        public virtual bool GetSupplyForSOPickingByPickSlip(string branch, double pickSlip, string[] allowedStatus,
            string[] allowedHoldCodes, string userId, string pickManagerMode)
        {
            ResetAll();

            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            string availabilityExpression = AvailabilityExpression(branch);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, " + locationTable.ColumnsForSelect + " , COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = " COALESCE(LD59RFLD02, N'1') as Supply, COALESCE(LD59RFLD04, N'0') as Stage, ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end AS Availability ";
            SQL = " FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F4211") + " ON LIITM = SDITM AND LIMCU = SDMCU AND LIPQOH > 0 AND ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end > 0 ";
            if (pickManagerMode.Trim() != "0")
            {
                if (PickManagerTagTableExists)
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPMT");
                    SQL = " ON SDKCOO = PTKCOO AND SDDOCO = PTDOCO AND SDDCTO = PTDCTO AND SDLNID = PTLNID ";
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON PTUKID = PMUKID ";
                }
                else
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON SDURAB = PMUKID ";
                }
            }

            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = " WHERE SDMCU = '" + branch + "' AND SDPSN = " + pickSlip + " ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";
            SetAllowedSOStatusAndHoldCodes(allowedStatus, allowedHoldCodes);

            using (F4211 temp = new F4211(DataAccess, "F4211PickManager_"))
            {
                SQL = temp.GetPickManagerFilter(userId, pickManagerMode);
            }

            //LD59RFLD02 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL) ");
            //LD59RFLD02 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            locationTable.Dispose();
            return FetchEfficient();
        }

        public virtual bool GetSupplyForSOPickingByShipment(string branch, double shipment, string[] allowedStatus,
            string[] allowedHoldCodes, string userId, string pickManagerMode)
        {
            ResetAll();

            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            string availabilityExpression = AvailabilityExpression(branch);

            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, " + locationTable.ColumnsForSelect + " , COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = " COALESCE(LD59RFLD02, N'1') as Supply, COALESCE(LD59RFLD04, N'0') as Stage, ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end AS Availability ";
            SQL = " FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F4211") + " ON LIITM = SDITM AND LIMCU = SDMCU AND LIPQOH > 0 AND ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end > 0 ";
            if (pickManagerMode.Trim() != "0")
            {
                if (PickManagerTagTableExists)
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPMT");
                    SQL = " ON SDKCOO = PTKCOO AND SDDOCO = PTDOCO AND SDDCTO = PTDCTO AND SDLNID = PTLNID ";
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON PTUKID = PMUKID ";
                }
                else
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON SDURAB = PMUKID ";
                }
            }

            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = " WHERE SDMCU = '" + branch + "' AND SDSHPN = " + shipment + " ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";

            SetAllowedSOStatusAndHoldCodes(allowedStatus, allowedHoldCodes);

            using (F4211 temp = new F4211(DataAccess, "F4211PickManager_"))
            {
                SQL = temp.GetPickManagerFilter(userId, pickManagerMode);
            }

            //LD59RFLD02 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL) ");
            //LD59RFLD02 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            locationTable.Dispose();
            return FetchEfficient();
        }

        protected virtual void SetAllowedSOStatusAndHoldCodes(string[] allowedStatus, string[] allowedHoldCodes)
        {
            if (allowedStatus != null && allowedStatus.Length != 0 && allowedStatus[0] != "" && allowedStatus[0] != "*")
            {
                SQL = String.Format("AND SDNXTR IN ('{0}') ", String.Join("','", allowedStatus));
            }

            SetAllowedHoldCodes(allowedHoldCodes);
        }

        #endregion

        #region Advanced Material Request Fulfillment

        public virtual bool GetSupplyForFulfillment(string branch, string[] allowedHoldCodes, bool allowExpiredLots)
        {
            SetBaseQueryForFulfillment(branch,
                allowedHoldCodes,
                allowExpiredLots);
            return FetchEfficient();
        }

        public virtual bool GetSupplyForFulfillmentByBatch(string branch, string[] allowedHoldCodes, bool allowExpiredLots, string batch)
        {
            SetBaseQueryForFulfillment(branch,
                allowedHoldCodes,
                allowExpiredLots);
            SQL = " AND MR59RFBT = '" + batch + "' ";
            return FetchEfficient();
        }

        public virtual bool GetSupplyForFulfillmentByLocation(string branch, string[] allowedHoldCodes, bool allowExpiredLots,
            string fromLocationRange, string toLocationRange)
        {
            SetBaseQueryForFulfillment(branch,
                allowedHoldCodes,
                allowExpiredLots);
            SQL = " AND MRTLOC BETWEEN '" + fromLocationRange + "' AND '" + toLocationRange + "' ";
            return FetchEfficient();
        }

        public virtual void SetBaseQueryForFulfillment(string branch, string[] allowedHoldCodes, bool allowExpiredLots)
        {
            ResetAll();

            string availabilityExpression = AvailabilityExpression(branch);
            F4100 tempLocation = new F4100(_DataAccess, "");
            SQL = "SELECT DISTINCT LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, LMWSQQ, COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = availabilityExpression;
            SQL = " AS Availability,  " + tempLocation.ColumnsForSelect;
            SQL = "FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F59RFMRA") + " ON LIITM = MRITM AND LIMCU = MRMCU AND LIPQOH > 0 AND MRTLOC <> LILOCN AND ";
            SQL = availabilityExpression + " > 0 ";
            SQL = "AND MR59RF019 IN (10, 20) ";
            SQL = " INNER JOIN " + GetFullTableName("F4102") + " ON LIITM = IBITM AND LIMCU = IBMCU ";
            SQL = "INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = "LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = "WHERE LIMCU = '" + _DataManipulatorTable.ManipulateDataForDisplay("F41021", "LIMCU", branch) + "' ";

            if (!allowExpiredLots)
            {
                SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";
            }

            SetAllowedHoldCodes(allowedHoldCodes);

            //Exclude All InterimLocation and Include Supply Locations
            //LD59RFLD01 AMF Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD01 = 1 OR LD59RFLD01 IS NULL) ");
            //LD59RFLD09 EndCap 1 = yes 0 = no
            Where(" AND (LD59RFLD09 = 0 OR LD59RFLD09 IS NULL)  ");
            //LD59RFLD10  ForkLift 1 = yes 0 = no
            Where(" AND (LD59RFLD10 = 0 OR LD59RFLD10 IS NULL)  ");
        }

        #endregion

        #region Obsolete methods no longer used by base

        // Obsolete methods no longer used by base

        #region CalculateAvailable (replaced by "Availability" column in data set

        /// <summary>
        /// Returns the available quantity using the Enterprise 1 logic in determining this value.
        /// </summary>
        /// <param name="QuantityOnHand">Quantity On Hand</param>
        /// <param name="OmitHeldLots">Omit Held Lots</param>
        /// <returns>Available Quantity</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by Availability column in DataTable.")]
        public virtual double CalculateAvailable(double QuantityOnHand, string OmitHeldLots)
        {
            Log.Debug("Entering CalculateAvailable");

            _WarehouseConstants.CIMCU = this.LIMCU;
            _WarehouseConstants.Fetch();

            if (_WarehouseConstants.CIDA01 == "-") // soft commits
                QuantityOnHand -= this.LIPCOM;
            if (_WarehouseConstants.CIDA02 == "-") // hard commits
                QuantityOnHand -= this.LIHCOM;
            if (_WarehouseConstants.CIDA03 == "-") // future commits
                QuantityOnHand -= this.LIFCOM;
            if (_WarehouseConstants.CIDA04 == "-") // hard commit to work order
                QuantityOnHand -= this.LIQOWO;
            if (_WarehouseConstants.CIDA05 == "-") // other quantity 1
                QuantityOnHand -= this.LIOT1P;
            if (_WarehouseConstants.CIDA06 == "-") // other quantity 2
                QuantityOnHand -= this.LIOT2P;
            if (_WarehouseConstants.CIDA07 == "-") // quantity on hold
            {
                if (this.LILOTS.Trim().Length > 0)
                {
                    QuantityOnHand -= Convert.ToDouble(this.LIPQOH);
                }
            }

            if (_WarehouseConstants.CIDA08 == "+") // on receipt
                QuantityOnHand += this.LIPREQ;
            if (_WarehouseConstants.CIDA09 == "+") // on work order receipt
                QuantityOnHand += this.LIQWBO;
            if (_WarehouseConstants.CIDA10 == "+") // on P/O other 1
                QuantityOnHand += this.LIOT1A;
            if (_WarehouseConstants.CIDA12 == "+") // inbound
                QuantityOnHand += this.LIQTIN;
            if (_WarehouseConstants.CIDA13 == "+") // in transit
                QuantityOnHand += this.LIQTTR;
            if (_WarehouseConstants.CIDA14 == "+") // qty in operation 1
                QuantityOnHand += this.LIQTO1;
            if (_WarehouseConstants.CIDA15 == "+") // qty in operation 2
                QuantityOnHand += this.LIQTO2;
            if (_WarehouseConstants.CIDA16 == "+") // qty on loan to Mfg.
                QuantityOnHand += this.LIQONL;

            Log.Debug("Leaving CalculateAvailable with RetValue: ");
            //_WarehouseConstants.Dispose();

            return (QuantityOnHand);
        }

        #endregion

        #region ShowItemLocationRecords (replaced by RetrieveItemLocationRecords)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101 including zero On hand
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItemNumber">Short Item Number</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveItemLocationRecords")]
        public virtual bool ShowItemLocationRecords(string sBranch, double dShortItemNumber)
        {
            Log.Debug("Entering ShowItemLocationRecords");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL  FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) ";
            this.SQL = sSQL;

            LIMCU = sBranch;
            LIITM = dShortItemNumber;

            _Conditionals.Append(" ORDER BY LILOTN,LILOCN");

            Log.Debug("Leaving ShowItemLocationRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemLocationRecordsWithOnHand (replaced by RetrieveItemLocationRecords)

        /// <summary>
        /// Displays Enterprise1 Item balance records from F41021/F4101
        /// for a particular sBranch/Plant and Item Number, with On Hand > 0
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItemNumber">Short Item Number</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveItemLocationRecords")]
        public virtual bool ShowItemLocationRecordsWithOnHand(string sBranch, double dShortItemNumber)
        {
            Log.Debug("Entering ShowItemLocationRecordsWithOnHand");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ";
            sSQL = sSQL + "LIMCU = '" + sBranch + "' AND LIITM = " + dShortItemNumber.ToString(CultureInfo.InvariantCulture);
            sSQL = sSQL + " AND LIPQOH > 0";
            sSQL = sSQL + " ORDER BY LILOTN,LILOCN";
            this.SQL = sSQL;

            Log.Debug("Leaving ShowItemLocationRecordsWithOnHand with RetValue: ");

            return (base.Fetch());
        }

        #endregion

        #region ShowBranchItemRecords (replaced by RetrieveBranchItemRecords)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101 including zero On hand
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveBranchItemRecords")]
        public virtual bool ShowBranchItemRecords(string sBranch)
        {
            Log.Debug("Entering ShowBranchItemRecords");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) ";
            this.SQL = sSQL;

            LIMCU = sBranch;

            _Conditionals.Append(" ORDER BY F4.IMDSC1, LILOTN, LILOCN");

            Log.Debug("Leaving ShowBranchItemRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101
        /// with and on hand quantity greater than zero.
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveBranchItemRecords.")]
        public virtual bool ShowBranchItemRecordsWithOnHand(string sBranch)
        {
            Log.Debug("Entering ShowBranchItemRecordsWithOnHand");

            this.ResetAll();

            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ";
            this.SQL = sSQL;

            LIMCU = sBranch;

            Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");

            _Conditionals.Append(" ORDER BY F4.IMDSC1, LILOTN, LILOCN");

            Log.Debug("Leaving ShowBranchItemRecordsWithOnHand with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemLotRecords (replaced by RetrieveItemLotRecords)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101
        /// for a given sBranch/Plant, Item Number, and Lot number, including zero On hand
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItemNumber">Short Item Number</param>
        /// <param name="sLotNumber">Lot Number</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart. Replaced by RetrieveItemLotRecords")]
        public virtual bool ShowItemLotRecords(string sBranch, double dShortItemNumber, string sLotNumber)
        {
            Log.Debug("Entering ShowItemLotRecords");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL, LIPQOH FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) ";
            this.SQL = sSQL;

            LIMCU = sBranch;
            LIITM = dShortItemNumber;
            LILOTN = sLotNumber;

            _Conditionals.Append(" ORDER BY LILOTN,LILOCN");

            Log.Debug("Leaving ShowItemLotRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemLotRecordsWithOnHand (replaced by RetrieveItemLotRecords)

        /// <summary>
        /// Displays JDE Item balance records from F41021/F4101 with On Hand > 0
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItemNumber">Short Item Number</param>
        /// <param name="sLotNumber">Lot Number</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveItemLotRecords")]
        public virtual bool ShowItemLotRecordsWithOnHand(string sBranch, double dShortItemNumber, string sLotNumber)
        {
            Log.Debug("Entering ShowItemLotRecordsWithOnHand");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL, LIPQOH FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ";
            sSQL = sSQL + "LIMCU = '" + sBranch + "' AND LIITM = " + dShortItemNumber.ToString(CultureInfo.InvariantCulture);
            sSQL = sSQL + " AND LIPQOH > 0";
            sSQL = sSQL + " AND LILOTN = '" + sLotNumber + "' ";
            sSQL = sSQL + " ORDER BY LILOTN,LILOCN";
            this.SQL = sSQL;

            Log.Debug("Leaving ShowItemLotRecordsWithOnHand with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemSerialRecords (replaced by RetrieveItemSerialRecords)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101
        /// for a given sBranch/Plant, Item Number, and Serial number, including zero On hand
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItemNumber">Short Item Number</param>
        /// <param name="sLocation">Location</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart. Replaced by RetrieveItemSerialRecords)")]
        public virtual bool ShowItemSerialRecords(string sBranch, double dShortItemNumber, string sLocation)
        {
            Log.Debug("Entering ShowItemSerialRecords");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) ";
            this.SQL = sSQL;

            LIMCU = sBranch;
            LIITM = dShortItemNumber;
            LILOCN = sLocation;

            _Conditionals.Append(" ORDER BY LILOTN,LILOCN");

            Log.Debug("Leaving ShowItemSerialRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemSerialRecordsWithOnHand (replaced by RetrieveItemSerialRecords)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101
        /// for a given sBranch/Plant, Item Number and sLocation, with On Hand > 0
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItemNumber">Short Item Number</param>
        /// <param name="sLocation">Location</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveItemSerialRecords")]
        public virtual bool ShowItemSerialRecordsWithOnHand(string sBranch, double dShortItemNumber, string sLocation)
        {
            Log.Debug("Entering ShowItemSerialRecordsWithOnHand");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ";
            sSQL = sSQL + "LIMCU = '" + sBranch + "' AND LIITM = " + dShortItemNumber.ToString(CultureInfo.InvariantCulture);
            sSQL = sSQL + " AND LIPQOH > 0";
            sSQL = sSQL + " AND LILOCN = '" + sLocation + "' ";
            sSQL = sSQL + " ORDER BY LILOTN,LILOCN";
            this.SQL = sSQL;

            Log.Debug("Leaving ShowItemSerialRecordsWithOnHand with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemsInLocation (replaced by RetrieveItemsInLocation)

        /// <summary>
        /// Displays Enterprise 1 Item Balance records from F41021/F4101
        /// for a given Branch and sLocation
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="sLocation">Location</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveItemsInLocation")]
        public virtual bool ShowItemsInLocation(string sBranch, string sLocation)
        {
            Log.Debug("Entering ShowItemsInLocation");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, LIPCOM, LIHCOM, ");
            sb.Append("LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ");
            sb.Append("LIQTTR, LIQTO1, LIQTO2, LIQONL, IMDSC1 FROM ");
            sb.Append(owner + ".F41021 F41 inner join ");
            sb.Append(owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ");
            sb.Append("LIMCU = '" + sBranch + "' AND LILOCN = '" + sLocation + "'");
            sb.Append(" ORDER BY LILOCN");
            this.SQL = sb.ToString();

            Log.Debug("Leaving ShowItemsInLocation with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowItemsInLocationWithOnHand (replaced by RetrieveItemsInLocation)

        /// <summary>
        /// Displays Enterprise 1 Item Balance records from F41021/F4101
        /// for a given Branch and sLocation
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="sLocation">Location</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveItemsInLocation")]
        public virtual bool ShowItemsInLocationWithOnHand(string sBranch, string sLocation)
        {
            Log.Debug("Entering ShowItemsInLocationWithOnHand");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, LIPCOM, LIHCOM, ");
            sb.Append("LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ");
            sb.Append("LIQTTR, LIQTO1, LIQTO2, LIQONL, IMDSC1 FROM ");
            sb.Append(owner + ".F41021 F41 inner join ");
            sb.Append(owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ");
            sb.Append("LIMCU = '" + sBranch + "' AND LILOCN = '" + sLocation + "'");
            sb.Append(" and LIPQOH > 0 ");
            sb.Append(" ORDER BY LILOCN");
            this.SQL = sb.ToString();

            Log.Debug("Leaving ShowItemsInLocationWithOnHand with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowLotSerialNumberRecords (replaced by RetrieveLotSerialNumberRecords)

        /// <summary>
        /// Displays Enterprise 1 Item Balance records from F41021/F4101
        /// for a given sBranch/Plant, Item Number, and sLot/Serial Number, including zero on hand
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItem">Item Number</param>
        /// <param name="sLOTN">Log Number</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveLotSerialNumberRecords")]
        public virtual bool ShowLotSerialNumberRecords(string sBranch, double dShortItem, string sLOTN)
        {
            Log.Debug("Entering ShowLotSerialNumberRecords");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ";
            sSQL = sSQL + "LIMCU = '" + sBranch + "' AND LIITM = " + dShortItem.ToString(CultureInfo.InvariantCulture);
            sSQL = sSQL + " AND LILOTN = '" + sLOTN + "'";
            sSQL = sSQL + " ORDER BY LILOCN";
            this.SQL = sSQL;

            Log.Debug("Leaving ShowLotSerialNumberRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowLotSerialNumberRecordsWithOnHand (replaced by RetrieveLotSerialNumberRecords)

        /// <summary>
        /// Displays Enterprise 1 Item Balance records from F41021/F4101
        /// for a given sBranch/Plant, Item Number, and sLot/Serial Number, where the on hand quantity > 0
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <param name="dShortItem">Item Number</param>
        /// <param name="sLOTN">Lot Number</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveLotSerialNumberRecords")]
        public virtual bool ShowLotSerialNumberRecordsWithOnHand(string sBranch, double dShortItem, string sLOTN)
        {
            Log.Debug("Entering ShowLotSerialNumberRecordsWithOnHand");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) WHERE ";
            sSQL = sSQL + "LIMCU = '" + sBranch + "' AND LIITM = " + dShortItem.ToString(CultureInfo.InvariantCulture);
            sSQL = sSQL + " AND LILOTN = '" + sLOTN + "' AND LIPQOH > 0 ";
            sSQL = sSQL + " ORDER BY LILOCN";
            this.SQL = sSQL;

            Log.Debug("Leaving ShowLotSerialNumberRecordsWithOnHand with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowContainersInBranch (replaced by RetrieveContainersInBranch)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101
        /// for a given sBranch/Plant including zero balace
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveContainersInBranch")]
        public virtual bool ShowContainersInBranch(string sBranch)
        {
            Log.Debug("Entering ShowContainersInBranch");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) ";
            this.SQL = sSQL;

            if (sBranch.Trim().Length > 0)
            {
                LIMCU = sBranch;
            } // end if

            Where("AND", Columns.LILOTN, Condition.NotEqual, " ", "");

            _Conditionals.Append(" ORDER BY LILOTN,LILOCN");

            Log.Debug("Leaving ShowContainersInBranch with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #region ShowContainersInBranchWithOnHand (replaced by RetrieveContainersInBranch)

        /// <summary>
        /// Displays Enterprise 1 Item balance records from F41021/F4101
        /// for a given sBranch/Plant excluding zero balace
        /// </summary>
        /// <param name="sBranch">Branch</param>
        /// <returns>True if records found.</returns>
        [Obsolete("No longer used by Base RFSmart.  Replaced by RetrieveContainersInBranch")]
        public virtual bool ShowContainersInBranchWithOnHand(string sBranch)
        {
            Log.Debug("Entering ShowContainersInBranchWithOnHand");

            this.ResetAll();
            string owner = this._DataAccess.Data.TableOwner;
            string sSQL = "SELECT LIMCU, LIITM, LILOCN, LIPQOH, LILOTN, F4.IMLITM, F4.IMUOM1, F4.IMDSC1, LIPCOM, LIHCOM, ";
            sSQL = sSQL + "LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, ";
            sSQL = sSQL + "LIQTTR, LIQTO1, LIQTO2, LIQONL FROM ";
            sSQL = sSQL + owner + ".F41021 F41 inner join ";
            sSQL = sSQL + owner + ".F4101 F4 ON (F41.LIITM = F4.IMITM) ";
            this.SQL = sSQL;

            if (sBranch.Trim().Length > 0)
            {
                LIMCU = sBranch;
            } // end if

            Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
            Where("AND", Columns.LILOTN, Condition.NotEqual, " ", "");

            _Conditionals.Append(" ORDER BY LILOTN,LILOCN");

            Log.Debug("Leaving ShowContainersInBranchWithOnHand with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        }

        #endregion

        #endregion

        #endregion

        public virtual bool RetrieveLooseItemsInLocation(string branch, string location, double shortItem = -1, string lot = null, string[] allowedHoldCodes = null)
        {
            return FetchLooseInventoryRecords(
                branch,
                shortItem,
                location,
                lot, // no lot for search
                "N", // Only show rows with LOOSE Quantity on hand
                " IMLITM, IOMMEJ, LILOTN", //Order By Item, Expriation, Lot
                allowedHoldCodes);
        }

        /// <summary>
        /// Fetches all records that contain the specified Branch, Short Item Number, Location,
        /// Lot number, and allowed Lot Hold Code where the Short Item Number is also in the
        /// F4101 (Item Master).  The fields included in the data set are the following:  LIMCU,
        /// LIITM, LILOCN, LIPQOH, LILOTN, IMLITM, IMAITM, IMUOM1, IMDSC1, IMDSC2, LIPCOM, LIHCOM,
        /// LIFCOM, LIQOWO, LIOT1P, LIOT2P, LILOTS, LIPREQ, LIQWBO, LIOT1A, LIQTIN, LIQTTR, LIQTO1,
        /// LIQTO2, and LIQONL.
        /// </summary>
        /// <param name="sBranch">Branch Plant (LIMCU).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="dShortItemNumber">Short Item Number (LIITM).  Default = -1, which excludes
        /// the criteria from the search.</param>
        /// <param name="sLocation">Location (LILOCN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sLot">Lot Number (LILOTN).  Default = null, which excludes the
        /// criteria from the search.</param>
        /// <param name="sShowZeroOnHand">Y/N option to determine if a check to ensure that the
        /// Quantity On Hand (LIPQOH) is greater than zero.  The default is "N".</param>
        /// <param name="sOrderByCols">Colums by which the fetched results should be sorted,
        /// delimited by commas.  For example, "LILOTN, LILOCN".  DO NOT INCLUDE THE ORDER BY
        /// CLAUSE WITHIN THE STRING JUST THE COLUMNS.  Default = "", which excludes the ORDER BY
        /// clause from the search.</param>
        /// <param name="sAllowedHoldCodes">Array of allowed Lot Hold Codes (LILOTS).</param>
        /// <returns>Returns true if records are fetched based on the Branch, Short Item Number,
        /// Location, Lot number, and allowed Lot Hold Codes specified; otherwise, false.</returns>
        public virtual bool FetchLooseInventoryRecords(string sBranch, double dShortItemNumber, string sLocation,
            string sLot, string sShowZeroOnHand, string sOrderByCols,
            string[] sAllowedHoldCodes)
        {
            Log.Debug("Entering FetchLooseInventoryRecords");
            string availabilityexpression = AvailabilityExpression(sBranch);
            ResetAll();

            #region Fields to select

            SQL = string.Format("SELECT {0}, ", ColumnsForSelect);
            SQL = "IMLITM, IMAITM, IMUOM1, IMDSC1, IMDSC2, IMITM, IOMMEJ, IOLOTS,";
            SQL = " LIPQOH - COALESCE(IDPQOR, 0) AS OnHand, COALESCE(IDPQOR, 0) AS IDPQOR, ";
            SQL = " case when iommej = 0 then Available ";
            SQL = string.Format(" when iommej < {0} then 0", _TodaysDateInJulian);
            SQL = " else Available ";
            SQL = " end as Availability ";

            #endregion

            SQL = string.Format(" FROM {0} Inner Join ", GetFullTableName("F4101"));

            #region SubSelect F41021 Join.

            SQL = string.Format("(SELECT {0}, IOLOTS, ", ColumnsForSelect);
            SQL = "  case ";
            SQL = string.Format(" when lilots = ' ' then {0} ", availabilityexpression);
            SQL = string.Format(" when lilots = '' then {0}  ", availabilityexpression);
            SQL = string.Format(" when lilots is null then {0} ", availabilityexpression);
            SQL = string.Format(" else  {0} - LIPQOH", availabilityexpression);
            SQL = " end as Available, ";
            SQL = " CASE WHEN IOMMEJ IS NOT NULL THEN IOMMEJ WHEN IOMMEJ = 0 THEN 999999 ELSE 999999 END AS IOMMEJ ";
            SQL = string.Format(" FROM {0} F41021_1 LEFT OUTER JOIN {1} ", GetFullTableName("F41021"), GetFullTableName("F4108"));
            SQL = " ON LIITM = IOITM AND LIMCU = IOMCU AND ";
            SQL = " LILOTN = IOLOTN) F41021 ON IMITM = LIITM ";

            #endregion

            SQL = string.Format(
                " LEFT JOIN (SELECT IDMCU, IDITM, IDLOTN, IDLOCN, SUM(IDPQOR) AS IDPQOR FROM {0} ",
                this.GetFullTableName("F46L11"));
            SQL = string.Format(" WHERE IDMCU = '{0}' AND IDLOCN = '{1}' ",
                _DataManipulatorTable.ManipulateDataForDisplay("F41021", "LIMCU", sBranch),
                sLocation);
            if (dShortItemNumber != -1)
            {
                SQL = string.Format(" AND IDITM = {0} ", dShortItemNumber);
            }

            SQL = " GROUP BY IDMCU, IDITM, IDLOTN, IDLOCN) LPDetailQuery ";

            SQL = " ON ((LIMCU = IDMCU) AND (LIITM = IDITM) AND (LILOTN = IDLOTN) AND (LILOCN = IDLOCN)) ";

            // *******

            #region Setting each key (LIMCU, LIITM, LILOCN, & LILOTN) if it is not null

            LIMCU = sBranch;

            if (dShortItemNumber != -1)
            {
                LIITM = dShortItemNumber;
            } // end if

            LILOCN = sLocation;

            if (sLot != null && sLot.Trim().Length > 0)
            {
                LILOTN = sLot;
            } // end if

            #endregion

            if ((sAllowedHoldCodes != null) && (sAllowedHoldCodes.Length > 0))
            {
                Where("AND", Columns.LILOTS, GenerateAllowedHoldCodes(sAllowedHoldCodes), "");
            } // end if

            // Checking to see if the Quantity On Hand should be checked.
            if (String.Compare(sShowZeroOnHand, "Y", true, CultureInfo.InvariantCulture) != 0)
            {
                Where("AND", Columns.LIPQOH, Condition.GreaterThan, 0, "");
                _Conditionals.Append(" AND LIPQOH - COALESCE(IDPQOR, 0) > 0 ");
            } // end if

            if ((sOrderByCols != null) && (sOrderByCols.Trim().Length > 0))
            {
                _Conditionals.Append("ORDER BY " + sOrderByCols);
            } // end if

            Log.Debug("Leaving FetchInventoryRecords with RetValue: ");

            return (Fetch()); // New fetch specific to the F41021 Table Object Class
        } // end FetchInventoryRecords

        public virtual bool GetLPSupplyForSOPickingByOrder(string branch, double salesOrder, string orderType,
            string[] allowedStatus, string[] allowedHoldCodes, string userId, string pickManagerMode, string line)
        {
            ResetAll();

            F4100 locationTable = new F4100(DataAccess, "F41021Location");
            string availabilityExpression = AvailabilityExpression(branch);

            SQL = "SELECT DISTINCT IDPQOR, IDLPNU, LIMCU, LILOCN, LILOTN, LIITM, LIPQOH, " + locationTable.ColumnsForSelect + " , COALESCE(IOMMEJ,999999) as IOMMEJ, ";
            SQL = " COALESCE(LD59RFLD02, N'1') as Supply, COALESCE(LD59RFLD04, N'0') as Stage, ";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end AS Availability ";
            SQL = " FROM " + GetFullTableName();
            SQL = " INNER JOIN " + GetFullTableName("F46L11") + " ON IDLOTN = LILOTN AND IDITM = LIITM AND IDMCU = LIMCU AND IDPQOR > 0 AND IDLOCN = LILOCN ";
            SQL = " INNER JOIN " + GetFullTableName("F4211") + " ON LIITM = SDITM AND LIMCU = SDMCU AND LIPQOH > 0 AND (IDLOTN = SDLOTN OR SDLOTN = N'') AND";
            SQL = " case	when (lilots = ' ' or lilots = '' or lilots is null) then " + availabilityExpression;
            SQL = "	else  " + availabilityExpression + " - LIPQOH ";
            SQL = "  end > 0 ";
            if (pickManagerMode.Trim() != "0")
            {
                if (PickManagerTagTableExists)
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPMT");
                    SQL = " ON SDKCOO = PTKCOO AND SDDOCO = PTDOCO AND SDDCTO = PTDCTO AND SDLNID = PTLNID ";
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON PTUKID = PMUKID ";
                }
                else
                {
                    SQL = " LEFT JOIN " + GetFullTableName("F59RFPM");
                    SQL = " ON SDURAB = PMUKID ";
                }
            }

            SQL = " INNER JOIN " + GetFullTableName("F4100") + " ON LMLOCN = LILOCN AND LMMCU = LIMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F59RFLD") + " ON LMLOCN = LDLOCN AND LMMCU = LDMCU ";
            SQL = " LEFT OUTER JOIN " + GetFullTableName("F4108") + " ON (LILOTN = IOLOTN AND LIITM = IOITM AND IOMCU = LIMCU) ";
            SQL = " WHERE SDMCU = '" + branch + "' AND SDDOCO = " + salesOrder + " AND SDDCTO = '" + orderType + "' ";
            SQL = " AND COALESCE(IOMMEJ, 999999) > " + this._TodaysDateInJulian + " ";

            SetAllowedSOStatusAndHoldCodes(allowedStatus, allowedHoldCodes);

            using (F4211 temp = new F4211(DataAccess, "F4211PickManager_"))
            {
                SQL = temp.GetPickManagerFilter(userId, pickManagerMode);
            }

            if (!string.IsNullOrWhiteSpace(line))
                Where(string.Format(" AND SDLNID = '{0}' ", line));
            //LD59RFLD02 Pick Supply 1 = yes 0 = no
            Where(" AND (LD59RFLD02 = 1 OR LD59RFLD02 IS NULL) ");
            //LD59RFLD02 Pick Stage 1 = yes 0 = no
            Where(" AND (LD59RFLD04 = 0 OR LD59RFLD04 IS NULL)  ");
            locationTable.Dispose();
            return FetchEfficient();
        }
    } // end F41021 Class

    #endregion

    #region F41021_Collection

    /// <summary>
    /// Provides Collection for F41021
    /// </summary>
    public class F41021_Collection : DataObjectBase_Collection
    {
        #region Properties

        #region Current

        /// <summary>
        /// Gets Current Row in DataSet.
        /// Used to Enumerate Through Collection of F41021.
        /// </summary>
        /// <value>The F41021 Object Used by IEnumerator</value>
        public override Object Current
        {
            get
            {
                F41021 temp = new F41021();

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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Collection Access for Table: F41021
        /// </summary>
        /// <param name="clsBase">Base Class F41021_Base Object Used in Collection</param>
        public F41021_Collection(F41021_Base clsBase)
            : base(clsBase)
        {
        } // end F41021_Collection constructor

        #endregion
    } // end F41021_Collection Class

    #endregion
} // end ICS.Database.TableObjects.JDE Namespace