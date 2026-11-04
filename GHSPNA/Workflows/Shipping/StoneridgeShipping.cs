using ICS.Display;
using ICS.Workflow.Base.WorkflowDesign.Components;

namespace ICS.Stoneridge.Wireless.JDE.Workflow.Shipping
{
    public partial class StoneridgeShipping : RFSWorkflowDesigner
    {
        #region Business connector

        private StoneridgeShippingConnector connectorWorkflow;

        public StoneridgeShippingConnector _WorkflowConnector
        {
            get { return connectorWorkflow ?? (connectorWorkflow = new StoneridgeShippingConnector(this)); }
        }

        #endregion

        #region Constructors and overrides

        public StoneridgeShipping(UserSession userSession) :
            base(ref userSession)
        {
            InitializeComponent();
            Transactions.ERP = "PeopleSoft";
        }

        public StoneridgeShipping()
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

        private bool CollectBranch_ValidateMethod(string sInput)
        {
            _WorkflowConnector.ValidateBranch(sInput);
            Next(CollectStagingLocation);
            return true;
        }

        private void CollectBranch_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListBranch_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkflowConnector.LoadBranchList(ListBranch);
        }

        #endregion

        #region Collect Staging Location

        private bool CollectStagingLocation_ValidateMethod(string sInput)
        {
            // Not implemented
            Next(CollectShipmentNo);
            return true;
        }

        private void CollectStagingLocation_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListStagingLocations_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        #endregion

        #region Collect Shipment

        private bool CollectShipmentNo_ValidateMethod(string sInput)
        {
            _WorkflowConnector.ValidateShipmentNumber(sInput);
            Next(CollectItemNo);
            return true;
        }

        private void CollectShipmentNo_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListShipments_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
            _WorkflowConnector.LoadShipmentList();
        }

        #endregion

        #region Collect Item

        private bool CollectItemNo_ValidateMethod(string sInput)
        {
            return true;
        }

        private void CollectItemNo_ListEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        private void ListItems_LoadEvent(RFSScreenControl sender, ScreenEventArgs e)
        {
        }

        #endregion

        #region Collect Lot

        private bool CollectLotNo_ValidateMethod(string sInput)
        {
            return true;
        }

        #endregion

        #region Collect Quantity

        private bool CollectQuantity_ValidateMethod(string sInput)
        {
            return true;
        }

        #endregion

        #region Collect Serial Number To Add

        private bool CollectSerialToAdd_ValidateMethod(string sInput)
        {
            return true;
        }

        #endregion

        #region Collect Serial Number To Remove

        private bool CollectSerialToRemove_ValidateMethod(string sInput)
        {
            return true;
        }

        #endregion

        #endregion
    }
}