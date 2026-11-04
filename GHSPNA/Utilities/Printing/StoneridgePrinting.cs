using System;
using System.Linq;
using ICS.Database;
using ICS.Database.TableObjects.RFS;
using ICS.Utilities;
using ICS.Display;
using ICS.Stoneridge.Wireless.JDE.Utilities.Extensions;
using CustomRFSTables = ICS.Stoneridge.Wireless.RFS.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Utilities.Printing
{
    /// <summary>
    /// A custom Printing class, used to add additional logic to the base Printing engine
    /// </summary>
    public class StoneridgePrinting : IDisposable
    {
        #region Class Level Variables

        protected DAL _DataAccess;

        protected Logging _Log;

        protected CustomRFSTables.RFS_PrinterDefinition _PrinterDefinitionTable;

        protected CustomRFSTables.RFS_PrintXref _PrintXrefTable;

        protected PrintXrefPatterns _PrintXrefPatterns;

        private string _PrinterName, _PrinterAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new StoneridgePrinting
        /// </summary>
        /// <param name="userSession">User Session</param>
        public StoneridgePrinting(UserSession userSession) : this(userSession.DataAccess, userSession.EnvironmentId, userSession.ClientInfo.WorkflowId, userSession.ClientInfo.Version, userSession.UserId)
        {
        }

        /// <summary>
        /// Initializes a new StoneridgePrinting
        /// </summary>
        /// <param name="dataAccess">Data Access Layer</param>
        /// <param name="environmentID">Environment ID</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="version">Workflow Version</param>
        /// <param name="userID">User ID</param>
        public StoneridgePrinting(DAL dataAccess, string environmentID, string workflowID, string version, string userID)
        {
            dataAccess.TablesForWorkflow.Log.Debug("Initializing StoneridgePrinting");

            _DataAccess = dataAccess;
            _Log = new Logging(environmentID, LogType.System, _DataAccess.System.Log.ID);

            EnvironmentID = environmentID;
            WorkflowID = workflowID;
            Version = version;
            UserID = userID;

            _PrinterDefinitionTable = new CustomRFSTables.RFS_PrinterDefinition(_DataAccess, "");
            _PrintXrefTable = new CustomRFSTables.RFS_PrintXref(_DataAccess, "");
            _DataAccess.TablesToPersist.Add("RFS_PrinterDefinition"); //done so if initialized elsewhere, it won't wipe any results fetched here
            _DataAccess.TablesToPersist.Add("RFS_PrintXref");

            _Log.Debug("Finished Initializing StoneridgePrinting");

            IsDisposed = false;
        }

        #endregion

        #region Properties

        #region Print Xref Fields

        #region EnvironmentID

        /// <summary>
        /// Get or Set the Environment ID, used for filtering the Printer related tables.
        /// </summary>
        public string EnvironmentID { get; protected set; }

        #endregion

        #region WorkflowID

        /// <summary>
        /// Get the Workflow ID, used for filtering the RFS_PrintXref table.
        /// </summary>
        public string WorkflowID { get; protected set; }

        #endregion

        #region Version

        /// <summary>
        /// Get the Version, used for filtering the RFS_PrintXref table. 
        /// </summary>
        public string Version { get; protected set; }

        #endregion

        #region Branch

        /// <summary>
        /// Get or Set the Branch, used for filtering the RFS_PrintXref table
        /// </summary>
        public string Branch { get; set; }

        #endregion

        #region ContainerType

        /// <summary>
        /// Get or Set the ContainerType, used for filtering the RFS_PrintXref table
        /// </summary>
        public string ContainerType { get; set; }

        #endregion

        #region Customer

        /// <summary>
        /// Get or Set the Customer, used for filtering the RFS_PrintXref table
        /// </summary>
        public string Customer { get; set; }

        #endregion

        #region Item

        /// <summary>
        /// Get or Set the Item, used for filtering the RFS_PrintXref table
        /// </summary>
        public string Item { get; set; }

        #endregion

        #region Location

        /// <summary>
        /// Get or Set the Location, used for filtering the RFS_PrintXref table
        /// </summary>
        public string Location { get; set; }

        #endregion

        #region MixedContainer

        /// <summary>
        /// Get or Set the MixedContainer, used for filtering the RFS_PrintXref table
        /// </summary>
        public bool MixedContainer { get; set; }

        #endregion

        #endregion

        #region PrintXrefPatterns

        /// <summary>
        /// Get the Print Xref search patterns collection
        /// </summary>
        public virtual PrintXrefPatterns PrintXrefPatterns
        {
            get { return _PrintXrefPatterns ?? (_PrintXrefPatterns = new PrintXrefPatterns()); }
        }

        #endregion

        #region UserID

        /// <summary>
        /// Get the User ID, used for filtering the Printer related tables.
        /// </summary>
        public string UserID { get; protected set; }

        #endregion

        #region DefaultPrinter

        /// <summary>
        /// Attempts to get the Default Printer Name by using the already set Print Xref parameters and checking against
        /// all the Printer related tables
        /// </summary>
        public string DefaultPrinter
        {
            get
            {
                if (RetrieveDefaultPrinters())
                {
                    return _PrinterDefinitionTable.PrinterName;
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

        #region PrinterName

        /// <summary>
        /// Get or Set the Printer Name. If the value is blank, it will attempt to retrieve a valid Name, based on the current
        /// set Print Xref parameters.
        /// </summary>
        public string PrinterName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_PrinterName))
                {
                    if (RetrieveDefaultPrinters())
                    {
                        _PrinterName = _PrinterDefinitionTable.PrinterName;
                    }
                }

                return _PrinterName;
            }
            set { _PrinterName = value; }
        }

        #endregion

        #region PrinterAddress

        /// <summary>
        /// Get or Set the Printer Address. If the value is blank, it will attempt to retrieve a valid Address, based on the current
        /// set Print Xref parameters, and Printer Name. Should the Printer Name be blank as well, it will attempt to be retrieved as well
        /// </summary>
        public string PrinterAddress
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_PrinterAddress))
                {
                    //Checks if the PrinterName is blank (by doing this, we force the query to populate the Printer Name).
                    if (!String.IsNullOrWhiteSpace(PrinterName))
                    {
                        ValidatePrinter(PrinterName); //this will populate the Printer Address value for us
                    }
                }

                return _PrinterAddress;
            }
            set { _PrinterAddress = value; }
        }

        #endregion

        #region ErrorMessageVocabulary

        /// <summary>
        /// Get the Error Message that occured, if any
        /// </summary>
        public string ErrorMessageVocabulary { get; protected set; }

        #endregion

        #region IsDisposed

        /// <summary>
        /// Get if the StoneridgePrinting has been disposed of
        /// </summary>
        public bool IsDisposed { get; private set; }

        #endregion

        #endregion

        #region ResetAll

        /// <summary>
        /// Resets all the Tables and Properties set to a default state
        /// </summary>
        public void ResetAll()
        {
            ResetAll(false);
        }

        /// <summary>
        /// Resets all the Tables and Properties set to a default state
        /// </summary>
        /// <param name="searchPatterns">Set to True to also reset the Print Xref Search Patterns to default</param>
        public virtual void ResetAll(bool searchPatterns)
        {
            _PrinterDefinitionTable.ResetAll();
            _PrintXrefTable.ResetAll();

            PrinterName = "";
            PrinterAddress = "";

            Branch = "";
            ContainerType = "";
            Customer = "";
            Item = "";
            Location = "";
            MixedContainer = false;

            ErrorMessageVocabulary = "";

            if (searchPatterns)
            {
                PrintXrefPatterns.Reset();
            }
        }

        #endregion

        #region RetrievePrinters

        /// <summary>
        /// Attempts to retrieve valid Printers based on the set Print Xref parameters, checking against all the relative Printer tables
        /// </summary>
        /// <returns>True if valid print records were found, otherwise, False</returns>
        public bool RetrievePrinters()
        {
            return RetrieveDefaultPrinters(true);
        }

        #endregion

        #region RetrieveDefaultPrinters

        /// <summary>
        /// Attempts to retrieve valid Printers based on the set Print Xref parameters, checking against all the relative Printer tables, using
        /// the DefaultPA on the PrintXref records if possible to limit the valid printers
        /// </summary>
        /// <returns>True if valid print records were found, otherwise, False</returns>
        public bool RetrieveDefaultPrinters()
        {
            return RetrieveDefaultPrinters(false);
        }

        /// <summary>
        /// Attempts to retrieve valid Printers based on the set Print Xref parameters, checking against all the relative Printer tables, using
        /// the DefaultPA on the PrintXref records if possible to limit the valid printers
        /// </summary>
        /// <param name="excludeDefaultPA">If false, the query that uses the "DefaultPA" from the PrintXref used in relation with the PrinterDefinition will be excluded</param>
        /// <returns>True if valid print records were found, otherwise, False</returns>
        protected virtual bool RetrieveDefaultPrinters(bool excludeDefaultPA)
        {
            if (_PrintXrefTable.RetrieveXref(EnvironmentID, WorkflowID, Version, Branch, ContainerType, Customer, Item, Location, MixedContainer, PrintXrefPatterns.ToDictionary(x => x.Key, x => x.Value)))
            {
                string printGroup = RFS_UserPreference.RetrievePreference(_DataAccess, UserID, "PrintGroup");

                //If the ExcludeDefaultPA is false, the code first attempts to find a record by joining the RFS_PrinterDefinition and the RFS_PrintFormat table, using the 
                //FormName and DefaultPA values from the RFS_PrintXref table (also using the RFS_PrintGroup table if a value exists). If the fetch fails, or if the ExcludeDefaultPA
                //if true, a similar fetch is done, but this time the DefaultPA is dropped. If that fails, we are now at the point of determining the error to throw.
                if ((!excludeDefaultPA) && (_PrinterDefinitionTable.RetrievePrinters(EnvironmentID, _PrintXrefTable.ThisDataTable.ColumnToEnumerable<string>("DefaultPA"), printGroup,
                        _PrintXrefTable.ThisDataTable.ColumnToEnumerable<string>("FormName"), true)))
                {
                    return true;
                }
                else if (_PrinterDefinitionTable.RetrievePrinters(EnvironmentID, null, printGroup, _PrintXrefTable.ThisDataTable.ColumnToEnumerable<string>("FormName"), true))
                {
                    return true;
                }
                else if (_PrinterDefinitionTable.RetrievePrinters(EnvironmentID, null, null, _PrintXrefTable.ThisDataTable.ColumnToEnumerable<string>("FormName"), true))
                {
                    _PrinterDefinitionTable.ResetAll(); //removes any possible records, as they are invalid
                    ErrorMessageVocabulary = "PRINTERNOTINGROUP";
                    return false;
                }
                else
                {
                    ErrorMessageVocabulary = "NOPRINTERRECORDS";
                    return false;
                }
            }
            else
            {
                ErrorMessageVocabulary = "INVALIDXREF";
                return false;
            }
        }

        #endregion

        #region ValidatePrinter

        /// <summary>
        /// Validates if the provided Printer Name is valid, using already set Print Xref parameters to check, to validate
        /// against all the related Print Tables
        /// </summary>
        /// <param name="printerName">Printer Name</param>
        /// <returns>True if the Printer Name is valid in relation to the Print Table, otherwise, False</returns>
        public virtual bool ValidatePrinter(string printerName)
        {
            if (_PrintXrefTable.RetrieveXref(EnvironmentID, WorkflowID, Version, Branch, ContainerType, Customer, Item, Location, MixedContainer, PrintXrefPatterns.ToDictionary(x => x.Key, x => x.Value)))
            {
                string printGroup = RFS_UserPreference.RetrievePreference(_DataAccess, UserID, "PrintGroup");

                //Validates the printer name provided is valid (including checking the Print Group if applicable). Should the first fetch fail, the following fetches
                //simply determine the error to throw
                if (_PrinterDefinitionTable.ValidatePrinterName(EnvironmentID, printerName, printGroup, _PrintXrefTable.ThisDataTable.ColumnToEnumerable<string>("FormName")))
                {
                    PrinterName = _PrinterDefinitionTable.PrinterName;
                    PrinterAddress = _PrinterDefinitionTable.Address;
                    return true;
                }
                else if (_PrinterDefinitionTable.ValidatePrinterName(EnvironmentID, printerName, null, _PrintXrefTable.ThisDataTable.ColumnToEnumerable<string>("FormName")))
                {
                    _PrinterDefinitionTable.ResetAll(); //removes any possible records, as they are invalid
                    ErrorMessageVocabulary = "PRINTERNOTINGROUP";
                    return false;
                }
                else
                {
                    ErrorMessageVocabulary = "INVALIDPRINTER";
                    return false;
                }
            }
            else
            {
                ErrorMessageVocabulary = "INVALIDXREF";
                return false;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of the Managed Resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Cleans up all the resources
        /// </summary>
        /// <param name="Disposing">When True, we clean up Managed Resources as well as Unmanaged</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (_PrinterDefinitionTable != null)
                {
                    _PrinterDefinitionTable.Dispose();
                }

                if (_PrintXrefTable != null)
                {
                    _PrintXrefTable.Dispose();
                }
            }

            IsDisposed = true;
        }

        #endregion
    }
}