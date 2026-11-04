using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ICS.Base.Wireless.JDE.BusinessConnector;
using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
using ICS.Database;
using ICS.Database.TableObjects.RFS;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.Stoneridge.Wireless.JDE.Utilities.Printing;
using ICS.Stoneridge.Wireless.RFS.Database.TableObjects;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    public class StoneridgeCartonLabelReprintConnector : BaseConnector, IDisposable
    {
        #region Class Data

        #region Printing

        #region StoneridgePrinting

        protected StoneridgePrinting _StoneridgePrinting;

        public virtual StoneridgePrinting StoneridgePrinting
        {
            get
            {
                if (_StoneridgePrinting == null)
                {
                    _StoneridgePrinting = new StoneridgePrinting(UserSession);

                    _StoneridgePrinting.PrintXrefPatterns.Reset();
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item | PrintXrefType.Location));
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item | PrintXrefType.BlankLocation));
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.BlankItem | PrintXrefType.BlankLocation));
                    _StoneridgePrinting.PrintXrefPatterns.Add((PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                                                               PrintXrefType.ContainerType | PrintXrefType.BlankCustomer | PrintXrefType.BlankItem | PrintXrefType.BlankLocation));
                }

                return _StoneridgePrinting;
            }
        }

        #endregion

        #region GenCartonPrintData

        protected GenCartonPrintData _GenCartonPrintData;

        public virtual GenCartonPrintData GenCartonPrintData
        {
            get
            {
                if (_GenCartonPrintData == null)
                {
                    _GenCartonPrintData = new GenCartonPrintData(DataAccess);
                    _GenCartonPrintData.VariableNamePrefix = "ASR_";
                }

                return _GenCartonPrintData;
            }
        }

        #endregion

        #endregion

        #region Collected Data

        public virtual string BranchPlant { get; protected set; }

        public virtual string BranchDescription { get; protected set; }

        public virtual string PrinterAddress { get; protected set; }

        public virtual string PrinterName { get; protected set; }

        public virtual double ShipmentNumber { get; protected set; }

        public virtual string SerialNumber { get; protected set; }

        public virtual int NumberOfLabels { get; protected set; }

        #endregion

        #region ERPI

        #endregion

        #region Tables

        #region SalesOrderDetailTable (F4211)

        protected CustomJDETables.F4211 _tableSalesOrderDetailTable;

        public virtual CustomJDETables.F4211 SalesOrderDetailTable
        {
            get { return (_tableSalesOrderDetailTable ?? (_tableSalesOrderDetailTable = new CustomJDETables.F4211(DataAccess, ""))); }
        }

        #endregion

        #region Carton Detail Information (F4620) JDE

        // Private Table Object
        private CustomJDETables.F4620 _tableJDECartonDetailInformation;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4620 JDECartonDetailInformationTable
        {
            get { return _tableJDECartonDetailInformation ?? (_tableJDECartonDetailInformation = new CustomJDETables.F4620(DataAccess, "")); }
        }

        #endregion

        #endregion

        #region Workflow Options

        #region Drop File Path

        // Private WFO variable
        private string _wfoDropFilePath;

        // Public WFO variable (Generate New If NULL)
        public string DropFilePath
        {
            get { return _wfoDropFilePath ?? (_wfoDropFilePath = WFOptions.RetrieveOptionValue("DropFilePath")); }
        }

        #endregion

        #region Max Labels

        // Private WFO variable
        private int? _wfoMaxLabels;

        // Public WFO variable (Generate New If NULL)
        public int MaxLabels
        {
            get { return (int) (_wfoMaxLabels ?? (_wfoMaxLabels = WFOptions.GetInt32WFOptionValue("MaxLabels"))); }
        }

        #endregion

        #region Default Number Of Labels

        // Private WFO variable
        private int? _wfoDefaultNumberOfLabels;

        // Public WFO variable (Generate New If NULL)
        public int DefaultNumberOfLabels
        {
            get { return (int) (_wfoDefaultNumberOfLabels ?? (_wfoDefaultNumberOfLabels = WFOptions.GetInt32WFOptionValue("DefaultNumberLabels"))); }
        }

        #endregion

        #region Allowed Shipment Statuses

        // Private WFO variable
        private string[] _wfoAllowedShipmentStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] AllowedShipmentStatuses
        {
            get { return _wfoAllowedShipmentStatuses ?? (_wfoAllowedShipmentStatuses = WFOptions.RetrieveOptionValue("ShipmentStatus").Split(' ')); }
        }

        #endregion

        #endregion

        #region Validators

        #region Base Branch Validator

        private BranchValidator _validatorBranch;

        public BranchValidator BranchValidator
        {
            get { return _validatorBranch ?? (_validatorBranch = new BranchValidator(Workflow)); }
        }

        #endregion

        #region Base Printer Validator

        private PrintValidator _validatorPrinter;

        public PrintValidator PrinterValidator
        {
            get { return _validatorPrinter ?? (_validatorPrinter = new PrintValidator(Workflow)); }
        }

        #endregion

        #endregion

        #endregion

        #region Constructors and Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflow"></param>
        public StoneridgeCartonLabelReprintConnector(RFSWorkflowDesigner workflow) : base(workflow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public StoneridgeCartonLabelReprintConnector() : base()
        {
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
        /// <param name="disposing">When True, we clean up Managed Resources as well as Unmanaged</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_GenCartonPrintData != null)
                {
                    _GenCartonPrintData.Dispose();
                }

                if (_StoneridgePrinting != null)
                {
                    _StoneridgePrinting.Dispose();
                }
            }
        }

        #endregion

        #region Input Validation

        #region Branch Plant

        public void ValidateBranchPlant(string BranchPlantInput)
        {
            BranchValidator.BranchValidate(BranchPlantInput);

            BranchPlant = BranchValidator.BranchPlant;
            BranchDescription = BranchValidator.BranchDescription;

            ScreenMap.Add("BranchPlant", BranchPlant);
            ScreenMap.Add("BranchDescription", BranchDescription);
        }

        public void LoadBranchPlantList(GenericList ListBranchPlants)
        {
            BranchValidator.BranchListLoad(ListBranchPlants);
        }

        #endregion

        #region Printer

        public string LoadDefaultPrinter()
        {
            StoneridgePrinting.ResetAll();
            StoneridgePrinting.Branch = BranchPlant.Trim();
            StoneridgePrinting.ContainerType = "C";

            return StoneridgePrinting.DefaultPrinter;
        }

        public void ValidatePrinter(string PrinterInput)
        {
            if (StoneridgePrinting.ValidatePrinter(PrinterInput))
            {
                PrinterName = StoneridgePrinting.PrinterName;
                PrinterAddress = StoneridgePrinting.PrinterAddress;

                ScreenMap.Add("PrinterName", PrinterName.Trim());
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        public void LoadPrinterList(GenericList ListPrinters)
        {
            PrinterValidator.PrinterListLoad(ListPrinters); //hooks up the screen to the proper TableObject
            StoneridgePrinting.RetrievePrinters(); //note parameters already setup in the load
        }

        #endregion

        #region Shipment Number

        public void ValidateShipmentNumber(string ShipmentNumberInput)
        {
            if (SalesOrderDetailTable.RetrieveShipmentLines(ShipmentNumberInput.GetDouble(), BranchPlant)) //first make sure the shipment even exists
            {
                if (JDECartonDetailInformationTable.GetDistinctShipmentsAtStatus(BranchPlant, SalesOrderDetailTable.SDSHPN, AllowedShipmentStatuses, -1))
                {
                    ShipmentNumber = JDECartonDetailInformationTable.CDSHPN;
                    ScreenMap.Add("ShipmentNumber", ShipmentNumber.ToString());
                }
                else
                {
                    throw new RFS_Exception("ShipmentStatus");
                }
            }
            else
            {
                throw new RFS_Exception("ShipmentInvalid");
            }
        }

        public void LoadShipmentNumberList(GenericList ListShipmentNumbers)
        {
            ListShipmentNumbers.Table = JDECartonDetailInformationTable;
            JDECartonDetailInformationTable.GetDistinctShipmentsAtStatus(BranchPlant, -1, AllowedShipmentStatuses, -1);
        }

        #endregion

        #region Serial Number

        public void ValidateSerialNumber(string SerialNumberInput)
        {
            if (JDECartonDetailInformationTable.GetDistinctCartonsAtStatus(BranchPlant, ShipmentNumber, SerialNumberInput, AllowedShipmentStatuses))
            {
                SerialNumber = JDECartonDetailInformationTable.CDURRF;

                ScreenMap.Add("SerialNumber", SerialNumber);
            }
            else
            {
                throw new RFS_Exception("InvalidSerialNumber");
            }
        }

        public void LoadSerialNumberList(GenericList ListSerialNumbers)
        {
            ListSerialNumbers.Table = JDECartonDetailInformationTable;
            JDECartonDetailInformationTable.GetDistinctCartonsAtStatus(BranchPlant, ShipmentNumber, null, AllowedShipmentStatuses);
        }

        #endregion

        #region Number Of Labels

        public string LoadDefaultNumberOfLabels()
        {
            return DefaultNumberOfLabels.ToString();
        }

        public void ValidaterNumberOfLabels(string NumberOfLabelsInput)
        {
            PrinterValidator._MaxLabels = MaxLabels;
            PrinterValidator.ValidateNumberOfLabels(NumberOfLabelsInput);

            NumberOfLabels = PrinterValidator._NumberOfLabels;

            ScreenMap.Add("NumberOfLabels", NumberOfLabels.ToString());
        }

        #endregion

        #endregion

        #region Helper Methods

        #region PrintBarcodeLabels

        /// <summary>
        /// 
        /// </summary>
        public void PrintBarcodeLabels()
        {
            JDECartonDetailInformationTable.GetCarton(BranchPlant, SerialNumber); //have to query the table to get back the CDUKID and CDITM

            StoneridgePrinting.ResetAll();
            StoneridgePrinting.Branch = BranchPlant.Trim();
            StoneridgePrinting.ContainerType = "C";
            StoneridgePrinting.Customer = SalesOrderDetailTable.SDAN8.ToString();
            StoneridgePrinting.Item = SalesOrderDetailTable.SDSHAN.ToString();
            StoneridgePrinting.Location = JDECartonDetailInformationTable.CDITM.ToString();

            //Need to make sure we still have a valid printer record to print with (since we have added the Item, Ship To, and Bill To to the Xref search)
            if (StoneridgePrinting.ValidatePrinter(PrinterName))
            {
                Workflow.PrintSystem.Reset();
                Workflow.PrintSystem.Branch = StoneridgePrinting.Branch;
                Workflow.PrintSystem.ContainerType = StoneridgePrinting.ContainerType;
                Workflow.PrintSystem.Customer = StoneridgePrinting.Customer;
                Workflow.PrintSystem.ItemNumber = StoneridgePrinting.Item;
                Workflow.PrintSystem.Location = StoneridgePrinting.Location;

                Workflow.PrintSystem.PrinterName = StoneridgePrinting.PrinterName;
                Workflow.PrintSystem.PrinterAddress = StoneridgePrinting.PrinterAddress;
                Workflow.PrintSystem.Copies = PrinterValidator._NumberOfLabels;

                GenCartonPrintData.ResetAll();
                GenCartonPrintData.GenerateData(ShipmentNumber, JDECartonDetailInformationTable.CDUKID);
                GenCartonPrintData.TransferPrintData(DataAccess); //copies the data generated to the RFS_AdditionalPrintData DataTable;

                if (!Workflow.PrintSystem.Print())
                {
                    Workflow.DisplayMessage(Workflow.PrintSystem.ErrorMessageVocabulary);
                }
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        #endregion

        #endregion
    }
}