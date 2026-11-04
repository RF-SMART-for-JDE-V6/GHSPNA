using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Cartonization
{
    partial class StoneridgeCartonMaintenance
    {
        private void InitializeComponent()
        {
            this._collectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._branch_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._collectPickMode = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectPickSlip = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._pickSlipList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._collectShipment = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._shipmentList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._collectPrinter = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._printerList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._collectSequence = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectToCarton = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._toCarton_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._RFSCarton_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectCarton = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectItem = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectLot = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectQuantity = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectUOM = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectFromCarton = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._confirmRemoveCarton = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._confirmDeletePallet = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._printBarCodeLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectWeight = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectLength = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectWidth = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._collectHeight = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._OrderDetail_List = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectUnpackSerialNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectUnpackPalletNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.WarnUnpackSerialNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ConfirmDoneUnpacking = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectUnpackLabelType = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectNumberOfUnpackLabels = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _collectBranch
            // 
            this._collectBranch.ListScreen = this._branch_List;
            this._collectBranch.ListScreenControl = this._branch_List;
            this._collectBranch.NextScreen = this._collectPickMode;
            this._collectBranch.ScreenName = "CollectBranch";
            this._collectBranch.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectBranch_EnterValidationEvent);
            // 
            // _branch_List
            // 
            this._branch_List.ListScreenParent = this._collectBranch;
            this._branch_List.ParentScreen = this._collectBranch;
            this._branch_List.ScreenName = "F0006_List";
            this._branch_List.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._branch_List_EnterValidationEvent);
            this._branch_List.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._branch_List_LoadEvent);
            // 
            // _collectPickMode
            // 
            this._collectPickMode.ScreenName = "CollectPickMode";
            this._collectPickMode.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectPickMode_EnterValidationEvent);
            // 
            // _collectPickSlip
            // 
            this._collectPickSlip.ListScreen = this._pickSlipList;
            this._collectPickSlip.ListScreenControl = this._pickSlipList;
            this._collectPickSlip.ScreenName = "CollectPickSlip";
            this._collectPickSlip.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectPickSlip_EnterValidationEvent);
            // 
            // _pickSlipList
            // 
            this._pickSlipList.ListScreenParent = this._collectPickSlip;
            this._pickSlipList.ParentScreen = this._collectPickSlip;
            this._pickSlipList.ScreenName = "F4211PickSlip_List";
            this._pickSlipList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._pickSlipList_EnterValidationEvent);
            this._pickSlipList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._pickSlipList_LoadEvent);
            // 
            // _collectShipment
            // 
            this._collectShipment.ListScreen = this._shipmentList;
            this._collectShipment.ListScreenControl = this._shipmentList;
            this._collectShipment.ScreenName = "CollectShipment";
            this._collectShipment.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectShipment_EnterValidationEvent);
            // 
            // _shipmentList
            // 
            this._shipmentList.ListScreenParent = this._collectShipment;
            this._shipmentList.ParentScreen = this._collectShipment;
            this._shipmentList.ScreenName = "F4211Shipment_List";
            this._shipmentList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._shipmentList_EnterValidationEvent);
            this._shipmentList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._shipmentList_LoadEvent);
            // 
            // _collectPrinter
            // 
            this._collectPrinter.ListScreen = this._printerList;
            this._collectPrinter.ListScreenControl = this._printerList;
            this._collectPrinter.NextScreen = this._collectSequence;
            this._collectPrinter.ScreenName = "CollectPrinter";
            this._collectPrinter.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectPrinter_EnterValidationEvent);
            this._collectPrinter.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectPrinter_LoadEvent);
            // 
            // _printerList
            // 
            this._printerList.ListScreenParent = this._collectPrinter;
            this._printerList.ParentScreen = this._collectPrinter;
            this._printerList.ScreenName = "FRFS_Printers";
            this._printerList.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._printerList_EnterValidationEvent);
            this._printerList.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._printerList_LoadEvent);
            // 
            // _collectSequence
            // 
            this._collectSequence.ScreenName = "CollectSequence";
            this._collectSequence.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectSequence_EnterValidationEvent);
            // 
            // _collectToCarton
            // 
            this._collectToCarton.ListScreen = this._toCarton_List;
            this._collectToCarton.ListScreenControl = this._toCarton_List;
            this._collectToCarton.ScreenName = "CollectToCarton";
            this._collectToCarton.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectToCarton_EnterValidationEvent);
            this._collectToCarton.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._collectToCarton_F2Method);
            // 
            // _toCarton_List
            // 
            this._toCarton_List.ListScreenParent = this._collectToCarton;
            this._toCarton_List.ParentScreen = this._collectToCarton;
            this._toCarton_List.ScreenName = "F4620Carton_List";
            this._toCarton_List.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._toCarton_List_EnterValidationEvent);
            this._toCarton_List.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._toCarton_List_LoadEvent);
            // 
            // _RFSCarton_List
            // 
            this._RFSCarton_List.ScreenName = "AvailableCarton_List";
            this._RFSCarton_List.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._RFSCarton_List_EnterValidationEvent);
            // 
            // _collectCarton
            // 
            this._collectCarton.ScreenName = "CollectCarton";
            this._collectCarton.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectCarton_EnterValidationEvent);
            this._collectCarton.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._collectCarton_F2Method);
            // 
            // _collectItem
            // 
            this._collectItem.NextScreen = this._collectLot;
            this._collectItem.ScreenName = "CollectItem";
            this._collectItem.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectItem_EnterValidationEvent);
            this._collectItem.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._collectItem_F2Method);
            // 
            // _collectLot
            // 
            this._collectLot.NextScreen = this._collectQuantity;
            this._collectLot.ScreenName = "CollectLot";
            this._collectLot.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectLot_EnterValidationEvent);
            this._collectLot.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectLot_LoadEvent);
            // 
            // _collectQuantity
            // 
            this._collectQuantity.NextScreen = this._collectUOM;
            this._collectQuantity.ScreenName = "CollectQuantity";
            this._collectQuantity.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectQuantity_EnterValidationEvent);
            // 
            // _collectUOM
            // 
            this._collectUOM.ScreenName = "CollectUnitOfMeasure";
            this._collectUOM.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectUOM_EnterValidationEvent);
            this._collectUOM.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectUOM_LoadEvent);
            // 
            // _collectFromCarton
            // 
            this._collectFromCarton.ScreenName = "CollectFromCarton";
            this._collectFromCarton.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectFromCarton_EnterValidationEvent);
            this._collectFromCarton.F2Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this._collectFromCarton_F2Method);
            // 
            // _confirmRemoveCarton
            // 
            this._confirmRemoveCarton.ScreenName = "ConfirmRemoveCarton";
            this._confirmRemoveCarton.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._confirmRemoveCarton_EnterValidationEvent);
            // 
            // _confirmDeletePallet
            // 
            this._confirmDeletePallet.ScreenName = "ConfirmDeletePallet";
            this._confirmDeletePallet.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._confirmDeletePallet_EnterValidationEvent);
            // 
            // _printBarCodeLabels
            // 
            this._printBarCodeLabels.ScreenName = "PrintBarCodeLabels";
            this._printBarCodeLabels.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._printBarCodeLabels_EnterValidationEvent);
            this._printBarCodeLabels.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._printBarCodeLabels_LoadEvent);
            // 
            // _collectWeight
            // 
            this._collectWeight.ScreenName = "CollectWeight";
            this._collectWeight.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectWeight_EnterValidationEvent);
            this._collectWeight.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectWeight_LoadEvent);
            // 
            // _collectLength
            // 
            this._collectLength.ScreenName = "CollectLength";
            this._collectLength.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectLength_EnterValidationEvent);
            this._collectLength.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectLength_LoadEvent);
            // 
            // _collectWidth
            // 
            this._collectWidth.ScreenName = "CollectWidth";
            this._collectWidth.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectWidth_EnterValidationEvent);
            this._collectWidth.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectWidth_LoadEvent);
            // 
            // _collectHeight
            // 
            this._collectHeight.ScreenName = "CollectHeight";
            this._collectHeight.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectHeight_EnterValidationEvent);
            this._collectHeight.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._collectHeight_LoadEvent);
            // 
            // _OrderDetail_List
            // 
            this._OrderDetail_List.ScreenName = "F4211_List";
            this._OrderDetail_List.EnterValidationEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._OrderDetail_List_EnterValidationEvent);
            // 
            // CollectUnpackSerialNumber
            // 
            this.CollectUnpackSerialNumber.ScreenName = "CollectUnpackSerialNumber";
            this.CollectUnpackSerialNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectUnpackSerialNumber_ValidateMethod);
            this.CollectUnpackSerialNumber.F6Method += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.FunctionKeyHandler(this.CollectUnpackSerialNumber_F6Method);
            // 
            // CollectUnpackPalletNumber
            // 
            this.CollectUnpackPalletNumber.ScreenName = "CollectUnpackPalletNumber";
            this.CollectUnpackPalletNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectUnpackPalletNumber_ValidateMethod);
            // 
            // WarnUnpackSerialNumbers
            // 
            this.WarnUnpackSerialNumbers.ScreenName = "WarnUnpackSerialNumbers";
            this.WarnUnpackSerialNumbers.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.WarnUnpackSerialNumbers_ValidateMethod);
            // 
            // ConfirmDoneUnpacking
            // 
            this.ConfirmDoneUnpacking.ScreenName = "ConfirmDoneUnpacking";
            this.ConfirmDoneUnpacking.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.ConfirmDoneUnpacking_ValidateMethod);
            // 
            // CollectUnpackLabelType
            // 
            this.CollectUnpackLabelType.ScreenName = "CollectUnpackLabelType";
            this.CollectUnpackLabelType.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectUnpackLabelType_ValidateMethod);
            // 
            // CollectNumberOfUnpackLabels
            // 
            this.CollectNumberOfUnpackLabels.ScreenName = "CollectNumberOfUnpackLabels";
            this.CollectNumberOfUnpackLabels.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectNumberOfUnpackLabels_ValidateMethod);
            // 
            // StoneridgeCartonMaintenance
            // 
            this.StartScreen = this._collectBranch;
            this.WorkflowID = "StoneridgeCartonMaintenance";
        }

        protected GenericScreen _collectBranch;

        protected GenericList _branch_List;

        protected GenericScreen _collectPickMode;

        protected GenericScreen _collectPickSlip;

        protected GenericScreen _collectShipment;

        protected GenericList _pickSlipList;

        protected GenericList _shipmentList;

        protected GenericScreen _collectPrinter;

        protected GenericList _printerList;

        protected GenericScreen _collectSequence;

        protected GenericScreen _collectToCarton;

        protected GenericList _toCarton_List;

        protected GenericScreen _RFSCarton_List;

        protected GenericScreen _collectCarton;

        protected GenericScreen _collectItem;

        protected GenericScreen _collectFromCarton;

        protected GenericScreen _confirmRemoveCarton;

        protected GenericScreen _confirmDeletePallet;

        protected GenericScreen _collectLot;

        protected GenericScreen _collectQuantity;

        protected GenericScreen _collectUOM;

        protected GenericScreen _printBarCodeLabels;

        protected GenericScreen _collectWeight;

        protected GenericScreen _collectLength;

        protected GenericScreen _collectWidth;

        protected GenericScreen _collectHeight;

        protected GenericList _OrderDetail_List;

        protected GenericScreen CollectUnpackSerialNumber;

        protected GenericScreen CollectUnpackPalletNumber;

        protected GenericScreen WarnUnpackSerialNumbers;

        protected GenericScreen ConfirmDoneUnpacking;

        protected GenericScreen CollectUnpackLabelType;

        protected GenericScreen CollectNumberOfUnpackLabels;
    }
}