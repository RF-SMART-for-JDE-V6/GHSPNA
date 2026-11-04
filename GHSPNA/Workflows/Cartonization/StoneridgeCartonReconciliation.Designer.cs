namespace ICS.Stoneridge.Wireless.JDE.Workflow.Cartonization
{
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgeCartonReconciliation
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this._CollectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ListBranchPlants = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this._CollectShipmentNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this._ListShipmentNumbers = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            this.BulkConfirm = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            // 
            // _CollectBranch
            // 
            this._CollectBranch.ListScreen = this._ListBranchPlants;
            this._CollectBranch.ListScreenControl = this._ListBranchPlants;
            this._CollectBranch.NextScreen = this._CollectShipmentNumber;
            this._CollectBranch.ScreenName = "CollectBranch";
            this._CollectBranch.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._CollectBranch_ValidateMethod);
            // 
            // _ListBranchPlants
            // 
            this._ListBranchPlants.DataColumnForValidation = "MCMCU";
            this._ListBranchPlants.ListScreenParent = this._CollectBranch;
            this._ListBranchPlants.ParentScreen = this._CollectBranch;
            this._ListBranchPlants.ScreenName = "ListBranchPlants";
            this._ListBranchPlants.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ListBranchPlants_LoadEvent);
            // 
            // _CollectShipmentNumber
            // 
            this._CollectShipmentNumber.ListScreen = this._ListShipmentNumbers;
            this._CollectShipmentNumber.ListScreenControl = this._ListShipmentNumbers;
            this._CollectShipmentNumber.NextScreen = this.BulkConfirm;
            this._CollectShipmentNumber.ScreenName = "CollectShipmentNumber";
            this._CollectShipmentNumber.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this._CollectShipmentNumber_ValidateMethod);
            // 
            // _ListShipmentNumbers
            // 
            this._ListShipmentNumbers.DataColumnForValidation = "SDSHPN";
            this._ListShipmentNumbers.ListScreenParent = this._CollectShipmentNumber;
            this._ListShipmentNumbers.ParentScreen = this._CollectShipmentNumber;
            this._ListShipmentNumbers.ScreenName = "ListShipmentNumbers";
            this._ListShipmentNumbers.LoadEvent += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ScreenControlEventHandler(this._ListShipmentNumbers_LoadEvent);
            // 
            // BulkConfirm
            // 
            this.BulkConfirm.ScreenName = "BulkConfirm";
            this.BulkConfirm.ValidateMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.ValidationHandler(this.BulkConfirm_ValidateMethod);
            // 
            // StoneridgeCartonReconciliation
            // 
            this.StartScreen = this._CollectBranch;
            this.WorkflowID = "StoneridgeCartonReconciliation";
        }

        #endregion

        protected GenericScreen _CollectBranch;

        protected GenericScreen _CollectShipmentNumber;

        protected GenericScreen BulkConfirm;

        protected GenericList _ListBranchPlants;

        protected GenericList _ListShipmentNumbers;
    }
}