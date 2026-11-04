namespace ICS.Stoneridge.Wireless.JDE.Workflow.Cartonization
{
    #region Usings

    using ICS.Display;
    using ICS.Utilities;
    using ICS.Workflow.Base.WorkflowDesign.Components;

    #endregion

    public partial class StoneridgeCartonReconciliation : RFSWorkflowDesigner
    {
        #region Business Connector

        private StoneridgeCartonReconciliationConnector _connectorWorkflow;

        public StoneridgeCartonReconciliationConnector WorkflowConnector
        {
            get { return _connectorWorkflow ?? (_connectorWorkflow = new StoneridgeCartonReconciliationConnector(this)); }
        }

        #endregion

        #region Constructors and overrides

        public StoneridgeCartonReconciliation(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeCartonReconciliation()
        {
            InitializeComponent();
        }

        protected override void Initialize()
        {
        }

        public override void Dispose()
        {
            ScreenMap.ClearAllWorkflows();
            base.Dispose();
        }

        #endregion

        #region Screen methods

        #region Collect Branch

        private bool _CollectBranch_ValidateMethod(string sInput)
        {
            return WorkflowConnector.ValidateBranchPlant(sInput);
        }

        private void _ListBranchPlants_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadBranchPlantList(_ListBranchPlants);
        }

        #endregion

        #region Collect Shipment Number

        private bool _CollectShipmentNumber_ValidateMethod(string sInput)
        {
            return WorkflowConnector.ValidateShipment(sInput);
        }

        private void _ListShipmentNumbers_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            WorkflowConnector.LoadShipmentList(_ListShipmentNumbers);
        }

        #endregion

        #region Bulk Confirm

        private bool BulkConfirm_ValidateMethod(string sInput)
        {
            if (sInput.GetYesNo()) WorkflowConnector.SendToERP();

            RestartLoop(_CollectShipmentNumber);

            return true;
        }

        #endregion

        #endregion
    }
}