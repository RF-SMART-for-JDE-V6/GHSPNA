using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ICS.Base.Wireless.JDE.BusinessConnector;
using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
using ICS.Database;
using ICS.Database.TableObjects.RFS;
using ICS.Database.TableObjects.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.ERPI.JDE;
using ICS.Stoneridge.Wireless.JDE.ERPI;
using ICS.Stoneridge.Wireless.JDE.Utilities.Printing;
using ICS.Stoneridge.Wireless.RFS.Database.TableObjects;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Picking
{
    public class StoneridgePickPackConnector : BaseConnector, IDisposable
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

        public virtual double SalesOrderAddressNumber { get; protected set; }

        public virtual double SalesOrderShipToNumber { get; protected set; }

        public virtual string PalletNumber { get; protected set; }

        public virtual string PalletContainerCode { get; protected set; }

        public virtual double PalletCrossReferenceNumber { get; protected set; }

        public virtual string CustomerContainerID { get; protected set; }

        public virtual double ShortItemNumber { get; protected set; }

        public virtual string LongItemNumber { get; protected set; }

        public virtual string LotNumber { get; protected set; }

        public virtual string SerialNumber { get; protected set; }

        public virtual string CartonContainerCode { get; protected set; }

        public virtual string CustomerCartonID { get; protected set; }

        public virtual double Quantity { get; protected set; }

        public virtual string QuantityUnitOfMeasure { get; protected set; }

        public virtual double Weight { get; protected set; }

        public virtual string WeightUnitOfMeasure { get; protected set; }

        public virtual int NumberOfCartonLabels { get; protected set; }

        public virtual int PalletLabelType { get; protected set; }

        public virtual int NumberOfPalletLabels { get; protected set; }

        public virtual string Location { get; protected set; }

        public virtual string LocationPutawayZone { get; protected set; }

        public virtual string LocationPickZone { get; protected set; }

        public virtual string LocationReplenishmentZone { get; protected set; }

        public virtual string LocationDimensionGroup { get; protected set; }

        #endregion

        #region ERPI

        SalesOrderPick PickSalesOrder_Tx = new SalesOrderPick();

        RFS_TxCartonMaintenaceDriver PackCarton_Tx = new RFS_TxCartonMaintenaceDriver();

        #endregion

        #region Tables

        #region Item Location File (F41021) JDE

        // Private Table Object
        private F41021 _tableJDEItemLocationFile;

        // Public Table Object (Generate New If NULL)
        public F41021 JDEItemLocationFileTable
        {
            get { return _tableJDEItemLocationFile ?? (_tableJDEItemLocationFile = new F41021(DataAccess, "")); }
        }

        #endregion

        #region Sales Order Detail File (F4211) JDE

        // Private Table Object
        private CustomJDETables.F4211 _tableJDESalesOrderDetailFile;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4211 JDESalesOrderDetailFileTable
        {
            get { return _tableJDESalesOrderDetailFile ?? (_tableJDESalesOrderDetailFile = new CustomJDETables.F4211(DataAccess, "")); }
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

        #region Carton Recommendations (F4615) JDE

        // Private Table Object
        private CustomJDETables.F4615 _tableJDECartonRecommendations;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4615 JDECartonRecommendationsTable
        {
            get { return _tableJDECartonRecommendations ?? (_tableJDECartonRecommendations = new CustomJDETables.F4615(DataAccess, "")); }
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

        #region Carton Labels (F59RFCTL) JDE Extended

        // Private Table Object
        private CustomJDETables.F59RFCTL _tableJDECartonLabels;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F59RFCTL JDECartonLabelsTable
        {
            get { return _tableJDECartonLabels ?? (_tableJDECartonLabels = new CustomJDETables.F59RFCTL(DataAccess, "")); }
        }

        #endregion

        #region Serial Number Detail (F5846001) JDE Extended

        // Private Table Object
        private CustomJDETables.F5846001 _tableJDESerialNumberDetail;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F5846001 JDESerialNumberDetailTable
        {
            get { return _tableJDESerialNumberDetail ?? (_tableJDESerialNumberDetail = new CustomJDETables.F5846001(DataAccess, "")); }
        }

        #endregion

        #region Data Manipulator (RFS_DataManipulator) RFS

        // Private Table Object
        private RFS_DataManipulator _tableRFSDataManipulator;

        // Public Table Object (Generate New If NULL)
        public RFS_DataManipulator RFSDataManipulatorTable
        {
            get { return _tableRFSDataManipulator ?? (_tableRFSDataManipulator = new RFS_DataManipulator(DataAccess, "")); }
        }

        #endregion

        #region Location Master (F4100) JDE

        // Private Table Object
        private F4100 _tableJDELocationMaster;

        // Public Table Object (Generate New If NULL)
        public F4100 JDELocationMasterTable
        {
            get { return _tableJDELocationMaster ?? (_tableJDELocationMaster = new F4100(DataAccess, "")); }
        }

        #endregion

        #endregion

        #region Workflow Options

        #region Allowed Shipment Statuses

        // Private WFO variable
        private string[] _wfoAllowedShipmentStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] AllowedShipmentStatuses
        {
            get { return _wfoAllowedShipmentStatuses ?? (_wfoAllowedShipmentStatuses = WFOptions.RetrieveOptionValue("ShipmentStatus").Split(' ')); }
        }

        #endregion

        #region Allowed Sales Order Statuses

        // Private WFO variable
        private string[] _wfoAllowedSalesOrderStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] AllowedSalesOrderStatuses
        {
            get { return _wfoAllowedSalesOrderStatuses ?? (_wfoAllowedSalesOrderStatuses = WFOptions.RetrieveOptionValue("SalesOrderStatus").Split(' ')); }
        }

        #endregion

        #region Drop File Path

        // Private WFO variable
        private string _wfoDropFilePath;

        // Public WFO variable (Generate New If NULL)
        public string DropFilePath
        {
            get { return _wfoDropFilePath ?? (_wfoDropFilePath = WFOptions.RetrieveOptionValue("DropFilePath")); }
        }

        #endregion

        #region ExpendablePackagingWarning

        private bool? _WfoExpendablePackagingWarning;

        public bool WfoExpendablePackagingWarning => (bool) (_WfoExpendablePackagingWarning ?? (_WfoExpendablePackagingWarning = WFOptions.GetYesNoWFOptionValue("ExpendablePackagingWarning")));

        #endregion

        #region Max Items Per Label

        // Private WFO variable
        private int? _wfoMaxItemsPerLabel;

        // Public WFO variable (Generate New If NULL)
        public int MaxItemsPerLabel
        {
            get { return (int) (_wfoMaxItemsPerLabel ?? (_wfoMaxItemsPerLabel = WFOptions.GetInt32WFOptionValue("MaxItemsPerLabel"))); }
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

        #region P4205 Version

        // Private WFO variable
        private string _wfoP4205Version;

        // Public WFO variable (Generate New If NULL)
        public string P4205Version
        {
            get { return _wfoP4205Version ?? (_wfoP4205Version = WFOptions.RetrieveOptionValue("P4205Version")); }
        }

        #endregion

        #region P4620 Version

        // Private WFO variable
        private string _wfoP4620Version;

        // Public WFO variable (Generate New If NULL)
        public string P4620Version
        {
            get { return _wfoP4620Version ?? (_wfoP4620Version = WFOptions.RetrieveOptionValue("P4620Version")); }
        }

        #endregion

        #region Serial Number Type

        // Private Variable
        private int? _wfoSerialNumberType;

        // Enumerated Type Definition
        [Flags]
        public enum SerialNumberTypeDesignator
        {
            None = 0,

            TareSSCC = 1,

            PackSSCC = 2,

            RFSNextNumber = 3
        }

        // Public Variable (Generate New If NULL)
        public SerialNumberTypeDesignator SerialNumberType
        {
            get
            {
                if (_wfoSerialNumberType == null)
                {
                    switch (WFOptions.RetrieveOptionValue("SerialNumberType").Trim())
                    {
                        case "1":
                            _wfoSerialNumberType = (int) SerialNumberTypeDesignator.TareSSCC;
                            break;
                        case "2":
                            _wfoSerialNumberType = (int) SerialNumberTypeDesignator.PackSSCC;
                            break;
                        case "3":
                            _wfoSerialNumberType = (int) SerialNumberTypeDesignator.RFSNextNumber;
                            break;
                        default:
                            _wfoSerialNumberType = (int) SerialNumberTypeDesignator.None;
                            break;
                    }
                }

                return (SerialNumberTypeDesignator) (int) _wfoSerialNumberType;
            }
        }

        #endregion

        #region Sync Transaction

        // Private Variable
        private bool? _wfoSyncTransaction;

        // Public Variable (Generate New If NULL)
        public bool SyncTransaction
        {
            get { return (bool) (_wfoSyncTransaction ?? (_wfoSyncTransaction = WFOptions.GetYesNoWFOptionValue("SyncTransaction", true))); }
        }

        #endregion

        #region Additional Sales Order Statuses

        // Private WFO variable
        private string[] _wfoAdditionalSalesOrderStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] AdditionalSalesOrderStatuses
        {
            get { return _wfoAdditionalSalesOrderStatuses ?? (_wfoAdditionalSalesOrderStatuses = WFOptions.RetrieveOptionValue("ExcludedOrderStatus").Split(' ')); }
        }

        #endregion

        #region Shipping Location

        // Private WFO variable
        private string _wfoShippingLocation;

        // Public WFO variable (Generate New If NULL)
        public string ShippingLocation
        {
            get { return _wfoShippingLocation ?? (_wfoShippingLocation = WFOptions.RetrieveOptionValue("ShippingLocation")); }
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

        public StoneridgePickPackConnector(RFSWorkflowDesigner workflow) :
            base(workflow)
        {
        }

        public StoneridgePickPackConnector() : base()
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

        public void ValidateWorkflowOptions()
        {
        }

        public void ValidateBranchPlant(string BranchPlantInput)
        {
            BranchValidator.BranchValidate(BranchPlantInput);

            if (JDELocationMasterTable.ValidateLocation(BranchValidator.BranchPlant, ShippingLocation))
            {
                Location = JDELocationMasterTable.LMLOCN;
                LocationPutawayZone = JDELocationMasterTable.LMPZON.ToString().Trim();
                LocationPickZone = JDELocationMasterTable.LMKZON.ToString().Trim();
                LocationReplenishmentZone = JDELocationMasterTable.LMZONR.ToString().Trim();
                LocationDimensionGroup = JDELocationMasterTable.LMSTY1.ToString().Trim();

                BranchPlant = BranchValidator.BranchPlant;
                BranchDescription = BranchValidator.BranchDescription;

                ScreenMap.Add("BranchPlant", BranchPlant);
                ScreenMap.Add("BranchDescription", BranchDescription);
            }
            else
            {
                throw new RFS_Exception("Ship Location Invalid");
            }
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
            StoneridgePrinting.ContainerType = "M";

            //Below we check if the PrinterName is blank, if so, it means no records were found for the 
            //ContainerType, so try another (we use "PrinterName" as once we have a valid value it stores
            //it and doesn't try to keep requirying like DefaultPrinter does)
            if (String.IsNullOrWhiteSpace(StoneridgePrinting.PrinterName))
            {
                StoneridgePrinting.ContainerType = "X";
            }

            if (String.IsNullOrWhiteSpace(StoneridgePrinting.PrinterName))
            {
                StoneridgePrinting.ContainerType = "C";
            }

            if (!String.IsNullOrWhiteSpace(StoneridgePrinting.PrinterName))
            {
                return StoneridgePrinting.PrinterName;
            }

            Workflow.Error(StoneridgePrinting.ErrorMessageVocabulary);
            return "";
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
            //First hooks up the screens to the proper TableObject, and then query for them (note that all PrintXref
            //parameters have already been setup in the default printer func)
            PrinterValidator.PrinterListLoad(ListPrinters);
            StoneridgePrinting.RetrievePrinters();
        }

        #endregion

        #region Shipment Number

        public void ValidateShipmentNumber(string ShipmentNumberInput)
        {
            double ParsedShipmentNumber;

            if (double.TryParse(ShipmentNumberInput, out ParsedShipmentNumber))
            {
                if (JDEShipmentHeaderTable.GetDistinctShipmentsAtStatus(BranchPlant, ParsedShipmentNumber, new string[] { }))
                {
                    if (JDEShipmentHeaderTable.GetDistinctShipmentsAtStatus(BranchPlant, ParsedShipmentNumber, AllowedShipmentStatuses))
                    {
                        if (!JDESalesOrderDetailFileTable.FetchShipmentLinesAtInvalidStatus(BranchPlant, JDEShipmentHeaderTable.XHSHPN, AllowedSalesOrderStatuses, AdditionalSalesOrderStatuses))
                        {
                            //AGCH
                            if (JDESalesOrderDetailFileTable.FetchShipmentLinesAtValidStatusNotSeq(BranchPlant, JDEShipmentHeaderTable.XHSHPN, AllowedSalesOrderStatuses, WFOptions.RetrieveOptionValue("ExcludedOrderTypes")))
                            {
                                ShipmentNumber = JDEShipmentHeaderTable.XHSHPN;
                                SalesOrderAddressNumber = JDESalesOrderDetailFileTable.SDAN8;
                                SalesOrderShipToNumber = JDESalesOrderDetailFileTable.SDSHAN;

                                ScreenMap.Add("ShipmentNumber", ShipmentNumber.ToString());
                            }
                            else
                            {
                                throw new RFS_Exception("No Open Sales Order Lines");
                            }
                        }
                        else
                        {
                            throw new RFS_Exception("Not All SO Lines Are At the Correct Status");
                        }
                    }
                    else
                    {
                        throw new RFS_Exception("Invalid Shipment Status");
                    }
                }
                else
                {
                    throw new RFS_Exception("Invalid Shipment");
                }
            }
            else
            {
                throw new RFS_Exception("Not Numeric");
            }
        }

        public void LoadShipmentNumberList(GenericList ListShipmentNumbers)
        {
            ListShipmentNumbers.Table = JDEShipmentHeaderTable;
            JDEShipmentHeaderTable.GetDistinctShipmentsAtStatus(BranchPlant, -1, AllowedShipmentStatuses);
        }

        #endregion

        #region Pallet Number

        public void LoadPalletNumberList(GenericList ListPalletNumbers)
        {
            ListPalletNumbers.Table = JDECartonLabelsTable;
            JDECartonLabelsTable.GetCarton(BranchPlant, ShipmentNumber, null);
        }

        public void ValidatePalletNumber(string PalletNumberInput)
        {
            double ParsedPalletNumber;

            if (double.TryParse(PalletNumberInput, out ParsedPalletNumber))
            {
                if (JDECartonLabelsTable.GetCarton(BranchPlant, -1, PalletNumberInput))
                {
                    if (JDECartonLabelsTable.GetCarton(BranchPlant, ShipmentNumber, PalletNumberInput))
                    {
                        PalletNumber = JDECartonLabelsTable.CLCEQT;
                        PalletContainerCode = JDECartonLabelsTable.CLEQTY;
                        PalletCrossReferenceNumber = JDECartonLabelsTable.CLCRID;

                        if (JDECartonRecommendationsTable.GetStandardPackPallet(ShipmentNumber, JDECartonLabelsTable.CLEQTY))
                        {
                            CustomerContainerID = JDECartonRecommendationsTable.CRCEQT;
                        }
                        else
                        {
                            CustomerContainerID = "";
                        }

                        ScreenMap.Add("PalletNumber", PalletNumber);
                    }
                    else
                    {
                        throw new RFS_Exception("Pallet in Different Shipment");
                    }
                }
                else
                {
                    throw new RFS_Exception("Invalid Pallet ID");
                }
            }
            else
            {
                throw new RFS_Exception("Not Numeric");
            }
        }

        #endregion

        #region Serial Number

        public void LoadStandardPack()
        {
        }

        public void LoadQuantityRemainingList(GenericList ListSerialNumbers)
        {
            ListSerialNumbers.Table = JDESalesOrderDetailFileTable;
            JDESalesOrderDetailFileTable.FetchConsolidatedQuantity(ShipmentNumber, AllowedSalesOrderStatuses, PalletCrossReferenceNumber);
        }

        public int ValidateSerialNumber(string SerialNumberInput)
        {
            if (!JDESerialNumberDetailTable.GetSerialNumber(BranchPlant, SerialNumberInput, false))
            {
                return 1;
            }

            if (!JDESerialNumberDetailTable.GetSerialNumber(BranchPlant, SerialNumberInput, true))
            {
                return 2;
            }

            if (!JDECartonRecommendationsTable.GetReturnableExpendableContainerCode(BranchPlant, ShipmentNumber, JDESerialNumberDetailTable.LPITM))
            {
                throw new RFS_Exception("Invalid Container Code");
            }

            if (JDECartonDetailInformationTable.GetPackCollisions(BranchPlant, ShipmentNumber, JDESerialNumberDetailTable.LPITM, PalletCrossReferenceNumber))
            {
                throw new RFS_Exception("Pack Collision");
            }

            Log.Debug("Short Item Number = " + JDESerialNumberDetailTable.LPITM);
            Log.Debug("Lot Number = '" + JDESerialNumberDetailTable.LPLOTN + "' ");
            Log.Debug("Serial Location = '" + JDELocationMasterTable.LMLOCN + "' ");
            if (!JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, JDESerialNumberDetailTable.LPITM, JDELocationMasterTable.LMLOCN, JDESerialNumberDetailTable.LPLOTN))
            {
                throw new RFS_Exception("Item/Lot Qty Not Available");
            }

            if (JDECartonDetailInformationTable.QuantityToPack(BranchPlant, ShipmentNumber, JDESerialNumberDetailTable.LPITM /*ShortItemNumber*/, PalletCrossReferenceNumber, JDESerialNumberDetailTable.LPLOTN))
            {
                Log.Debug("F4620 Records Found with Total Quantity of = " + JDECartonDetailInformationTable.CDTQTY);
                Log.Debug("F41021 Quantity on Hand = " + JDEItemLocationFileTable.LIPQOH);
                Log.Debug("F5846001 Serial Quantity = " + JDESerialNumberDetailTable.LPTQTY);
                if (JDESerialNumberDetailTable.LPTQTY <= (JDEItemLocationFileTable.LIPQOH - JDECartonDetailInformationTable.CDTQTY))
                {
                    return SetQuantityInformation();
                }

                throw new RFS_Exception("Insufficient Qty for Item/Lot");
            }

            Log.Debug("F4620 Records Not Found");
            Log.Debug("F41021 Quantity on Hand = " + JDEItemLocationFileTable.LIPQOH);
            Log.Debug("F5846001 Serial Quantity = " + JDESerialNumberDetailTable.LPTQTY);
            if (JDESerialNumberDetailTable.LPTQTY <= JDEItemLocationFileTable.LIPQOH)
            {
                return SetQuantityInformation();
            }

            throw new RFS_Exception("Insufficient Qty for Item/Lot");
        }

        //public int SetQuantityInformation()
        //                {
        //                    Log.Debug("Branch Plant = '" + BranchPlant + "' ");
        //                    Log.Debug("Serial Location = '" + JDESerialNumberDetailTable.LPLOCN + "' ");
        //                    if (JDELocationMasterTable.ValidateLocation(BranchPlant, JDESerialNumberDetailTable.LPLOCN))
        //                    {
        //                        Log.Debug("WFO Location = '" + Location + "' ");
        //                        if (JDESerialNumberDetailTable.LPLOCN.Trim().ToUpper() == Location.Trim().ToUpper())
        //                        {
        //                            Log.Debug("Short Item Number = " + JDESerialNumberDetailTable.LPITM);
        //                            Log.Debug("Lot Number = '" + JDESerialNumberDetailTable.LPLOTN + "' ");
        //                            Log.Debug("Serial Location = '" + JDELocationMasterTable.LMLOCN + "' ");
        //                            if (JDEItemLocationFileTable.RetrieveSpecificRecord(BranchPlant, JDESerialNumberDetailTable.LPITM, JDELocationMasterTable.LMLOCN, JDESerialNumberDetailTable.LPLOTN))
        //                            {
        //                                if (JDECartonDetailInformationTable.QuantityToPack(BranchPlant, ShipmentNumber, ShortItemNumber, PalletCrossReferenceNumber))
        //                                {
        //                                    Log.Debug("F4620 Records Found with Total Quantity of = " + JDECartonDetailInformationTable.CDTQTY);
        //                                    Log.Debug("F41021 Quantity on Hand = " + JDEItemLocationFileTable.LIPQOH);
        //                                    Log.Debug("F5846001 Serial Quantity = " + JDESerialNumberDetailTable.LPTQTY);
        //                                    if (JDESerialNumberDetailTable.LPTQTY <= (JDEItemLocationFileTable.LIPQOH - JDECartonDetailInformationTable.CDTQTY))
        //                                    {
        //                                        return SetQuantityInformation();
        //                                    }
        //                                    else
        //                                    {
        //                                        throw new RFS_Exception("Insufficient Qty for Item/Lot");
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Log.Debug("F4620 Records Not Found");
        //                                    Log.Debug("F41021 Quantity on Hand = " + JDEItemLocationFileTable.LIPQOH);
        //                                    Log.Debug("F5846001 Serial Quantity = " + JDESerialNumberDetailTable.LPTQTY);
        //                                    if (JDESerialNumberDetailTable.LPTQTY <= JDEItemLocationFileTable.LIPQOH)
        //                                    {
        //                                        return SetQuantityInformation();
        //                                    }
        //                                    else
        //                                    {
        //                                        throw new RFS_Exception("Insufficient Qty for Item/Lot");
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                throw new RFS_Exception("Item/Lot Qty Not Available");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            throw new RFS_Exception("Carton Location Mismatch");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        throw new RFS_Exception("Invalid Carton Location");
        //    }
        //}

        public int SetQuantityInformation()
        {
            ShortItemNumber = JDESerialNumberDetailTable.LPITM;
            LongItemNumber = JDESerialNumberDetailTable.LPLITM;
            LotNumber = JDESerialNumberDetailTable.LPLOTN;
            SerialNumber = JDESerialNumberDetailTable.LPLPNU;
            Quantity = JDESerialNumberDetailTable.LPTQTY;
            QuantityUnitOfMeasure = JDESerialNumberDetailTable.LPUOM;
            Weight = JDESerialNumberDetailTable.LPGWEI;
            WeightUnitOfMeasure = JDESerialNumberDetailTable.LPUWUM;

            if (JDESerialNumberDetailTable.LPURCD == "0")
            {
                CartonContainerCode = JDECartonRecommendationsTable.CREQTY;
                CustomerCartonID = JDECartonRecommendationsTable.CRCEQT;

                ScreenMap.Add("SerialNumber", SerialNumber);

                return 0;
            }

            if (JDESerialNumberDetailTable.LPURCD == "1")
            {
                if (JDECartonRecommendationsTable["IDEXCN"].ToString().Trim() == "1")
                {
                    CartonContainerCode = JDECartonRecommendationsTable.CREQTY;
                    CustomerCartonID = JDECartonRecommendationsTable.CRCEQT;

                    ScreenMap.Add("SerialNumber", SerialNumber);

                    return 0;
                }

                if (JDECartonRecommendationsTable["IDEXCN"].ToString().Trim() == "0" || string.IsNullOrWhiteSpace(JDECartonRecommendationsTable["IDEXCN"].ToString()))
                {
                    CartonContainerCode = "EXP1";
                    CustomerCartonID = "";

                    ScreenMap.Add("SerialNumber", SerialNumber);

                    return 0;
                }

                throw new RFS_Exception("Invalid Container Code");
            }

            throw new RFS_Exception("Invalid Container Type");
        }

        #endregion

        #region Full Pallet Confirmation

        public bool ValidateYesNoConfirmation(string YesNoConfirmationInput)
        {
            return YesNoConfirmationInput.GetYesNo();
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

        #region Pallet Label Type

        public void ValidatePalletLabelType(string PalletLabelTypeInput)
        {
            switch (PalletLabelTypeInput.Trim().ToUpper())
            {
                case "N":
                    PalletLabelType = 0;
                    ScreenMap.Add("LabelType", "None");
                    break;
                case "M":
                    PalletLabelType = 1;
                    ScreenMap.Add("LabelType", "Master");
                    break;
                case "X":
                    PalletLabelType = 2;
                    ScreenMap.Add("LabelType", "Mixed Load");
                    break;
                default:
                    throw new RFS_Exception("InvalidLabelType");
            }
        }

        #endregion

        #region Number Of Pallet Labels

        public void ValidateNumberOfPalletLabels(string NumberOfPalletLabelsInput)
        {
            PrinterValidator._MaxLabels = MaxLabels;
            PrinterValidator.ValidateNumberOfLabels(NumberOfPalletLabelsInput);

            NumberOfPalletLabels = PrinterValidator._NumberOfLabels;

            ScreenMap.Add("NumberOfPalletLabels", NumberOfPalletLabels.ToString());
        }

        #endregion

        #endregion

        #region Helper Methods

        #region Full Item Quantity Is Packed

        public bool FullItemQuantityIsPacked()
        {
            double InitialQuantityToPack;

            if (JDESalesOrderDetailFileTable.QuantityToPack(BranchPlant, ShipmentNumber, ShortItemNumber, AllowedSalesOrderStatuses))
            {
                if (JDECartonDetailInformationTable.QuantityToPack(BranchPlant, ShipmentNumber, ShortItemNumber, -1))
                {
                    InitialQuantityToPack = JDESalesOrderDetailFileTable.SDSOQS - JDECartonDetailInformationTable.CDTQTY;
                }
                else
                {
                    InitialQuantityToPack = JDESalesOrderDetailFileTable.SDSOQS;
                }

                return (InitialQuantityToPack <= 0);
            }

            return true;
        }

        #endregion

        #region Full Shipment Quantity Is Packed

        public bool FullShipmentQuantityIsPacked()
        {
            double InitialQuantityToPack;

            if (JDESalesOrderDetailFileTable.QuantityToPack(BranchPlant, ShipmentNumber, -1, AllowedSalesOrderStatuses))
            {
                if (JDECartonDetailInformationTable.QuantityToPack(BranchPlant, ShipmentNumber, -1, -1))
                {
                    InitialQuantityToPack = JDESalesOrderDetailFileTable.SDSOQS - JDECartonDetailInformationTable.CDTQTY;
                }
                else
                {
                    InitialQuantityToPack = JDESalesOrderDetailFileTable.SDSOQS;
                }

                return (InitialQuantityToPack <= 0);
            }

            return true;
        }

        #endregion

        #region Transaction Sent For Shipment

        public bool TransactionSentForShipment()
        {
            if (JDECartonDetailInformationTable.QuantityToPack(BranchPlant, ShipmentNumber, -1, PalletCrossReferenceNumber))
            {
                if (JDECartonDetailInformationTable.CDTQTY > 0)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        #endregion

        #region Enough Quantity to Pick

        public void EnoughQuantityToPick()
        {
            if (JDECartonDetailInformationTable.FindRecordsWithInsufficientQOH(BranchPlant, ShipmentNumber, PalletCrossReferenceNumber, Location, AllowedSalesOrderStatuses))
            {
                throw new RFS_Exception("NotEnoughQuantityToPick");
            }
        }

        #endregion

        #region Send Pick Sales Order To ERP

        public bool SendPickSalesOrderToERP()
        {
            string LastOrderNumber = "";
            string LastLineNumber = "";
            string NewLineNumber = "";

            if (JDECartonDetailInformationTable.QuantityToPick(BranchPlant, ShipmentNumber, PalletCrossReferenceNumber, AllowedSalesOrderStatuses))
            {
                Transactions.Add(PickSalesOrder_Tx);
                Transactions.Branch = BranchPlant.Trim().ToUpper();
                PickSalesOrder_Tx.Initialize();

                foreach (DataRow CartonRow in JDECartonDetailInformationTable.Rows)
                {
                    JDECartonDetailInformationTable.CurrentRow = CartonRow;

                    #region Set Item Number Parameters

                    PickSalesOrder_Tx.SecondItemNumber = JDECartonDetailInformationTable["IMLITM"].ToString().Trim();
                    PickSalesOrder_Tx.ShortItemNumber = JDECartonDetailInformationTable["IMITM"].ToString();
                    PickSalesOrder_Tx.ThirdItemNumber = JDECartonDetailInformationTable["IMAITM"].ToString().Trim();
                    PickSalesOrder_Tx.ItemDescription1 = JDECartonDetailInformationTable["IMDSC1"].ToString().Trim();
                    PickSalesOrder_Tx.ItemDescription2 = JDECartonDetailInformationTable["IMDSC2"].ToString().Trim();
                    PickSalesOrder_Tx.ItemDimGroup = JDECartonDetailInformationTable["IBPRP6"].ToString().Trim();
                    PickSalesOrder_Tx.ItemLotProcType = JDECartonDetailInformationTable["IBSRCE"].ToString();

                    #endregion

                    #region Set From Location Parameters

                    PickSalesOrder_Tx.BranchPlant = BranchPlant.Trim();
                    //PickSalesOrder_Tx.Location = JDECartonDetailInformationTable["LMLOCN"].ToString();
                    //PickSalesOrder_Tx.FromLocnPutZone = JDECartonDetailInformationTable["LMPZON"].ToString().Trim();
                    //PickSalesOrder_Tx.FromLocnPickZone = JDECartonDetailInformationTable["LMKZON"].ToString().Trim();
                    //PickSalesOrder_Tx.FromLocnReplZone = JDECartonDetailInformationTable["LMZONR"].ToString().Trim();
                    //PickSalesOrder_Tx.FromLocnDimGroup = JDECartonDetailInformationTable["LMSTY1"].ToString().Trim();
                    PickSalesOrder_Tx.Location = Location;
                    PickSalesOrder_Tx.FromLocnPutZone = LocationPutawayZone;
                    PickSalesOrder_Tx.FromLocnPickZone = LocationPickZone;
                    PickSalesOrder_Tx.FromLocnReplZone = LocationReplenishmentZone;
                    PickSalesOrder_Tx.FromLocnDimGroup = LocationDimensionGroup;

                    #endregion

                    #region Set To Location Parameters

                    //PickSalesOrder_Tx.ToLocnPutZone = JDECartonDetailInformationTable["LMPZON"].ToString().Trim();
                    //PickSalesOrder_Tx.ToLocnPickZone = JDECartonDetailInformationTable["LMKZON"].ToString().Trim();
                    //PickSalesOrder_Tx.ToLocnReplZone = JDECartonDetailInformationTable["LMZONR"].ToString().Trim();
                    //PickSalesOrder_Tx.ToLocnDimGroup = JDECartonDetailInformationTable["LMSTY1"].ToString().Trim();
                    PickSalesOrder_Tx.ToLocation = Location;
                    PickSalesOrder_Tx.ToLocnPutZone = LocationPutawayZone;
                    PickSalesOrder_Tx.ToLocnPickZone = LocationPickZone;
                    PickSalesOrder_Tx.ToLocnReplZone = LocationReplenishmentZone;
                    PickSalesOrder_Tx.ToLocnDimGroup = LocationDimensionGroup;

                    #endregion

                    #region Set Ship To Parameters

                    PickSalesOrder_Tx.CustShipTo = JDECartonDetailInformationTable["SDSHAN"].ToString();
                    PickSalesOrder_Tx.CustShipToDescr = JDECartonDetailInformationTable["ShipTo_ABALPH"].ToString();

                    #endregion

                    #region Set Sold To Parameters

                    PickSalesOrder_Tx.CustSoldTo = JDECartonDetailInformationTable["SDAN8"].ToString();
                    PickSalesOrder_Tx.CustSoldToDescr = JDECartonDetailInformationTable["SoldTo_ABALPH"].ToString();

                    #endregion

                    #region Set Order Number Parameters

                    PickSalesOrder_Tx.OrderNumber = JDECartonDetailInformationTable["SDDOCO"].ToString();
                    PickSalesOrder_Tx.OrderType = JDECartonDetailInformationTable["SDDCTO"].ToString().Trim();
                    PickSalesOrder_Tx.OrderCompany = JDECartonDetailInformationTable["SDKCOO"].ToString().Trim();
                    if (LastLineNumber == JDECartonDetailInformationTable["SDLNID"].ToString() && LastOrderNumber == JDECartonDetailInformationTable["SDDOCO"].ToString())
                    {
                        PickSalesOrder_Tx.LineNumber = NewLineNumber;
                    }
                    else
                    {
                        PickSalesOrder_Tx.LineNumber = RFSDataManipulatorTable.ManipulateDataForErp("F4211", "SDLNID", JDECartonDetailInformationTable["SDLNID"].GetDouble());
                        LastLineNumber = JDECartonDetailInformationTable["SDLNID"].ToString();
                        LastOrderNumber = JDECartonDetailInformationTable["SDDOCO"].ToString();
                    }

                    #endregion

                    #region Set Lot Number Parameters

                    PickSalesOrder_Tx.Lot = JDECartonDetailInformationTable["CDLOT"].ToString().Trim();

                    #endregion

                    #region Set Quantity Parameters

                    Log.Debug(string.Format("<***> CDTQTY = {0}", RFSDataManipulatorTable.ManipulateDataForErp("F4620", "CDTQTY", JDECartonDetailInformationTable["CDTQTY"].ToString())));
                    //Per Dale P changed to use DataManipulator for the QTYINV change
                    //PickSalesOrder_Tx.Quantity = JDECartonDetailInformationTable["CDTQTY"].ToString();
                    PickSalesOrder_Tx.Quantity = RFSDataManipulatorTable.ManipulateDataForErp("F4620", "CDTQTY", JDECartonDetailInformationTable["CDTQTY"].ToString());
                    PickSalesOrder_Tx.UOM = JDECartonDetailInformationTable["SDUOM"].ToString().Trim();
                    PickSalesOrder_Tx.SecondaryQuantity = string.Empty;
                    PickSalesOrder_Tx.SecondaryUOM = string.Empty;

                    #endregion

                    #region Set Additional Parameters

                    PickSalesOrder_Tx.ManuallySplitSalesOrder = "1";
                    PickSalesOrder_Tx.Mode = "1";
                    PickSalesOrder_Tx.P4205Version = WFOptions.RetrieveOptionValue("P4205Version");
                    PickSalesOrder_Tx.P4113Version = "";
                    PickSalesOrder_Tx.RFUserID = UserSession.CurrentUser.LoginName;
                    PickSalesOrder_Tx.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
                    //PickSalesOrder_Tx.UserInput01 = string.Empty;
                    PickSalesOrder_Tx.UserInput01 = RFSDataManipulatorTable.ManipulateDataForErp("F4211", "SDLNID", JDECartonDetailInformationTable["SDLNID"].ToString());
                    PickSalesOrder_Tx.UserInput02 = string.Empty;
                    PickSalesOrder_Tx.UserInput03 = string.Empty;
                    PickSalesOrder_Tx.UserInput04 = string.Empty;
                    PickSalesOrder_Tx.UserInput05 = string.Empty;
                    PickSalesOrder_Tx.UserInput06 = string.Empty;
                    PickSalesOrder_Tx.UserInput07 = string.Empty;

                    #endregion

                    #region Set Modes

                    PickSalesOrder_Tx.AddToBoundaryWithData();
                    PickSalesOrder_Tx.Mode = "2";
                    PickSalesOrder_Tx.AddToBoundaryWithData();
                    PickSalesOrder_Tx.Mode = "3";

                    #endregion

                    Transactions.Send(SyncTransaction ? "Y" : "N");
                    Workflow.DisplayMessages();

                    if (!Transactions.Error)
                    {
                        NewLineNumber = PickSalesOrder_Tx.ReturnField("SplitLineNumber", 0, 0);
                    }
                    else
                    {
                        break;
                    }
                }

                return !Transactions.Error;
            }

            throw new RFS_Exception("No Carton Records to Pick");
        }

        #endregion

        #region Send Pack Carton To ERP

        public bool SendPackCartonToERP()
        {
            double RemainingQuantityToPack = Quantity;
            double InitialQuantityToPack;
            int RowIndex = 0;

            if (JDESalesOrderDetailFileTable.QuantityToPack(BranchPlant, ShipmentNumber, ShortItemNumber, AllowedSalesOrderStatuses))
            {
                if (JDECartonDetailInformationTable.QuantityToPack(BranchPlant, ShipmentNumber, ShortItemNumber, PalletCrossReferenceNumber))
                {
                    InitialQuantityToPack = JDESalesOrderDetailFileTable.SDSOQS - JDECartonDetailInformationTable.CDTQTY;
                }
                else
                {
                    InitialQuantityToPack = JDESalesOrderDetailFileTable.SDSOQS;
                }

                // Check if there are sales order quantities to eventually pick against
                if (InitialQuantityToPack > 0)
                {
                    // Check if the quantity to be packed for this serial is positive
                    if (RemainingQuantityToPack > 0)
                    {
                        // Select the order lines to pack to
                        if (JDESalesOrderDetailFileTable.FetchShipmentItemLines(BranchPlant, ShipmentNumber, ShortItemNumber, AllowedSalesOrderStatuses))
                        {
                            Transactions.Add(PackCarton_Tx);
                            Transactions.Branch = BranchPlant.Trim().ToUpper();
                            PackCarton_Tx.Initialize();

                            PackCarton_Tx.BranchPlant = BranchPlant;
                            PackCarton_Tx.ShipmentNumber = ShipmentNumber.ToString();
                            PackCarton_Tx.ItemNumber = LongItemNumber;
                            PackCarton_Tx.LotNumber = LotNumber;
                            PackCarton_Tx.CartonSerialNumber = SerialNumber;
                            PackCarton_Tx.CartonCode = CartonContainerCode;
                            PackCarton_Tx.UserInput03 = CustomerCartonID;
                            PackCarton_Tx.UserInput04 = CustomerContainerID;
                            PackCarton_Tx.UserInput05 = WFOptions.RetrieveOptionValue("CartonWeightSource");
                            PackCarton_Tx.CustomerCartonId = PalletNumber;
                            PackCarton_Tx.SerialNumberType = "3";
                            PackCarton_Tx.Mode = "P"; //changed from "P"
                            PackCarton_Tx.CreateNewCarton = "1";
                            PackCarton_Tx.P4620Version = P4205Version;
                            PackCarton_Tx.RFUserID = UserSession.CurrentUser.LoginName;
                            PackCarton_Tx.TransDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                            //PackCarton_Tx.SuggestionBatchNumber = "JOB#TST        ";
                            //PackCarton_Tx.SuggestionSequenceNumber = "JOB#TST        ";

                            foreach (DataRow OrderRow in JDESalesOrderDetailFileTable.Rows)
                            {
                                JDESalesOrderDetailFileTable.CurrentRow = OrderRow;
                                double RemainingQuantity = RFSDataManipulatorTable.ManipulateDataForErp("F4211", "RemainingQuantity", JDESalesOrderDetailFileTable["RemainingQuantity"]).GetDouble();
                                Log.Debug("RowIndex = " + RowIndex);
                                Log.Debug("RemainingQuantityToPack = " + RemainingQuantityToPack);
                                Log.Debug("JDESalesOrderDetailFileTable['RemainingQuantity'].GetDouble() = " + RemainingQuantity); //JDESalesOrderDetailFileTable["RemainingQuantity"].GetDouble());

                                // Check if we still have quantity to pack
                                if (RemainingQuantityToPack > 0)
                                {
                                    // Check if the current line has quantity to left to pick
                                    if (RemainingQuantity > 0)
                                        //if (JDESalesOrderDetailFileTable["RemainingQuantity"].GetDouble() > 0)
                                    {
                                        Log.Debug("OrderNumber = " + JDESalesOrderDetailFileTable.SDDOCO.ToString());
                                        Log.Debug("OrderLineNumber = " + JDESalesOrderDetailFileTable.SDLNID.ToString());

                                        PackCarton_Tx.OrderNumber = JDESalesOrderDetailFileTable.SDDOCO.ToString();
                                        PackCarton_Tx.OrderType = JDESalesOrderDetailFileTable.SDDCTO;
                                        PackCarton_Tx.OrderCompany = JDESalesOrderDetailFileTable.SDKCOO;
                                        PackCarton_Tx.OrderLineNumber = JDESalesOrderDetailFileTable.SDLNID.ToString();
                                        PackCarton_Tx.OrderSuffix = JDESalesOrderDetailFileTable.SDSFXO;

                                        // Check if the quantity remaining to pick can be fully picked by the current line
                                        if (RemainingQuantityToPack <= RemainingQuantity) //JDESalesOrderDetailFileTable["RemainingQuantity"].GetDouble())
                                        {
                                            Log.Debug("Quantity = " + RemainingQuantity); //RemainingQuantityToPack.ToString());

                                            PackCarton_Tx.Quantity = RemainingQuantityToPack.ToString();
                                            PackCarton_Tx.UnitOfMeasure = QuantityUnitOfMeasure;
                                            PackCarton_Tx.UserInput01 = (Weight * (RemainingQuantityToPack / Quantity)).ToString();
                                            PackCarton_Tx.UserInput02 = WeightUnitOfMeasure;

                                            RemainingQuantityToPack = 0;
                                            break;
                                        }

                                        Log.Debug("Quantity = " + RemainingQuantity); //JDESalesOrderDetailFileTable["RemainingQuantity"].ToString());

                                        PackCarton_Tx.Quantity = RemainingQuantity.ToString(); // JDESalesOrderDetailFileTable["RemainingQuantity"].ToString();
                                        PackCarton_Tx.UnitOfMeasure = QuantityUnitOfMeasure;
                                        PackCarton_Tx.UserInput01 = (Weight * (RemainingQuantity / Quantity)).ToString();
                                        //PackCarton_Tx.UserInput01 = (Weight * (JDESalesOrderDetailFileTable["RemainingQuantity"].GetDouble() / Quantity)).ToString();
                                        PackCarton_Tx.UserInput02 = WeightUnitOfMeasure;

                                        RemainingQuantityToPack -= RemainingQuantity; //JDESalesOrderDetailFileTable["RemainingQuantity"].GetDouble();
                                        PackCarton_Tx.AddToBoundaryWithData();
                                    }
                                }

                                RowIndex++;
                            }

                            // Check if there is any remaining quantity which I couldn't reconcile to full lines or split across multiple lines
                            if (RemainingQuantityToPack == 0)
                            {
                                Transactions.Send(SyncTransaction ? "Y" : "N");
                                Workflow.DisplayMessages();

                                // Whether the transaction errored or not, it could have updated the cartons and written to the F59RFCTL
                                if (JDECartonLabelsTable.GetCarton(BranchPlant, ShipmentNumber, PalletNumber))
                                {
                                    PalletCrossReferenceNumber = JDECartonLabelsTable.CLCRID;
                                }
                                else
                                {
                                    PalletCrossReferenceNumber = 0;
                                }

                                return !Transactions.Error;
                            }

                            throw new RFS_Exception("Serial Exceeds Pickable Quantity");
                        }

                        throw new RFS_Exception("No Open SO Lines");
                    }

                    throw new RFS_Exception("Transaction Quantity Not Positive");
                }

                throw new RFS_Exception("No Open SO Quantity to Pack");
            }

            throw new RFS_Exception("No Open SO Lines");
        }

        #endregion

        #region Print Carton Labels

        /// <summary>
        /// Prints a Carton Label for the currently collected Serial Number
        /// </summary>
        public void PrintCartonLabels()
        {
            //Fetch the F4620 record first before anything, because this workflow will have created the record
            //so we won't have anything primed yet
            if (!JDECartonDetailInformationTable.GetCarton(BranchPlant, SerialNumber))
            {
                throw new RFS_Exception("MissingCarton");
            }

            StoneridgePrinting.ResetAll();
            StoneridgePrinting.Branch = BranchPlant.Trim();
            StoneridgePrinting.ContainerType = "C";
            StoneridgePrinting.Customer = SalesOrderAddressNumber.ToString();
            StoneridgePrinting.Item = SalesOrderShipToNumber.ToString();
            StoneridgePrinting.Location = JDECartonDetailInformationTable.CDITM.ToString();

            //Need to make sure we still have a valid printer record to print with (since when we originally validated we didn't have anything
            //except the Branch, and had to guess with ContainerTypes till a valid match was found)
            if (StoneridgePrinting.ValidatePrinter(PrinterName))
            {
                PrintLabels(NumberOfCartonLabels);
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        #endregion

        #region Print Pallet Labels

        /// <summary>
        /// Prints a Pallet Label for the currently collected Pallet Number
        /// </summary>
        public void PrintPalletLabels()
        {
            //Once again, we need to fetch the F4620 first before anything, because the workflow will be creating the
            //Pallet record, and won't have the TableObject primed with it yet 
            if (!JDECartonDetailInformationTable.GetCarton(BranchPlant, PalletNumber))
            {
                throw new RFS_Exception("MissingPallet");
            }

            StoneridgePrinting.ResetAll();
            StoneridgePrinting.Branch = BranchPlant.Trim();

            switch (PalletLabelType)
            {
                case 1:
                    StoneridgePrinting.ContainerType = "M"; //Master
                    break;
                case 2:
                    StoneridgePrinting.ContainerType = "X"; //Mixed
                    break;
            }

            StoneridgePrinting.Customer = SalesOrderAddressNumber.ToString();
            StoneridgePrinting.Item = SalesOrderShipToNumber.ToString();

            //Need to make sure we still have a valid printer record to print with (since when we originally validated we didn't have anything
            //except the Branch, and had to guess with ContainerTypes till a valid match was found)
            if (StoneridgePrinting.ValidatePrinter(PrinterName))
            {
                PrintLabels(NumberOfPalletLabels);
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        #endregion

        #region Print Labels

        /// <summary>
        /// Does the actual printing, assuming the "StoneridgePrinting" variable has already been primed with
        /// data through validation, and the F4620 has already been fetched as well, containing the record that
        /// wishes to be printed
        /// </summary>
        /// <param name="numOfLabels">Number of Labels</param>
        protected virtual void PrintLabels(int numOfLabels)
        {
            Workflow.PrintSystem.Reset();
            Workflow.PrintSystem.Branch = StoneridgePrinting.Branch;
            Workflow.PrintSystem.ContainerType = StoneridgePrinting.ContainerType;
            Workflow.PrintSystem.Customer = StoneridgePrinting.Customer;
            Workflow.PrintSystem.ItemNumber = StoneridgePrinting.Item;
            Workflow.PrintSystem.Location = StoneridgePrinting.Location;

            Workflow.PrintSystem.PrinterName = StoneridgePrinting.PrinterName;
            Workflow.PrintSystem.PrinterAddress = StoneridgePrinting.PrinterAddress;
            Workflow.PrintSystem.Copies = numOfLabels;

            GenCartonPrintData.ResetAll();
            GenCartonPrintData.GenerateData(ShipmentNumber, JDECartonDetailInformationTable.CDUKID);
            GenCartonPrintData.TransferPrintData(DataAccess); //copies over the data generated to the RFS_AdditionalPrintData DataTable

            if (!Workflow.PrintSystem.Print())
            {
                Workflow.DisplayMessage(Workflow.PrintSystem.ErrorMessageVocabulary);
            }
        }

        #endregion

        #endregion
    }
}