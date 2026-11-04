using System;
using System.Collections.Generic;
using System.Linq;
using ICS.Database;

namespace ICS.Stoneridge.Wireless.RFS.Database.TableObjects
{
    #region Enums

    #region PrintXrefType

    /// <summary>
    /// An enumerator to represent the columns that make up the searching
    /// of the RFS_PrintXref table
    /// </summary>
    [Flags]
    public enum PrintXrefType
    {
        None = 0,

        EnvironmentID = 1,

        WorkflowID = 2,

        BlankWorkflowID = 4,

        Version = 8,

        DefaultVersion = 16,

        BlankVersion = 32,

        Branch = 64,

        BlankBranch = 128,

        ContainerType = 256,

        BlankContainerType = 512,

        Customer = 1024,

        BlankCustomer = 2048,

        Item = 4096,

        BlankItem = 8192,

        Location = 16384,

        BlankLocation = 32768,

        MixedContainer = 65536,

        BlankMixedContainer = 131072
    }

    #endregion

    #endregion

    #region RFS_PrintXref

    /// <summary>
    /// Class for the RFS_PrintXref Database Table
    /// </summary>
    public class RFS_PrintXref : ICS.Database.TableObjects.RFS.RFS_PrintXref
    {
        #region Constructors

        /// <summary>
        /// Initializes a new RFS_PrintXref
        /// </summary>
        public RFS_PrintXref() : base()
        {
        }

        /// <summary>
        /// Initializes a new RFS_PrintXref for the provided Data Access Layer and Table ID
        /// </summary>
        /// <param name="dbConnections">Database Acces to the Data and System Tables</param>
        /// <param name="tableID">ID appended to the front of the table name to allow for multiple instances of the same table to exist in the UserDS</param>
        public RFS_PrintXref(DAL dbConnections, string tableID) : base(dbConnections, tableID)
        {
        }

        #endregion

        #region RetrieveXref

        /// <summary>
        /// Attempts to retrieve a valid Print Xref record using the provided collection of Search Patterns and parameters
        /// </summary>
        /// <param name="environemntID">Environment ID</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="version">Version</param>
        /// <param name="branch">Branch</param>
        /// <param name="containerType">Container Type</param>
        /// <param name="customer">Customer</param>
        /// <param name="item">Item</param>
        /// <param name="location">Location</param>
        /// <param name="mixedContainer">Mixed Container</param>
        /// <param name="searchPatterns">Collection of Search Patterns to attempt to find a match for</param>
        /// <returns>True if the one of the Fetches successfully returns any records, otherwise, False</returns>
        public bool RetrieveXref(string environemntID, string workflowID, string version, string branch, string containerType, string customer, string item,
            string location, bool mixedContainer, Dictionary<int, PrintXrefType> searchPatterns)
        {
            return RetrieveXref(environemntID, workflowID, version, branch, containerType, customer, item, location, mixedContainer, null, searchPatterns);
        }

        /// <summary>
        /// Attempts to retrieve a valid Print Xref record using the provided collection of Search Patterns, parameters, and Form Names
        /// </summary>
        /// <param name="environemntID">Environment ID</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="version">Version</param>
        /// <param name="branch">Branch</param>
        /// <param name="containerType">Container Type</param>
        /// <param name="customer">Customer</param>
        /// <param name="item">Item</param>
        /// <param name="location">Location</param>
        /// <param name="mixedContainer">Mixed Container</param>
        /// <param name="formNames">Collection of Form Names</param>
        /// <param name="searchPatterns">Collection of Search Patterns to attempt to find a match for</param>
        /// <returns>True if the one of the Fetches successfully returns any records, otherwise, False</returns>
        public bool RetrieveXref(string environemntID, string workflowID, string version, string branch, string containerType, string customer, string item,
            string location, bool mixedContainer, IEnumerable<string> formNames, Dictionary<int, PrintXrefType> searchPatterns)
        {
            foreach (KeyValuePair<int, PrintXrefType> pattern in searchPatterns)
            {
                if (ValidateXref(environemntID, workflowID, version, branch, containerType, customer, item, location, mixedContainer, formNames, pattern.Value))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region ValidateXref

        /// <summary>
        /// Validates if a Print Xref record exists for the provided parameters and Search Pattern
        /// </summary>
        /// <param name="environemntID">Environment ID</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="version">Version</param>
        /// <param name="branch">Branch</param>
        /// <param name="containerType">Container Type</param>
        /// <param name="customer">Customer</param>
        /// <param name="item">Item</param>
        /// <param name="location">Location</param>
        /// <param name="mixedContainer">Mixed Container</param>
        /// <param name="searchPattern">Search Pattern for fields to use for the fetch, including which to search as blank</param>
        /// <returns>True if the Fetch successfully returns any records, otherwise, False</returns>
        public bool ValidateXref(string environemntID, string workflowID, string version, string branch, string containerType, string customer, string item,
            string location, bool mixedContainer, PrintXrefType searchPattern)
        {
            return ValidateXref(environemntID, workflowID, version, branch, containerType, customer, item, location, mixedContainer, null, searchPattern);
        }

        /// <summary>
        /// Validates if a Print Xref record exists for the provided parameters, Search Pattern, and Form Names
        /// </summary>
        /// <param name="environemntID">Environment ID</param>
        /// <param name="workflowID">Workflow ID</param>
        /// <param name="version">Version</param>
        /// <param name="branch">Branch</param>
        /// <param name="containerType">Container Type</param>
        /// <param name="customer">Customer</param>
        /// <param name="item">Item</param>
        /// <param name="location">Location</param>
        /// <param name="mixedContainer">Mixed Container</param>
        /// <param name="formNames">Collection of Form Names</param>
        /// <param name="searchPattern">Search Pattern for fields to use for the fetch, including which to search as blank</param>
        /// <returns>True if the Fetch successfully returns any records, otherwise, False</returns>
        public bool ValidateXref(string environemntID, string workflowID, string version, string branch, string containerType, string customer, string item,
            string location, bool mixedContainer, IEnumerable<string> formNames, PrintXrefType searchPattern)
        {
            Log.Debug(String.Format("Validating Xref for Search Pattern \"{0}\"", searchPattern));

            ResetAll();

            if (searchPattern.HasFlag(PrintXrefType.MixedContainer))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankMixedContainer))
                {
                    this.MixedContainer = "";
                }
                else
                {
                    this.MixedContainer = (mixedContainer ? "Y" : "N");
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.Location))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankLocation))
                {
                    this.Location = "";
                }
                else
                {
                    this.Location = location;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.Item))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankItem))
                {
                    this.Item = "";
                }
                else
                {
                    this.Item = item;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.Customer))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankCustomer))
                {
                    this.Customer = "";
                }
                else
                {
                    this.Customer = customer;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.ContainerType))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankContainerType))
                {
                    this.ContainerType = "";
                }
                else
                {
                    this.ContainerType = containerType;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.Branch))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankBranch))
                {
                    this.Branch = "";
                }
                else
                {
                    this.Branch = branch;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.Version))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankVersion))
                {
                    this.Version = "";
                }
                else if (searchPattern.HasFlag(PrintXrefType.DefaultVersion))
                {
                    this.Version = "Default";
                }
                else
                {
                    this.Version = version;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.WorkflowID))
            {
                if (searchPattern.HasFlag(PrintXrefType.BlankWorkflowID))
                {
                    this.WorkflowID = "";
                }
                else
                {
                    this.WorkflowID = workflowID;
                }
            }

            if (searchPattern.HasFlag(PrintXrefType.EnvironmentID))
            {
                this.EnvironmentID = environemntID;
            }

            if ((formNames != null) && (formNames.Count() > 0) && (!String.IsNullOrWhiteSpace(formNames.First())))
            {
                Where("AND", Columns.FormName, formNames.ToArray(), "");
            }

            return Fetch();
        }

        #endregion
    }

    #endregion
}