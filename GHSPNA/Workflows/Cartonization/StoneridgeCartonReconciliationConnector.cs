using System;
using ICS.Base.Wireless.JDE.BusinessConnector;
using ICS.Base.Wireless.JDE.BusinessConnector.Validators;
using ICS.Utilities;
using ICS.Workflow.Base.WorkflowDesign.Components;
using ICS.Database.TableObjects.JDE;
using ICS.ERPI.JDE;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Cartonization
{
    public class StoneridgeCartonReconciliationConnector : BaseConnector
    {
        #region Class Data

        #region Collected Data

        public virtual string BranchPlant { get; protected set; }

        public virtual string BranchDescription { get; protected set; }

        public virtual double ShipmentNumber { get; protected set; }

        public virtual double CustomerSoldTo { get; protected set; }

        public virtual string CustomerSoldToDescription { get; protected set; }

        public virtual double CustomerShipTo { get; protected set; }

        public virtual string CustomerShipToDescription { get; protected set; }

        public virtual string OrderCompany { get; protected set; }

        #endregion

        #region ERPI

        ICS.ERPI.JDE.BulkPick _bulkPickTx = new BulkPick();

        #endregion

        #region Tables

        #region Address Book (F0101) JDE

        // Private Table Object
        private F0101 _tableJDEAddressBook;

        // Public Table Object (Generate New If NULL)
        public F0101 JDEAddressBook
        {
            get { return _tableJDEAddressBook ?? (_tableJDEAddressBook = new F0101(DataAccess, "")); }
        }

        #endregion

        #region User Defined Code Values (F0005) JDE

        // Private Table Object
        private F0005 _tableJDEUserDefinedCodeValues;

        // Public Table Object (Generate New If NULL)
        public F0005 JDEUserDefinedCodeValuesTable
        {
            get { return _tableJDEUserDefinedCodeValues ?? (_tableJDEUserDefinedCodeValues = new F0005(DataAccess, "")); }
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

        #region Inventory Constants (F41001) JDE

        // Private Table Object
        private F41001 _tableJDEInventoryConstants;

        // Public Table Object (Generate New If NULL)
        public F41001 JDEInventoryConstantsTable
        {
            get { return _tableJDEInventoryConstants ?? (_tableJDEInventoryConstants = new F41001(DataAccess, "")); }
        }

        #endregion

        #region Item Units Of Measure Conversion Factors (F41002) JDE

        // Private Table Object
        private F41002 _tableJDEItemUnitsOfMeasureConversionFactors;

        // Public Table Object (Generate New If NULL)
        public F41002 JDEItemUnitsOfMeasureConversionFactorsTable
        {
            get { return _tableJDEItemUnitsOfMeasureConversionFactors ?? (_tableJDEItemUnitsOfMeasureConversionFactors = new F41002(DataAccess, "")); }
        }

        #endregion

        #region Unit Of Measure Standard Conversion (F41003) JDE

        // Private Table Object
        private F41003 _tableJDEUnitOfMeasureStandardConversion;

        // Public Table Object (Generate New If NULL)
        public F41003 JDEUnitOfMeasureStandardConversionTable
        {
            get { return _tableJDEUnitOfMeasureStandardConversion ?? (_tableJDEUnitOfMeasureStandardConversion = new F41003(DataAccess, "")); }
        }

        #endregion

        #region Item Master (F4101) JDE

        // Private Table Object
        private F4101 _tableJDEItemMaster;

        // Public Table Object (Generate New If NULL)
        public F4101 JDEItemMasterTable
        {
            get { return _tableJDEItemMaster ?? (_tableJDEItemMaster = new F4101(DataAccess, "")); }
        }

        #endregion

        #region Item Branch File (F4102) JDE

        // Private Table Object
        private F4102 _tableJDEItemBranchFile;

        // Public Table Object (Generate New If NULL)
        public F4102 JDEItemBranchFileTable
        {
            get { return _tableJDEItemBranchFile ?? (_tableJDEItemBranchFile = new F4102(DataAccess, "")); }
        }

        #endregion

        #region Item Location File (F41021) JDE

        // Private Table Object
        private F41021 _tableJDEItemLocationFile;

        // Public Table Object (Generate New If NULL)
        public F41021 JDEItemLocationFileTable
        {
            get { return _tableJDEItemLocationFile ?? (_tableJDEItemLocationFile = new F41021(DataAccess, "")); }
        }

        #endregion

        #region Lot Master (F4108) JDE

        // Private Table Object
        private F4108 _tableJDELotMaster;

        // Public Table Object (Generate New If NULL)
        public F4108 JDELotMasterTable
        {
            get { return _tableJDELotMaster ?? (_tableJDELotMaster = new F4108(DataAccess, "")); }
        }

        #endregion

        #region Sales Order Detail (F4211) JDE

        // Private Table Object
        private F4211 _tableJDESalesOrderDetail;

        // Public Table Object (Generate New If NULL)
        public F4211 JDESalesOrderDetail
        {
            get { return _tableJDESalesOrderDetail ?? (_tableJDESalesOrderDetail = new F4211(DataAccess, "")); }
        }

        #endregion

        #endregion

        #region Workflow Options

        #region Email Error  (UNUSED, also unused in base workflow)

        // Private Variable
        private string _wfoEmailError;

        // Public Variable (Generate New If NULL)
        public string EmailError
        {
            get { return _wfoEmailError ?? (_wfoEmailError = WFOptions.RetrieveOptionValue("EmailError")); }
        }

        #endregion

        #region Include Status

        // Private Variable
        private string[] _wfIncludeStatus;

        // Public Variable (Generate New If NULL)
        public string[] IncludeStatus
        {
            get { return _wfIncludeStatus ?? (_wfIncludeStatus = WFOptions.RetrieveOptionValue("IncludeStatus").Split(' ', ',', ';')); }
        }

        #endregion

        #region P4205Version-Bulk

        // Private Variable
        private string _wfoP4205VersionBulk;

        // Public Variable (Generate New If NULL)
        public string P4205Version
        {
            get { return _wfoP4205VersionBulk ?? (_wfoP4205VersionBulk = WFOptions.RetrieveOptionValue("P4205Version-Bulk")); }
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

        #region Unique Doc Number (UNUSED - For bulk pick by SO doco)

        // Private Variable
        private bool? _wfoUniqueDocNumber;

        // Public Variable (Generate New If NULL)
        public bool UniqueDocNumber
        {
            get { return (bool) (_wfoUniqueDocNumber ?? (_wfoUniqueDocNumber = WFOptions.GetYesNoWFOptionValue("UniqueDocNumber", false))); }
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

        #endregion

        #endregion

        #region Constructors and Overrides

        public StoneridgeCartonReconciliationConnector(RFSWorkflowDesigner workflow) : base(workflow)
        {
        }

        public StoneridgeCartonReconciliationConnector() : base()
        {
        }

        #endregion

        #region Input Validation

        #region Branch Plant

        public bool ValidateBranchPlant(string BranchPlantInput)
        {
            if (IncludeStatus.Length > 1)
            {
                throw new RFS_Exception("OnlyAllow1Status");
            }

            BranchValidator.SalesOrderBranchValidate(BranchPlantInput, IncludeStatus, null, "0");

            BranchPlant = BranchValidator.BranchPlant;
            BranchDescription = BranchValidator.BranchDescription;

            ScreenMap.Add("BranchPlant", BranchPlant);
            ScreenMap.Add("BranchDescription", BranchDescription);
            return true;
        }

        public void LoadBranchPlantList(GenericList BranchPlantList)
        {
            BranchValidator.SalesOrderBranchListLoad(BranchPlantList, IncludeStatus, null, "0");
        }

        #endregion

        #region Shipment

        public bool ValidateShipment(string ShipmentInput)
        {
            if (!JDESalesOrderDetail.ValidShipment(BranchPlant, ShipmentInput, IncludeStatus, null, UserSession.UserId, "0"))
            {
                if (!JDESalesOrderDetail.ValidShipment(BranchPlant, ShipmentInput, null, null, UserSession.UserId, "0")) throw new RFS_Exception("INVALIDShipment");
                else throw new RFS_Exception("NoOpenLines");
            }

            ShipmentNumber = JDESalesOrderDetail.SDSHPN;
            OrderCompany = JDESalesOrderDetail.SDKCOO;

            CustomerSoldTo = JDESalesOrderDetail.SDAN8;
            JDEAddressBook.RetrieveAddress(CustomerSoldTo.ToString());
            CustomerSoldToDescription = JDEAddressBook.ABALPH;

            CustomerShipTo = JDESalesOrderDetail.SDSHAN;
            JDEAddressBook.RetrieveAddress(CustomerShipTo.ToString());
            CustomerShipToDescription = JDEAddressBook.ABALPH;

            return true;
        }

        public void LoadShipmentList(GenericList ShipmentList)
        {
            JDESalesOrderDetail.FetchShipments(BranchPlant, IncludeStatus, null, UserSession.UserId, "0");
            ShipmentList.Table = JDESalesOrderDetail;
        }

        #endregion

        #endregion

        #region Helper Methods

        #region Send To ERP

        public override bool SendToERP()
        {
            Transactions.Add(_bulkPickTx);
            Transactions.Branch = BranchPlant.Trim().ToUpperInvariant();

            _bulkPickTx.Initialize();

            _bulkPickTx.CustSoldTo = CustomerSoldTo.ToString();
            _bulkPickTx.CustSoldToDescr = CustomerSoldToDescription.Trim();
            _bulkPickTx.CustShipTo = CustomerShipTo.ToString();
            _bulkPickTx.CustShipToDescr = CustomerShipToDescription.Trim();
            _bulkPickTx.BranchPlant = BranchPlant;

            _bulkPickTx.OrderNumber = String.Empty;
            _bulkPickTx.OrderType = String.Empty;
            _bulkPickTx.PickSlipNumber = String.Empty;

            _bulkPickTx.ShipmentNumber = ShipmentNumber.ToString();
            _bulkPickTx.OrderCompany = OrderCompany.Trim();
            _bulkPickTx.RFUserID = UserSession.CurrentUser.LoginName;
            _bulkPickTx.TransDate_YYYYMMDD = System.DateTime.Now.ToString("yyyyMMdd");
            ;
            _bulkPickTx.Version = P4205Version.Trim();

            _bulkPickTx.StatusCodeNext = IncludeStatus[0].ToString().Trim();

            _bulkPickTx.UserInput01 = "1";
            _bulkPickTx.UserInput02 = string.Empty;
            _bulkPickTx.UserInput03 = string.Empty;
            _bulkPickTx.UserInput04 = string.Empty;
            _bulkPickTx.UserInput05 = string.Empty;
            _bulkPickTx.UserInput06 = string.Empty;
            _bulkPickTx.UserInput07 = string.Empty;

            Transactions.Send(SyncTransaction ? "Y" : "N");
            Workflow.DisplayMessages();

            return !Transactions.Error;
        }

        #endregion

        #endregion
    }
}