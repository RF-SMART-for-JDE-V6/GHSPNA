using System;
using System.Data;
using ICS.Base.Wireless.JDE.BusinessConnector;
using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
using ICS.Database.TableObjects.JDE;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.Stoneridge.Wireless.JDE.ERPI;
using ICS.Stoneridge.Wireless.JDE.Utilities.Printing;
using ICS.Stoneridge.Wireless.RFS.Database.TableObjects;
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    public class StoneridgeShipConfirmConnector : BaseConnector, IDisposable
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

        public virtual double ShipmentNumber { get; protected set; }

        public virtual string SCAC { get; protected set; }

        public virtual string DefaultTrailerNumber { get; protected set; }

        public virtual string TrailerNumber { get; protected set; }

        public virtual string DefaultPRONumber { get; protected set; }

        public virtual string PRONumber { get; protected set; }

        public virtual string DefaultSealNumber { get; protected set; }

        public virtual string SealNumber { get; protected set; }

        public virtual double IntermediateShipmentWeight { get; protected set; }

        public virtual double DefaultShipmentWeight { get; protected set; }

        public virtual double ShipmentWeight { get; protected set; }

        public virtual string DefaultShipmentUnitOfMeasure { get; protected set; }

        public virtual string ShipmentUnitOfMeasure { get; protected set; }

        public virtual int NumberOfLabels { get; protected set; }

        #endregion

        #region ERPI

        RFS_TxTransShipmentConfirm TrailerNumberUpdate_Tx = new RFS_TxTransShipmentConfirm();

        RFS_TxTransShipmentConfirm PRONumberUpdate_Tx = new RFS_TxTransShipmentConfirm();

        RFS_TxTransShipmentConfirm SealNumberUpdate_Tx = new RFS_TxTransShipmentConfirm();

        RFS_TxTransShipmentConfirm ShipConfirmation_Tx = new RFS_TxTransShipmentConfirm();

        #endregion

        #region Tables

        #region User Defined Code Values (F0005) JDE

        // Private Table Object
        private F0005 _tableJDEUserDefinedCodeValues;

        // Public Table Object (Generate New If NULL)
        public F0005 JDEUserDefinedCodeValuesTable
        {
            get { return _tableJDEUserDefinedCodeValues ?? (_tableJDEUserDefinedCodeValues = new F0005(DataAccess, "")); }
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

        #region Sales Order Header File (F4201) JDE

        // Private Table Object
        private CustomJDETables.F4201 _tableJDESalesOrderHeaderFile;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4201 JDESalesOrderHeaderFileTable
        {
            get { return _tableJDESalesOrderHeaderFile ?? (_tableJDESalesOrderHeaderFile = new CustomJDETables.F4201(DataAccess, "")); }
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

        #region Shipment Reference Numbers (F4217) Customized JDE Extended

        // Private Table Object
        private CustomJDETables.F4217 _tableJDEShipmentReferenceNumbers;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4217 JDEShipmentReferenceNumbersTable
        {
            get { return _tableJDEShipmentReferenceNumbers ?? (_tableJDEShipmentReferenceNumbers = new CustomJDETables.F4217(DataAccess, "")); }
        }

        #endregion

        #region Shipment Routing Steps (F4941) JDE

        // Private Table Object
        private CustomJDETables.F4941 _tableJDEShipmentRoutingSteps;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4941 JDEShipmentRoutingStepsTable
        {
            get { return _tableJDEShipmentRoutingSteps ?? (_tableJDEShipmentRoutingSteps = new CustomJDETables.F4941(DataAccess, "")); }
        }

        #endregion

        #region Carrier Master (F4906) JDE

        // Private Table Object
        private CustomJDETables.F4906 _tableJDECarrierMaster;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4906 JDECarrierMasterTable
        {
            get { return _tableJDECarrierMaster ?? (_tableJDECarrierMaster = new CustomJDETables.F4906(DataAccess, "")); }
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

        #region Shipment Hold Statuses

        // Private WFO variable
        private string[] _wfoShipmentHoldStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] ShipmentHoldStatuses
        {
            get { return _wfoShipmentHoldStatuses ?? (_wfoShipmentHoldStatuses = WFOptions.RetrieveOptionValue("ShipmentHoldStatus").Split(' ')); }
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

        #region Drop File Patch

        // Private WFO variable
        private string _wfoDropFilePath;

        // Public WFO variable (Generate New If NULL)
        public string DropFilePath
        {
            get { return _wfoDropFilePath ?? (_wfoDropFilePath = WFOptions.RetrieveOptionValue("DropFilePath")); }
        }

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

        #region P49645 Version

        // Private WFO variable
        private string _wfoP49645Version;

        // Public WFO variable (Generate New If NULL)
        public string P49645Version
        {
            get { return _wfoP49645Version ?? (_wfoP49645Version = WFOptions.RetrieveOptionValue("P49645Version")); }
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

        public StoneridgeShipConfirmConnector(RFSWorkflowDesigner workflow) :
            base(workflow)
        {
        }

        public StoneridgeShipConfirmConnector() : base()
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

        public void LoadBranchPlantList(GenericList BranchPlantList)
        {
            BranchValidator.BranchListLoad(BranchPlantList);
        }

        #endregion

        #region Shipment Number

        public int ValidateShipmentNumber(string ShipmentNumberInput)
        {
            double ParsedShipmentNumber;

            if (double.TryParse(ShipmentNumberInput, out ParsedShipmentNumber))
            {
                if (ContainsNoHolds(ParsedShipmentNumber))
                {
                    if (JDEShipmentHeaderTable.GetDistinctShipmentsForBranch(BranchPlant, ParsedShipmentNumber, new string[] { }, new string[] { }))
                    {
                        if (JDEShipmentHeaderTable.GetDistinctShipmentsForBranch(BranchPlant, ParsedShipmentNumber, AllowedShipmentStatuses, new string[] { }))
                        {
                            if (JDEShipmentHeaderTable.GetDistinctShipmentsForBranch(BranchPlant, ParsedShipmentNumber, AllowedShipmentStatuses, AllowedSalesOrderStatuses))
                            {
                                ShipmentNumber = JDEShipmentHeaderTable.XHSHPN;

                                ScreenMap.Add("ShipmentNumber", ShipmentNumber.ToString());

                                return 0;
                            }
                            else
                            {
                                // Warn: "Invalid Shipment Status"

                                return 2;
                            }
                        }
                        else
                        {
                            // Warn: "Not All SO Lines Are At The Correct Status"

                            return 1;
                        }
                    }
                    else
                    {
                        throw new RFS_Exception("NotShipmentNumber");
                    }
                }
                else
                {
                    throw new RFS_Exception("SHIPMENTONHOLD");
                }
            }
            else
            {
                throw new RFS_Exception("NotNumeric");
            }
        }

        public void LoadShipmentNumberList(GenericList ShipmentNumberList)
        {
            ShipmentNumberList.Table = JDEShipmentHeaderTable;
            JDEShipmentHeaderTable.GetDistinctShipmentsForBranch(BranchPlant, -1, AllowedShipmentStatuses, AllowedSalesOrderStatuses);
        }

        #endregion

        #region SCAC

        public string LoadShipmentSCAC()
        {
            if (JDEShipmentRoutingStepsTable.GetRouteForShipmentNumber(ShipmentNumber))
            {
                if (JDEShipmentRoutingStepsTable.RSCARS != 0)
                {
                    if (JDECarrierMasterTable.GetSCACForShipmentNumber(ShipmentNumber))
                    {
                        return JDECarrierMasterTable.CMSCAC;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public void ValidateSCAC(string SCACInput)
        {
            if (!string.IsNullOrWhiteSpace(SCACInput))
            {
                if (JDECarrierMasterTable.GetDistinctSCACs(SCACInput))
                {
                    SCAC = JDECarrierMasterTable.CMSCAC;

                    ScreenMap.Add("SCAC", SCAC);
                }
                else
                {
                    throw new RFS_Exception("Invalid SCAC");
                }
            }
            else
            {
                throw new RFS_Exception("BlanksInvalid");
            }
        }

        public void LoadSCACList(GenericList SCACList)
        {
            SCACList.Table = JDECarrierMasterTable;
            JDECarrierMasterTable.GetDistinctSCACs(null);
        }

        #endregion

        #region Trailer Number

        public string LoadShipmentTrailerNumber()
        {
            if (JDEShipmentReferenceNumbersTable.GetTrailerNumberForShipment(ShipmentNumber, "TN"))
            {
                DefaultTrailerNumber = JDEShipmentReferenceNumbersTable.XIREFN;
                return DefaultTrailerNumber;
            }
            else
            {
                DefaultTrailerNumber = "";
                return "";
            }
        }

        public void ValidateTrailerNumber(string TrailerNumberInput)
        {
            if (TrailerNumberInput.Trim().Length <= 30)
            {
                TrailerNumber = TrailerNumberInput.Trim();

                ScreenMap.Add("TrailerNumber", TrailerNumber);
            }
            else
            {
                throw new RFS_Exception("InputLength");
            }
        }

        #endregion

        #region PRO Number

        public string LoadShipmentPRONumber()
        {
            if (JDEShipmentReferenceNumbersTable.GetTrailerNumberForShipment(ShipmentNumber, "CN"))
            {
                DefaultPRONumber = JDEShipmentReferenceNumbersTable.XIREFN;
                return DefaultPRONumber;
            }
            else
            {
                DefaultPRONumber = "";
                return "";
            }
        }

        public void ValidatePRONumber(string PRONumberInput)
        {
            if (PRONumberInput.Trim().Length <= 30)
            {
                PRONumber = PRONumberInput.Trim();

                ScreenMap.Add("PRONumber", PRONumber);
            }
            else
            {
                throw new RFS_Exception("InputLength");
            }
        }

        #endregion

        #region Seal Number

        public string LoadShipmentSealNumber()
        {
            if (JDEShipmentReferenceNumbersTable.GetTrailerNumberForShipment(ShipmentNumber, "SN"))
            {
                DefaultSealNumber = JDEShipmentReferenceNumbersTable.XIREFN;
                return DefaultSealNumber;
            }
            else
            {
                DefaultSealNumber = "";
                return "";
            }
        }

        public void ValidateSealNumber(string SealNumberInput)
        {
            if (SealNumberInput.Trim().Length <= 30)
            {
                SealNumber = SealNumberInput.Trim();

                ScreenMap.Add("SealNumber", SealNumber);
            }
            else
            {
                throw new RFS_Exception("InputLength");
            }
        }

        public void LoadOriginalShipmentWeight()
        {
            if (JDEShipmentRoutingStepsTable.GetRouteForShipmentNumber(ShipmentNumber))
            {
                DefaultShipmentWeight = JDEShipmentRoutingStepsTable.RSWGTS;
                DefaultShipmentUnitOfMeasure = JDEShipmentRoutingStepsTable.RSWTUM;
                ShipmentWeight = JDEShipmentRoutingStepsTable.RSWGTS;
                ShipmentUnitOfMeasure = JDEShipmentRoutingStepsTable.RSWTUM;

                ScreenMap.Add("ShipmentWeight", ShipmentWeight.ToString());
                ScreenMap.Add("ShipmentUnitOfMeasure", ShipmentUnitOfMeasure);
            }
            else
            {
                DefaultShipmentWeight = 0;
                DefaultShipmentUnitOfMeasure = "";
                ShipmentWeight = 0;
                ShipmentUnitOfMeasure = "";

                ScreenMap.Add("ShipmentWeight", "");
                ScreenMap.Add("ShipmentUnitOfMeasure", "");
            }
        }

        #endregion

        #region Shipment Confirmation

        public bool ValidateYesNoConfirmation(string YesNoConfirmationInput)
        {
            return YesNoConfirmationInput.GetYesNo();
        }

        #endregion

        #region Shipment Weight

        public void ValidateShipmentWeight(string ShipmentWeightInput)
        {
            double ParsedShipmentWeight;

            if (double.TryParse(ShipmentWeightInput, out ParsedShipmentWeight))
            {
                IntermediateShipmentWeight = ParsedShipmentWeight;
            }
            else
            {
                throw new RFS_Exception("NotANumber");
            }
        }

        #endregion

        #region Shipment Unit Of Measure

        public string LoadShipmentUnitOfMeasure()
        {
            return DefaultShipmentUnitOfMeasure;
        }

        public void ValidateShipmentUnitOfMeasure(string ShipmentUnitOfMeasureInput)
        {
            if (JDEUserDefinedCodeValuesTable.ValidateUDC("00  ", "UM", ShipmentUnitOfMeasureInput))
            {
                ShipmentWeight = IntermediateShipmentWeight;
                ShipmentUnitOfMeasure = ShipmentUnitOfMeasureInput;

                ScreenMap.Add("ShipmentWeight", ShipmentWeight.ToString());
                ScreenMap.Add("ShipmentUnitOfMeasure", ShipmentUnitOfMeasure);
            }
            else
            {
                throw new RFS_Exception("InvalidUOM");
            }
        }

        public void LoadShipmentUnitOfMeasureList(GenericList ShipmentWeightUnitOfMeasureList)
        {
            ShipmentWeightUnitOfMeasureList.Table = JDEUserDefinedCodeValuesTable;
            JDEUserDefinedCodeValuesTable.FetchTypeOfHours("00  ", "UM");
        }

        #endregion

        #region Number Of Labels

        public void ValidateNumberOfLabels(string NumberOfLabelsInput)
        {
            PrinterValidator._MaxLabels = MaxLabels;
            PrinterValidator.ValidateNumberOfLabels(NumberOfLabelsInput);

            NumberOfLabels = PrinterValidator._NumberOfLabels;

            ScreenMap.Add("NumberOfLabels", NumberOfLabels.ToString());
        }

        #endregion

        #region Printer

        public string LoadDefaultPrinter()
        {
            JDEShipmentHeaderTable.ValidateShipment(BranchPlant, ShipmentNumber); //have to requery to get back the AN8 and SHAN

            StoneridgePrinting.ResetAll();
            StoneridgePrinting.Branch = BranchPlant.Trim();
            StoneridgePrinting.ContainerType = "Q";
            StoneridgePrinting.Customer = JDEShipmentHeaderTable.XHAN8.ToString();
            StoneridgePrinting.Item = JDEShipmentHeaderTable.XHSHAN.ToString();

            return StoneridgePrinting.DefaultPrinter.Trim();
        }

        public void ValidatePrinter(string PrinterInput)
        {
            if (StoneridgePrinting.ValidatePrinter(PrinterInput))
            {
                //Printer Name and Address are stored in "StoneridgePrinting"
                ScreenMap.Add("PrinterName", StoneridgePrinting.PrinterName.Trim());
            }
            else
            {
                throw new RFS_Exception(StoneridgePrinting.ErrorMessageVocabulary);
            }
        }

        public void LoadPrinterList(GenericList PrinterList)
        {
            //First hooks up the screens to the proper TableObject, and then query for them (note that all PrintXref
            //parameters have already been setup in the default printer func)
            PrinterValidator.PrinterListLoad(PrinterList);
            StoneridgePrinting.RetrievePrinters();
        }

        #endregion

        #endregion

        #region Helper Methods

        #region ContainsNoHolds

        private bool ContainsNoHolds(double ParsedShipmentNumber)
        {
            JDESalesOrderHeaderFileTable.CheckForHoldStatus(BranchPlant, ParsedShipmentNumber);
            foreach (DataRow dr in JDESalesOrderHeaderFileTable.Rows)
            {
                JDESalesOrderHeaderFileTable.CurrentRow = dr;
                foreach (string HoldStatus in ShipmentHoldStatuses)
                {
                    Log.Debug($"<***>Holdstatus {HoldStatus}");
                    if (JDESalesOrderHeaderFileTable.SHHOLD == HoldStatus && HoldStatus != "")
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Quick Return Label Is Required

        public bool QuickReturnLabelIsRequired()
        {
            if (JDESalesOrderDetailFileTable.IsQuickLabelRequired(ShipmentNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Send Trailer Number Update To ERP

        public bool SendTrailerNumberUpdateToERP()
        {
            if (!string.IsNullOrWhiteSpace(TrailerNumber))
            {
                if (TrailerNumber.Trim().ToUpper() != DefaultTrailerNumber.Trim().ToUpper())
                {
                    Log.Debug("*** Trailer Number is Populated: " + TrailerNumber);
                    Log.Debug("*** Updating Reference 'TN'");

                    Transactions.Add(TrailerNumberUpdate_Tx);
                    Transactions.Branch = BranchPlant.Trim();
                    TrailerNumberUpdate_Tx.Initialize();

                    TrailerNumberUpdate_Tx.ShipmentNumber = ShipmentNumber.ToString();
                    TrailerNumberUpdate_Tx.ReferenceNumber = TrailerNumber;
                    TrailerNumberUpdate_Tx.RefNumberQualifier = "TN";
                    TrailerNumberUpdate_Tx.P49645Version = P49645Version;
                    TrailerNumberUpdate_Tx.RFSUserId = UserSession.CurrentUser.LoginName;
                    TrailerNumberUpdate_Tx.TransactionDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                    TrailerNumberUpdate_Tx.Mode = "R";

                    Transactions.Send(SyncTransaction ? "Y" : "N");
                    Workflow.DisplayMessages();

                    return !Transactions.Error;
                }
                else
                {
                    Log.Debug("*** Trailer Number Was Not Changed from Default: No Need to Update Reference");
                    return true;
                }
            }
            else
            {
                Log.Debug("*** Trailer Number is Blank: No Need to Update Reference");
                return true;
            }
        }

        #endregion

        #region Send PRO Number Update To ERP

        public bool SendPRONumberUpdateToERP()
        {
            if (!string.IsNullOrWhiteSpace(PRONumber))
            {
                if (PRONumber.Trim().ToUpper() != DefaultPRONumber.Trim().ToUpper())
                {
                    Log.Debug("*** PRO Number is Populated: " + PRONumber);
                    Log.Debug("*** Updating Reference 'CN'");

                    Transactions.Add(PRONumberUpdate_Tx);
                    Transactions.Branch = BranchPlant.Trim();
                    PRONumberUpdate_Tx.Initialize();

                    PRONumberUpdate_Tx.ShipmentNumber = ShipmentNumber.ToString();
                    PRONumberUpdate_Tx.ReferenceNumber = PRONumber;
                    PRONumberUpdate_Tx.RefNumberQualifier = "CN";
                    PRONumberUpdate_Tx.P49645Version = P49645Version;
                    PRONumberUpdate_Tx.RFSUserId = UserSession.CurrentUser.LoginName;
                    PRONumberUpdate_Tx.TransactionDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                    PRONumberUpdate_Tx.Mode = "R";

                    Transactions.Send(SyncTransaction ? "Y" : "N");
                    Workflow.DisplayMessages();

                    return !Transactions.Error;
                }
                else
                {
                    Log.Debug("*** PRO Number Was Not Changed from Default: No Need to Update Reference");
                    return true;
                }
            }
            else
            {
                Log.Debug("*** PRO Number is Blank: No Need to Update Reference");
                return true;
            }
        }

        #endregion

        #region Send Seal Number Update To ERP

        public bool SendSealNumberUpdateToERP()
        {
            Log.Debug("*** Seal Number is Populated: " + SealNumber);
            Log.Debug("*** Updating Reference 'SN'");

            if (!string.IsNullOrWhiteSpace(SealNumber))
            {
                if (SealNumber.Trim().ToUpper() != DefaultSealNumber.Trim().ToUpper())
                {
                    Transactions.Add(SealNumberUpdate_Tx);
                    Transactions.Branch = BranchPlant.Trim();
                    SealNumberUpdate_Tx.Initialize();

                    SealNumberUpdate_Tx.ShipmentNumber = ShipmentNumber.ToString();
                    SealNumberUpdate_Tx.ReferenceNumber = SealNumber;
                    SealNumberUpdate_Tx.RefNumberQualifier = "SN";
                    SealNumberUpdate_Tx.P49645Version = P49645Version;
                    SealNumberUpdate_Tx.RFSUserId = UserSession.CurrentUser.LoginName;
                    SealNumberUpdate_Tx.TransactionDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
                    SealNumberUpdate_Tx.Mode = "R";

                    Transactions.Send(SyncTransaction ? "Y" : "N");
                    Workflow.DisplayMessages();

                    return !Transactions.Error;
                }
                else
                {
                    Log.Debug("*** Seal Number Was Not Changed from Default: No Need to Update Reference");
                    return true;
                }
            }
            else
            {
                Log.Debug("*** Seal Number is Blank: No Need to Update Reference");
                return true;
            }
        }

        #endregion

        #region Send Ship Confirmation To ERP

        public bool SendShipConfirmationToERP()
        {
            Transactions.Add(ShipConfirmation_Tx);
            Transactions.Branch = BranchPlant.Trim();
            ShipConfirmation_Tx.Initialize();

            ShipConfirmation_Tx.ShipmentNumber = ShipmentNumber.ToString();
            if ((ShipmentWeight != DefaultShipmentWeight) || (ShipmentUnitOfMeasure.Trim().ToUpper() != DefaultShipmentUnitOfMeasure.Trim().ToUpper()))
            {
                ShipConfirmation_Tx.OverrideWeight = ShipmentWeight.ToString();
                ShipConfirmation_Tx.WeightUnitOfMeasure = ShipmentUnitOfMeasure;
            }
            else
            {
                Log.Debug("*** Shipment Weight and UOM was not Changed from Default (" + ShipmentWeight + " " + ShipmentUnitOfMeasure + " == " + DefaultShipmentWeight + " " + DefaultShipmentUnitOfMeasure + "): No Need to Update");
            }

            ShipConfirmation_Tx.P49645Version = P49645Version;
            ShipConfirmation_Tx.RFSUserId = UserSession.CurrentUser.LoginName;
            ShipConfirmation_Tx.TransactionDate_YYYYMMDD = DateTime.Now.ToString("yyyyMMdd");
            ShipConfirmation_Tx.Mode = "1";

            Transactions.Send(SyncTransaction ? "Y" : "N");
            Workflow.DisplayMessages();

            return !Transactions.Error;
        }

        #endregion

        #region Print Barcode Labels

        public void PrintBarcodeLabels()
        {
            Workflow.PrintSystem.Reset();
            Workflow.PrintSystem.Branch = StoneridgePrinting.Branch;
            Workflow.PrintSystem.ContainerType = StoneridgePrinting.ContainerType;
            Workflow.PrintSystem.Customer = StoneridgePrinting.Customer;
            Workflow.PrintSystem.ItemNumber = StoneridgePrinting.Item;

            Workflow.PrintSystem.PrinterName = StoneridgePrinting.PrinterName;
            Workflow.PrintSystem.PrinterAddress = StoneridgePrinting.PrinterAddress;
            Workflow.PrintSystem.DropFilePathOverride = DropFilePath;
            Workflow.PrintSystem.Copies = NumberOfLabels;

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