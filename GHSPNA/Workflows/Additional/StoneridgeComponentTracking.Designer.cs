namespace ICS.Stoneridge.Wireless.JDE.Workflow.Additional
{
    using ICS.Workflow.Base.WorkflowDesign.Components;

    public partial class StoneridgeComponentTracking
    {
        #region Screen defs

        #endregion

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.CollectBranch = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectCartonSerial = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectFGNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectCPNTNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectWorkOrder = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.CollectLotNumber = new ICS.Workflow.Base.WorkflowDesign.Components.GenericScreen(this);
            this.BranchPlantList = new ICS.Workflow.Base.WorkflowDesign.Components.GenericList(this);
            // 
            // CollectBranch
            // 
            this.CollectBranch.ListScreen = this.BranchPlantList;
            this.CollectBranch.ListScreenControl = this.BranchPlantList;
            this.CollectBranch.NextScreen = this.CollectWorkOrder;
            this.CollectBranch.ScreenName = "CollectBranch";
            // 
            // CollectCartonSerial
            // 
            this.CollectCartonSerial.ScreenName = "CollectCartonSerial";
            // 
            // CollectFGNumber
            // 
            this.CollectFGNumber.ScreenName = "CollectFGNumber";
            // 
            // CollectCPNTNumber
            // 
            this.CollectCPNTNumber.ScreenName = "CollectCPNTNumber";
            // 
            // CollectWorkOrder
            // 
            this.CollectWorkOrder.ScreenName = "CollectWorkOrder";
            // 
            // CollectLotNumber
            // 
            this.CollectLotNumber.ScreenName = "CollectLotNumber";
            // 
            // BranchPlantList
            // 
            this.BranchPlantList.ListScreenParent = this.CollectBranch;
            this.BranchPlantList.ParentScreen = this.CollectBranch;
            this.BranchPlantList.ScreenName = "F0006_List";
            this.BranchPlantList.LoadingMethod += new ICS.Workflow.Base.WorkflowDesign.Components.RFSScreenControl.LoadingHandler(this.BranchPlantList_LoadingMethod);
            // 
            // StoneridgeComponentTracking
            // 
            this.WorkflowID = "StoneridgeComponentTracking";
        }

        #endregion

        protected GenericScreen CollectBranch;

        protected GenericScreen CollectCartonSerial;

        protected GenericScreen CollectFGNumber;

        protected GenericScreen CollectCPNTNumber;

        protected GenericScreen CollectWorkOrder;

        protected GenericScreen CollectLotNumber;

        protected GenericList BranchPlantList;
    }
}