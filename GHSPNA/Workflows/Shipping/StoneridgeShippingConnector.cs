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
using CustomJDETables = ICS.Stoneridge.Wireless.JDE.Database.TableObjects;
using ICS.Database.TableObjects.JDE;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Shipping
{
    public class StoneridgeShippingConnector : BaseConnector
    {
        #region Class data

        #region Collected data

        public virtual string BranchPlant { get; protected set; }

        public virtual string BranchDescription { get; protected set; }

        public virtual double ShipmentNo { get; protected set; }

        public virtual double ShortItemNo { get; protected set; }

        public virtual string LongItemNo { get; protected set; }

        public virtual string ThirdItemNo { get; protected set; }

        public virtual string PrimaryItemNo { get; protected set; }

        public virtual string ItemDescription1 { get; protected set; }

        public virtual string ItemDescription2 { get; protected set; }

        public virtual string LotNo { get; protected set; }

        public virtual double Quantity { get; protected set; }

        #endregion

        #region ERPI

        #endregion

        #region Tables

        #region Sales Order Detail File (F4211) JDE

        // Private Table Object
        private CustomJDETables.F4211 _tableJDESalesOrderDetailFile;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F4211 JDESalesOrderDetailFileTable
        {
            get { return _tableJDESalesOrderDetailFile ?? (_tableJDESalesOrderDetailFile = new CustomJDETables.F4211(DataAccess, "")); }
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

        #region Item Location File (F41021) JDE

        // Private Table Object
        private CustomJDETables.F41021 _tableJDEItemLocationFile;

        // Public Table Object (Generate New If NULL)
        public CustomJDETables.F41021 JDEItemLocationFileTable
        {
            get { return _tableJDEItemLocationFile ?? (_tableJDEItemLocationFile = new CustomJDETables.F41021(DataAccess, "")); }
        }

        #endregion

        #endregion

        #region Workflow options

        #region Default Staging Location

        // Private WFO variable
        private string _wfoDefaultStagingLocation;

        // Public WFO variable (Generate New If NULL)
        public string DefaultStagingLocation
        {
            get { return _wfoDefaultStagingLocation ?? (_wfoDefaultStagingLocation = WFOptions.RetrieveOptionValue("DefaultStagingLocation")); }
        }

        #endregion

        #region Default UOM

        // Private WFO variable
        private string _wfoDefaultUOM;

        // Public WFO variable (Generate New If NULL)
        public string DefaultUOM
        {
            get { return _wfoDefaultUOM ?? (_wfoDefaultUOM = WFOptions.RetrieveOptionValue("DefaultUOM")); }
        }

        #endregion

        #region Default Labels

        // Private WFO variable
        private int? _wfoDefaultLabels;

        // Public WFO variable (Generate New If NULL)
        public int DefaultLabels
        {
            get { return (int) (_wfoDefaultLabels ?? (_wfoDefaultLabels = WFOptions.GetInt32WFOptionValue("DefaultNumberLabels"))); }
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

        #region Allowed Sales Order Statuses

        // Private WFO variable
        private string[] _wfoAllowedSalesOrderStatuses;

        // Public WFO variable (Generate New If NULL)
        public string[] AllowedSalesOrderStatuses
        {
            get { return _wfoAllowedSalesOrderStatuses ?? (_wfoAllowedSalesOrderStatuses = WFOptions.RetrieveOptionValue("OpenOrderStatus").Split(' ')); }
        }

        #endregion

        #endregion

        #region Base Validation

        #region Base Branch Validator

        private BranchValidator validatorBranch;

        public BranchValidator _BranchValidator
        {
            get { return validatorBranch ?? (validatorBranch = new BranchValidator(Workflow)); }
        }

        #endregion

        #region Base Printer Validator

        private PrintValidator validatorPrinter;

        public PrintValidator _PrinterValidator
        {
            get { return validatorPrinter ?? (validatorPrinter = new PrintValidator(Workflow)); }
        }

        #endregion

        #region Base Item Validator

        private ItemValidator validatorItem;

        public ItemValidator _ItemValidator
        {
            get { return validatorItem ?? (validatorItem = new ItemValidator(Workflow)); }
        }

        #endregion

        #endregion

        #endregion

        #region Constructors and overrides

        public StoneridgeShippingConnector(RFSWorkflowDesigner workflow) :
            base(workflow)
        {
        }

        public StoneridgeShippingConnector() : base()
        {
        }

        #endregion

        #region Input validation

        #region Branch

        public void ValidateBranch(string Branch)
        {
            _BranchValidator.BranchValidate(Branch);

            BranchPlant = _BranchValidator.BranchPlant;
            BranchDescription = _BranchValidator.BranchDescription;

            ScreenMap.Add("BranchPlant", BranchPlant);
            ScreenMap.Add("BranchDescription", BranchDescription);
        }

        public void LoadBranchList(GenericList ScreenList)
        {
            _BranchValidator.BranchListLoad(ScreenList);
        }

        #endregion

        #region Shipment Number

        public void ValidateShipmentNumber(string ShipmentNoInput)
        {
            if (JDESalesOrderDetailFileTable.OrderReadyToShip(BranchPlant, ShipmentNoInput.GetDouble(), AllowedSalesOrderStatuses))
            {
                ShipmentNo = JDESalesOrderDetailFileTable.SDSHPN;
                ScreenMap.Add("ShipmentNo", ShipmentNo.ToString());
            }
            else
            {
                throw new RFS_Exception("INVALIDSHIPMENTNUMBER");
            }
        }

        public void LoadShipmentList()
        {
            JDESalesOrderDetailFileTable.OrderReadyToShip(BranchPlant, -1, AllowedSalesOrderStatuses);
        }

        #endregion

        #region Item Number

        public void ValidateItem(string ItemNumber)
        {
            _ItemValidator.ValidateItemNumber(BranchPlant, ItemNumber);
            //if (JDESalesOrderDetailFileTable.sho)
        }

        #endregion

        #region Collect Serial Number To Add

        public void ValidateSerialNumberToAdd(string SerialNumber)
        {
            if (JDEItemLocationFileTable.FetchSerialNumber(SerialNumber))
            {
                if (JDESalesOrderDetailFileTable.ItemOnOrder(BranchPlant, ShipmentNo, JDEItemLocationFileTable.LIITM, new string[] { }))
                {
                    if (JDESalesOrderDetailFileTable.ItemOnOrder(BranchPlant, ShipmentNo, JDEItemLocationFileTable.LIITM, AllowedSalesOrderStatuses))
                    {
                        //Fetch to a custom table!!!
                    }
                    else
                    {
                        throw new RFS_Exception("ALLLINESCLOSEDFORITEM");
                    }
                }
                else
                {
                    throw new RFS_Exception("ITEMNOTONSHIPMENT");
                }
            }
            else
            {
                throw new RFS_Exception("INVALIDSERIALNUMBER");
            }
        }

        #endregion

        #endregion
    }
}