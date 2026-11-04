namespace ICS.Stoneridge.Wireless.JDE.Workflow.Shipping
{
    using System;
    using ICS.Database.TableObjects.RFS;
    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgeShipping
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.CollectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectQuantity = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLotNo = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectItemNo = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectShipmentNo = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListShipments = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.ListItems = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectStagingLocation = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.ListStagingLocations = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.CollectSerialToAdd = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectSerialToRemove = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // CollectBranch
            // 
            this.CollectBranch.ListScreen = this.ListBranch;
            this.CollectBranch.ListScreenControl = this.ListBranch;
            this.CollectBranch.ScreenName = "CollectBranch";
            this.CollectBranch.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectBranch_ListEvent);
            this.CollectBranch.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectBranch_ValidateMethod);
            // 
            // ListBranch
            // 
            this.ListBranch.DataColumnForValidation = "MCMCU";
            this.ListBranch.ListScreenParent = this.CollectBranch;
            this.ListBranch.ParentScreen = this.CollectBranch;
            this.ListBranch.ScreenName = "ListBranch";
            this.ListBranch.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListBranch_LoadEvent);
            // 
            // CollectQuantity
            // 
            this.CollectQuantity.ScreenName = "CollectQuantity";
            this.CollectQuantity.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectQuantity_ValidateMethod);
            // 
            // CollectLotNo
            // 
            this.CollectLotNo.ScreenName = "CollectLotNo";
            this.CollectLotNo.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectLotNo_ValidateMethod);
            // 
            // CollectItemNo
            // 
            this.CollectItemNo.ScreenName = "CollectItemNo";
            this.CollectItemNo.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectItemNo_ListEvent);
            this.CollectItemNo.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectItemNo_ValidateMethod);
            // 
            // CollectShipmentNo
            // 
            this.CollectShipmentNo.ListScreen = this.ListShipments;
            this.CollectShipmentNo.ListScreenControl = this.ListShipments;
            this.CollectShipmentNo.ScreenName = "CollectShipmentNo";
            this.CollectShipmentNo.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectShipmentNo_ListEvent);
            this.CollectShipmentNo.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectShipmentNo_ValidateMethod);
            // 
            // ListShipments
            // 
            this.ListShipments.DataColumnForValidation = "SDSHPN";
            this.ListShipments.ListScreenParent = this.CollectShipmentNo;
            this.ListShipments.ParentScreen = this.CollectShipmentNo;
            this.ListShipments.ScreenName = "ListShipments";
            this.ListShipments.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListShipments_LoadEvent);
            // 
            // ListItems
            // 
            this.ListItems.ScreenName = "ListItems";
            this.ListItems.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListItems_LoadEvent);
            // 
            // CollectStagingLocation
            // 
            this.CollectStagingLocation.ScreenName = "CollectStagingLocation";
            this.CollectStagingLocation.ListEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.CollectStagingLocation_ListEvent);
            this.CollectStagingLocation.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectStagingLocation_ValidateMethod);
            // 
            // ListStagingLocations
            // 
            this.ListStagingLocations.ScreenName = "ListStagingLocations";
            this.ListStagingLocations.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this.ListStagingLocations_LoadEvent);
            // 
            // CollectSerialToAdd
            // 
            this.CollectSerialToAdd.ScreenName = "CollectSerialToAdd";
            this.CollectSerialToAdd.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectSerialToAdd_ValidateMethod);
            // 
            // CollectSerialToRemove
            // 
            this.CollectSerialToRemove.ScreenName = "CollectSerialToRemove";
            this.CollectSerialToRemove.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.CollectSerialToRemove_ValidateMethod);
            // 
            // StoneridgeShipping
            // 
            this.StartScreen = this.CollectBranch;
            this.WorkflowID = "StoneridgeShipping";
        }

        #endregion

        protected GenericScreen CollectBranch;

        protected GenericScreen CollectQuantity;

        protected GenericScreen CollectLotNo;

        protected GenericScreen CollectItemNo;

        protected GenericScreen CollectShipmentNo;

        protected GenericList ListBranch;

        protected GenericList ListShipments;

        protected GenericList ListItems;

        protected GenericScreen CollectStagingLocation;

        protected GenericList ListStagingLocations;

        protected GenericScreen CollectSerialToAdd;

        protected GenericScreen CollectSerialToRemove;
    }
}