using ICS.Database;
using System;
using System.Globalization;

namespace ICS.Stoneridge.Wireless.JDE.Database.TableObjects
{
    public class F3111 : ICS.Database.TableObjects.JDE.F3111
    {
        #region Constructors

        public F3111() : base()
        {
        }

        public F3111(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        /// <summary>
        /// Returns true for a particular open item on the work order parts list
        ///		Input - Work order Number, Document Type, Short Item Number
        /// </summary>
        /// <param name="dDOCO"></param>
        /// <param name="sDCTO"></param>
        /// <param name="dITM"></param>
        /// <returns></returns>
        public virtual bool RetrieveOpenPartsListRecord(double dDOCO, string sDCTO, double dITM, string[] _IssueTypesAllowed, string orderBy = null)
        {
            ResetAll();

            WMDOCO = dDOCO;
            WMDCTO = sDCTO;
            WMCPIT = dITM;

            if (_IssueTypesAllowed[0].ToString(CultureInfo.InvariantCulture).Trim().Length > 0)
            {
                Where("", Columns.WMITC, _IssueTypesAllowed, "");
            }

            _Conditionals.Append(" AND WMQNTA <> 999999999999999 AND (WMUORG > WMTRQT) ");
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                AppendToWhere(string.Format(" ORDER BY {0} ", orderBy));
            }

            return (Fetch());
        }

        #region RetrievePartsListRecord

        /// <summary>
        /// Searches the database for a record containing the specified Work Order, Document
        /// Type, and Short Item Number.
        /// </summary>
        /// <param name="dDOCO">Work Order Number</param>
        /// <param name="sDCTO">Document Type</param>
        /// <param name="dITM">Short Item Number</param>
        /// <returns>Returns true if a record is found matching the specified work order,
        /// document type, and short item number; otherwise, false is returned.</returns>
        public virtual bool RetrievePartsListRecord(double dDOCO, string sDCTO, double dITM)
        {
            ResetAll();

            WMDOCO = dDOCO;
            WMDCTO = sDCTO;
            WMCPIT = dITM;

            return (FetchAndSetRemainingExpression());
        } // end RetrievePartsListRecord

        /// <summary>
        /// Searches the database for a record containing the specified Work Order, Document
        /// Type, and Short Item Number.
        /// </summary>
        /// <param name="dDOCO">Work Order Number</param>
        /// <param name="sDCTO">Document Type</param>
        /// <param name="dITM">Short Item Number</param>
        /// <param name="AllowOverIssue">Allow Over Issues</param>
        /// <returns>Returns true if a record is found matching the specified work order,
        /// document type, and short item number; otherwise, false is returned.</returns>
        public virtual bool RetrievePartsListRecord(double dDOCO, string sDCTO, double dITM, bool AllowOverIssue)
        {
            ResetAll();

            WMDOCO = dDOCO;
            WMDCTO = sDCTO;
            WMCPIT = dITM;
            _Conditionals.Append(" and WMUORG <> 0  AND WMTRQT < WMUORG ");

            // for the first attemt at retrieval, try to get only the first open item
            if (FetchAndSetRemainingExpression())
            {
                return (true);
            } // end if
            else if (AllowOverIssue)
            {
                ResetAll();

                WMDOCO = dDOCO;
                WMDCTO = sDCTO;
                WMCPIT = dITM;
                _Conditionals.Append(" and WMUORG <> 0 ");

                return (Fetch());
            } // end else if
            else
            {
                return (false);
            } // end else
        } // end RetrievePartsListRecord

        /// <summary>
        /// Searches the database for a record containing the specified Work Order, Document
        /// Type, and Short Item Number.
        /// </summary>
        /// <param name="dDOCO">Work Order Number</param>
        /// <param name="sDCTO">Document Type</param>
        /// <param name="dITM">Short Item Number</param>
        /// <param name="AllowOverIssue">Allow Over Issues</param>
        /// <param name="_IssueTypesAllowed"></param>
        /// <param name="orderBy"></param>
        /// <returns>Returns true if a record is found matching the specified work order,
        /// document type, and short item number; otherwise, false is returned.</returns>
        public virtual bool RetrievePartsListRecord(double dDOCO, string sDCTO, double dITM, bool AllowOverIssue, string[] _IssueTypesAllowed, string orderBy = null)
        {
            ResetAll();

            if (_IssueTypesAllowed[0].ToString(CultureInfo.InvariantCulture).Trim().Length > 0)
            {
                Where("", Columns.WMITC, _IssueTypesAllowed, "");
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                AppendToWhere($" ORDER BY {orderBy} ");
            }

            WMDOCO = dDOCO;
            WMDCTO = sDCTO;
            WMCPIT = dITM;
            _Conditionals.Append(" and WMUORG <> 0  AND WMTRQT < WMUORG ");

            // for the first attemt at retrieval, try to get only the first open item
            if (FetchAndSetRemainingExpression())
            {
                return (true);
            } // end if
            else if (AllowOverIssue)
            {
                ResetAll();

                WMDOCO = dDOCO;
                WMDCTO = sDCTO;
                WMCPIT = dITM;
                _Conditionals.Append(" and WMUORG <> 0 ");

                return (Fetch());
            } // end else if
            else
            {
                return (false);
            } // end else
        } // end RetrievePartsListRecord

        public virtual bool RetrievePartsListRecord(string sDOCO, string sDCTO, string[] IssueTypesAllowed, string OrderBy, double dITM, string sLOT)
        {
            ResetAll();

            WMDOCO = Convert.ToDouble(sDOCO);
            WMDCTO = sDCTO;
            WMCPIT = dITM;
            WMLOTN = sLOT;

            Where("AND", Columns.WMITC, IssueTypesAllowed, "");
            _Conditionals.Append("AND WMQNTA <> 999999999999999 ");

            if (OrderBy.Trim().Length > 0)
            {
                AppendToWhere(" ORDER BY " + OrderBy);
            } // end if

            return (Fetch());
        }

        public virtual bool RetrievePartsListRecord(string sDOCO, string sDCTO, string[] IssueTypesAllowed, bool AllowOverIssue, string OrderBy, double dITM)
        {
            ResetAll();

            WMDOCO = Convert.ToDouble(sDOCO);
            WMDCTO = sDCTO;
            WMCPIT = dITM;
            Where("AND", Columns.WMITC, IssueTypesAllowed, "");
            _Conditionals.Append("AND WMQNTA <> 999999999999999 ");

            if (AllowOverIssue)
            {
                _Conditionals.Append("AND ((abs(WMUORG) + abs(WMTRQT)) <> 0) ");
            } // end if
            else
            {
                _Conditionals.Append("AND ((abs(WMUORG) + abs(WMTRQT)) <> 0) AND WMUORG <> WMTRQT ");
            } // end else

            if (OrderBy.Trim().Length > 0)
            {
                AppendToWhere(" ORDER BY " + OrderBy);
            } // end if

            return (FetchAndSetRemainingExpression());
        } // end ShowPartsList

        #endregion

        #region ShowPartsList
        /// <summary>
        /// Return parts list for a given work order number, work order type. An
        /// array of the issue types allowed is also passed to this method
        /// I     Manual issue (default)
        /// F     Floor stock (no issue)
        /// B     Backflush (when part is reported as complete
        /// P     Preflush (when parts list is generated)
        /// U     Backflush (at pay-point operation)
        /// S     Sub-contract item (send to supplier)
        /// Blank  Shippable end item
        /// </summary>
        /// <param name="sDOCO">Work Order used in query</param>
        /// <param name="sDCTO">Document Type used in query</param>
        /// <param name="IssueTypesAllowed">An array of Allowed Issue Types (WMITC)</param>
        /// <param name="AllowOverIssue">Specifies whether or not over issues are allowed</param>
        /// <returns>Returns true if a record is found matching the criteria; otherwise false</returns>
        public override bool ShowPartsList(string sDOCO, string sDCTO, string[] IssueTypesAllowed, bool AllowOverIssue)
        {
            //The original design simple used AllowOverIssue (if false) to exclude where (WMUORG <> WMTRQT)
            return ValidatePartsListItem(Convert.ToDouble(sDOCO), sDCTO, -1, IssueTypesAllowed, false, true, AllowOverIssue, null);
        }

        /// <summary>
        /// Allows for Order by statement.
        /// </summary>
        /// <param name="sDOCO">Work Order used in query</param>
        /// <param name="sDCTO">Document Type used in query</param>
        /// <param name="IssueTypesAllowed">An array of Allowed Issue Types (WMITC)</param>
        /// <param name="AllowOverIssue">Specifies whether or not over issues are allowed</param>
        /// <param name="OrderBy">Should be in the format Column1, Column2, .... DESC/ASC</param>
        /// <returns>Returns true if a record is found matching the criteria; otherwise false</returns>
        public override bool ShowPartsList(string sDOCO, string sDCTO, string[] IssueTypesAllowed, bool AllowOverIssue, string OrderBy)
        {
            //The original design simple used AllowOverIssue (if false) to exclude where (WMUORG <> WMTRQT)
            return ValidatePartsListItem(Convert.ToDouble(sDOCO), sDCTO, -1, IssueTypesAllowed, false, true, AllowOverIssue, OrderBy);
        }

        #endregion
    }
}