using System;
using System.Collections.Generic;
using System.Linq;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.RFS.Database.TableObjects
{
    /// <summary>
    /// Class for the RFS_PrinterDefinition Database Table
    /// </summary>
    public class RFS_PrinterDefinition : ICS.Database.TableObjects.RFS.RFS_PrinterDefinition
    {
        #region Constructors

        /// <summary>
        /// Initializes a new RFS_PrinterDefinition
        /// </summary>
        public RFS_PrinterDefinition() : base()
        {
        }

        /// <summary>
        /// Initializes a new RFS_PrinterDefinition for the provided Data Access Layer and Table ID
        /// </summary>
        /// <param name="dbConnections">Database Acces to the Data and System Tables</param>
        /// <param name="tableID">ID appended to the front of the table name to allow for multiple instances of the same table to exist in the UserDS</param>
        public RFS_PrinterDefinition(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        #region RetrievePrinters

        /// <summary>
        /// Fetches all the valid printers based on the provided data, includng the RFS_PrinterDefinition, RFS_PrintFormat, and
        /// if applicable, RFS_PrintGroup, to ensure accurate records are returned.
        /// </summary>
        /// <param name="environmentID">Environment ID</param>
        /// <param name="printerAddresses">Collection of Printer Addresses (excludes any null or blank items)</param>
        /// <param name="printGroup">Print Group (excluded if null or blank)</param>
        /// <param name="formNames">Collection of Form Names (excludes any null or blank items)</param>
        /// <param name="distinctPrinters">If True, only retrieves distinct printer names</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool RetrievePrinters(string environmentID, IEnumerable<string> printerAddresses, string printGroup, IEnumerable<string> formNames, bool distinctPrinters)
        {
            SQL = String.Format("SELECT {0} ", (distinctPrinters ? "DISTINCT(PrinterName)" : "*"));

            SQL = String.Format("FROM {0} PrintDef INNER JOIN {1} Label ON ", GetFullTableName(), GetFullTableName("RFS_PrintFormat"));
            SQL = "(PrintDef.EnvironmentID = Label.EnvironmentID) AND (PrintDef.PrinterModel = Label.PrinterModel) ";

            //If the PrintGroup value isn't null, we also join with the RFS_PrintGroup table help ensure accurate results are returned, so
            //even if the user provides invalid printer addresses, we can make sure to filter down the list properly
            if (!String.IsNullOrWhiteSpace(printGroup))
            {
                SQL = String.Format("INNER JOIN {0} Groups ON ", GetFullTableName("RFS_PrintGroup"));
                SQL = "(Groups.EnvironmentID = PrintDef.EnvironmentID) AND (Groups.Address = PrintDef.Address) ";
            }

            SQL = String.Format("WHERE (PrintDef.EnvironmentID = '{0}') ", environmentID);

            //If FormNames isn't null, and there is at least one element in the collection that isn't null or blank
            if ((formNames != null) && (formNames.Where(x => (!String.IsNullOrWhiteSpace(x))).Any()))
            {
                SQL = String.Format("AND (Label.FormName IN ('{0}')) ", String.Join("','", formNames.Where(x => (!String.IsNullOrWhiteSpace(x)))));
            }

            //If PrinterAddresses isn't null, and there is at least one element in the collection that isn't null or blank
            if ((printerAddresses != null) && (printerAddresses.Where(x => (!String.IsNullOrWhiteSpace(x))).Any()))
            {
                SQL = String.Format("AND (PrintDef.Address IN ('{0}')) ", String.Join("','", printerAddresses.Where(x => (!String.IsNullOrWhiteSpace(x)))));
            }

            if (!String.IsNullOrWhiteSpace(printGroup))
            {
                SQL = String.Format("AND (Groups.PrintGroup = '{0}') ", printGroup);
            }

            return Fetch();
        }

        #endregion

        #region ValidatePrinterName

        /// <summary>
        /// Validates the Printer Name against the RFS_PrinterDefinition and RFS_PrintFormat tables, with the provided parameters to ensure. Also
        /// including the RFS_PrintGroup, should a non-null or blank group value be provided.
        /// </summary>
        /// <param name="environmentID">Environment ID</param>
        /// <param name="printerName">Printer Name</param>
        /// <param name="printGroup">Print Group (excluded if null or blank)</param>
        /// <param name="formNames">Collection of Form Names (excludes any null or blank items)</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool ValidatePrinterName(string environmentID, string printerName, string printGroup, IEnumerable<string> formNames)
        {
            ResetAll();

            SQL = String.Format("SELECT * FROM {0} PrintDef JOIN {1} Label ON ", GetFullTableName(), GetFullTableName("RFS_PrintFormat"));
            SQL = "(PrintDef.EnvironmentID = Label.EnvironmentID) AND (PrintDef.PrinterModel = Label.PrinterModel) ";

            //If the PrintGroup value isn't null, we also join with the RFS_PrintGroup table help ensure accurate results are returned, so
            //even if the user provides invalid printer name, we can make sure to filter down the list properly
            if (!String.IsNullOrWhiteSpace(printGroup))
            {
                SQL = String.Format("INNER JOIN {0} Groups ON ", GetFullTableName("RFS_PrintGroup"));
                SQL = "(Groups.EnvironmentID = PrintDef.EnvironmentID) AND (Groups.Address = PrintDef.Address) ";
            }

            SQL = String.Format("WHERE (PrintDef.EnvironmentID = '{0}') AND (PrintDef.PrinterName = '{1}') ", environmentID, printerName);

            //If FormNames isn't null, and there is at least one element in the collection that isn't null or blank
            if ((formNames != null) && (formNames.Where(x => (!String.IsNullOrWhiteSpace(x))).Any()))
            {
                SQL = String.Format("AND (Label.FormName IN ('{0}')) ", String.Join("','", formNames.Where(x => (!String.IsNullOrWhiteSpace(x)))));
            }

            if (!String.IsNullOrWhiteSpace(printGroup))
            {
                SQL = String.Format("AND (Groups.PrintGroup = '{0}') ", printGroup);
            }

            return Fetch();
        }

        #endregion

        #region ValidatePrintData

        /// <summary>
        /// Validates the a valid print record exists for the provided data, checking the RFS_PrinterDefinition, RFS_PrintFormat, and if 
        /// applicable, te RFS_PrintGroup tables
        /// </summary>
        /// <param name="environmentID">Environment ID</param>
        /// <param name="printerName">Printer Name (excluded if null or blank)</param>
        /// <param name="printerAddress">Printer Address (excluded if null or blank)</param>
        /// <param name="printGroup">Print Group (excluded if null or blank)</param>
        /// <param name="formName">Form Name</param>
        /// <returns>True if the Fetch was successful at retrieving any records, otherwise, False</returns>
        public bool ValidatePrintData(string environmentID, string printerName, string printerAddress, string printGroup, string formName)
        {
            ResetAll();

            SQL = String.Format("SELECT * FROM {0} PrintDef JOIN {1} Label ON ", GetFullTableName(), GetFullTableName("RFS_PrintFormat"));
            SQL = "(PrintDef.EnvironmentID = Label.EnvironmentID) AND (PrintDef.PrinterModel = Label.PrinterModel) ";

            //If the PrintGroup value isn't null, we also join with the RFS_PrintGroup table
            if (!String.IsNullOrWhiteSpace(printGroup))
            {
                SQL = String.Format("INNER JOIN {0} Groups ON ", GetFullTableName("RFS_PrintGroup"));
                SQL = "(Groups.EnvironmentID = PrintDef.EnvironmentID) AND (Groups.Address = PrintDef.Address) ";
            }

            SQL = String.Format("WHERE (PrintDef.EnvironmentID = '{0}') AND (Label.FormName = '{1}') ", environmentID, formName);

            if (!String.IsNullOrWhiteSpace(printerName)) //if a printer name was provided
            {
                SQL = String.Format("AND (PrintDef.PrinterName = '{0}') ", printerName);
            }

            if (!String.IsNullOrWhiteSpace(printerAddress)) //if a printer address was provided
            {
                SQL = String.Format("AND (PrintDef.Address = '{0}') ", printerAddress);
            }

            if (!String.IsNullOrWhiteSpace(printGroup)) //again if we are filtering with a PrintGroup as well
            {
                SQL = String.Format("AND (Groups.PrintGroup = '{0}') ", printGroup);
            }

            return Fetch();
        }

        #endregion
    }
}