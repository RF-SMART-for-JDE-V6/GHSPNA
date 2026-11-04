using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Print
{
    #region Usings

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

    #endregion

    public class StoneridgeQuickRecieveLabelConnector : BaseConnector, IDisposable
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

        public virtual string ContainerType { get; protected set; } = "Q";

        public virtual double ShipmentNumber { get; protected set; }

        public virtual string Customer { get; protected set; }

        public virtual string ItemNumber { get; protected set; }

        public virtual string PalletNumber { get; protected set; }

        public virtual int NumberOfCartonLabels { get; protected set; }

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

        #region Shipment Header (F4215) JDE

        // Private Table Object
        private CustomJDETables.F4215 _tableJDEShipmentHeader;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4215 JDEShipmentHeaderTable
        {
            get { return _tableJDEShipmentHeader ?? (_tableJDEShipmentHeader = new CustomJDETables.F4215(DataAccess, "")); }
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

        public StoneridgeQuickRecieveLabelConnector(RFSWorkflowDesigner workflow) :
            base(workflow)
        {
        }

        public StoneridgeQuickRecieveLabelConnector() : base()
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
            StoneridgePrinting.ContainerType = ContainerType;

            return StoneridgePrinting.DefaultPrinter.Trim();
        }

        public void ValidatePrinter(string PrinterInput)
        {
            if (StoneridgePrinting.ValidatePrinter(PrinterInput))
            {
                PrinterAddress = StoneridgePrinting.PrinterAddress;
                PrinterName = StoneridgePrinting.PrinterName;

                ScreenMap.Add("PrinterName", StoneridgePrinting.PrinterName.Trim());
                ScreenMap.Add("PrinterAddress", StoneridgePrinting.PrinterAddress.Trim());
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        public void LoadPrinterList(GenericList ListPrinters)
        {
            PrinterValidator.PrinterListLoad(ListPrinters); //hooks up the screen to the proper TableObject
            StoneridgePrinting.RetrievePrinters(); //note the proper params would have been setup in the load (with an M or X container type)
        }

        #endregion

        #region Shipment Number

        public void ValidateShipmentNumber(string ShipmentNumberInput)
        {
            if (SalesOrderDetailTable.RetrieveShipmentLines(ShipmentNumberInput.GetDouble(), BranchPlant)
                && JDEShipmentHeaderTable.ValidateShipment(BranchPlant, ShipmentNumberInput.GetDouble())) //first make sure the shipment even exists
            {
                if (JDECartonDetailInformationTable.GetDistinctShipmentsAtStatus(BranchPlant, SalesOrderDetailTable.SDSHPN, AllowedShipmentStatuses, -1))
                {
                    ShipmentNumber = JDECartonDetailInformationTable.CDSHPN;
                    Customer = JDEShipmentHeaderTable.XHAN8.ToString();
                    ItemNumber = JDEShipmentHeaderTable.XHSHAN.ToString();

                    ScreenMap.Add("ShipmentNumber", ShipmentNumber.ToString());
                    ScreenMap.Add("Customer", Customer);
                    ScreenMap.Add("ItemNumber", ItemNumber);
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

        #region Pallet Number

        public void ValidatePalletNumber(string PalletNumberInput)
        {
            if (JDECartonDetailInformationTable.GetDistinctPalletsAtStatus(BranchPlant, ShipmentNumber, PalletNumberInput, AllowedShipmentStatuses))
            {
                PalletNumber = JDECartonDetailInformationTable.CDURRF;

                ScreenMap.Add("PalletNumber", PalletNumber);
            }
            else
            {
                throw new RFS_Exception("InvalidPalletNumber");
            }
        }

        public void LoadPalletNumberList(GenericList ListPalletNumbers)
        {
            ListPalletNumbers.Table = JDECartonDetailInformationTable;
            JDECartonDetailInformationTable.GetDistinctPalletsAtStatus(BranchPlant, ShipmentNumber, null, AllowedShipmentStatuses);
        }

        #endregion

        #region Print Label

        public void ValidatePrintLabel(string PrintLabelInput)
        {
            if (PrintLabelInput.ToUpper().Trim() == "Y")
            {
                PrintBarcodeLabels();
            }
            else if (PrintLabelInput.ToUpper().Trim() != "N") throw new RFS_Exception("InvalidInput");
        }

        #endregion

        #region Number Of Carton Labels

        public void ValidateNumberOfCartonLabels(string NumberOfCartonLabelsInput)
        {
            PrinterValidator._MaxLabels = MaxLabels;
            PrinterValidator.ValidateNumberOfLabels(NumberOfCartonLabelsInput);

            NumberOfCartonLabels = PrinterValidator._NumberOfLabels;

            ScreenMap.Add("NumberOfCartonLabels", NumberOfCartonLabels.ToString());
        }

        #endregion

        #endregion

        #region Helper Methods

        #region PrintBarcodeLabels

        public void PrintBarcodeLabels()
        {
            Workflow.PrintSystem.Reset();
            Workflow.PrintSystem.Branch = BranchPlant.Trim();
            Workflow.PrintSystem.ContainerType = ContainerType;
            Workflow.PrintSystem.Customer = Customer;
            Workflow.PrintSystem.ItemNumber = ItemNumber;

            Workflow.PrintSystem.PrinterName = PrinterName;
            Workflow.PrintSystem.PrinterAddress = PrinterAddress;
            Workflow.PrintSystem.DropFilePathOverride = DropFilePath;
            Workflow.PrintSystem.Copies = NumberOfCartonLabels;

            //Note that while we are still using the Generate Carton Print Data, we won't actually be generating any specific
            //Carton/Pallet data. This will be focused just on more generic information about the Shipment
            GenCartonPrintData.ResetAll();
            GenCartonPrintData.GenerateData(ShipmentNumber);
            GenCartonPrintData.TransferPrintData(DataAccess); //copies the data generated to the RFS_AdditionalPrintData DataTable

            if (!Workflow.PrintSystem.Print())
            {
                Workflow.DisplayMessage(Workflow.PrintSystem.ErrorMessageVocabulary);
            }
        }

        #endregion

        #endregion
    }
}