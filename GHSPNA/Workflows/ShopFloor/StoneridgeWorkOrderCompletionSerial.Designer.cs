using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.ShopFloor
{
    public partial class StoneridgeWorkOrderCompletionSerial : ICS.Workflow.Base.WorkflowDesign.Components.RFSWorkflowDesigner
    {
        #region Screens

        ICS.Workflow.Base.JDE.Components.BranchPlantScreen _CollectBranch;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLocation;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLot;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLotSerialExpDate;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectGrade;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPotency;

        ICS.Workflow.Base.JDE.Components.QuantityScreen _CollectQuantity;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectQuantityScrapped;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectScrapUnitOfMeasure;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectSerialNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectShiftCode;

        ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen _CollectUnitOfMeasure;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectWorkOrderNumber;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabels;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _PrintBarCodeLabelsStartOfOrder;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinter;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectPrinterStartOfOrder;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _VerifyWorkOrderInformation;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _DisplayToleranceWarning;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _CollectLicensePlate;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen _ConfirmNewLicensePlate;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen CollectContainerType;

        // list/lookup screens
        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _BranchPlantList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UserDefinedCodesList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LocationMasterList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _WorkOrderMasterList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _EmptyLocationsList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _UnitOfMeasureConversionList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _DefinedPrintersTableList;

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _ItemBalanceList; // item balance list

        ICS.Workflow.Base.WorkflowDesign.Components.GenericList _LicensePlateMasterList;

        protected ICS.Workflow.Base.JDE.Components.QuantityScreen _CollectDualQuantity;

        protected ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen _CollectDualUnitOfMeasure;

        protected ICS.Workflow.Base.JDE.Components.ItemScreen _CollectItemNumber;

        GenericScreen CollectIssueItemNumber;

        GenericList WorkOrderPartsList;

        #endregion

        private void InitializeComponent()
        {
			this._CollectGrade = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._UserDefinedCodesList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectShiftCode = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._CollectWorkOrderNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._WorkOrderMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._VerifyWorkOrderInformation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._PrintBarCodeLabelsStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._CollectUnitOfMeasure = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
			this._CollectBranch = new ICS.Workflow.Base.JDE.Components.BranchPlantScreen(this);
			this._BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this.CollectEmployee = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.EmployeeList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectItemNumber = new ICS.Workflow.Base.JDE.Components.ItemScreen(this);
			this._UnitOfMeasureConversionList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectDualUnitOfMeasure = new ICS.Workflow.Base.JDE.Components.UnitOfMeasureScreen(this);
			this._CollectDualQuantity = new ICS.Workflow.Base.JDE.Components.QuantityScreen(this);
			this._CollectQuantityScrapped = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._CollectQuantity = new ICS.Workflow.Base.JDE.Components.QuantityScreen(this);
			this._CollectScrapUnitOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.CollectContainerType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._CollectLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._ItemBalanceList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectLot = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._LocationMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._DefinedPrintersTableList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectPrinterStartOfOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._EmptyLocationsList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectPotency = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._DisplayToleranceWarning = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._CollectLotSerialExpDate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._LicensePlateMasterList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this._CollectLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._CollectSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._PrintBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this._ConfirmNewLicensePlate = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.CollectIssueItemNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.CollectIssueLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.WorkOrderPartsList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
			this.CollectIssueLotNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.CollectIssueQuantity = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.CollectIssueUnitOfMeasure = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			this.CollectIssueSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
			// 
			// _CollectGrade
			// 
			this._CollectGrade.ListScreen = this._UserDefinedCodesList;
			this._CollectGrade.ListScreenControl = this._UserDefinedCodesList;
			this._CollectGrade.ScreenName = "CollectGrade";
			this._CollectGrade.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectGrade_EnterEvent);
			this._CollectGrade.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectGrade_BackEvent);
			this._CollectGrade.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectGrade_ListEvent);
			// 
			// _UserDefinedCodesList
			// 
			this._UserDefinedCodesList.ListScreenParent = this._CollectShiftCode;
			this._UserDefinedCodesList.ParentScreen = this._CollectShiftCode;
			this._UserDefinedCodesList.ScreenName = "F0005_List";
			this._UserDefinedCodesList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UserDefinedCodesList_EnterEvent);
			this._UserDefinedCodesList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UserDefinedCodesList_LoadEvent);
			// 
			// _CollectShiftCode
			// 
			this._CollectShiftCode.ListScreen = this._UserDefinedCodesList;
			this._CollectShiftCode.ListScreenControl = this._UserDefinedCodesList;
			this._CollectShiftCode.NextScreen = this._CollectWorkOrderNumber;
			this._CollectShiftCode.ScreenName = "CollectShiftCode";
			this._CollectShiftCode.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShiftCode_EnterEvent);
			this._CollectShiftCode.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShiftCode_ListEvent);
			// 
			// _CollectWorkOrderNumber
			// 
			this._CollectWorkOrderNumber.ListScreen = this._WorkOrderMasterList;
			this._CollectWorkOrderNumber.ListScreenControl = this._WorkOrderMasterList;
			this._CollectWorkOrderNumber.NextScreen = this._VerifyWorkOrderInformation;
			this._CollectWorkOrderNumber.ScreenName = "CollectWorkOrderNumber";
			this._CollectWorkOrderNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectWorkOrderNumber_EnterEvent);
			// 
			// _WorkOrderMasterList
			// 
			this._WorkOrderMasterList.ListScreenParent = this._CollectWorkOrderNumber;
			this._WorkOrderMasterList.ParentScreen = this._CollectWorkOrderNumber;
			this._WorkOrderMasterList.ScreenName = "F4801_List";
			this._WorkOrderMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.WorkOrderMasterList_EnterEvent);
			this._WorkOrderMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._WorkOrderMasterList_LoadEvent);
			// 
			// _VerifyWorkOrderInformation
			// 
			this._VerifyWorkOrderInformation.NextScreen = this._PrintBarCodeLabelsStartOfOrder;
			this._VerifyWorkOrderInformation.ScreenName = "VerifyWorkOrderInformation";
			this._VerifyWorkOrderInformation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.VerifyWorkOrderInformation_EnterEvent);
			// 
			// _PrintBarCodeLabelsStartOfOrder
			// 
			this._PrintBarCodeLabelsStartOfOrder.ScreenName = "PrintBarCodeLabelsStartOfOrder";
			this._PrintBarCodeLabelsStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabelsStartOfOrder_EnterEvent);
			// 
			// _CollectUnitOfMeasure
			// 
			this._CollectUnitOfMeasure.BranchScreen = this._CollectBranch;
			this._CollectUnitOfMeasure.DisableToleranceCheck = true;
			this._CollectUnitOfMeasure.DualQtyScreen = null;
			this._CollectUnitOfMeasure.ItemScreen = this._CollectItemNumber;
			this._CollectUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
			this._CollectUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
			this._CollectUnitOfMeasure.NextScreen = this._CollectDualQuantity;
			this._CollectUnitOfMeasure.OriginalQtyScreen = this._CollectQuantity;
			this._CollectUnitOfMeasure.OriginalUOMScreen = null;
			this._CollectUnitOfMeasure.ScreenName = "CollectUnitofMeasure";
			this._CollectUnitOfMeasure.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUnitOfMeasure_EnterEvent);
			this._CollectUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectUnitOfMeasure_ListEvent);
			this._CollectUnitOfMeasure.UOMConversionsList.ScreenName = "F41002_List";
			// 
			// _CollectBranch
			// 
			this._CollectBranch.ListScreen = this._BranchPlantList;
			this._CollectBranch.ListScreenControl = this._BranchPlantList;
			this._CollectBranch.NextScreen = this.CollectEmployee;
			this._CollectBranch.ScreenName = "CollectBranch";
			this._CollectBranch.WarehouseConstantsTable = null;
			this._CollectBranch.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_EnterEvent);
			this._CollectBranch.BranchPlantsList.BackScreen = this._CollectBranch;
			this._CollectBranch.BranchPlantsList.ScreenName = "F0006_List";
			// 
			// _BranchPlantList
			// 
			this._BranchPlantList.ListScreenParent = this._CollectBranch;
			this._BranchPlantList.ParentScreen = this._CollectBranch;
			this._BranchPlantList.ScreenName = "F0006_List";
			this._BranchPlantList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.BranchPlantList_EnterEvent);
			this._BranchPlantList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._BranchPlantList_LoadEvent);
			// 
			// CollectEmployee
			// 
			this.CollectEmployee.ListScreen = this.EmployeeList;
			this.CollectEmployee.ListScreenControl = this.EmployeeList;
			this.CollectEmployee.NextScreen = this._CollectShiftCode;
			this.CollectEmployee.ScreenName = "CollectEmployee";
			this.CollectEmployee.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectEmployee_ValidateMethod);
			// 
			// EmployeeList
			// 
			this.EmployeeList.ListScreenParent = this.CollectEmployee;
			this.EmployeeList.ParentScreen = this.CollectEmployee;
			this.EmployeeList.ScreenName = "F0101_List";
			this.EmployeeList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.EmployeeList_LoadEvent);
			// 
			// _CollectItemNumber
			// 
			this._CollectItemNumber.BranchScreen = this._CollectBranch;
			this._CollectItemNumber.ItemBranchesTable = null;
			this._CollectItemNumber.ItemsTable = null;
			this._CollectItemNumber.NextScreen = this._VerifyWorkOrderInformation;
			this._CollectItemNumber.ScreenName = "CollectItemNumber";
			// 
			// _UnitOfMeasureConversionList
			// 
			this._UnitOfMeasureConversionList.ListScreenParent = this._CollectScrapUnitOfMeasure;
			this._UnitOfMeasureConversionList.ParentScreen = this._CollectScrapUnitOfMeasure;
			this._UnitOfMeasureConversionList.ScreenName = "F41002_List";
			this._UnitOfMeasureConversionList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.UnitOfMeasureList_EnterEvent);
			this._UnitOfMeasureConversionList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._UnitOfMeasureConversionList_LoadEvent);
			// 
			// _CollectDualUnitOfMeasure
			// 
			this._CollectDualUnitOfMeasure.BranchScreen = this._CollectBranch;
			this._CollectDualUnitOfMeasure.DualQtyScreen = this._CollectDualQuantity;
			this._CollectDualUnitOfMeasure.IsDualUnitOfMeasureScreen = true;
			this._CollectDualUnitOfMeasure.ItemScreen = this._CollectItemNumber;
			this._CollectDualUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
			this._CollectDualUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
			this._CollectDualUnitOfMeasure.NextScreen = this._CollectQuantityScrapped;
			this._CollectDualUnitOfMeasure.OriginalQtyScreen = this._CollectQuantity;
			this._CollectDualUnitOfMeasure.OriginalUOMScreen = this._CollectUnitOfMeasure;
			this._CollectDualUnitOfMeasure.ScreenMapKey = "DualUnitOfMeasure";
			this._CollectDualUnitOfMeasure.ScreenName = "CollectDualUnitofMeasure";
			this._CollectDualUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualUnitOfMeasure_ListEvent);
			this._CollectDualUnitOfMeasure.UOMConversionsList.ScreenName = "F41002_List";
			// 
			// _CollectDualQuantity
			// 
			this._CollectDualQuantity.IsDualQuantityScreen = true;
			this._CollectDualQuantity.ItemScreen = this._CollectItemNumber;
			this._CollectDualQuantity.NextScreen = this._CollectDualUnitOfMeasure;
			this._CollectDualQuantity.ScreenMapKey = "DualQuantity";
			this._CollectDualQuantity.ScreenName = "CollectDualQuantity";
			this._CollectDualQuantity.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectDualQuantity_LoadEvent);
			// 
			// _CollectQuantityScrapped
			// 
			this._CollectQuantityScrapped.ScreenName = "CollectQuantityScrapped";
			this._CollectQuantityScrapped.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantityScrapped_EnterEvent);
			// 
			// _CollectQuantity
			// 
			this._CollectQuantity.ItemScreen = this._CollectItemNumber;
			this._CollectQuantity.NextScreen = this._CollectUnitOfMeasure;
			this._CollectQuantity.ScreenName = "CollectQuantity";
			this._CollectQuantity.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantity_EnterEvent);
			this._CollectQuantity.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectQuantity_LoadEvent);
			// 
			// _CollectScrapUnitOfMeasure
			// 
			this._CollectScrapUnitOfMeasure.ListScreen = this._UnitOfMeasureConversionList;
			this._CollectScrapUnitOfMeasure.ListScreenControl = this._UnitOfMeasureConversionList;
			this._CollectScrapUnitOfMeasure.NextScreen = this.CollectContainerType;
			this._CollectScrapUnitOfMeasure.ScreenName = "CollectScrapUnitofMeasure";
			this._CollectScrapUnitOfMeasure.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectScrapUnitOfMeasure_EnterEvent);
			this._CollectScrapUnitOfMeasure.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectScrapUnitOfMeasure_ListEvent);
			// 
			// CollectContainerType
			// 
			this.CollectContainerType.NextScreen = this._CollectLocation;
			this.CollectContainerType.ScreenName = "CollectContainerType";
			this.CollectContainerType.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectContainerType_ValidateMethod);
			// 
			// _CollectLocation
			// 
			this._CollectLocation.ListScreen = this._ItemBalanceList;
			this._CollectLocation.ListScreenControl = this._ItemBalanceList;
			this._CollectLocation.ScreenName = "CollectLocation";
			this._CollectLocation.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_EnterEvent);
			this._CollectLocation.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_ListEvent);
			this._CollectLocation.KeyEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLocation_KeyEvent);
			// 
			// _ItemBalanceList
			// 
			this._ItemBalanceList.ListScreenParent = this._CollectLot;
			this._ItemBalanceList.ParentScreen = this._CollectLot;
			this._ItemBalanceList.ScreenName = "F41021_List";
			this._ItemBalanceList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ItemLocationList_EnterEvent);
			this._ItemBalanceList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ItemBalanceList_LoadEvent);
			// 
			// _CollectLot
			// 
			this._CollectLot.ListScreen = this._ItemBalanceList;
			this._CollectLot.ListScreenControl = this._ItemBalanceList;
			this._CollectLot.ScreenName = "CollectLot";
			this._CollectLot.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLot_EnterEvent);
			this._CollectLot.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLot_ListEvent);
			this._CollectLot.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLot_LoadEvent);
			// 
			// _LocationMasterList
			// 
			this._LocationMasterList.ScreenName = "F4100_List";
			this._LocationMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LocationMasterList_LoadEvent);
			this._LocationMasterList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LocationMasterList_LeavingEvent);
			// 
			// _CollectPrinter
			// 
			this._CollectPrinter.ScreenName = "CollectPrinter";
			this._CollectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_EnterEvent);
			this._CollectPrinter.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinter_ListEvent);
			// 
			// _DefinedPrintersTableList
			// 
			this._DefinedPrintersTableList.ListScreenParent = this._CollectPrinterStartOfOrder;
			this._DefinedPrintersTableList.ParentScreen = this._CollectPrinterStartOfOrder;
			this._DefinedPrintersTableList.ScreenName = "FRFS_Printers";
			this._DefinedPrintersTableList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DefinedPrinters_EnterEvent);
			this._DefinedPrintersTableList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._DefinedPrintersTableList_LoadEvent);
			// 
			// _CollectPrinterStartOfOrder
			// 
			this._CollectPrinterStartOfOrder.ListScreen = this._DefinedPrintersTableList;
			this._CollectPrinterStartOfOrder.ListScreenControl = this._DefinedPrintersTableList;
			this._CollectPrinterStartOfOrder.NextScreen = this._CollectQuantity;
			this._CollectPrinterStartOfOrder.ScreenName = "CollectPrinterStartOfOrder";
			this._CollectPrinterStartOfOrder.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinterStartOfOrder_EnterEvent);
			this._CollectPrinterStartOfOrder.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPrinterStartOfOrder_ListEvent);
			// 
			// _EmptyLocationsList
			// 
			this._EmptyLocationsList.ScreenName = "F4100Empty_List";
			this._EmptyLocationsList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.EmptyLocationList_EnterEvent);
			this._EmptyLocationsList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_LoadEvent);
			this._EmptyLocationsList.LeavingEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._EmptyLocationsList_LeavingEvent);
			// 
			// _CollectPotency
			// 
			this._CollectPotency.ScreenName = "CollectPotency";
			this._CollectPotency.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectPotency_EnterEvent);
			// 
			// _DisplayToleranceWarning
			// 
			this._DisplayToleranceWarning.ScreenName = "DisplayToleranceWarning";
			this._DisplayToleranceWarning.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.DisplayToleranceWarning_EnterEvent);
			// 
			// _CollectLotSerialExpDate
			// 
			this._CollectLotSerialExpDate.ScreenName = "CollectLotSerialExpDate";
			this._CollectLotSerialExpDate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LotSerialExpDate_EnterEvent);
			this._CollectLotSerialExpDate.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectLotSerialExpDate_LoadEvent);
			// 
			// _LicensePlateMasterList
			// 
			this._LicensePlateMasterList.ListScreenParent = this._CollectLicensePlate;
			this._LicensePlateMasterList.ParentScreen = this._CollectLicensePlate;
			this._LicensePlateMasterList.ScreenName = "LicensePlateMaster_List";
			this._LicensePlateMasterList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.LicensePlateMasterList_EnterEvent);
			this._LicensePlateMasterList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._LicensePlateMasterList_LoadEvent);
			// 
			// _CollectLicensePlate
			// 
			this._CollectLicensePlate.ListScreen = this._LicensePlateMasterList;
			this._CollectLicensePlate.ListScreenControl = this._LicensePlateMasterList;
			this._CollectLicensePlate.NextScreen = this._CollectLocation;
			this._CollectLicensePlate.ScreenName = "CollectLicensePlate";
			this._CollectLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_EnterEvent);
			this._CollectLicensePlate.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_ListEvent);
			this._CollectLicensePlate.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectLicensePlate_LoadEvent);
			// 
			// _CollectSerialNumber
			// 
			this._CollectSerialNumber.ScreenName = "CollectSerialNumber";
			this._CollectSerialNumber.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectSerialNumber_EnterEvent);
			this._CollectSerialNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._CollectSerialNumber_LoadEvent);
			// 
			// _PrintBarCodeLabels
			// 
			this._PrintBarCodeLabels.ScreenName = "PrintBarCodeLabels";
			this._PrintBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.PrintBarCodeLabels_EnterEvent);
			// 
			// _ConfirmNewLicensePlate
			// 
			this._ConfirmNewLicensePlate.ScreenName = "ConfirmNewLicensePlate";
			this._ConfirmNewLicensePlate.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_EnterEvent);
			this._ConfirmNewLicensePlate.BackValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ConfirmNewLicensePlate_BackEvent);
			// 
			// CollectIssueItemNumber
			// 
			this.CollectIssueItemNumber.NextScreen = this.CollectIssueLocation;
			this.CollectIssueItemNumber.ScreenName = "CollectIssueItemNumber";
			this.CollectIssueItemNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectIssueItemNumber_ValidateMethod);
			// 
			// CollectIssueLocation
			// 
			this.CollectIssueLocation.ScreenName = "CollectIssueLocation";
			this.CollectIssueLocation.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectIssueLocation_ValidateMethod);
			// 
			// WorkOrderPartsList
			// 
			this.WorkOrderPartsList.ScreenName = "F3111_List";
			this.WorkOrderPartsList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.WorkOrderPartsList_LoadEvent);
			// 
			// CollectIssueLotNumber
			// 
			this.CollectIssueLotNumber.ScreenName = "CollectIssueLotNumber";
			this.CollectIssueLotNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectIssueLotNumber_ValidateMethod);
			// 
			// CollectIssueQuantity
			// 
			this.CollectIssueQuantity.NextScreen = CollectIssueUnitOfMeasure;
			this.CollectIssueQuantity.ScreenName = "CollectIssueQuantity";
			this.CollectIssueQuantity.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectIssueQuantity_ValidateMethod);
			// 
			// CollectIssueUnitOfMeasure
			// 
			this.CollectIssueUnitOfMeasure.ScreenName = "CollectIssueUnitOfMeasure";
			this.CollectIssueUnitOfMeasure.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectIssueUnitOfMeasure_ValidateMethod);
			// 
			// CollectIssueSerialNumber
			// 
			this.CollectIssueSerialNumber.ScreenName = "CollectIssueSerialNumber";
			this.CollectIssueSerialNumber.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectIssueSerialNumber_LoadEvent);
			this.CollectIssueSerialNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectIssueSerialNumber_ValidateMethod);
			// 
			// StoneridgeWorkOrderCompletionSerial
			// 
			this.StartScreen = this._CollectBranch;
			this.WorkflowID = "StoneridgeWorkOrderCompletionSerial";

        }

        protected GenericScreen CollectIssueLotNumber;
        protected GenericScreen CollectIssueLocation;
        protected GenericScreen CollectIssueQuantity;
        protected GenericScreen CollectIssueUnitOfMeasure;
        protected GenericScreen CollectIssueSerialNumber;
        protected GenericScreen CollectEmployee;
        protected GenericList EmployeeList;
    }
}
